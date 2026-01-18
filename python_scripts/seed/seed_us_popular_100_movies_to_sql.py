"""
seed_us_popular_100_movies_to_sql.py

Generate a SQL seed file for the FindMyFlick `movies` table using TMDB Discover (US).
This script guarantees:
- No duplicate movies by imdb_id in the generated SQL.
- Exactly TARGET_UNIQUE unique movies (unless TMDB/API limits prevent it).

Expected environment variable:
- TMDB_API_KEY

Output:
- database/seed/001_seed_us_popular_100_movies.sql
"""

from __future__ import annotations

import os
import time
from datetime import datetime
from pathlib import Path
from typing import Any, Dict, List, Optional, Tuple
from dotenv import load_dotenv
load_dotenv()

import requests


# -----------------------------
# Config
# -----------------------------
TMDB_API_KEY = os.getenv("TMDB_API_KEY")

TMDB_BASE_URL = "https://api.themoviedb.org/3"
TMDB_IMAGE_BASE = "https://image.tmdb.org/t/p/w500"

TARGET_UNIQUE = 100
DISCOVER_SORT_BY = "popularity.desc"
DISCOVER_REGION = "US"
DISCOVER_LANGUAGE = "en-US"

# Safety limits so we don't loop forever
MAX_PAGES_TO_SCAN = 25  # usually plenty for 100 uniques
REQUEST_TIMEOUT_SECONDS = 20

OUTPUT_SQL_PATH = Path("database/seed/001_seed_us_popular_100_movies.sql")


# -----------------------------
# Helpers
# -----------------------------
def _require_env() -> None:
    if not TMDB_API_KEY:
        raise RuntimeError(
            "Missing TMDB_API_KEY environment variable. "
            "Set it in your environment (or .env if you load env vars elsewhere)."
        )


def sql_literal(value: Optional[Any]) -> str:
    """
    Convert a Python value to a SQL literal.
    """
    if value is None:
        return "NULL"
    if isinstance(value, bool):
        return "TRUE" if value else "FALSE"
    if isinstance(value, (int, float)):
        return str(value)
    # string
    s = str(value)
    s = s.replace("'", "''")
    return f"'{s}'"


def parse_release_year(release_date: Optional[str]) -> Optional[int]:
    """
    TMDB release_date is usually 'YYYY-MM-DD'. Return year as int or None.
    """
    if not release_date or len(release_date) < 4:
        return None
    try:
        return int(release_date[:4])
    except ValueError:
        return None


