"""
seed_warnings_from_catalog_to_sql.py
------------------------------------
Purpose:
  Generate a SQL seed file that populates ONLY the `warnings` table from the
  existing DTDD topics catalog CSV you already have in the repo.

Input:
  python_scripts/assets/dtdd_topics_catalog.csv

Output:
  database/seed/000_seed_warnings_catalog.sql

Run:
  python -m python_scripts.seed.seed_warnings_from_catalog_to_sql
"""

from __future__ import annotations

from pathlib import Path
import csv
from datetime import datetime


PROJECT_ROOT = Path(__file__).resolve().parents[2]
CSV_PATH = PROJECT_ROOT / "python_scripts" / "assets" / "dtdd_topics_catalog.csv"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "000_seed_warnings_catalog.sql"


def sql_escape(s: str) -> str:
    return s.replace("'", "''")


def sql_str(value: str | None, max_len: int | None = None) -> str:
    if value is None:
        return "NULL"
    v = value.strip()
    if not v:
        return "NULL"
    if max_len is not None:
        v = v[:max_len]
    return "'" + sql_escape(v) + "'"


def main() -> None:
    if not CSV_PATH.exists():
        raise FileNotFoundError(f"Could not find catalog CSV: {CSV_PATH}")

    OUT_SQL.parent.mkdir(parents=True, exist_ok=True)

    rows: list[dict] = []
    with CSV_PATH.open("r", encoding="utf-8", newline="") as f:
        reader = csv.DictReader(f)
        for r in reader:
            # your CSV header uses topic_id, topic_name, description, hit_count, example_titles
            tid_raw = (r.get("topic_id") or "").strip()
            name = (r.get("topic_name") or "").strip()

            if not tid_raw.isdigit():
                continue
            tid = int(tid_raw)
            if not name:
                continue

            rows.append(
                {
                    "dtdd_topic_id": tid,
                    "topic_name": name,
                    # Your CSV doesn’t have topic_type/parent/tier; keep NULL and enrich later.
                    "topic_type": None,
                    "parent_dtdd_topic_id": None,
                    "tier": None,
                }
            )

    rows.sort(key=lambda x: x["dtdd_topic_id"])

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    lines: list[str] = []
    lines.append("\\encoding UTF8\n")
    lines.append(f"-- Generated {ts}\nBEGIN;\n\n")

    for r in rows:
        lines.append(
            "INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)\n"
            f"VALUES ({r['dtdd_topic_id']}, {sql_str(r['topic_name'], 500)}, "
            f"{sql_str(r['topic_type'], 200)}, "
            f"{'NULL' if r['parent_dtdd_topic_id'] is None else int(r['parent_dtdd_topic_id'])}, "
            f"{'NULL' if r['tier'] is None else int(r['tier'])})\n"
            "ON CONFLICT (dtdd_topic_id) DO UPDATE SET\n"
            "  topic_name = EXCLUDED.topic_name,\n"
            "  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),\n"
            "  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),\n"
            "  tier = COALESCE(EXCLUDED.tier, warnings.tier),\n"
            "  updated_at = now();\n\n"
        )

    lines.append("COMMIT;\n")
    OUT_SQL.write_text("".join(lines), encoding="utf-8")

    print(f"Loaded {len(rows)} topics from catalog CSV.")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()