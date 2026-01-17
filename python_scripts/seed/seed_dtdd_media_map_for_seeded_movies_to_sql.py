"""
seed_dtdd_media_map_for_seeded_movies_to_sql.py
Purpose:
  Populate movie_dtdd_titles with DTDD media ids using match priority:
    1) IMDb match via dddsearch?imdb= (tt or numeric)
    2) TMDB match by filtering dddsearch?q=title results where tmdbId matches
    3) Title + release year match (STRICT year match)
  Also: ALWAYS write one row per movie (matched or unmatched) so you can spot-check.
"""

import os
import re
import time
import json
from difflib import SequenceMatcher
from pathlib import Path

import requests
from dotenv import load_dotenv
import psycopg

load_dotenv(Path.cwd() / ".env")

DTDD_BASE = "https://www.doesthedogdie.com"
HEADERS = {
    "Accept": "application/json",
    "X-API-KEY": os.environ.get("DTDD_API_KEY", ""),
}
if not HEADERS["X-API-KEY"]:
    raise SystemExit("DTDD_API_KEY is not set in the environment.")

DB_HOST = os.environ.get("DB_HOST", "localhost")
DB_PORT = int(os.environ.get("DB_PORT", "5432"))
DB_NAME = os.environ.get("DB_NAME", "findmyflick")
DB_USER = os.environ.get("DB_USER", "postgres")
DB_PASS = os.environ.get("DB_PASS", "")

OUT_SQL = os.path.join("database", "seed", "009_seed_us_streamable_dtdd_media_map.sql")
OUT_LOG = os.path.join("database", "seed", "009_seed_us_streamable_dtdd_media_map.log.json")


def imdb_numeric(imdb_id: str) -> str:
    # tt31193180 -> 31193180
    return re.sub(r"^tt", "", (imdb_id or "").strip())


def sim(a: str, b: str) -> float:
    return SequenceMatcher(None, (a or "").lower().strip(), (b or "").lower().strip()).ratio()


def as_int(v):
    try:
        return int(v)
    except Exception:
        return None


def is_movie_item(it: dict) -> bool:
    # DTDD responses can use ItemTypeId or itemTypeId. Movie in your examples is 15.
    item_type = it.get("ItemTypeId")
    if item_type is None:
        item_type = it.get("itemTypeId")
    item_type_i = as_int(item_type)
    # If missing, allow it. Some items omit it.
    return True if item_type_i is None else (item_type_i == 15)


def dtdd_search_imdb(imdb_id: str) -> list[dict]:
    url = f"{DTDD_BASE}/dddsearch?imdb={imdb_id}"
    r = requests.get(url, headers=HEADERS, timeout=30)
    r.raise_for_status()
    return (r.json() or {}).get("items", []) or []


def dtdd_search_title(title: str) -> list[dict]:
    url = f"{DTDD_BASE}/dddsearch?q={requests.utils.quote(title)}"
    r = requests.get(url, headers=HEADERS, timeout=30)
    r.raise_for_status()
    return (r.json() or {}).get("items", []) or []


def pick_imdb_match(items: list[dict], title: str) -> tuple[dict | None, float]:
    """
    IMDb path:
      If DTDD returns any items, pick the best Movie-ish item by title similarity.
      Year mismatch is allowed here.
    """
    if not items:
        return None, 0.0

    best = None
    best_score = -1.0
    for it in items:
        if not is_movie_item(it):
            continue
        title_score = sim(title, it.get("name", ""))
        score = 10.0 + title_score  # mark this as strong in logs
        if score > best_score:
            best_score = score
            best = it

    return best, float(best_score) if best else (None, 0.0)


def pick_tmdb_from_title_search(items: list[dict], tmdb_id: int, title: str) -> tuple[dict | None, float]:
    """
    TMDB path:
      Only accept items where it.tmdbId == tmdb_id (ignore year).
      If multiple, pick best title similarity.
    """
    matches = []
    for it in items or []:
        if not is_movie_item(it):
            continue
        if it.get("tmdbId") == tmdb_id:
            matches.append(it)

    if not matches:
        return None, 0.0

    best = None
    best_score = -1.0
    for it in matches:
        title_score = sim(title, it.get("name", ""))
        score = 8.0 + title_score
        if score > best_score:
            best_score = score
            best = it

    return best, float(best_score)


def pick_title_year_fallback(items: list[dict], title: str, year: int) -> tuple[dict | None, float, str | None]:
    """
    Title+Year fallback:
      Only accept items with DTDD releaseYear present AND exactly equal to year.
      If there are candidates but none match year, return a reason.
    """
    if not items:
        return None, 0.0, None

    year_matches = []
    any_year_present = False

    for it in items:
        if not is_movie_item(it):
            continue

        it_year_i = as_int(it.get("releaseYear"))
        if it_year_i is not None:
            any_year_present = True

        if it_year_i == year:
            year_matches.append(it)

    if not year_matches:
        # If DTDD returned candidates but none match the year, flag it explicitly.
        if any_year_present:
            return None, 0.0, "unmatched_year_mismatch"
        return None, 0.0, "unmatched_missing_dtdd_year"

    best = None
    best_score = -1.0
    for it in year_matches:
        title_score = sim(title, it.get("name", ""))
        score = 6.0 + title_score
        if score > best_score:
            best_score = score
            best = it

    return best, float(best_score), None


