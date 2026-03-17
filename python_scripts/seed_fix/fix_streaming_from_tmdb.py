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
WATCH_REGION  = "US"
SLEEP_SECONDS = 0.25

if not TMDB_API_KEY:
    raise SystemExit("Missing TMDB_API_KEY env var. Add it to .env, then rerun.")


def fetch_watch_providers(tmdb_id: int) -> list[dict]:
    """
    Returns a flat list of provider dicts for the US region.
    Includes all offer types (subscription, free, free_with_ads, rent, buy)
    so the database has complete data. The search layer filters out rent/buy.
    """
    url    = f"https://api.themoviedb.org/3/movie/{tmdb_id}/watch/providers"
    params = {"api_key": TMDB_API_KEY}

    try:
        r = requests.get(url, params=params, timeout=30)
        r.raise_for_status()
        data = r.json()
    except Exception as e:
        print(f"  ERROR fetching providers for tmdb_id={tmdb_id}: {e}")
        return []

    region = data.get("results", {}).get(WATCH_REGION, {})

    bucket_map = {
        "flatrate": "subscription",
        "free":     "free",
        "ads":      "free_with_ads",
        "rent":     "rent",
        "buy":      "buy",
    }

    providers = []
    seen = set()

    for bucket, offer_type in bucket_map.items():
        for p in region.get(bucket, []):
            pid   = p.get("provider_id")
            pname = (p.get("provider_name") or "").strip()
            if not pid or not pname:
                continue
            key = (pid, offer_type)
            if key in seen:
                continue
            seen.add(key)
            providers.append({
                "provider_id":   pid,
                "provider_name": pname,
                "offer_type":    offer_type,
            })

    return providers


def main() -> None:
    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT imdb_id, tmdb_id
                FROM public.movies
                WHERE tmdb_id IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM public.movie_streaming ms
                      WHERE ms.imdb_id = movies.imdb_id
                  )
                ORDER BY imdb_id;
            """)
            rows = cur.fetchall()

    print(f"Movies missing streaming data: {len(rows)}")

    updated = 0
    skipped = 0

    with psycopg.connect(DB_CONN_STR) as conn:
        with conn.cursor() as cur:
            for i, (imdb_id, tmdb_id) in enumerate(rows, start=1):
                providers = fetch_watch_providers(tmdb_id)

                if not providers:
                    print(f"[{i}/{len(rows)}] {imdb_id}: no providers found")
                    skipped += 1
                    time.sleep(SLEEP_SECONDS)
                    continue

                for p in providers:
                    # Upsert the provider record
                    cur.execute("""
                        INSERT INTO public.streaming_providers
                            (tmdb_provider_id, provider_name, created_at, updated_at)
                        VALUES (%s, %s, now(), now())
                        ON CONFLICT (tmdb_provider_id) DO UPDATE
                            SET provider_name = EXCLUDED.provider_name,
                                updated_at    = now();
                    """, (p["provider_id"], p["provider_name"]))

                    # Insert the streaming link
                    cur.execute("""
                        INSERT INTO public.movie_streaming
                            (imdb_id, tmdb_provider_id, offer_type, created_at)
                        VALUES (%s, %s, %s, now())
                        ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;
                    """, (imdb_id, p["provider_id"], p["offer_type"]))

                provider_names = ", ".join(sorted(set(p["provider_name"] for p in providers)))
                print(f"[{i}/{len(rows)}] {imdb_id}: {provider_names}")
                updated += 1
                time.sleep(SLEEP_SECONDS)

        conn.commit()

    print()
    print(f"Updated:         {updated}")
    print(f"Skipped/no data: {skipped}")


if __name__ == "__main__":
    main()