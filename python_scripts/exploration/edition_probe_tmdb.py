"""
edition_probe_tmdb.py
---------------------
Purpose:
    Explore how "different editions" of the same movie appear in TMDB.

What it does:
    1) Searches TMDB for two title queries you enter
    2) Lets you pick the correct result for each query (so we don't guess wrong)
    3) Pulls details for each selected TMDB movie id:
        - /movie/{id}
        - /movie/{id}/external_ids
        - /movie/{id}/release_dates
        - /movie/{id}/watch/providers
    4) Saves JSON files under python_scripts/exploration/output/
    5) Prints a small comparison summary in the terminal

Run:
    python python_scripts/exploration/edition_probe_tmdb.py
"""

import os
import json
from datetime import datetime

from python_scripts.shared.schema_utils import load_api_key, fetch_json
from python_scripts.shared.constants import DEFAULT_LANGUAGE, WATCH_REGION


TMDB_BASE = "https://api.themoviedb.org/3"


def _outdir() -> str:
    d = os.path.join(os.path.dirname(__file__), "output")
    os.makedirs(d, exist_ok=True)
    return d


def _write_json(filename: str, data: dict):
    path = os.path.join(_outdir(), filename)
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Saved: {os.path.normpath(path)}")


def tmdb_search_movie(api_key: str, query: str, year: str | None = None) -> list[dict]:
    url = f"{TMDB_BASE}/search/movie"
    params = {
        "api_key": api_key,
        "language": DEFAULT_LANGUAGE,
        "query": query,
        "include_adult": "false",
        "page": 1,
    }
    if year and year.strip():
        params["year"] = year.strip()

    data = fetch_json(url, params=params)
    return data.get("results", []) or []


def pick_result(results: list[dict], label: str) -> dict:
    if not results:
        raise RuntimeError(f"No results returned for {label}.")

    print("")
    print(f"--- Pick the correct match for: {label} ---")
    for i, r in enumerate(results[:10], start=1):
        title = r.get("title")
        release_date = r.get("release_date")
        tmdb_id = r.get("id")
        popularity = r.get("popularity")
        print(f"{i}) id={tmdb_id} | {title} | release_date={release_date} | popularity={popularity}")

    print("")
    while True:
        choice = input("Type the number you want (1-10): ").strip()
        if choice.isdigit():
            idx = int(choice)
            if 1 <= idx <= min(10, len(results)):
                return results[idx - 1]
        print("Not a valid choice. Try again.")


def fetch_bundle(api_key: str, tmdb_id: int) -> dict:
    core = fetch_json(
        f"{TMDB_BASE}/movie/{tmdb_id}",
        params={"api_key": api_key, "language": DEFAULT_LANGUAGE},
    )

    external_ids = fetch_json(
        f"{TMDB_BASE}/movie/{tmdb_id}/external_ids",
        params={"api_key": api_key},
    )

    release_dates = fetch_json(
        f"{TMDB_BASE}/movie/{tmdb_id}/release_dates",
        params={"api_key": api_key},
    )

    watch_providers = fetch_json(
        f"{TMDB_BASE}/movie/{tmdb_id}/watch/providers",
        params={"api_key": api_key},
    )

    return {
        "core": core,
        "external_ids": external_ids,
        "release_dates": release_dates,
        "watch_providers": watch_providers,
    }


def summarize(bundle: dict) -> dict:
    core = bundle["core"]
    ext = bundle["external_ids"]

    imdb_id = ext.get("imdb_id")
    title = core.get("title")
    original_title = core.get("original_title")
    release_date = core.get("release_date")
    runtime = core.get("runtime")
    status = core.get("status")

    # US watch providers summary (if present)
    providers_us = bundle["watch_providers"].get("results", {}).get(WATCH_REGION, {})
    provider_keys = [k for k in ["flatrate", "free", "ads", "rent", "buy"] if k in providers_us]
    provider_summary = {}
    for k in provider_keys:
        provider_summary[k] = [p.get("provider_name") for p in (providers_us.get(k) or [])]

    return {
        "tmdb_id": core.get("id"),
        "title": title,
        "original_title": original_title,
        "release_date": release_date,
        "runtime": runtime,
        "status": status,
        "imdb_id": imdb_id,
        "us_watch_providers": provider_summary,
    }


def main():
    api_key = load_api_key("TMDB_API_KEY")

    print("")
    print("Enter two movie title queries to compare (example: standard vs extended cut).")
    q1 = input("Query 1 (standard): ").strip()
    q2 = input("Query 2 (alternate/extended): ").strip()
    year = input("Optional year (press Enter to skip): ").strip()

    if not q1 or not q2:
        print("You must enter both queries.")
        return

    r1 = tmdb_search_movie(api_key, q1, year=year)
    r2 = tmdb_search_movie(api_key, q2, year=year)

    pick1 = pick_result(r1, q1)
    pick2 = pick_result(r2, q2)

    id1 = int(pick1["id"])
    id2 = int(pick2["id"])

    print("")
    print(f"Fetching TMDB bundles for ids: {id1} and {id2} ...")
    b1 = fetch_bundle(api_key, id1)
    b2 = fetch_bundle(api_key, id2)

    stamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    _write_json(f"{stamp}_tmdb_{id1}_bundle.json", b1)
    _write_json(f"{stamp}_tmdb_{id2}_bundle.json", b2)

    s1 = summarize(b1)
    s2 = summarize(b2)

    print("")
    print("=== Quick comparison ===")
    print(f"1) TMDB {s1['tmdb_id']} | {s1['title']} | {s1['release_date']} | runtime={s1['runtime']} | imdb={s1['imdb_id']}")
    print(f"2) TMDB {s2['tmdb_id']} | {s2['title']} | {s2['release_date']} | runtime={s2['runtime']} | imdb={s2['imdb_id']}")

    print("")
    if s1["imdb_id"] and s2["imdb_id"]:
        if s1["imdb_id"] == s2["imdb_id"]:
            print("IMDB id match: YES (both point to the same IMDb title)")
        else:
            print("IMDB id match: NO (different IMDb titles)")
    else:
        print("IMDB id match: unknown (one or both missing imdb_id)")

    print("")
    print("US watch providers (if present):")
    print("1)", s1["us_watch_providers"])
    print("2)", s2["us_watch_providers"])
    print("")


if __name__ == "__main__":
    main()
