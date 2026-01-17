"""
seed_dtdd_warnings_for_seeded_movies_to_sql.py
----------------------------------------------
Purpose:
  For the seeded movies (from 001_seed_us_popular_100_movies.sql), call DoesTheDogDie API:
    - Resolve DTDD item id using imdb id search
    - Fetch /media/{itemId} to get topic stats
  Then generate a seed SQL file to populate:
    - movie_dtdd_titles
    - warnings (upsert any topics not already present)
    - movie_warnings (per-movie answers)

Output:
  database/seed/008_seed_us_popular_100_movie_warnings.sql

Assumptions:
  - DTDD_API_KEY is available via .env (schema_utils.load_api_key handles this)
  - TMDB seed already ran and movies exist in DB
  - warnings table already exists (topics catalog), but script can upsert missing topics

Run:
  python -m python_scripts.seed.seed_dtdd_warnings_for_seeded_movies_to_sql
"""

from __future__ import annotations

from pathlib import Path
from datetime import datetime
from typing import Any, Dict, List, Tuple
import re
import time
import json
import urllib.request

from python_scripts.shared.schema_utils import load_api_key

PROJECT_ROOT = Path(__file__).resolve().parents[2]

MOVIES_SEED_SQL = PROJECT_ROOT / "database" / "seed" / "001_seed_us_popular_100_movies.sql"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "008_seed_us_popular_100_movie_warnings.sql"

DTDD_SEARCH_URL = "https://www.doesthedogdie.com/dddsearch"
DTDD_MEDIA_URL = "https://www.doesthedogdie.com/media"  # /{itemId}

SLEEP_SECONDS = 0.25


def sql_escape(s: str) -> str:
    return s.replace("'", "''")


def sql_literal(v: Any) -> str:
    if v is None:
        return "NULL"
    if isinstance(v, bool):
        return "TRUE" if v else "FALSE"
    if isinstance(v, (int, float)):
        return str(v)
    return f"'{sql_escape(str(v))}'"


def http_get_json(url: str, headers: Dict[str, str]) -> Dict[str, Any]:
    req = urllib.request.Request(url, headers=headers, method="GET")
    with urllib.request.urlopen(req, timeout=30) as resp:
        raw = resp.read()
    return json.loads(raw.decode("utf-8", errors="replace"))


def extract_imdb_ids(seed_sql_path: Path) -> List[str]:
    """
    Extract imdb ids from INSERT statements like:
      ('tt123...', 12345, 'Title', 2025, ...)
    """
    text = seed_sql_path.read_text(encoding="utf-8", errors="replace")
    pattern = re.compile(r"\(\s*'(?P<imdb>tt\d+)'\s*,", re.MULTILINE)
    ids = [m.group("imdb") for m in pattern.finditer(text)]

    # de-dupe keep order
    seen = set()
    out: List[str] = []
    for imdb_id in ids:
        if imdb_id in seen:
            continue
        seen.add(imdb_id)
        out.append(imdb_id)
    return out


def pick_best_dtdd_item(items: List[Dict[str, Any]]) -> Dict[str, Any] | None:
    """
    Prefer Movie itemTypeId=15 when available.
    If multiple, pick the one with a tmdbId when present, else first.
    """
    if not items:
        return None

    movies = [i for i in items if i.get("itemTypeId") == 15]
    pool = movies if movies else items

    with_tmdb = [i for i in pool if i.get("tmdbId") is not None]
    return with_tmdb[0] if with_tmdb else pool[0]


def majority_answer(yes_sum: Any, no_sum: Any) -> str:
    try:
        y = int(yes_sum or 0)
        n = int(no_sum or 0)
    except Exception:
        return "unknown"

    if y > n:
        return "yes"
    if n > y:
        return "no"
    return "unknown"


