"""
seed_tmdb_genres_for_seeded_movies_to_sql.py
--------------------------------------------
Purpose:
  For already-seeded movies, fetch TMDB genres and create SQL to populate:
    - public.genres
    - public.movie_genres

Inputs:
  - database/seed/001_seed_us_popular_100_movies.sql  (must contain imdb_id + tmdb_id values)

Output:
  - database/seed/003_seed_us_popular_100_movie_genres.sql

Run:
  python -m python_scripts.seed.seed_tmdb_genres_for_seeded_movies_to_sql
"""

from __future__ import annotations

import os
import re
import time
from pathlib import Path
from typing import Dict, List, Tuple, Any
import requests

try:
    from dotenv import load_dotenv
except Exception:
    load_dotenv = None


PROJECT_ROOT = Path(__file__).resolve().parents[2]
INPUT_SEED = PROJECT_ROOT / "database" / "seed" / "001_seed_us_popular_100_movies.sql"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "003_seed_us_popular_100_movie_genres.sql"

TMDB_BASE = "https://api.themoviedb.org/3"
SLEEP_SECONDS = 0.25

# Matches tuples like: ('tt1234567', 98765, ...
# Captures imdb_id and tmdb_id from your INSERT VALUES lines.
MOVIE_PAIR_RE = re.compile(r"\(\s*'(?P<imdb>tt\d+)'\s*,\s*(?P<tmdb>\d+)\s*,", re.IGNORECASE)


def require_env() -> str:
    if load_dotenv is not None:
        load_dotenv(PROJECT_ROOT / ".env")
    key = os.getenv("TMDB_API_KEY")
    if not key:
        raise RuntimeError("Missing TMDB_API_KEY. Confirm it exists in your root .env.")
    return key


def sql_literal(v: Any) -> str:
    if v is None:
        return "NULL"
    if isinstance(v, bool):
        return "TRUE" if v else "FALSE"
    if isinstance(v, (int, float)):
        return str(v)
    s = str(v).replace("'", "''")
    return f"'{s}'"


def parse_imdb_tmdb_pairs(path: Path) -> List[Tuple[str, int]]:
    text = path.read_text(encoding="utf-8", errors="replace")
    pairs: List[Tuple[str, int]] = []
    for m in MOVIE_PAIR_RE.finditer(text):
        imdb_id = m.group("imdb").strip()
        tmdb_id = int(m.group("tmdb"))
        pairs.append((imdb_id, tmdb_id))

    # de-dupe by imdb_id, keep first occurrence
    seen = set()
    out: List[Tuple[str, int]] = []
    for imdb_id, tmdb_id in pairs:
        if imdb_id in seen:
            continue
        seen.add(imdb_id)
        out.append((imdb_id, tmdb_id))
    return out


def tmdb_get_json(endpoint: str, params: Dict[str, Any], tries: int = 4, backoff: float = 1.5) -> Dict[str, Any]:
    url = f"{TMDB_BASE}{endpoint}"
    last_exc: Exception | None = None

    for attempt in range(1, tries + 1):
        try:
            r = requests.get(url, params=params, timeout=30)
            if r.status_code == 429:
                time.sleep(backoff * attempt)
                continue
            r.raise_for_status()
            return r.json()
        except Exception as e:
            last_exc = e
            time.sleep(backoff * attempt)

    raise RuntimeError(f"TMDB request failed after retries: {url}") from last_exc


def main() -> None:
    tmdb_key = require_env()

    if not INPUT_SEED.exists():
        raise RuntimeError(f"Missing input seed file: {INPUT_SEED}")

    pairs = parse_imdb_tmdb_pairs(INPUT_SEED)
    print(f"Found {len(pairs)} unique IMDb IDs in: {INPUT_SEED.name}")

    genre_map: Dict[int, str] = {}          # tmdb_genre_id -> genre_name
    movie_genres: Dict[str, List[int]] = {} # imdb_id -> [genre_ids]

    for imdb_id, tmdb_id in pairs:
        details = tmdb_get_json(
            f"/movie/{tmdb_id}",
            params={"api_key": tmdb_key, "language": "en-US"},
        )

        genres = details.get("genres") or []
        ids: List[int] = []
        for g in genres:
            gid = g.get("id")
            gname = (g.get("name") or "").strip()
            if isinstance(gid, int) and gname:
                genre_map[gid] = gname
                ids.append(gid)

        movie_genres[imdb_id] = sorted(set(ids))
        time.sleep(SLEEP_SECONDS)

    # Write SQL
    lines: List[str] = []
    lines.append("BEGIN;\n\n")

    # Insert genres first
    for gid in sorted(genre_map.keys()):
        lines.append(
            "INSERT INTO public.genres (tmdb_genre_id, genre_name)\n"
            f"VALUES ({gid}, {sql_literal(genre_map[gid])})\n"
            "ON CONFLICT (tmdb_genre_id) DO UPDATE SET\n"
            "  genre_name = EXCLUDED.genre_name,\n"
            "  updated_at = now();\n\n"
        )

    # Then links
    link_count = 0
    movies_with_any = 0
    for imdb_id in sorted(movie_genres.keys()):
        gids = movie_genres[imdb_id]
        if gids:
            movies_with_any += 1
        for gid in gids:
            link_count += 1
            lines.append(
                "INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)\n"
                f"VALUES ({sql_literal(imdb_id)}, {gid})\n"
                "ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;\n"
            )
        if gids:
            lines.append("\n")

    lines.append("COMMIT;\n")
    OUT_SQL.parent.mkdir(parents=True, exist_ok=True)
    OUT_SQL.write_text("".join(lines), encoding="utf-8")

    print(f"Genres found: {len(genre_map)}")
    print(f"Movies with at least one genre: {movies_with_any}")
    print(f"Total genre links written: {link_count}")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()