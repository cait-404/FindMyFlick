"""
seed_dtdd_warnings_for_seeded_movies_to_sql.py

Purpose
- Generate SQL that populates public.movie_warnings for any movie that already has a DTDD match in public.movie_dtdd_titles.
- This script DOES NOT try to match DTDD again. It trusts movie_dtdd_titles.dtdd_media_id.
- It writes one row per (imdb_id, dtdd_topic_id) using the existing topic catalog in public.warnings.
- For a given movie:
  - If DTDD provides Yes/No counts, answer is derived from whichever is larger.
  - If both are 0 or missing, answer is 'unknown'.
  - If DTDD does not return a topic, answer defaults to 'unknown'.

Requirements
- Python packages: requests, psycopg (preferred) or psycopg2
- Environment variables (recommended):
  - DTDD_API_KEY
  - DB_HOST (default localhost)
  - DB_PORT (default 5432)
  - DB_NAME (default findmyflick)
  - DB_USER (default postgres)
  - DB_PASS (default empty)
Optional:
  - DTDD_RATE_LIMIT_SECONDS (default 0.25)
  - DTDD_LIMIT_MOVIES (default 0 = no limit)
"""

from __future__ import annotations

import json
import os
import time
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Dict, List, Optional, Sequence, Tuple

import requests

try:
    import psycopg  # psycopg v3
except ImportError:
    psycopg = None

try:
    import psycopg2  # type: ignore
except ImportError:
    psycopg2 = None


DTDD_BASE_URL = "https://www.doesthedogdie.com"
DTDD_MEDIA_URL = f"{DTDD_BASE_URL}/media"


@dataclass(frozen=True)
class DbConfig:
    host: str
    port: int
    dbname: str
    user: str
    password: str


def get_repo_root() -> Path:
    # .../python_scripts/seed/script.py -> parents[0]=seed, [1]=python_scripts, [2]=repo root
    return Path(__file__).resolve().parents[2]


def get_output_paths() -> Tuple[Path, Path]:
    repo_root = get_repo_root()
    sql_path = repo_root / "database" / "seed" / "010_seed_us_streamable_dtdd_warnings.sql"
    log_path = repo_root / "python_scripts" / "assets" / "dtdd_warnings_seed_log.json"
    log_path.parent.mkdir(parents=True, exist_ok=True)
    return sql_path, log_path


def get_env_db_config() -> DbConfig:
    return DbConfig(
        host=os.environ.get("DB_HOST", "localhost"),
        port=int(os.environ.get("DB_PORT", "5432")),
        dbname=os.environ.get("DB_NAME", "findmyflick"),
        user=os.environ.get("DB_USER", "postgres"),
        password=os.environ.get("DB_PASS", ""),
    )


def connect_db(cfg: DbConfig):
    if psycopg is not None:
        return psycopg.connect(
            host=cfg.host,
            port=cfg.port,
            dbname=cfg.dbname,
            user=cfg.user,
            password=cfg.password,
        )
    if psycopg2 is not None:
        return psycopg2.connect(
            host=cfg.host,
            port=cfg.port,
            dbname=cfg.dbname,
            user=cfg.user,
            password=cfg.password,
        )
    raise RuntimeError("Neither psycopg (v3) nor psycopg2 is installed. Install one of them.")


def http_get_json(url: str, headers: Dict[str, str], timeout: int = 30, retries: int = 3) -> Dict[str, Any]:
    last_err: Optional[Exception] = None
    for attempt in range(1, retries + 1):
        try:
            r = requests.get(url, headers=headers, timeout=timeout)
            r.raise_for_status()
            return r.json()
        except Exception as e:
            last_err = e
            # small backoff
            time.sleep(0.75 * attempt)
    raise RuntimeError(f"Failed GET {url} after {retries} attempts: {last_err}")


def sql_escape_literal(value: str) -> str:
    # Escape for SQL single-quoted literals
    return value.replace("'", "''")


def fetch_dtdd_targets(conn) -> List[Tuple[str, int, str, float]]:
    """
    Returns: [(imdb_id, dtdd_media_id, match_method, match_score), ...]
    """
    q = """
        SELECT imdb_id, dtdd_media_id, match_method, COALESCE(match_score, 0) AS match_score
        FROM public.movie_dtdd_titles
        WHERE dtdd_media_id IS NOT NULL
        ORDER BY match_method, match_score DESC, imdb_id;
    """
    with conn.cursor() as cur:
        cur.execute(q)
        rows = cur.fetchall()
    targets: List[Tuple[str, int, str, float]] = []
    for imdb_id, dtdd_media_id, match_method, match_score in rows:
        targets.append((str(imdb_id), int(dtdd_media_id), str(match_method), float(match_score)))
    return targets


def fetch_warning_topics(conn) -> List[Tuple[int, str]]:
    """
    Returns: [(dtdd_topic_id, topic_name), ...]
    """
    q = """
        SELECT dtdd_topic_id, topic_name
        FROM public.warnings
        ORDER BY dtdd_topic_id;
    """
    with conn.cursor() as cur:
        cur.execute(q)
        rows = cur.fetchall()
    topics: List[Tuple[int, str]] = [(int(r[0]), str(r[1])) for r in rows]
    return topics


def derive_answer_from_topic_stat(stat: Dict[str, Any]) -> str:
    """
    DTDD topicItemStats commonly includes yes/no counts with different key casing.
    We derive:
      - 'yes' if yes_count > no_count
      - 'no'  if no_count > yes_count
      - 'unknown' otherwise
    """
    def pick_int(d: Dict[str, Any], keys: Sequence[str]) -> int:
        for k in keys:
            if k in d and d[k] is not None:
                try:
                    return int(d[k])
                except Exception:
                    pass
        return 0

    yes_ct = pick_int(stat, ["YesSum", "yesSum", "yes", "Yes", "yes_count", "yesCount"])
    no_ct = pick_int(stat, ["NoSum", "noSum", "no", "No", "no_count", "noCount"])

    if yes_ct > no_ct:
        return "yes"
    if no_ct > yes_ct:
        return "no"
    return "unknown"


