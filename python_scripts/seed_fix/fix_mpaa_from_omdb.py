from __future__ import annotations
from pathlib import Path
from dotenv import load_dotenv
import os
import sys
import time
import requests
import psycopg

ROOT = Path(__file__).resolve().parents[2]
load_dotenv(ROOT / ".env")

OMDB_API_KEY = os.getenv("OMDB_API_KEY")
DB_CONN_STR  = os.getenv("DB_CONN_STR", "host=localhost port=5432 dbname=findmyflick user=postgres password=p@ssw0rd")

if not OMDB_API_KEY:
    raise SystemExit("Missing OMDB_API_KEY env var. Add it to .env, then rerun.")

MARK_UNRATED = "--mark-unrated" in sys.argv


def normalize_rating(rated: str) -> str | None:
    """
    Normalize an OMDB rating string to a standard MPAA value.
    Returns None only when the caller should decide (i.e. no rating found at all).
    Legacy mappings:
      GP, M/PG, M  -> PG  (pre-1972 MPAA ratings)
      X            -> NC-17 (pre-1990 MPAA rating)
      AO, 13+, etc -> Not Rated (non-US or non-standard ratings)
    """
    r = rated.strip().upper()

    if r in ("NOT RATED", "UNRATED", "NR", "N/A", "PASSED", "APPROVED"):
        return "Not Rated"
    if r == "G":
        return "G"
    if r in ("PG", "GP", "M/PG", "M"):
        return "PG"
    if r == "PG-13":
        return "PG-13"
    if r == "R":
        return "R"
    if r in ("NC-17", "X"):
        return "NC-17"
    if r.startswith("TV-"):
        return rated.strip()

    # Anything else (AO, 13+, foreign ratings, etc.) -> Not Rated
    return "Not Rated"


def fetch_omdb_rating(imdb_id: str) -> str | None:
    """
    Fetch MPAA rating from OMDB.
    Returns a normalized rating string, or None if OMDB had no data at all
    (so the caller can decide whether to write 'Not Rated' or skip).
    """
    url = "https://www.omdbapi.com/"
    params = {"apikey": OMDB_API_KEY, "i": imdb_id}
    headers = {"User-Agent": "FindMyFlick/seed-fix (mpaa backfill)"}

    r = requests.get(url, params=params, headers=headers, timeout=30)
    r.raise_for_status()
    data = r.json()

    if data.get("Response") != "True":
        return None

    rated = data.get("Rated")
    if not rated:
        return None

    return normalize_rating(rated)


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT imdb_id
                FROM public.movies
                WHERE mpaa_rating IS NULL
                ORDER BY imdb_id;
            """)
            imdb_ids = [row[0] for row in cur.fetchall()]

    print(f"Movies needing MPAA backfill: {len(imdb_ids)}")
    if MARK_UNRATED:
        print("Mode: marking unresolved movies as 'Not Rated'")
    else:
        print("Mode: skipping movies where OMDB returns no rating")
        print("      Run with --mark-unrated to fill remaining nulls as 'Not Rated'")

    updated = 0
    skipped = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, imdb_id in enumerate(imdb_ids, start=1):
                try:
                    rated = fetch_omdb_rating(imdb_id)
                except Exception as e:
                    print(f"[{i}/{len(imdb_ids)}] {imdb_id}: ERROR {e}")
                    skipped += 1
                    time.sleep(0.25)
                    continue

                if rated is None:
                    if MARK_UNRATED:
                        rated = "Not Rated"
                    else:
                        print(f"[{i}/{len(imdb_ids)}] {imdb_id}: no rating — skipping")
                        skipped += 1
                        time.sleep(0.25)
                        continue

                cur.execute(
                    """
                    UPDATE public.movies
                    SET mpaa_rating = %s,
                        updated_at  = now()
                    WHERE imdb_id = %s;
                    """,
                    (rated, imdb_id),
                )
                updated += cur.rowcount
                print(f"[{i}/{len(imdb_ids)}] {imdb_id}: set mpaa_rating = {rated}")
                time.sleep(0.25)

        conn.commit()

    print()
    print(f"Updated:        {updated}")
    print(f"Skipped/errors: {skipped}")


if __name__ == "__main__":
    main()