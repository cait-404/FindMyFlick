from __future__ import annotations
from pathlib import Path
from dotenv import load_dotenv
import os
import time
import requests
import psycopg

ROOT = Path(__file__).resolve().parents[2]
load_dotenv(ROOT / ".env")

TMDB_API_KEY  = os.getenv("TMDB_API_KEY")
DB_CONN_STR   = os.getenv("DB_CONN_STR", "host=localhost port=5432 dbname=findmyflick user=postgres password=p@ssw0rd")
SLEEP_SECONDS = 0.25

if not TMDB_API_KEY:
    raise SystemExit("Missing TMDB_API_KEY env var. Add it to .env, then rerun.")


def fetch_keywords(tmdb_id: int) -> list[dict]:
    """Returns a list of dicts with keyword_id and keyword_name."""
    url    = f"https://api.themoviedb.org/3/movie/{tmdb_id}/keywords"
    params = {"api_key": TMDB_API_KEY}

    try:
        r = requests.get(url, params=params, timeout=30)
        r.raise_for_status()
        data = r.json()
    except Exception as e:
        print(f"  ERROR fetching keywords for tmdb_id={tmdb_id}: {e}")
        return []

    keywords = []
    for k in data.get("keywords", []):
        kid   = k.get("id")
        kname = (k.get("name") or "").strip()
        if kid and kname:
            keywords.append({"keyword_id": kid, "keyword_name": kname})

    return keywords


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT imdb_id, tmdb_id
                FROM public.movies
                WHERE tmdb_id IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM public.movie_keywords mk
                      WHERE mk.imdb_id = movies.imdb_id
                  )
                ORDER BY imdb_id;
            """)
            rows = cur.fetchall()

    print(f"Movies missing keywords: {len(rows)}")

    updated = 0
    skipped = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, (imdb_id, tmdb_id) in enumerate(rows, start=1):
                keywords = fetch_keywords(tmdb_id)

                if not keywords:
                    print(f"[{i}/{len(rows)}] {imdb_id}: no keywords found")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                # Remove existing keywords for this movie
                cur.execute(
                    "DELETE FROM public.movie_keywords WHERE imdb_id = %s;",
                    (imdb_id,)
                )

                inserted = 0
                for k in keywords:
                    # Upsert the keyword itself
                    cur.execute("""
                        INSERT INTO public.keywords
                            (tmdb_keyword_id, keyword_name, created_at, updated_at)
                        VALUES (%s, %s, now(), now())
                        ON CONFLICT (tmdb_keyword_id) DO UPDATE
                            SET keyword_name = EXCLUDED.keyword_name,
                                updated_at   = now();
                    """, (k["keyword_id"], k["keyword_name"]))

                    # Insert the movie-keyword link
                    cur.execute("""
                        INSERT INTO public.movie_keywords
                            (imdb_id, tmdb_keyword_id, created_at)
                        VALUES (%s, %s, now())
                        ON CONFLICT (imdb_id, tmdb_keyword_id) DO NOTHING;
                    """, (imdb_id, k["keyword_id"]))
                    inserted += 1

                print(f"[{i}/{len(rows)}] {imdb_id}: {inserted} keywords")
                updated += 1
                time.sleep(SLEEP_SECONDS)

        conn.commit()

    print()
    print(f"Updated:         {updated}")
    print(f"Skipped/no data: {skipped}")


if __name__ == "__main__":
    main()