def extract_topic_id(stat: Dict[str, Any]) -> Optional[int]:
    for k in ["TopicId", "topicId", "TopicItemId", "topicItemId", "topic_id", "topicItemID"]:
        if k in stat and stat[k] is not None:
            try:
                return int(stat[k])
            except Exception:
                return None
    return None


def build_movie_topic_answer_map(media_json: Dict[str, Any]) -> Dict[int, str]:
    stats = media_json.get("topicItemStats") or []
    if not isinstance(stats, list):
        return {}

    out: Dict[int, str] = {}
    for item in stats:
        if not isinstance(item, dict):
            continue
        topic_id = extract_topic_id(item)
        if topic_id is None:
            continue
        out[topic_id] = derive_answer_from_topic_stat(item)
    return out


def generate_sql(
    targets: List[Tuple[str, int, str, float]],
    topics: List[Tuple[int, str]],
    dtdd_api_key: str,
    rate_limit_seconds: float,
    limit_movies: int,
) -> Tuple[str, Dict[str, Any]]:
    headers = {"Accept": "application/json", "X-API-KEY": dtdd_api_key}

    lines: List[str] = []
    log: Dict[str, Any] = {
        "generated_at": time.strftime("%Y-%m-%d %H:%M:%S"),
        "targets_total": len(targets),
        "topics_total": len(topics),
        "processed_movies": 0,
        "movies_written": 0,
        "errors": [],
        "notes": [],
    }

    lines.append("-- Auto-generated by python_scripts/seed/seed_dtdd_warnings_for_seeded_movies_to_sql.py")
    lines.append("BEGIN;")
    lines.append("")
    lines.append("-- Ensure consistent lookup table exists (warnings) before inserting movie_warnings.")
    lines.append("-- This script assumes public.warnings is already populated.")
    lines.append("")

    processed = 0
    for imdb_id, dtdd_media_id, match_method, match_score in targets:
        if limit_movies > 0 and processed >= limit_movies:
            break

        processed += 1
        log["processed_movies"] = processed

        media_url = f"{DTDD_MEDIA_URL}/{dtdd_media_id}"
        try:
            media_json = http_get_json(media_url, headers=headers)
            topic_map = build_movie_topic_answer_map(media_json)

            # Insert one row per topic, defaulting to 'unknown' when missing
            # Assumptions about schema:
            #   public.movie_warnings(imdb_id text, dtdd_topic_id int, answer text)
            #   Unique/PK on (imdb_id, dtdd_topic_id)
            for topic_id, _topic_name in topics:
                answer = topic_map.get(topic_id, "unknown")
                lines.append(
                    "INSERT INTO public.movie_warnings (imdb_id, dtdd_topic_id, answer) "
                    f"VALUES ('{sql_escape_literal(imdb_id)}', {topic_id}, '{answer}') "
                    "ON CONFLICT (imdb_id, dtdd_topic_id) DO UPDATE SET answer = EXCLUDED.answer;"
                )

            lines.append("")
            log["movies_written"] += 1

        except Exception as e:
            log["errors"].append(
                {
                    "imdb_id": imdb_id,
                    "dtdd_media_id": dtdd_media_id,
                    "match_method": match_method,
                    "match_score": match_score,
                    "error": str(e),
                }
            )
            # Keep going so one failure does not stop the entire script
            log["notes"].append(f"Skipped {imdb_id} (dtdd_media_id={dtdd_media_id}) due to error.")
        finally:
            time.sleep(rate_limit_seconds)

    lines.append("COMMIT;")
    lines.append("")
    lines.append("-- End of generated seed SQL")

    return "\n".join(lines) + "\n", log


def main() -> None:
    dtdd_api_key = os.environ.get("DTDD_API_KEY", "").strip()
    if not dtdd_api_key:
        raise RuntimeError("DTDD_API_KEY is not set. Set it in your environment before running this script.")

    rate_limit_seconds = float(os.environ.get("DTDD_RATE_LIMIT_SECONDS", "0.25"))
    limit_movies = int(os.environ.get("DTDD_LIMIT_MOVIES", "0"))

    sql_path, log_path = get_output_paths()
    db_cfg = get_env_db_config()

    conn = connect_db(db_cfg)
    try:
        targets = fetch_dtdd_targets(conn)
        topics = fetch_warning_topics(conn)

        if not topics:
            raise RuntimeError(
                "public.warnings is empty. Populate warnings topics first, then rerun this script."
            )

        sql_text, log = generate_sql(
            targets=targets,
            topics=topics,
            dtdd_api_key=dtdd_api_key,
            rate_limit_seconds=rate_limit_seconds,
            limit_movies=limit_movies,
        )

        sql_path.parent.mkdir(parents=True, exist_ok=True)
        sql_path.write_text(sql_text, encoding="utf-8")
        log_path.write_text(json.dumps(log, indent=2), encoding="utf-8")

        print(f"Wrote SQL: {sql_path}")
        print(f"Wrote log: {log_path}")
        print(f"Targets found: {len(targets)} (dtdd_media_id not null)")
        print(f"Topics found: {len(topics)} (from public.warnings)")
        print(f"Movies written: {log.get('movies_written')}")

    finally:
        try:
            conn.close()
        except Exception:
            pass


if __name__ == "__main__":
    main()