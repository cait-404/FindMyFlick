"""
extract_edition_notes_from_bundle.py
------------------------------------
Purpose:
    Read a previously saved TMDB "bundle" JSON (from edition_probe_tmdb.py),
    extract any release_dates notes that look like special editions (extended cut,
    unrated, director's cut, etc.), and write a small JSON artifact to
    python_scripts/exploration/output/.

Why:
    TMDB often does NOT represent extended/unrated cuts as separate movie IDs.
    The only signal can be release_dates.note. This script turns that into a
    clean, repeatable output you can use to inform database design.

Run (from project root):
    python -m python_scripts.exploration.extract_edition_notes_from_bundle

Inputs:
    Prompts for the path to a TMDB bundle JSON file.

Outputs:
    Writes a JSON file to:
      python_scripts/exploration/output/<timestamp>_edition_notes_<tmdb_id_or_unknown>.json
"""

from __future__ import annotations

import json
import os
import re
from datetime import datetime
from typing import Any, Dict, List


# Keywords/phrases we treat as "edition-like" signals.
# Add to this list as you find more in the wild.
EDITION_PATTERNS = [
    r"\bextended\b",
    r"\bunrated\b",
    r"\bdirector'?s cut\b",
    r"\bfinal cut\b",
    r"\btheatrical cut\b",
    r"\buncut\b",
    r"\bultimate\b",
    r"\bcollector'?s\b",
    r"\banniversary\b",
    r"\bremastered\b",
    r"\brestored\b",
    r"\bimax\b",
]


def looks_like_edition(note: str) -> bool:
    note_l = (note or "").strip().lower()
    if not note_l:
        return False
    for pat in EDITION_PATTERNS:
        if re.search(pat, note_l, flags=re.IGNORECASE):
            return True
    return False


def safe_tmdb_id(bundle: Dict[str, Any]) -> str:
    core = bundle.get("core") or {}
    tmdb_id = core.get("id")
    return str(tmdb_id) if tmdb_id is not None else "unknown"


def extract_notes(bundle: Dict[str, Any]) -> List[Dict[str, Any]]:
    out: List[Dict[str, Any]] = []
    release_dates = (bundle.get("release_dates") or {}).get("results") or []
    for r in release_dates:
        cc = r.get("iso_3166_1")
        for rd in r.get("release_dates", []) or []:
            note = (rd.get("note") or "").strip()
            if looks_like_edition(note):
                out.append(
                    {
                        "country": cc,
                        "date": rd.get("release_date"),
                        "type": rd.get("type"),
                        "certification": rd.get("certification"),
                        "note": note,
                    }
                )
    return out


def main() -> None:
    bundle_path = input("Path to TMDB bundle JSON file: ").strip().strip('"').strip("'")
    if not bundle_path:
        print("No path provided. Exiting.")
        return

    if not os.path.exists(bundle_path):
        print(f"File not found: {bundle_path}")
        return

    with open(bundle_path, "r", encoding="utf-8") as f:
        bundle = json.load(f)

    tmdb_id = safe_tmdb_id(bundle)
    notes = extract_notes(bundle)

    # Print a quick summary
    print(f"\nTMDB id: {tmdb_id}")
    print(f"Edition-like notes found: {len(notes)}")
    for n in notes:
        print(f"- {n.get('country')} | {n.get('date')} | type={n.get('type')} | {n.get('note')}")

    # Write output JSON
    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    outdir = os.path.join(os.path.dirname(__file__), "output")
    os.makedirs(outdir, exist_ok=True)
    outfile = os.path.join(outdir, f"{ts}_edition_notes_{tmdb_id}.json")

    payload = {
        "tmdb_id": tmdb_id,
        "source_bundle": os.path.basename(bundle_path),
        "extracted_at": ts,
        "edition_notes": notes,
    }

    with open(outfile, "w", encoding="utf-8") as f:
        json.dump(payload, f, indent=2, ensure_ascii=False)

    print(f"\nSaved: {os.path.normpath(outfile)}")


if __name__ == "__main__":
    main()
