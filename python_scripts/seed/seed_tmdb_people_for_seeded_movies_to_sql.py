"""
seed_tmdb_people_for_seeded_movies_to_sql.py
--------------------------------------------
Purpose:
  For the seeded movies (from 001_seed_us_popular_100_movies.sql), fetch TMDB credits and
  generate a seed SQL file to populate:
    - people
    - movie_cast
    - movie_crew

Output:
  database/seed/004_seed_us_popular_100_people_and_credits.sql

Assumptions:
  - TMDB_API_KEY is available via .env (schema_utils.load_api_key handles this)
  - movies already seeded so imdb_id + tmdb_id exist in the seed SQL

Run:
  python -m python_scripts.seed.seed_tmdb_people_for_seeded_movies_to_sql
"""

from __future__ import annotations

from pathlib import Path
from datetime import datetime
from typing import Any, Dict, List, Tuple
import re
import time

from python_scripts.shared.schema_utils import load_api_key, fetch_json

PROJECT_ROOT = Path(__file__).resolve().parents[2]

MOVIES_SEED_SQL = PROJECT_ROOT / "database" / "seed" / "001_seed_us_popular_100_movies.sql"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "004_seed_us_popular_100_people_and_credits.sql"

TMDB_BASE = "https://api.themoviedb.org/3"
TMDB_IMAGE_BASE = "https://image.tmdb.org/t/p/w185"  # good size for headshots

SLEEP_SECONDS = 0.25
MAX_CAST_PER_MOVIE = 60  # adjust if you want more


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


def tmdb_get_json(tmdb_key: str, path: str, params: Dict[str, Any] | None = None) -> Dict[str, Any]:
    p = {"api_key": tmdb_key}
    if params:
        p.update(params)
    return fetch_json(f"{TMDB_BASE}{path}", params=p)


def extract_imdb_tmdb_pairs(seed_sql_path: Path) -> List[Tuple[str, int]]:
    """
    Extract (imdb_id, tmdb_id) from INSERT statements like:
      VALUES (
        'tt123...', 12345, 'Title', 2025, ...
      )
    Works across multiline SQL by scanning the entire file.
    """
    text = seed_sql_path.read_text(encoding="utf-8", errors="replace")

    # Look for: ('tt123', 12345,
    pattern = re.compile(r"\(\s*'(?P<imdb>tt\d+)'\s*,\s*(?P<tmdb>\d+)\s*,", re.MULTILINE)
    out: List[Tuple[str, int]] = []

    for m in pattern.finditer(text):
        imdb_id = m.group("imdb")
        tmdb_id = int(m.group("tmdb"))
        out.append((imdb_id, tmdb_id))

    # de-dupe by imdb_id (keep first)
    seen = set()
    unique: List[Tuple[str, int]] = []
    for imdb_id, tmdb_id in out:
        if imdb_id in seen:
            continue
        seen.add(imdb_id)
        unique.append((imdb_id, tmdb_id))

    return unique


