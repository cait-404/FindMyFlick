"""
seed_dtdd_topics_to_sql.py
--------------------------
Purpose:
  Generate a SQL seed file for the `warnings` table using the DTDD topic catalog
  you've already exported (python_scripts/assets/dtdd_topics_catalog.csv).

Output:
  database/seed/000_seed_warnings_catalog.sql

Run:
  python -m python_scripts.seed.seed_dtdd_topics_to_sql
"""

from __future__ import annotations
import csv
from pathlib import Path
from datetime import datetime


PROJECT_ROOT = Path(__file__).resolve().parents[2]
CSV_PATH = PROJECT_ROOT / "python_scripts" / "assets" / "dtdd_topics_catalog.csv"
OUT_SQL = PROJECT_ROOT / "database" / "seed" / "000_seed_warnings_catalog.sql"


def sql_escape(s: str) -> str:
    return s.replace("'", "''")


def main() -> None:
    if not CSV_PATH.exists():
        raise FileNotFoundError(f"Missing: {CSV_PATH}")

    rows = []
    with CSV_PATH.open("r", encoding="utf-8", newline="") as f:
        reader = csv.DictReader(f)
        for r in reader:
            topic_id = int(r["topic_id"])
            topic_name = (r["topic_name"] or "").strip()
            if not topic_name:
                continue
            rows.append((topic_id, topic_name))

    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    header = f"-- Generated {ts}\nBEGIN;\n"

    lines = [header]
    lines.append("-- Seed warnings catalog (tier/parent/topic_type left NULL for now)\n")

    for topic_id, topic_name in rows:
        lines.append(
            "INSERT INTO warnings (dtdd_topic_id, topic_name)\n"
            f"VALUES ({topic_id}, '{sql_escape(topic_name)}')\n"
            "ON CONFLICT (dtdd_topic_id) DO UPDATE\n"
            "SET topic_name = EXCLUDED.topic_name,\n"
            "    updated_at = now();\n"
        )

    lines.append("COMMIT;\n")

    OUT_SQL.write_text("".join(lines), encoding="utf-8")
    print(f"Saved: {OUT_SQL}")


if __name__ == "__main__":
    main()