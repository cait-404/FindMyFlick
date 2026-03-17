"""
fix_genres_from_tmdb.py
-----------------------
Purpose:
  Backfill missing genre links for movies already in the database that
  have no entries in movie_genres. Fetches genres from TMDB and writes
  directly to public.genres and public.movie_genres.

Run:
  python -m python_scripts.seed_fix.fix_genres_from_tmdb
"""

from __future__ import annotations

import os
import time
from pathlib import Path

import requests
import psycopg

try:
    from dotenv import load_dotenv
    load_dotenv(Path(__file__).resolve().parents[2] / ".env")
except Exception:
    pass

TMDB_API_KEY = os.getenv("TMDB_API_KEY")
DB_CONN_STR  = "host=localhost port=5432 dbname=findmyflick user=postgres password=p@ssw0rd"

TMDB_BASE    = "https://api.themoviedb.org/3"
SLEEP_SECONDS = 0.25


if not TMDB_API_KEY:
    raise SystemExit("Missing TMDB_API_KEY env var.")


def fetch_tmdb_genres(tmdb_id: int) -> list[tuple[int, str]]:
    """Return list of (tmdb_genre_id, genre_name) for a movie."""
    url = f"{TMDB_BASE}/movie/{tmdb_id}"
    try:
        r = requests.get(url, params={"api_key": TMDB_API_KEY, "language": "en-US"}, timeout=30)
        r.raise_for_status()
        data = r.json()
        genres = data.get("genres") or []
        return [(g["id"], g["name"].strip()) for g in genres if g.get("id") and g.get("name")]
    except Exception as e:
        print(f"  ERROR fetching tmdb_id={tmdb_id}: {e}")
        return []


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT imdb_id, tmdb_id
                FROM public.movies
                WHERE tmdb_id IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM public.movie_genres mg
                      WHERE mg.imdb_id = movies.imdb_id
                  )
                ORDER BY imdb_id;
            """)
            rows = cur.fetchall()

    print(f"Movies missing genre links: {len(rows)}")

    updated   = 0
    skipped   = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, (imdb_id, tmdb_id) in enumerate(rows, start=1):
                genres = fetch_tmdb_genres(tmdb_id)

                if not genres:
                    print(f"[{i}/{len(rows)}] {imdb_id}: no genres found")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                for gid, gname in genres:
                    # Upsert the genre itself
                    cur.execute("""
                        INSERT INTO public.genres (tmdb_genre_id, genre_name)
                        VALUES (%s, %s)
                        ON CONFLICT (tmdb_genre_id) DO UPDATE
                            SET genre_name = EXCLUDED.genre_name,
                                updated_at = now();
                    """, (gid, gname))

                    # Insert the movie-genre link
                    cur.execute("""
                        INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
                        VALUES (%s, %s)
                        ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
                    """, (imdb_id, gid))

                genre_names = ", ".join(g[1] for g in genres)
                print(f"[{i}/{len(rows)}] {imdb_id}: {genre_names}")
                updated += 1
                time.sleep(SLEEP_SECONDS)

        conn.commit()

    print()
    print(f"Updated: {updated}")
    print(f"Skipped/no genres: {skipped}")


if __name__ == "__main__":
    main()
