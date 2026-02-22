"""
seed_warnings_from_catalog_to_sql.py
-----------------------------------
Purpose:
  Generate the SQL seed file that populates the `warnings` table from
  the current taxonomy snapshot at:

    python_scripts/content_warnings/taxonomy/expanded.json

  This script also PRUNES any dtdd_topic_id rows that are NOT present in
  expanded.json, so rerunning seeds won’t reintroduce “extra” topics.

Output:
  database/seed/000_seed_warnings_catalog.sql

Run (from repo root):
  python python_scripts/seed/seed_warnings_from_catalog_to_sql.py
"""

from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime
import json
from pathlib import Path
from typing import Any


PROJECT_ROOT = Path(__file__).resolve().parents[2]

EXPANDED_JSON = (
    PROJECT_ROOT
    / "python_scripts"
    / "content_warnings"
    / "taxonomy"
    / "expanded.json"
)

OUT_SQL = PROJECT_ROOT / "database" / "seed" / "000_seed_warnings_catalog.sql"


@dataclass(frozen=True)
class WarningRow:
    dtdd_topic_id: int
    topic_name: str
    topic_type: str | None = None
    parent_dtdd_topic_id: int | None = None
    tier: int | None = None


def sql_escape(s: str) -> str:
    return s.replace("'", "''")


def sql_str(value: str | None, max_len: int | None = None) -> str:
    if value is None:
        return "NULL"
    v = str(value).strip()
    if not v:
        return "NULL"
    if max_len is not None:
        v = v[:max_len]
    return "'" + sql_escape(v) + "'"


def _coerce_int(value: Any) -> int | None:
    if value is None:
        return None
    if isinstance(value, int):
        return value
    s = str(value).strip()
    if not s:
        return None
    try:
        return int(s)
    except ValueError:
        return None


def load_topics_from_expanded(path: Path) -> list[WarningRow]:
    if not path.exists():
        raise FileNotFoundError(f"Could not find expanded.json: {path}")

    data = json.loads(path.read_text(encoding="utf-8"))

    topics = data.get("topics")
    if topics is None:
        raise ValueError(
            f"expanded.json does not contain a 'topics' key. Found keys: {list(data.keys())}"
        )

    # Support both:
    # - {"topics": { "158": {"topic_id": 158, "topic_name": "...", ...}, ... }}
    # - {"topics": [ {"topic_id": 158, "topic_name": "..."}, ... ]}
    if isinstance(topics, dict):
        topic_items = list(topics.values())
    elif isinstance(topics, list):
        topic_items = topics
    else:
        raise ValueError(
            f"expanded.json 'topics' must be a dict or list. Found: {type(topics).__name__}"
        )

    rows: list[WarningRow] = []
    seen: set[int] = set()

    for t in topic_items:
        if not isinstance(t, dict):
            continue

        tid = _coerce_int(t.get("topic_id"))
        name = (t.get("topic_name") or "").strip()

        if tid is None or not name:
            continue
        if tid in seen:
            continue
        seen.add(tid)

        topic_type = t.get("topic_type")
        if topic_type is not None:
            topic_type = str(topic_type).strip() or None

        parent_tid = _coerce_int(t.get("parent_topic_id"))
        tier = _coerce_int(t.get("tier"))

        rows.append(
            WarningRow(
                dtdd_topic_id=tid,
                topic_name=name,
                topic_type=topic_type,
                parent_dtdd_topic_id=parent_tid,
                tier=tier,
            )
        )

    rows.sort(key=lambda r: r.dtdd_topic_id)
    return rows


def main() -> None:
    OUT_SQL.parent.mkdir(parents=True, exist_ok=True)

    rows = load_topics_from_expanded(EXPANDED_JSON)

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    lines: list[str] = []
    lines.append("\\encoding UTF8\n")
    lines.append(f"-- Generated {ts}\n")
    lines.append("-- Source: python_scripts/content_warnings/taxonomy/expanded.json\n")
    lines.append("-- Note: This seed PRUNES topics not present in expanded.json.\n")
    lines.append("BEGIN;\n\n")

    # Create a temp table of allowed topic ids (the current “universe”)
    lines.append("-- Prune topics not in the current expanded.json universe\n")
    lines.append(
        "CREATE TEMP TABLE _allowed_warning_topics (dtdd_topic_id int PRIMARY KEY) ON COMMIT DROP;\n"
    )
    lines.append("INSERT INTO _allowed_warning_topics (dtdd_topic_id) VALUES\n")
    lines.append(",\n".join([f"  ({r.dtdd_topic_id})" for r in rows]))
    lines.append(";\n\n")

    # Prune dependent tables first (FK safety)
    lines.append("-- Remove any movie_warnings rows that reference pruned topics\n")
    lines.append("DELETE FROM public.movie_warnings mw\n")
    lines.append("WHERE NOT EXISTS (\n")
    lines.append(
        "  SELECT 1 FROM _allowed_warning_topics a WHERE a.dtdd_topic_id = mw.dtdd_topic_id\n"
    )
    lines.append(");\n\n")

    lines.append("-- Remove any category mappings that reference pruned topics\n")
    lines.append("DELETE FROM public.warning_category_topics wct\n")
    lines.append("WHERE NOT EXISTS (\n")
    lines.append(
        "  SELECT 1 FROM _allowed_warning_topics a WHERE a.dtdd_topic_id = wct.dtdd_topic_id\n"
    )
    lines.append(");\n\n")

    # Now prune the catalog itself
    lines.append("-- Remove catalog rows not in expanded.json\n")
    lines.append("DELETE FROM public.warnings w\n")
    lines.append("WHERE NOT EXISTS (\n")
    lines.append(
        "  SELECT 1 FROM _allowed_warning_topics a WHERE a.dtdd_topic_id = w.dtdd_topic_id\n"
    )
    lines.append(");\n\n")

    # Upsert the allowed catalog rows
    lines.append("-- Upsert catalog rows from expanded.json\n")
    for r in rows:
        lines.append(
            "INSERT INTO public.warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)\n"
            f"VALUES ({r.dtdd_topic_id}, {sql_str(r.topic_name, 500)}, "
            f"{sql_str(r.topic_type, 200)}, "
            f"{'NULL' if r.parent_dtdd_topic_id is None else int(r.parent_dtdd_topic_id)}, "
            f"{'NULL' if r.tier is None else int(r.tier)})\n"
            "ON CONFLICT (dtdd_topic_id) DO UPDATE SET\n"
            "  topic_name = EXCLUDED.topic_name,\n"
            "  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),\n"
            "  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),\n"
            "  tier = COALESCE(EXCLUDED.tier, warnings.tier),\n"
            "  updated_at = now();\n\n"
        )

    lines.append("COMMIT;\n")
    OUT_SQL.write_text("".join(lines), encoding="utf-8")

    print(f"Loaded {len(rows)} topics from expanded.json.")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()