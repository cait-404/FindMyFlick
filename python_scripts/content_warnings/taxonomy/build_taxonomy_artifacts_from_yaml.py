#!/usr/bin/env python3
"""
Build taxonomy artifacts from claude_taxonomy.yml (source of truth).

Outputs:
  1) python_scripts/content_warnings/taxonomy/expanded.json
     - umbrellas
     - topics
     - umbrella_to_topic_ids
     - topic_id_to_umbrellas

  2) database/seed/011_seed_warning_taxonomy.sql
     - warning_categories
     - warning_category_topics

  3) database/seed/012_seed_warning_subcategories.sql
     - warning_subcategories
     - warning_subcategory_topics

Assumptions:
- claude_taxonomy.yml uses keys like U01, U02, ...
- The umbrella name is stored as an inline comment on the same line, e.g.:
    U01: # Animal Harm & Death
- Topic names come from dtdd_topics_catalog.json (snapshot).
"""

from __future__ import annotations

import json
import re
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Any

import yaml


HERE = Path(__file__).resolve().parent


def find_repo_root(start: Path) -> Path:
    """
    Walk upward until we find the repo root containing 'database' and 'python_scripts'.
    """
    for p in [start] + list(start.parents):
        if (p / "database").exists() and (p / "python_scripts").exists():
            return p
    raise RuntimeError("Could not locate repo root (expected folders: database/, python_scripts/).")


REPO_ROOT = find_repo_root(HERE)

YAML_PATH = HERE / "claude_taxonomy.yml"
EXPANDED_JSON_OUT = HERE / "expanded.json"

CATALOG_JSON = (
    REPO_ROOT
    / "python_scripts"
    / "content_warnings"
    / "data"
    / "dtdd"
    / "dtdd_topics_catalog.json"
)

SEED_011_OUT = REPO_ROOT / "database" / "seed" / "011_seed_warning_taxonomy.sql"
SEED_012_OUT = REPO_ROOT / "database" / "seed" / "012_seed_warning_subcategories.sql"


@dataclass(frozen=True)
class SubcatRow:
    umbrella_id: str
    umbrella_name: str
    subcategory_name: str
    topic_ids: tuple[int, ...]


def sql_str(val: str | None, max_len: int | None = None) -> str:
    if val is None:
        return "NULL"
    s = str(val)
    if max_len is not None:
        s = s[:max_len]
    s = s.replace("'", "''")
    return f"'{s}'"


def load_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8"))


def load_catalog_topic_names(catalog_path: Path) -> dict[int, str]:
    """
    dtdd_topics_catalog.json can be either:
      - a list of { topic_id, topic_name, ... }
      - or a dict with a top-level key holding the list

    We support both.
    """
    data = load_json(catalog_path)

    items: list[dict[str, Any]]
    if isinstance(data, list):
        items = data
    elif isinstance(data, dict):
        # Try common keys; if none match, fail loudly.
        for k in ("topics", "data", "catalog", "items"):
            if k in data and isinstance(data[k], list):
                items = data[k]
                break
        else:
            raise ValueError(
                f"Unrecognized catalog JSON structure in {catalog_path}. "
                f"Top-level keys: {list(data.keys())}"
            )
    else:
        raise ValueError(f"Unrecognized catalog JSON type: {type(data)}")

    out: dict[int, str] = {}
    for row in items:
        if not isinstance(row, dict):
            continue
        tid = row.get("topic_id")
        name = row.get("topic_name")
        if isinstance(tid, int) and isinstance(name, str) and name.strip():
            out[tid] = name.strip()

    if not out:
        raise ValueError(f"No topic_id/topic_name pairs found in {catalog_path}")

    return out


def parse_umbrella_names_from_yaml_comments(yaml_text: str) -> dict[str, str]:
    """
    Extract umbrella name from inline comment:
      U03: # Violence & Gore
    """
    names: dict[str, str] = {}
    # Only match top-level umbrella lines at start of line.
    pattern = re.compile(r"^(U\d+):\s*#\s*(.+?)\s*$", re.MULTILINE)
    for m in pattern.finditer(yaml_text):
        umbrella_id = m.group(1).strip()
        umbrella_name = m.group(2).strip()
        if umbrella_id and umbrella_name:
            names[umbrella_id] = umbrella_name
    return names