def main() -> None:
    tmdb_key = load_api_key("TMDB_API_KEY")

    if not MOVIES_SEED_SQL.exists():
        raise RuntimeError(f"Movies seed SQL not found: {MOVIES_SEED_SQL}")

    pairs = extract_imdb_tmdb_pairs(MOVIES_SEED_SQL)
    print(f"Found {len(pairs)} unique (imdb_id, tmdb_id) pairs in: {MOVIES_SEED_SQL.name}")

    people_by_id: Dict[int, Dict[str, Any]] = {}
    cast_rows: List[Dict[str, Any]] = []
    crew_rows: List[Dict[str, Any]] = []

    for imdb_id, tmdb_id in pairs:
        data = tmdb_get_json(tmdb_key, f"/movie/{tmdb_id}/credits")

        cast = data.get("cast") or []
        crew = data.get("crew") or []

        # Cast: keep top N by TMDB "order"
        cast_sorted = sorted(
            [c for c in cast if isinstance(c, dict)],
            key=lambda c: c.get("order") if isinstance(c.get("order"), int) else 9999,
        )[:MAX_CAST_PER_MOVIE]

        for c in cast_sorted:
            credit_id = c.get("credit_id")
            person_id = c.get("id")
            name = (c.get("name") or "").strip()
            if not credit_id or not isinstance(person_id, int) or not name:
                continue

            profile_path = c.get("profile_path")
            profile_url = f"{TMDB_IMAGE_BASE}{profile_path}" if profile_path else None

            people_by_id[person_id] = {
                "tmdb_person_id": person_id,
                "person_name": name,
                "known_for_department": c.get("known_for_department"),
                "profile_url": profile_url,
            }

            cast_rows.append(
                {
                    "tmdb_credit_id": credit_id,
                    "imdb_id": imdb_id,
                    "tmdb_person_id": person_id,
                    "character_name": c.get("character"),
                    "cast_order": c.get("order"),
                }
            )

        for c in crew:
            if not isinstance(c, dict):
                continue

            credit_id = c.get("credit_id")
            person_id = c.get("id")
            name = (c.get("name") or "").strip()
            if not credit_id or not isinstance(person_id, int) or not name:
                continue

            profile_path = c.get("profile_path")
            profile_url = f"{TMDB_IMAGE_BASE}{profile_path}" if profile_path else None

            people_by_id[person_id] = {
                "tmdb_person_id": person_id,
                "person_name": name,
                "known_for_department": c.get("known_for_department"),
                "profile_url": profile_url,
            }

            crew_rows.append(
                {
                    "tmdb_credit_id": credit_id,
                    "imdb_id": imdb_id,
                    "tmdb_person_id": person_id,
                    "department": c.get("department"),
                    "job": c.get("job"),
                }
            )

        time.sleep(SLEEP_SECONDS)

    print(f"Unique people collected: {len(people_by_id)}")
    print(f"Cast credit rows: {len(cast_rows)}")
    print(f"Crew credit rows: {len(crew_rows)}")

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")

    lines: List[str] = []
    lines.append(f"-- Generated {ts}\nBEGIN;\n\n")

    # Upsert people (safe if rerun)
    for pid in sorted(people_by_id.keys()):
        p = people_by_id[pid]
        lines.append(
            "INSERT INTO public.people (\n"
            "  tmdb_person_id, person_name, known_for_department, profile_url\n"
            ") VALUES (\n"
            f"  {sql_literal(p['tmdb_person_id'])}, {sql_literal(p['person_name'])}, "
            f"{sql_literal(p.get('known_for_department'))}, {sql_literal(p.get('profile_url'))}\n"
            ")\n"
            "ON CONFLICT (tmdb_person_id) DO UPDATE SET\n"
            "  person_name = EXCLUDED.person_name,\n"
            "  known_for_department = EXCLUDED.known_for_department,\n"
            "  profile_url = EXCLUDED.profile_url,\n"
            "  updated_at = NOW();\n\n"
        )

    # Insert cast credits
    for r in cast_rows:
        lines.append(
            "INSERT INTO public.movie_cast (\n"
            "  tmdb_credit_id, imdb_id, tmdb_person_id, character_name, cast_order\n"
            ") VALUES (\n"
            f"  {sql_literal(r['tmdb_credit_id'])}, {sql_literal(r['imdb_id'])}, {sql_literal(r['tmdb_person_id'])}, "
            f"{sql_literal(r.get('character_name'))}, {sql_literal(r.get('cast_order'))}\n"
            ")\n"
            "ON CONFLICT (tmdb_credit_id) DO NOTHING;\n"
        )
    lines.append("\n")

    # Insert crew credits
    for r in crew_rows:
        lines.append(
            "INSERT INTO public.movie_crew (\n"
            "  tmdb_credit_id, imdb_id, tmdb_person_id, department, job\n"
            ") VALUES (\n"
            f"  {sql_literal(r['tmdb_credit_id'])}, {sql_literal(r['imdb_id'])}, {sql_literal(r['tmdb_person_id'])}, "
            f"{sql_literal(r.get('department'))}, {sql_literal(r.get('job'))}\n"
            ")\n"
            "ON CONFLICT (tmdb_credit_id) DO NOTHING;\n"
        )
    lines.append("\n")

    lines.append("COMMIT;\n")

    OUT_SQL.write_text("".join(lines), encoding="utf-8")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()