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
CREW_LIMIT    = 25

PRIORITY_JOBS = {
    "Director", "Writer", "Screenplay", "Story", "Characters",
    "Producer", "Executive Producer", "Director of Photography",
    "Original Music Composer", "Editor"
}

if not TMDB_API_KEY:
    raise SystemExit("Missing TMDB_API_KEY env var. Add it to .env, then rerun.")


def fetch_credits(tmdb_id: int) -> dict:
    """Returns dict with 'cast' and 'crew' lists."""
    url    = f"https://api.themoviedb.org/3/movie/{tmdb_id}/credits"
    params = {"api_key": TMDB_API_KEY}

    try:
        r = requests.get(url, params=params, timeout=30)
        r.raise_for_status()
        data = r.json()
    except Exception as e:
        print(f"  ERROR fetching credits for tmdb_id={tmdb_id}: {e}")
        return {"cast": [], "crew": []}

    cast = []
    for c in data.get("cast", []):
        pid      = c.get("id")
        name     = (c.get("name") or "").strip()
        credit_id = c.get("credit_id")
        if not pid or not credit_id:
            continue
        cast.append({
            "person_id":  pid,
            "name":       name,
            "character":  c.get("character"),
            "order":      c.get("order"),
            "credit_id":  credit_id,
        })

    crew = []
    for c in data.get("crew", []):
        pid      = c.get("id")
        name     = (c.get("name") or "").strip()
        credit_id = c.get("credit_id")
        if not pid or not credit_id:
            continue
        crew.append({
            "person_id":  pid,
            "name":       name,
            "department": c.get("department"),
            "job":        c.get("job"),
            "credit_id":  credit_id,
        })

    # Sort crew by priority jobs first, then department, then job
    crew.sort(key=lambda c: (
        0 if c.get("job") in PRIORITY_JOBS else 1,
        c.get("department") or "",
        c.get("job") or ""
    ))
    crew = crew[:CREW_LIMIT]

    return {"cast": cast, "crew": crew}


def upsert_people(cur, credits: dict, now: str = "now()") -> None:
    """Upsert all people from cast and crew into the people table."""
    people = {}
    for c in credits["cast"]:
        people[c["person_id"]] = c["name"]
    for c in credits["crew"]:
        people[c["person_id"]] = c["name"]

    for pid, name in people.items():
        if not name:
            continue
        cur.execute("""
            INSERT INTO public.people (tmdb_person_id, person_name, created_at, updated_at)
            VALUES (%s, %s, now(), now())
            ON CONFLICT (tmdb_person_id) DO UPDATE
                SET person_name = EXCLUDED.person_name,
                    updated_at  = now();
        """, (pid, name))


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT imdb_id, tmdb_id
                FROM public.movies
                WHERE tmdb_id IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM public.movie_cast mc
                      WHERE mc.imdb_id = movies.imdb_id
                  )
                  AND NOT EXISTS (
                      SELECT 1 FROM public.movie_crew mc
                      WHERE mc.imdb_id = movies.imdb_id
                  )
                ORDER BY imdb_id;
            """)
            rows = cur.fetchall()

    print(f"Movies missing cast and crew: {len(rows)}")

    updated = 0
    skipped = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, (imdb_id, tmdb_id) in enumerate(rows, start=1):
                credits = fetch_credits(tmdb_id)

                if not credits["cast"] and not credits["crew"]:
                    print(f"[{i}/{len(rows)}] {imdb_id}: no credits found")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                # Upsert people first
                upsert_people(cur, credits)

                # Remove existing cast and crew
                cur.execute("DELETE FROM public.movie_cast  WHERE imdb_id = %s;", (imdb_id,))
                cur.execute("DELETE FROM public.movie_crew WHERE imdb_id = %s;", (imdb_id,))

                # Insert cast
                cast_count = 0
                for c in credits["cast"]:
                    cur.execute("""
                        INSERT INTO public.movie_cast
                            (tmdb_credit_id, imdb_id, tmdb_person_id, character_name, cast_order, created_at)
                        VALUES (%s, %s, %s, %s, %s, now())
                        ON CONFLICT (tmdb_credit_id) DO NOTHING;
                    """, (c["credit_id"], imdb_id, c["person_id"], c["character"], c["order"]))
                    cast_count += 1

                # Insert crew
                crew_count = 0
                for c in credits["crew"]:
                    cur.execute("""
                        INSERT INTO public.movie_crew
                            (tmdb_credit_id, imdb_id, tmdb_person_id, department, job, created_at)
                        VALUES (%s, %s, %s, %s, %s, now())
                        ON CONFLICT (tmdb_credit_id) DO NOTHING;
                    """, (c["credit_id"], imdb_id, c["person_id"], c["department"], c["job"]))
                    crew_count += 1

                print(f"[{i}/{len(rows)}] {imdb_id}: {cast_count} cast, {crew_count} crew")
                updated += 1
                time.sleep(SLEEP_SECONDS)

        conn.commit()

    print()
    print(f"Updated:         {updated}")
    print(f"Skipped/no data: {skipped}")


if __name__ == "__main__":
    main()