def load_yaml_structure(path: Path) -> tuple[dict[str, str], dict[str, Any]]:
    raw = path.read_text(encoding="utf-8")
    umbrella_names = parse_umbrella_names_from_yaml_comments(raw)

    parsed = yaml.safe_load(raw)
    if not isinstance(parsed, dict):
        raise ValueError(f"Expected YAML top-level mapping in {path}, got: {type(parsed)}")

    # Ensure umbrellas in YAML have names from comments
    missing = [k for k in parsed.keys() if isinstance(k, str) and k.startswith("U") and k not in umbrella_names]
    if missing:
        raise ValueError(
            "Missing umbrella name comments for: "
            + ", ".join(sorted(missing))
            + "\nExpected format: UXX: # Umbrella Name"
        )

    return umbrella_names, parsed


def normalize_topic_id_list(val: Any) -> list[int]:
    """
    YAML should have lists like [158, 168]. We accept:
      - list of ints
      - list of strings convertible to int
    """
    if not isinstance(val, list):
        raise ValueError(f"Expected list of topic_ids, got: {type(val)}")

    out: list[int] = []
    for x in val:
        if isinstance(x, int):
            out.append(x)
        elif isinstance(x, str) and x.strip().isdigit():
            out.append(int(x.strip()))
        else:
            raise ValueError(f"Invalid topic_id value in YAML list: {x!r}")

    return out


def build_rows(umbrella_names: dict[str, str], yaml_obj: dict[str, Any]) -> list[SubcatRow]:
    """
    Convert YAML to a flat list of (umbrella, subcategory, topic_ids).
    YAML shape expected:
      U03:
        General Violence:
          Blood/gore: [188]
          ...
        Actions:
          Stabbing: [343]
    """
    rows: list[SubcatRow] = []

    for umbrella_id, umbrella_block in yaml_obj.items():
        if not (isinstance(umbrella_id, str) and umbrella_id.startswith("U")):
            continue

        if not isinstance(umbrella_block, dict):
            raise ValueError(f"{umbrella_id} must map to subcategories, got: {type(umbrella_block)}")

        umbrella_name = umbrella_names[umbrella_id]

        for subcat_name, subcat_block in umbrella_block.items():
            if not isinstance(subcat_name, str):
                raise ValueError(f"{umbrella_id} subcategory name must be a string, got: {type(subcat_name)}")
            if not isinstance(subcat_block, dict):
                raise ValueError(f"{umbrella_id}.{subcat_name} must map to items, got: {type(subcat_block)}")

            topic_ids: list[int] = []
            for _label, ids in subcat_block.items():
                topic_ids.extend(normalize_topic_id_list(ids))

            # Deduplicate within a subcategory while preserving stable order
            seen: set[int] = set()
            deduped: list[int] = []
            for tid in topic_ids:
                if tid not in seen:
                    seen.add(tid)
                    deduped.append(tid)

            rows.append(
                SubcatRow(
                    umbrella_id=umbrella_id,
                    umbrella_name=umbrella_name,
                    subcategory_name=subcat_name.strip(),
                    topic_ids=tuple(deduped),
                )
            )

    # Stable ordering: by umbrella_id then subcategory_name
    rows.sort(key=lambda r: (r.umbrella_id, r.subcategory_name))
    return rows


