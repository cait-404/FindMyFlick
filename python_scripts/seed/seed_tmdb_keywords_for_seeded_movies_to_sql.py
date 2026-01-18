"""
seed_tmdb_keywords_for_seeded_movies_to_sql.py
----------------------------------------------
Purpose:
  For the seeded movies (from 001_seed_us_popular_100_movies.sql), fetch TMDB keywords
  and generate a seed SQL file to populate:
    - keywords
    - movie_keywords

Output:
  database/seed/006_seed_us_popular_100_movie_keywords.sql

Run:
  python -m python_scripts.seed.seed_tmdb_keywords_for_seeded_movies_to_sql
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
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "006_seed_us_popular_100_movie_keywords.sql"

TMDB_BASE = "https://api.themoviedb.org/3"
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


def tmdb_get_json(tmdb_key: str, path: str, params: Dict[str, Any] | None = None) -> Dict[str, Any]:
    p = {"api_key": tmdb_key}
    if params:
        p.update(params)
    return fetch_json(f"{TMDB_BASE}{path}", params=p)


def extract_imdb_tmdb_pairs(seed_sql_path: Path) -> List[Tuple[str, int]]:
    """
    Extract (imdb_id, tmdb_id) pairs from 001_seed_us_popular_100_movies.sql.
    Matches patterns like: ('tt123...', 12345, ...
    """
    text = seed_sql_path.read_text(encoding="utf-8", errors="replace")
    pattern = re.compile(r"\(\s*'(?P<imdb>tt\d+)'\s*,\s*(?P<tmdb>\d+)\s*,", re.MULTILINE)

    pairs: List[Tuple[str, int]] = []
    for m in pattern.finditer(text):
        pairs.append((m.group("imdb"), int(m.group("tmdb"))))

    # de-dupe by imdb_id (keep first)
    seen = set()
    unique: List[Tuple[str, int]] = []
    for imdb_id, tmdb_id in pairs:
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

    keywords_by_id: Dict[int, str] = {}
    movie_keyword_rows: List[Tuple[str, int]] = []

    for imdb_id, tmdb_id in pairs:
        data = tmdb_get_json(
            tmdb_key,
            f"/movie/{tmdb_id}/keywords",
            params={"language": "en-US"},
        )

        kw_list = data.get("keywords") or []
        for kw in kw_list:
            if not isinstance(kw, dict):
                continue
            kid = kw.get("id")
            name = (kw.get("name") or "").strip()
            if not isinstance(kid, int) or not name:
                continue

            keywords_by_id[kid] = name
            movie_keyword_rows.append((imdb_id, kid))

        time.sleep(SLEEP_SECONDS)

    # De-dupe movie-keyword pairs
    movie_keyword_rows = sorted(set(movie_keyword_rows), key=lambda x: (x[0], x[1]))

    print(f"Unique keywords collected: {len(keywords_by_id)}")
    print(f"Movie-keyword rows: {len(movie_keyword_rows)}")

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    lines: List[str] = []
    lines.append(f"-- Generated {ts}\nBEGIN;\n\n")

    # Upsert keywords
    for kid in sorted(keywords_by_id.keys()):
        name = keywords_by_id[kid]
        lines.append(
            "INSERT INTO public.keywords (\n"
            "  tmdb_keyword_id, keyword_name\n"
            ") VALUES (\n"
            f"  {sql_literal(kid)}, {sql_literal(name)}\n"
            ")\n"
            "ON CONFLICT (tmdb_keyword_id) DO UPDATE SET\n"
            "  keyword_name = EXCLUDED.keyword_name,\n"
            "  updated_at = NOW();\n\n"
        )

    # Insert movie_keywords
    for imdb_id, kid in movie_keyword_rows:
        lines.append(
            "INSERT INTO public.movie_keywords (\n"
            "  imdb_id, tmdb_keyword_id\n"
            ") VALUES (\n"
            f"  {sql_literal(imdb_id)}, {sql_literal(kid)}\n"
            ")\n"
            "ON CONFLICT (imdb_id, tmdb_keyword_id) DO NOTHING;\n"
        )

    lines.append("\nCOMMIT;\n")
    OUT_SQL.write_text("".join(lines), encoding="utf-8")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()