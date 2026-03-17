from __future__ import annotations
from pathlib import Path
from dotenv import load_dotenv
import os
import time
import requests
import psycopg

ROOT = Path(__file__).resolve().parents[2]
load_dotenv(ROOT / ".env")

DTDD_API_KEY = os.getenv("DTDD_API_KEY")
DB_CONN_STR  = os.getenv("DB_CONN_STR", "host=localhost port=5432 dbname=findmyflick user=postgres password=p@ssw0rd")
SLEEP_SECONDS = 0.25

if not DTDD_API_KEY:
    raise SystemExit("Missing DTDD_API_KEY env var. Add it to .env, then rerun.")


def fetch_dtdd_media_id(imdb_id: str, tmdb_id: int | None, title: str, release_year: int) -> int | None:
    headers = {"Accept": "application/json", "X-API-KEY": DTDD_API_KEY}

    # Try by IMDb ID first
    try:
        r = requests.get(
            f"https://www.doesthedogdie.com/dddsearch?imdb={imdb_id}",
            headers=headers, timeout=30)
        if r.status_code == 200 and "application/json" in r.headers.get("Content-Type", ""):
            items = r.json().get("items", [])
            if items:
                return items[0].get("id")
    except Exception:
        pass

    time.sleep(SLEEP_SECONDS)

    # Try by TMDB ID
    if tmdb_id:
        try:
            r = requests.get(
                f"https://www.doesthedogdie.com/dddsearch?tmdb={tmdb_id}",
                headers=headers, timeout=30)
            if r.status_code == 200 and "application/json" in r.headers.get("Content-Type", ""):
                items = r.json().get("items", [])
                if items:
                    return items[0].get("id")
        except Exception:
            pass

    time.sleep(SLEEP_SECONDS)

    # Try by title + year
    try:
        r = requests.get(
            f"https://www.doesthedogdie.com/dddsearch?q={requests.utils.quote(title)}",
            headers=headers, timeout=30)
        if r.status_code == 200 and "application/json" in r.headers.get("Content-Type", ""):
            for item in r.json().get("items", []):
                if item.get("releaseYear") == release_year:
                    return item.get("id")
            items = r.json().get("items", [])
            if items:
                return items[0].get("id")
    except Exception:
        pass

    return None


def fetch_dtdd_warnings(dtdd_media_id: int) -> list[dict]:
    headers = {"Accept": "application/json", "X-API-KEY": DTDD_API_KEY}
    try:
        r = requests.get(
            f"https://www.doesthedogdie.com/media/{dtdd_media_id}",
            headers=headers, timeout=30)
        r.raise_for_status()
        data = r.json()
    except Exception as e:
        print(f"  ERROR fetching warnings for dtdd_id={dtdd_media_id}: {e}")
        return []

    warnings = []
    for s in data.get("topicItemStats", []):
        topic_id = s.get("TopicId")
        if not topic_id:
            continue

        yes_sum = s.get("yesSum")
        no_sum  = s.get("noSum")
        is_yes  = s.get("isYes")

        if yes_sum is not None or no_sum is not None:
            y = yes_sum or 0
            n = no_sum  or 0
            if y == 0 and n == 0:
                answer = "unknown"
            else:
                answer = "yes" if (y >= n and y > 0) else "no"
        elif is_yes is not None:
            answer = "yes" if is_yes == 1 else "no"
        else:
            answer = "unknown"

        is_spoiler = None
        topic = s.get("topic", {})
        if isinstance(topic, dict):
            is_spoiler = topic.get("isSpoiler")

        warnings.append({
            "topic_id":   topic_id,
            "answer":     answer,
            "is_spoiler": is_spoiler,
            "comment":    s.get("comment"),
        })

    return warnings


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            # Get all known topic IDs so we only insert warnings we recognize
            cur.execute("SELECT dtdd_topic_id FROM public.warnings;")
            known_topic_ids = {row[0] for row in cur.fetchall()}

            cur.execute("""
                SELECT imdb_id, tmdb_id, title, release_year
                FROM public.movies
                WHERE NOT EXISTS (
                    SELECT 1 FROM public.movie_warnings mw
                    WHERE mw.imdb_id = movies.imdb_id
                )
                ORDER BY imdb_id;
            """)
            rows = cur.fetchall()

    print(f"Movies missing warnings: {len(rows)}")

    updated = 0
    skipped = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, (imdb_id, tmdb_id, title, release_year) in enumerate(rows, start=1):
                dtdd_id = fetch_dtdd_media_id(imdb_id, tmdb_id, title, release_year)

                if not dtdd_id:
                    print(f"[{i}/{len(rows)}] {imdb_id}: could not find DTDD media ID")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                warnings = fetch_dtdd_warnings(dtdd_id)

                if not warnings:
                    print(f"[{i}/{len(rows)}] {imdb_id}: no warnings found")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                # Remove existing warnings first
                cur.execute(
                    "DELETE FROM public.movie_warnings WHERE imdb_id = %s;",
                    (imdb_id,)
                )

                inserted = 0
                for w in warnings:
                    if w["topic_id"] not in known_topic_ids:
                        continue
                    cur.execute("""
                        INSERT INTO public.movie_warnings
                            (imdb_id, dtdd_topic_id, answer, is_spoiler, warning_comment, created_at, updated_at)
                        VALUES (%s, %s, %s, %s, %s, now(), now())
                        ON CONFLICT (imdb_id, dtdd_topic_id) DO UPDATE
                            SET answer          = EXCLUDED.answer,
                                is_spoiler      = EXCLUDED.is_spoiler,
                                warning_comment = EXCLUDED.warning_comment,
                                updated_at      = now();
                    """, (imdb_id, w["topic_id"], w["answer"], w["is_spoiler"], w["comment"]))
                    inserted += 1

                print(f"[{i}/{len(rows)}] {imdb_id}: inserted {inserted} warnings")
                updated += 1
                time.sleep(SLEEP_SECONDS)

        conn.commit()

    print()
    print(f"Updated:          {updated}")
    print(f"Skipped/no data:  {skipped}")


if __name__ == "__main__":
    main()