def tmdb_get_json(endpoint: str, params: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
    """
    GET JSON from TMDB with basic retry.
    """
    url = f"{TMDB_BASE_URL}{endpoint}"
    params = params or {}
    params["api_key"] = TMDB_API_KEY

    # simple retries for transient failures
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


def get_discover_page(page: int) -> List[Dict[str, Any]]:
    today = datetime.now().strftime("%Y-%m-%d")

    params = {
        "sort_by": DISCOVER_SORT_BY,
        "region": DISCOVER_REGION,
        "with_original_language": None,  # leave open
        "include_adult": "false",
        "include_video": "false",
        "language": DISCOVER_LANGUAGE,
        "page": page,
        # Exclude future releases
        "primary_release_date.lte": today,
    }

    data = tmdb_get_json("/discover/movie", params=params)
    return data.get("results", []) or []


def get_movie_details(tmdb_id: int) -> Dict[str, Any]:
    return tmdb_get_json(
        f"/movie/{tmdb_id}",
        params={
            "language": DISCOVER_LANGUAGE,
        },
    )


def build_movie_row(details: Dict[str, Any]) -> Dict[str, Any]:
    """
    Map TMDB details into your movies table structure.
    Columns expected (based on your seed file):
      imdb_id, tmdb_id, title, release_year,
      mpaa_rating, runtime_minutes, plot_summary, poster_url,
      original_language, media_type, tagline, status
    """
    imdb_id = details.get("imdb_id")
    tmdb_id = details.get("id")
    title = details.get("title") or details.get("original_title")
    release_year = parse_release_year(details.get("release_date"))

    # TMDB does not provide MPAA directly here without release_dates endpoint.
    # Keeping NULL unless you later enrich via another endpoint/source.
    mpaa_rating = None

    runtime = details.get("runtime")
    runtime_minutes = runtime if (isinstance(runtime, int) and runtime > 0) else None
    plot_summary = details.get("overview")
    poster_path = details.get("poster_path")
    poster_url = f"{TMDB_IMAGE_BASE}{poster_path}" if poster_path else None

    original_language = details.get("original_language")
    media_type = "movie"
    tagline = details.get("tagline")
    status = details.get("status")

    return {
        "imdb_id": imdb_id,
        "tmdb_id": tmdb_id,
        "title": title,
        "release_year": release_year,
        "mpaa_rating": mpaa_rating,
        "runtime_minutes": runtime_minutes,
        "plot_summary": plot_summary,
        "poster_url": poster_url,
        "original_language": original_language,
        "media_type": media_type,
        "tagline": tagline,
        "status": status,
    }


def render_insert(row: Dict[str, Any]) -> str:
    runtime = row.get("runtime_minutes")
    if runtime is not None:
        try:
            runtime_int = int(runtime)
            if runtime_int <= 0:
                runtime = None
            else:
                runtime = runtime_int
        except (TypeError, ValueError):
            runtime = None

    return (
        "INSERT INTO movies (\n"
        "  imdb_id, tmdb_id, title, release_year,\n"
        "  mpaa_rating, runtime_minutes, plot_summary, poster_url,\n"
        "  original_language, media_type, tagline, status\n"
        ") VALUES (\n"
        f"  {sql_literal(row['imdb_id'])}, {sql_literal(row['tmdb_id'])}, {sql_literal(row['title'])}, {sql_literal(row['release_year'])},\n"
        f"  {sql_literal(row['mpaa_rating'])}, {sql_literal(runtime)}, {sql_literal(row['plot_summary'])}, {sql_literal(row['poster_url'])},\n"
        f"  {sql_literal(row['original_language'])}, {sql_literal(row['media_type'])}, {sql_literal(row['tagline'])}, {sql_literal(row['status'])}\n"
        ")\n"
        "ON CONFLICT (imdb_id) DO NOTHING;\n"
    )


# -----------------------------
# Main
# -----------------------------
def main() -> None:
    _require_env()

    print(f"Collecting {TARGET_UNIQUE} unique movies (US discover)...")

    seen_imdb_ids = set()
    unique_rows: List[Dict[str, Any]] = []

    skipped_no_imdb = 0
    skipped_dup_imdb = 0
    scanned = 0

    for page in range(1, MAX_PAGES_TO_SCAN + 1):
        results = get_discover_page(page)
        if not results:
            break

        for item in results:
            tmdb_id = item.get("id")
            if not tmdb_id:
                continue

            scanned += 1

            # Fetch full details (includes imdb_id, runtime, tagline, status, etc.)
            details = get_movie_details(int(tmdb_id))

            row = build_movie_row(details)
            imdb_id = row.get("imdb_id")

            if not imdb_id:
                skipped_no_imdb += 1
                continue

            if imdb_id in seen_imdb_ids:
                skipped_dup_imdb += 1
                continue

            seen_imdb_ids.add(imdb_id)
            unique_rows.append(row)

            if len(unique_rows) >= TARGET_UNIQUE:
                break

        if len(unique_rows) >= TARGET_UNIQUE:
            break

    if len(unique_rows) < TARGET_UNIQUE:
        print(
            f"Warning: only collected {len(unique_rows)} unique movies "
            f"after scanning {page} page(s)."
        )

    # Write SQL file
    OUTPUT_SQL_PATH.parent.mkdir(parents=True, exist_ok=True)

    sql_lines: List[str] = []
    sql_lines.append("BEGIN;\n\n")
    for row in unique_rows:
        sql_lines.append(render_insert(row))
        sql_lines.append("\n")
    sql_lines.append("COMMIT;\n")

    OUTPUT_SQL_PATH.write_text("".join(sql_lines), encoding="utf-8")

    print(f"Collected {len(unique_rows)} unique movies (US discover).")
    print(f"Scanned {scanned} candidates from TMDB discover.")
    print(f"Skipped (no imdb_id): {skipped_no_imdb}")
    print(f"Skipped (duplicate imdb_id): {skipped_dup_imdb}")
    print(f"Saved: {OUTPUT_SQL_PATH.resolve()}")


if __name__ == "__main__":
    main()