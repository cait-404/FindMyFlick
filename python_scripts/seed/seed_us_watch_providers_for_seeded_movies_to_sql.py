"""
seed_us_watch_providers_for_seeded_movies_to_sql.py

Reads your seeded movies SQL (which contains imdb_id + tmdb_id),
calls TMDB /movie/{id}/watch/providers, and generates a SQL seed file that inserts:

- streaming_providers (provider catalog)
- movie_streaming (bridge: imdb_id <-> provider + offer_type)

US-only.

Expected environment variable:
- TMDB_API_KEY

Inputs:
- database/seed/001_seed_us_popular_100_movies.sql

Output:
- database/seed/002_seed_us_popular_100_movie_streaming.sql
"""

from __future__ import annotations

import os
import re
import time
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Dict, Iterable, List, Optional, Tuple

import requests
from dotenv import load_dotenv

load_dotenv()

TMDB_API_KEY = os.getenv("TMDB_API_KEY")

TMDB_BASE_URL = "https://api.themoviedb.org/3"
REQUEST_TIMEOUT_SECONDS = 20

INPUT_MOVIES_SEED = Path("database/seed/001_seed_us_popular_100_movies.sql")
OUTPUT_SQL_PATH = Path("database/seed/002_seed_us_popular_100_movie_streaming.sql")

REGION = "US"

# Include these buckets now; you can ignore rent/buy later in the UI if you want.
TMDB_BUCKET_TO_OFFER_TYPE = {
    "flatrate": "subscription",
    "free": "free",
    "ads": "free_with_ads",
    "rent": "rent",
    "buy": "buy",
}


def _require_env() -> None:
    if not TMDB_API_KEY:
        raise RuntimeError(
            "Missing TMDB_API_KEY environment variable. "
            "Set it in your environment (or ensure load_dotenv() finds your .env)."
        )


def sql_literal(value: Optional[Any]) -> str:
    if value is None:
        return "NULL"
    if isinstance(value, bool):
        return "TRUE" if value else "FALSE"
    if isinstance(value, (int, float)):
        return str(value)
    s = str(value).replace("'", "''")
    return f"'{s}'"