def build_expanded_json(
    rows: list[SubcatRow], topic_names: dict[int, str]
) -> dict[str, Any]:
    # Umbrellas
    umbrella_ids = sorted({r.umbrella_id for r in rows})
    umbrellas = [
        {"umbrella_id": uid, "umbrella_name": next(r.umbrella_name for r in rows if r.umbrella_id == uid)}
        for uid in umbrella_ids
    ]

    # umbrella_to_topic_ids
    umbrella_to_topic_ids: dict[str, list[int]] = {}
    for uid in umbrella_ids:
        ids: list[int] = []
        seen: set[int] = set()
        for r in rows:
            if r.umbrella_id != uid:
                continue
            for tid in r.topic_ids:
                if tid not in seen:
                    seen.add(tid)
                    ids.append(tid)
        umbrella_to_topic_ids[uid] = ids

    # topic_id_to_umbrellas
    topic_id_to_umbrellas: dict[str, list[str]] = {}
    for uid, ids in umbrella_to_topic_ids.items():
        for tid in ids:
            key = str(tid)
            topic_id_to_umbrellas.setdefault(key, [])
            if uid not in topic_id_to_umbrellas[key]:
                topic_id_to_umbrellas[key].append(uid)

    # topics list (only those used)
    used_topic_ids = sorted({int(k) for k in topic_id_to_umbrellas.keys()})
    missing_names = [tid for tid in used_topic_ids if tid not in topic_names]
    if missing_names:
        raise ValueError(
            "Topic IDs referenced in YAML are missing from dtdd_topics_catalog.json: "
            + ", ".join(map(str, missing_names[:30]))
            + (" ..." if len(missing_names) > 30 else "")
        )

    topics = [{"dtdd_topic_id": tid, "topic_name": topic_names[tid]} for tid in used_topic_ids]

    return {
        "umbrellas": umbrellas,
        "topics": topics,
        "umbrella_to_topic_ids": umbrella_to_topic_ids,
        "topic_id_to_umbrellas": topic_id_to_umbrellas,
    }


