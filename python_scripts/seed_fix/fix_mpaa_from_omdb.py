from __future__ import annotations

from pathlib import Path
from dotenv import load_dotenv

import os
import time
import requests
import psycopg

ROOT = Path(__file__).resolve().parents[2]  # seed_fix -> python_scripts -> repo root
load_dotenv(ROOT / ".env")

OMDB_API_KEY = os.getenv("OMDB_API_KEY")
DB_CONN_STR = "host=localhost port=5432 dbname=findmyflick user=postgres"

if not OMDB_API_KEY:
    raise SystemExit("Missing OMDB_API_KEY env var. Add it to .env, then rerun.")


def fetch_omdb_rating(imdb_id: str) -> str | None:
    url = "https://www.omdbapi.com/"
    params = {"apikey": OMDB_API_KEY, "i": imdb_id}
    headers = {"User-Agent": "FindMyFlick/seed-fix (mpaa backfill)"}

    r = requests.get(url, params=params, headers=headers, timeout=30)
    r.raise_for_status()
    data = r.json()

    if data.get("Response") != "True":
        return None

    rated = data.get("Rated")
    if not rated or rated == "N/A":
        return None

    rated = rated.strip()
    # optional: normalize weird whitespace
    rated = " ".join(rated.split())
    return rated


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

                if not rated:
                    print(f"[{i}/{len(imdb_ids)}] {imdb_id}: no rating")
                    skipped += 1
                else:
                    cur.execute(
                        """
                        UPDATE public.movies
                        SET mpaa_rating = %s,
                            updated_at = now()
                        WHERE imdb_id = %s;
                        """,
                        (rated, imdb_id),
                    )
                    updated += cur.rowcount
                    print(f"[{i}/{len(imdb_ids)}] {imdb_id}: set mpaa_rating = {rated}")

                time.sleep(0.25)

        conn.commit()

    print()
    print(f"Updated: {updated}")
    print(f"Skipped/no rating/errors: {skipped}")


if __name__ == "__main__":
    main()