def tmdb_get_json(endpoint: str, params: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
    url = f"{TMDB_BASE_URL}{endpoint}"
    params = params or {}
    params["api_key"] = TMDB_API_KEY

    last_exc: Optional[Exception] = None
    for attempt in range(1, 4):
        try:
            resp = requests.get(url, params=params, timeout=REQUEST_TIMEOUT_SECONDS)
            resp.raise_for_status()
            return resp.json()
        except Exception as e:
            last_exc = e
            time.sleep(0.6 * attempt)

    raise RuntimeError(f"TMDB request failed after retries: {url}") from last_exc


def extract_imdb_tmdb_pairs_from_movies_seed(path: Path) -> List[Tuple[str, int]]:
    """
    Your movies seed inserts look like:
      INSERT INTO movies (...) VALUES (
        'tt123', 456, 'Title', ...
      );

    This regex grabs:
      group1 = imdb_id (tt...)
      group2 = tmdb_id (int)
    """
    text = path.read_text(encoding="utf-8", errors="replace")

    pattern = re.compile(r"VALUES\s*\(\s*'(?P<imdb>tt\d+)'\s*,\s*(?P<tmdb>\d+)\s*,", re.IGNORECASE)
    pairs: List[Tuple[str, int]] = []
    for m in pattern.finditer(text):
        imdb_id = m.group("imdb")
        tmdb_id = int(m.group("tmdb"))
        pairs.append((imdb_id, tmdb_id))

    # de-dupe on imdb_id just in case
    seen = set()
    unique_pairs: List[Tuple[str, int]] = []
    for imdb_id, tmdb_id in pairs:
        if imdb_id in seen:
            continue
        seen.add(imdb_id)
        unique_pairs.append((imdb_id, tmdb_id))

    return unique_pairs


def fetch_us_watch_providers(tmdb_id: int) -> Dict[str, List[Dict[str, Any]]]:
    data = tmdb_get_json(f"/movie/{tmdb_id}/watch/providers", params={})
    results = (data.get("results") or {}).get(REGION) or {}
    # results may have keys: link, flatrate, free, ads, rent, buy
    bucketed: Dict[str, List[Dict[str, Any]]] = {}
    for bucket in TMDB_BUCKET_TO_OFFER_TYPE.keys():
        items = results.get(bucket)
        if isinstance(items, list) and items:
            bucketed[bucket] = items
    return bucketed


@dataclass(frozen=True)
class Provider:
    provider_id: int
    provider_name: str
    logo_path: Optional[str]
    display_priority: Optional[int]


def render_provider_insert(p: Provider) -> str:
    return (
        "INSERT INTO streaming_providers (\n"
        "  tmdb_provider_id, provider_name, logo_path, display_priority\n"
        ") VALUES (\n"
        f"  {sql_literal(p.provider_id)}, {sql_literal(p.provider_name)}, {sql_literal(p.logo_path)}, {sql_literal(p.display_priority)}\n"
        ")\n"
        "ON CONFLICT (tmdb_provider_id) DO UPDATE SET\n"
        "  provider_name = EXCLUDED.provider_name,\n"
        "  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),\n"
        "  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),\n"
        "  updated_at = NOW();\n"
    )


def render_movie_streaming_insert(imdb_id: str, provider_id: int, offer_type: str) -> str:
    return (
        "INSERT INTO movie_streaming (\n"
        "  imdb_id, tmdb_provider_id, offer_type\n"
        ") VALUES (\n"
        f"  {sql_literal(imdb_id)}, {sql_literal(provider_id)}, {sql_literal(offer_type)}\n"
        ")\n"
        "ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;\n"
    )


def main() -> None:
    _require_env()

    if not INPUT_MOVIES_SEED.exists():
        raise RuntimeError(f"Missing input seed file: {INPUT_MOVIES_SEED.resolve()}")

    pairs = extract_imdb_tmdb_pairs_from_movies_seed(INPUT_MOVIES_SEED)
    print(f"Found {len(pairs)} unique (imdb_id, tmdb_id) pairs in: {INPUT_MOVIES_SEED.name}")

    providers_catalog: Dict[int, Provider] = {}
    links: List[Tuple[str, int, str]] = []

    movies_with_any_provider = 0

    for idx, (imdb_id, tmdb_id) in enumerate(pairs, start=1):
        # small delay to be gentle on rate limits
        if idx % 20 == 0:
            time.sleep(0.4)

        buckets = fetch_us_watch_providers(tmdb_id)
        if not buckets:
            continue

        movies_with_any_provider += 1

        for bucket, items in buckets.items():
            offer_type = TMDB_BUCKET_TO_OFFER_TYPE[bucket]
            for item in items:
                pid = item.get("provider_id")
                pname = item.get("provider_name")
                if not pid or not pname:
                    continue

                provider = Provider(
                    provider_id=int(pid),
                    provider_name=str(pname),
                    logo_path=item.get("logo_path"),
                    display_priority=item.get("display_priority"),
                )
                providers_catalog[provider.provider_id] = provider
                links.append((imdb_id, provider.provider_id, offer_type))

    # write SQL
    OUTPUT_SQL_PATH.parent.mkdir(parents=True, exist_ok=True)

    sql_lines: List[str] = []
    sql_lines.append("BEGIN;\n\n")

    # providers first
    for pid in sorted(providers_catalog.keys()):
        sql_lines.append(render_provider_insert(providers_catalog[pid]))
        sql_lines.append("\n")

    sql_lines.append("\n")

    # then links
    for imdb_id, pid, offer_type in links:
        sql_lines.append(render_movie_streaming_insert(imdb_id, pid, offer_type))
        sql_lines.append("\n")

    sql_lines.append("COMMIT;\n")
    OUTPUT_SQL_PATH.write_text("".join(sql_lines), encoding="utf-8")

    print(f"Movies with at least one US provider bucket: {movies_with_any_provider}")
    print(f"Providers discovered: {len(providers_catalog)}")
    print(f"Total links written: {len(links)}")
    print(f"Saved: {OUTPUT_SQL_PATH.resolve()}")


if __name__ == "__main__":
    main()