def write_expanded_json(data: dict[str, Any], path: Path) -> None:
    path.write_text(json.dumps(data, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")


def generate_seed_011(expanded: dict[str, Any], path: Path) -> dict[str, int]:
    """
    Seed warning_categories + warning_category_topics based on expanded.json.
    Prunes category-topic mappings that are no longer allowed.
    """
    ts = datetime.now().strftime("%Y%m%d_%H%M%S")

    umbrellas: list[dict[str, Any]] = expanded["umbrellas"]
    umbrella_to_topic_ids: dict[str, list[int]] = expanded["umbrella_to_topic_ids"]

    # Map umbrella_id -> umbrella_name
    umbrella_name_by_id: dict[str, str] = {u["umbrella_id"]: u["umbrella_name"] for u in umbrellas}

    # Build allowed mappings (category_name, dtdd_topic_id)
    allowed: list[tuple[str, int]] = []
    for uid, ids in umbrella_to_topic_ids.items():
        cname = umbrella_name_by_id[uid]
        for tid in ids:
            allowed.append((cname, int(tid)))

    # De-dupe allowed mappings
    allowed = sorted(set(allowed), key=lambda x: (x[0], x[1]))

    lines: list[str] = []
    lines.append("\\encoding UTF8\n")
    lines.append(f"-- Generated {ts}\n")
    lines.append("-- Source: python_scripts/content_warnings/taxonomy/claude_taxonomy.yml\n")
    lines.append("-- Builds: warning_categories, warning_category_topics\n")
    lines.append("BEGIN;\n\n")

    # Ensure categories exist / upsert
    lines.append("-- Upsert categories\n")
    for u in umbrellas:
        lines.append(
            "INSERT INTO public.warning_categories (category_name)\n"
            f"VALUES ({sql_str(u['umbrella_name'])})\n"
            "ON CONFLICT (category_name) DO UPDATE SET updated_at = now();\n\n"
        )

    # Temp table of allowed mappings
    lines.append("-- Allowed category-topic mappings (from YAML)\n")
    lines.append(
        "CREATE TEMP TABLE _allowed_category_topics (\n"
        "  category_name text NOT NULL,\n"
        "  dtdd_topic_id int NOT NULL,\n"
        "  PRIMARY KEY (category_name, dtdd_topic_id)\n"
        ") ON COMMIT DROP;\n"
    )
    if allowed:
        lines.append("INSERT INTO _allowed_category_topics (category_name, dtdd_topic_id) VALUES\n")
        lines.append(",\n".join([f"  ({sql_str(c)}, {tid})" for (c, tid) in allowed]))
        lines.append(";\n\n")

    # Prune old mappings
    lines.append("-- Prune mappings no longer present in YAML\n")
    lines.append(
        "DELETE FROM public.warning_category_topics wct\n"
        "USING public.warning_categories wc\n"
        "WHERE wct.category_id = wc.category_id\n"
        "  AND NOT EXISTS (\n"
        "    SELECT 1\n"
        "    FROM _allowed_category_topics a\n"
        "    WHERE a.category_name = wc.category_name\n"
        "      AND a.dtdd_topic_id = wct.dtdd_topic_id\n"
        "  );\n\n"
    )

    # Insert allowed mappings
    lines.append("-- Insert allowed mappings\n")
    lines.append(
        "INSERT INTO public.warning_category_topics (category_id, dtdd_topic_id)\n"
        "SELECT wc.category_id, a.dtdd_topic_id\n"
        "FROM _allowed_category_topics a\n"
        "JOIN public.warning_categories wc\n"
        "  ON wc.category_name = a.category_name\n"
        "ON CONFLICT DO NOTHING;\n\n"
    )

    lines.append("COMMIT;\n")

    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text("".join(lines), encoding="utf-8")

    return {
        "categories": len(umbrellas),
        "distinct_topics": len({tid for _, tid in allowed}),
        "category_topic_rows": len(allowed),
    }


def generate_seed_012(rows: list[SubcatRow], path: Path) -> dict[str, int]:
    """
    Seed warning_subcategories + warning_subcategory_topics based on YAML subcategory headings.
    Prunes subcategories and mappings no longer present in YAML.
    """
    ts = datetime.now().strftime("%Y%m%d_%H%M%S")

    # Build allowed subcategories: (category_name, subcategory_name)
    allowed_subcats: set[tuple[str, str]] = set()
    # Build allowed subcategory-topic mappings: (category_name, subcategory_name, dtdd_topic_id)
    allowed_links: set[tuple[str, str, int]] = set()

    for r in rows:
        cname = r.umbrella_name
        sname = r.subcategory_name
        allowed_subcats.add((cname, sname))
        for tid in r.topic_ids:
            allowed_links.add((cname, sname, int(tid)))

    allowed_subcats_list = sorted(allowed_subcats, key=lambda x: (x[0], x[1]))
    allowed_links_list = sorted(allowed_links, key=lambda x: (x[0], x[1], x[2]))

    lines: list[str] = []
    lines.append("\\encoding UTF8\n")
    lines.append(f"-- Generated {ts}\n")
    lines.append("-- Source: python_scripts/content_warnings/taxonomy/claude_taxonomy.yml\n")
    lines.append("-- Builds: warning_subcategories, warning_subcategory_topics\n")
    lines.append("BEGIN;\n\n")

    # Temp tables
    lines.append(
        "CREATE TEMP TABLE _allowed_subcategories (\n"
        "  category_name text NOT NULL,\n"
        "  subcategory_name text NOT NULL,\n"
        "  PRIMARY KEY (category_name, subcategory_name)\n"
        ") ON COMMIT DROP;\n"
    )
    if allowed_subcats_list:
        lines.append("INSERT INTO _allowed_subcategories (category_name, subcategory_name) VALUES\n")
        lines.append(",\n".join([f"  ({sql_str(c)}, {sql_str(s)})" for (c, s) in allowed_subcats_list]))
        lines.append(";\n\n")

    lines.append(
        "CREATE TEMP TABLE _allowed_subcategory_topics (\n"
        "  category_name text NOT NULL,\n"
        "  subcategory_name text NOT NULL,\n"
        "  dtdd_topic_id int NOT NULL,\n"
        "  PRIMARY KEY (category_name, subcategory_name, dtdd_topic_id)\n"
        ") ON COMMIT DROP;\n"
    )
    if allowed_links_list:
        lines.append("INSERT INTO _allowed_subcategory_topics (category_name, subcategory_name, dtdd_topic_id) VALUES\n")
        lines.append(
            ",\n".join([f"  ({sql_str(c)}, {sql_str(s)}, {tid})" for (c, s, tid) in allowed_links_list])
        )
        lines.append(";\n\n")

    # Upsert subcategories
    lines.append("-- Upsert subcategories\n")
    lines.append(
        "INSERT INTO public.warning_subcategories (category_id, subcategory_name)\n"
        "SELECT wc.category_id, a.subcategory_name\n"
        "FROM _allowed_subcategories a\n"
        "JOIN public.warning_categories wc\n"
        "  ON wc.category_name = a.category_name\n"
        "ON CONFLICT (category_id, subcategory_name)\n"
        "DO UPDATE SET updated_at = now();\n\n"
    )

    # Prune subcategory-topic links that are no longer allowed
    lines.append("-- Prune subcategory-topic mappings no longer present in YAML\n")
    lines.append(
        "DELETE FROM public.warning_subcategory_topics wst\n"
        "USING public.warning_subcategories ws, public.warning_categories wc\n"
        "WHERE wst.subcategory_id = ws.subcategory_id\n"
        "  AND ws.category_id = wc.category_id\n"
        "  AND NOT EXISTS (\n"
        "    SELECT 1\n"
        "    FROM _allowed_subcategory_topics a\n"
        "    WHERE a.category_name = wc.category_name\n"
        "      AND a.subcategory_name = ws.subcategory_name\n"
        "      AND a.dtdd_topic_id = wst.dtdd_topic_id\n"
        "  );\n\n"
    )

    # Insert allowed subcategory-topic links
    lines.append("-- Insert allowed subcategory-topic mappings\n")
    lines.append(
        "INSERT INTO public.warning_subcategory_topics (subcategory_id, dtdd_topic_id)\n"
        "SELECT ws.subcategory_id, a.dtdd_topic_id\n"
        "FROM _allowed_subcategory_topics a\n"
        "JOIN public.warning_categories wc\n"
        "  ON wc.category_name = a.category_name\n"
        "JOIN public.warning_subcategories ws\n"
        "  ON ws.category_id = wc.category_id\n"
        " AND ws.subcategory_name = a.subcategory_name\n"
        "ON CONFLICT DO NOTHING;\n\n"
    )

    # Prune subcategories no longer present in YAML (after pruning links)
    lines.append("-- Prune subcategories no longer present in YAML\n")
    lines.append(
        "DELETE FROM public.warning_subcategories ws\n"
        "USING public.warning_categories wc\n"
        "WHERE ws.category_id = wc.category_id\n"
        "  AND NOT EXISTS (\n"
        "    SELECT 1\n"
        "    FROM _allowed_subcategories a\n"
        "    WHERE a.category_name = wc.category_name\n"
        "      AND a.subcategory_name = ws.subcategory_name\n"
        "  );\n\n"
    )

    lines.append("COMMIT;\n")

    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text("".join(lines), encoding="utf-8")

    return {
        "subcategories": len(allowed_subcats_list),
        "subcategory_topic_rows": len(allowed_links_list),
        "distinct_topics_in_subcats": len({tid for _, _, tid in allowed_links_list}),
    }


def main() -> None:
    umbrella_names, yaml_obj = load_yaml_structure(YAML_PATH)
    topic_names = load_catalog_topic_names(CATALOG_JSON)

    rows = build_rows(umbrella_names, yaml_obj)

    expanded = build_expanded_json(rows, topic_names)
    write_expanded_json(expanded, EXPANDED_JSON_OUT)

    # Summary numbers
    unique_topic_ids = {t["dtdd_topic_id"] for t in expanded["topics"]}
    multi_umbrella = sum(1 for k, v in expanded["topic_id_to_umbrellas"].items() if len(v) > 1)
    total_category_topic_rows = sum(len(v) for v in expanded["umbrella_to_topic_ids"].values())

    stats_011 = generate_seed_011(expanded, SEED_011_OUT)
    stats_012 = generate_seed_012(rows, SEED_012_OUT)

    print("Built artifacts from claude_taxonomy.yml")
    print(f"  expanded.json written: {EXPANDED_JSON_OUT}")
    print(f"    unique topic_ids: {len(unique_topic_ids)}")
    print(f"    topics mapped to >1 umbrella: {multi_umbrella}")
    print(f"    total category-topic mappings (incl duplicates across umbrellas): {total_category_topic_rows}")
    print(f"  011_seed_warning_taxonomy.sql written: {SEED_011_OUT}")
    print(f"    categories: {stats_011['categories']}")
    print(f"    distinct topics mapped: {stats_011['distinct_topics']}")
    print(f"    category-topic rows: {stats_011['category_topic_rows']}")
    print(f"  012_seed_warning_subcategories.sql written: {SEED_012_OUT}")
    print(f"    subcategories: {stats_012['subcategories']}")
    print(f"    subcategory-topic rows: {stats_012['subcategory_topic_rows']}")
    print(f"    distinct topics in subcats: {stats_012['distinct_topics_in_subcats']}")


if __name__ == "__main__":
    main()