def fetch_movies() -> list[dict]:
    sql = """
    SELECT imdb_id, tmdb_id, title, release_year
    FROM public.v_movies_streamable_us_agg
    ORDER BY title;
    """
    with psycopg.connect(
        host=DB_HOST,
        port=DB_PORT,
        user=DB_USER,
        password=DB_PASS,
        dbname=DB_NAME,
    ) as conn:
        with conn.cursor() as cur:
            cur.execute(sql)
            rows = cur.fetchall()

    return [
        {"imdb_id": r[0], "tmdb_id": r[1], "title": r[2], "release_year": r[3]}
        for r in rows
    ]


def sql_escape(s: str) -> str:
    return (s or "").replace("'", "''")


def main():
    movies = fetch_movies()
    out_lines = []
    log = {"matched": [], "unmatched": []}

    out_lines.append("/*")
    out_lines.append("009_seed_us_streamable_dtdd_media_map.sql")
    out_lines.append("Purpose: Map movies.imdb_id to DTDD media ids for DTDD warnings.")
    out_lines.append("Also inserts placeholder rows for unmatched movies for spot checking.")
    out_lines.append("*/")
    out_lines.append("")
    out_lines.append("\\set ON_ERROR_STOP on")
    out_lines.append("BEGIN;")
    out_lines.append("")

    for m in movies:
        imdb_id = m["imdb_id"]
        tmdb_id = m["tmdb_id"]
        title = m["title"]
        year = m["release_year"]

        picked = None
        match_method = None
        match_score = 0.0

        # 1) IMDb exact match with tt
        items = dtdd_search_imdb(imdb_id)
        if items:
            picked, match_score = pick_imdb_match(items, title)
            if picked:
                match_method = "imdb_tt"

        # 2) IMDb numeric match (if not matched yet)
        if not picked:
            items = dtdd_search_imdb(imdb_numeric(imdb_id))
            if items:
                picked, match_score = pick_imdb_match(items, title)
                if picked:
                    match_method = "imdb_numeric"

        # 3) Title search (if still not matched)
        title_items = None
        if not picked:
            title_items = dtdd_search_title(title)

            # 3a) TMDB from title search
            if tmdb_id is not None:
                picked, match_score = pick_tmdb_from_title_search(title_items, int(tmdb_id), title)
                if picked:
                    match_method = "tmdb_from_search"

            # 3b) Title + year fallback (strict)
            if (not picked) and (year is not None):
                picked, match_score, reason = pick_title_year_fallback(title_items, title, int(year))
                if picked:
                    match_method = "title_year_fallback"
                else:
                    if reason:
                        match_method = reason

            # 3c) Still nothing
            if (not picked) and (not match_method):
                # title search returned 0 items, or we rejected them all
                match_method = "unmatched_no_results" if not title_items else "unmatched_rejected"

        # If we matched, fill DTDD fields. Otherwise insert NULL dtdd_* fields.
        if picked:
            dtdd_media_id = picked.get("id")
            dtdd_title = sql_escape(picked.get("name") or "")
            dtdd_release_year_i = as_int(picked.get("releaseYear"))
            dtdd_release_year_sql = str(dtdd_release_year_i) if dtdd_release_year_i is not None else "NULL"
            dtdd_imdb_id = sql_escape(picked.get("imdbId") or "")
            dtdd_tmdb_id = picked.get("tmdbId")
            dtdd_tmdb_sql = str(dtdd_tmdb_id) if dtdd_tmdb_id is not None else "NULL"
        else:
            dtdd_media_id = "NULL"
            dtdd_title = ""
            dtdd_release_year_sql = "NULL"
            dtdd_imdb_id = ""
            dtdd_tmdb_sql = "NULL"
            match_score = 0.0

        out_lines.append(
            f"""INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('{sql_escape(imdb_id)}', {dtdd_media_id if dtdd_media_id != "NULL" else "NULL"}, '{dtdd_imdb_id}', {dtdd_tmdb_sql}, '{dtdd_title}', {dtdd_release_year_sql}, '{sql_escape(match_method or "unmatched")}', {match_score:.3f}, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();"""
        )

        if picked:
            log["matched"].append(
                {
                    "imdb_id": imdb_id,
                    "tmdb_id": tmdb_id,
                    "title": title,
                    "release_year": year,
                    "dtdd_media_id": None if dtdd_media_id == "NULL" else dtdd_media_id,
                    "dtdd_tmdb_id": None if dtdd_tmdb_sql == "NULL" else int(dtdd_tmdb_sql),
                    "dtdd_imdb_id": dtdd_imdb_id,
                    "dtdd_title": picked.get("name"),
                    "dtdd_release_year": picked.get("releaseYear"),
                    "match_method": match_method,
                    "match_score": round(match_score, 3),
                }
            )
        else:
            log["unmatched"].append(
                {
                    "imdb_id": imdb_id,
                    "tmdb_id": tmdb_id,
                    "title": title,
                    "release_year": year,
                    "match_method": match_method,
                    "candidates_returned": 0 if not title_items else len(title_items),
                }
            )

        # Be nice to DTDD
        time.sleep(0.20)

    out_lines.append("")
    out_lines.append("COMMIT;")
    out_lines.append("")

    os.makedirs(os.path.dirname(OUT_SQL), exist_ok=True)
    with open(OUT_SQL, "w", encoding="utf-8") as f:
        f.write("\n".join(out_lines) + "\n")

    with open(OUT_LOG, "w", encoding="utf-8") as f:
        json.dump(log, f, indent=2)

    print(f"Wrote: {OUT_SQL}")
    print(f"Wrote: {OUT_LOG}")
    print(f"Matched: {len(log['matched'])}  Unmatched: {len(log['unmatched'])}")


if __name__ == "__main__":
    main()