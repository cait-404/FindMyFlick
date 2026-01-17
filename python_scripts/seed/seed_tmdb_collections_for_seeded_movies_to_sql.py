"""
seed_tmdb_collections_for_seeded_movies_to_sql.py
-------------------------------------------------
Purpose:
  For the seeded movies (from 001_seed_us_popular_100_movies.sql), fetch TMDB movie details
  and generate a seed SQL file to populate:
    - collections
    - movie_collections

Output:
  database/seed/007_seed_us_popular_100_movie_collections.sql

Assumptions:
  - TMDB_API_KEY is available via .env (schema_utils.load_api_key handles this)
  - movies already seeded so imdb_id + tmdb_id exist in the seed SQL

Run:
  python -m python_scripts.seed.seed_tmdb_collections_for_seeded_movies_to_sql
"""

from __future__ import annotations

from pathlib import Path
from datetime import datetime
from typing import Any, Dict, List, Tuple
import re
import time

from python_scripts.shared.schema_utils import load_api_key, fetch_json

PROJECT_ROOT = Path(__file__).resolve().parents[2]

MOVIES_SEED_SQL = PROJECT_ROOT / "database" / "seed" / "001_seed_us_popular_100_movies.sql"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "007_seed_us_popular_100_movie_collections.sql"

TMDB_BASE = "https://api.themoviedb.org/3"
TMDB_IMAGE_BASE = "https://image.tmdb.org/t/p/w500"  # decent size for collection poster/backdrop

SLEEP_SECONDS = 0.25


def sql_escape(s: str) -> str:
    return s.replace("'", "''")


def sql_literal(v: Any) -> str:
    if v is None:
        return "NULL"
    if isinstance(v, bool):
        return "TRUE" if v else "FALSE"
    if isinstance(v, (int, float)):
        return str(v)
    return f"'{sql_escape(str(v))}'"


def tmdb_get_json(tmdb_key: str, path: str, params: Dict[str, Any] | None = None) -> Dict[str, Any]:
    p = {"api_key": tmdb_key}
    if params:
        p.update(params)
    return fetch_json(f"{TMDB_BASE}{path}", params=p)


def extract_imdb_tmdb_pairs(seed_sql_path: Path) -> List[Tuple[str, int]]:
    """
    Extract (imdb_id, tmdb_id) from INSERT statements like:
      VALUES (
        'tt123...', 12345, 'Title', 2025, ...
      )
    Works across multiline SQL by scanning the entire file.
    """
    text = seed_sql_path.read_text(encoding="utf-8", errors="replace")

    pattern = re.compile(r"\(\s*'(?P<imdb>tt\d+)'\s*,\s*(?P<tmdb>\d+)\s*,", re.MULTILINE)
    out: List[Tuple[str, int]] = []

    for m in pattern.finditer(text):
        imdb_id = m.group("imdb")
        tmdb_id = int(m.group("tmdb"))
        out.append((imdb_id, tmdb_id))

    seen = set()
    unique: List[Tuple[str, int]] = []
    for imdb_id, tmdb_id in out:
        if imdb_id in seen:
            continue
        seen.add(imdb_id)
        unique.append((imdb_id, tmdb_id))

    return unique


def main() -> None:
    tmdb_key = load_api_key("TMDB_API_KEY")

    if not MOVIES_SEED_SQL.exists():
        raise RuntimeError(f"Movies seed SQL not found: {MOVIES_SEED_SQL}")

    pairs = extract_imdb_tmdb_pairs(MOVIES_SEED_SQL)
    print(f"Found {len(pairs)} unique (imdb_id, tmdb_id) pairs in: {MOVIES_SEED_SQL.name}")

    collections_by_id: Dict[int, Dict[str, Any]] = {}
    movie_collection_rows: List[Dict[str, Any]] = []

    for imdb_id, tmdb_id in pairs:
        # movie details endpoint has belongs_to_collection
        data = tmdb_get_json(tmdb_key, f"/movie/{tmdb_id}")

        btc = data.get("belongs_to_collection")
        if not isinstance(btc, dict):
            time.sleep(SLEEP_SECONDS)
            continue

        collection_id = btc.get("id")
        collection_name = (btc.get("name") or "").strip()

        if not isinstance(collection_id, int) or not collection_name:
            time.sleep(SLEEP_SECONDS)
            continue

        poster_path = btc.get("poster_path")
        backdrop_path = btc.get("backdrop_path")

        poster_url = f"{TMDB_IMAGE_BASE}{poster_path}" if poster_path else None
        backdrop_url = f"{TMDB_IMAGE_BASE}{backdrop_path}" if backdrop_path else None

        collections_by_id[collection_id] = {
            "tmdb_collection_id": collection_id,
            "collection_name": collection_name,
            "poster_url": poster_url,
            "backdrop_url": backdrop_url,
        }

        movie_collection_rows.append(
            {
                "imdb_id": imdb_id,
                "tmdb_collection_id": collection_id,
            }
        )

        time.sleep(SLEEP_SECONDS)

    print(f"Unique collections collected: {len(collections_by_id)}")
    print(f"Movie->collection rows: {len(movie_collection_rows)}")

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    lines: List[str] = []
    lines.append(f"-- Generated {ts}\nBEGIN;\n\n")

    # Upsert collections (safe if rerun)
    for cid in sorted(collections_by_id.keys()):
        c = collections_by_id[cid]
        lines.append(
            "INSERT INTO public.collections (\n"
            "  tmdb_collection_id, collection_name, poster_url, backdrop_url\n"
            ") VALUES (\n"
            f"  {sql_literal(c['tmdb_collection_id'])}, {sql_literal(c['collection_name'])}, "
            f"{sql_literal(c.get('poster_url'))}, {sql_literal(c.get('backdrop_url'))}\n"
            ")\n"
            "ON CONFLICT (tmdb_collection_id) DO UPDATE SET\n"
            "  collection_name = EXCLUDED.collection_name,\n"
            "  poster_url = EXCLUDED.poster_url,\n"
            "  backdrop_url = EXCLUDED.backdrop_url,\n"
            "  updated_at = NOW();\n\n"
        )

    # Upsert movie_collections (one collection per movie)
    for r in movie_collection_rows:
        lines.append(
            "INSERT INTO public.movie_collections (\n"
            "  imdb_id, tmdb_collection_id\n"
            ") VALUES (\n"
            f"  {sql_literal(r['imdb_id'])}, {sql_literal(r['tmdb_collection_id'])}\n"
            ")\n"
            "ON CONFLICT (imdb_id) DO UPDATE SET\n"
            "  tmdb_collection_id = EXCLUDED.tmdb_collection_id;\n"
        )
    lines.append("\n")

    lines.append("COMMIT;\n")

    OUT_SQL.write_text("".join(lines), encoding="utf-8")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()