def main() -> None:
    dtdd_key = load_api_key("DTDD_API_KEY")

    if not MOVIES_SEED_SQL.exists():
        raise RuntimeError(f"Movies seed SQL not found: {MOVIES_SEED_SQL}")

    imdb_ids = extract_imdb_ids(MOVIES_SEED_SQL)
    print(f"Found {len(imdb_ids)} imdb ids in: {MOVIES_SEED_SQL.name}")

    headers = {"Accept": "application/json", "X-API-KEY": dtdd_key}

    # Collect rows
    dtdd_titles: List[Dict[str, Any]] = []
    warnings_by_id: Dict[int, Dict[str, Any]] = {}
    movie_warning_rows: List[Dict[str, Any]] = []

    for imdb_id in imdb_ids:
        # 1) Resolve DTDD item id via imdb search
        search_url = f"{DTDD_SEARCH_URL}?imdb={imdb_id}"
        search = http_get_json(search_url, headers=headers)
        items = search.get("items") or []
        best = pick_best_dtdd_item([i for i in items if isinstance(i, dict)])

        if not best or not isinstance(best.get("id"), int):
            # no DTDD record found for this imdb id
            time.sleep(SLEEP_SECONDS)
            continue

        dtdd_item_id = int(best["id"])
        dtdd_titles.append(
            {
                "imdb_id": imdb_id,
                "dtdd_title_id": dtdd_item_id,
                "match_method": "imdb",
            }
        )

        # 2) Fetch media details (topic stats)
        media_url = f"{DTDD_MEDIA_URL}/{dtdd_item_id}"
        media = http_get_json(media_url, headers=headers)
        stats = media.get("topicItemStats") or []

        for s in stats:
            if not isinstance(s, dict):
                continue

            topic = s.get("topic") or {}
            if not isinstance(topic, dict):
                continue

            topic_id = topic.get("id")
            topic_name = topic.get("name")

            if not isinstance(topic_id, int) or not topic_name:
                continue

            warnings_by_id[topic_id] = {
                "dtdd_topic_id": topic_id,
                "topic_name": str(topic_name),
                # optional fields in your table, keep NULL if you don't have them
                "topic_type": None,
                "parent_dtdd_topic_id": None,
                "tier": None,
            }

            answer = majority_answer(s.get("yesSum"), s.get("noSum"))

            # comment is often on the stat record, fall back to first comment if present
            comment = s.get("comment")
            if not comment and isinstance(s.get("comments"), list) and s["comments"]:
                c0 = s["comments"][0]
                if isinstance(c0, dict):
                    comment = c0.get("comment")

            is_spoiler = topic.get("isSpoiler")
            is_spoiler_bool = bool(is_spoiler) if isinstance(is_spoiler, bool) else None

            movie_warning_rows.append(
                {
                    "imdb_id": imdb_id,
                    "dtdd_topic_id": topic_id,
                    "answer": answer,
                    "is_spoiler": is_spoiler_bool,
                    "warning_comment": comment,
                }
            )

        time.sleep(SLEEP_SECONDS)

    print(f"DTDD titles mapped: {len(dtdd_titles)}")
    print(f"Topics collected (unique): {len(warnings_by_id)}")
    print(f"Movie warning rows: {len(movie_warning_rows)}")

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    lines: List[str] = []
    lines.append(f"-- Generated {ts}\nBEGIN;\n\n")

    # Upsert movie_dtdd_titles
    for r in dtdd_titles:
        lines.append(
            "INSERT INTO public.movie_dtdd_titles (\n"
            "  imdb_id, dtdd_title_id, match_method\n"
            ") VALUES (\n"
            f"  {sql_literal(r['imdb_id'])}, {sql_literal(r['dtdd_title_id'])}, {sql_literal(r.get('match_method'))}\n"
            ")\n"
            "ON CONFLICT (imdb_id) DO UPDATE SET\n"
            "  dtdd_title_id = EXCLUDED.dtdd_title_id,\n"
            "  match_method = EXCLUDED.match_method,\n"
            "  updated_at = NOW();\n\n"
        )

    # Upsert warnings (topics catalog)
    for tid in sorted(warnings_by_id.keys()):
        t = warnings_by_id[tid]
        lines.append(
            "INSERT INTO public.warnings (\n"
            "  dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier\n"
            ") VALUES (\n"
            f"  {sql_literal(t['dtdd_topic_id'])}, {sql_literal(t['topic_name'])}, "
            f"{sql_literal(t.get('topic_type'))}, {sql_literal(t.get('parent_dtdd_topic_id'))}, {sql_literal(t.get('tier'))}\n"
            ")\n"
            "ON CONFLICT (dtdd_topic_id) DO UPDATE SET\n"
            "  topic_name = EXCLUDED.topic_name,\n"
            "  updated_at = NOW();\n\n"
        )

    # Upsert movie_warnings
    for r in movie_warning_rows:
        lines.append(
            "INSERT INTO public.movie_warnings (\n"
            "  imdb_id, dtdd_topic_id, source, answer, is_spoiler, warning_comment\n"
            ") VALUES (\n"
            f"  {sql_literal(r['imdb_id'])}, {sql_literal(r['dtdd_topic_id'])}, 'DTDD', "
            f"{sql_literal(r.get('answer'))}, {sql_literal(r.get('is_spoiler'))}, {sql_literal(r.get('warning_comment'))}\n"
            ")\n"
            "ON CONFLICT (imdb_id, dtdd_topic_id) DO UPDATE SET\n"
            "  answer = EXCLUDED.answer,\n"
            "  is_spoiler = EXCLUDED.is_spoiler,\n"
            "  warning_comment = EXCLUDED.warning_comment,\n"
            "  updated_at = NOW();\n\n"
        )

    lines.append("COMMIT;\n")
    OUT_SQL.write_text("".join(lines), encoding="utf-8")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()