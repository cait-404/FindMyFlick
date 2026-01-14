"""
!!! THIS SCRIPT DOES NOT PRODUCE OUTPUT !!!

schema_utils.py
----------------
Purpose:
    This file contains shared helper functions used by the TMDB and DoesTheDogDie
    schema extraction scripts. It provides reusable code for:
        - Loading API keys from the .env file
        - Fetching JSON responses from an API endpoint
        - Flattening nested JSON objects into a table-like list of fields
        - Writing the results into a Markdown (.md) file

    These helpers are imported and used by the schema_*.py scripts in the tmdb/ 
    and dtdd/ folders.

Usage:
    DO NOT run this script directly.
    Instead, run one of the endpoint scripts such as:
        python python_scripts/tmdb/schema_movie_core.py

    Those scripts call the functions here to create Markdown tables that list:
        - Each field path
        - Its inferred data type
        - A short sample value
"""

import os
import json
import requests
from collections import deque
from dotenv import load_dotenv

MAX_SAMPLE_LEN = 160

def load_api_key(name: str) -> str:
    load_dotenv()
    key = os.getenv(name)
    if not key:
        raise RuntimeError(f"Missing required environment variable: {name}")
    return key

def fetch_json(url, params=None, headers=None, timeout=30):
    r = requests.get(url, params=params, headers=headers, timeout=timeout)

    # Raise on HTTP errors (401/403/429/etc.)
    try:
        r.raise_for_status()
    except requests.HTTPError as e:
        snippet = (r.text or "")[:500]
        raise RuntimeError(
            f"HTTP error for {r.url}\n"
            f"Status: {r.status_code}\n"
            f"Content-Type: {r.headers.get('Content-Type')}\n"
            f"Body (first 500 chars):\n{snippet}"
        ) from e

    # If status is OK but body isn't JSON, show what it is
    try:
        return r.json()
    except Exception as e:
        snippet = (r.text or "")[:500]
        raise RuntimeError(
            f"Response was not JSON for {r.url}\n"
            f"Status: {r.status_code}\n"
            f"Content-Type: {r.headers.get('Content-Type')}\n"
            f"Body (first 500 chars):\n{snippet}"
        ) from e

def infer_type(value) -> str:
    if value is None:
      return "null"
    if isinstance(value, bool):
      return "boolean"
    if isinstance(value, int):
      return "integer"
    if isinstance(value, float):
      return "number"
    if isinstance(value, str):
      return "string"
    if isinstance(value, list):
      return "array"
    if isinstance(value, dict):
      return "object"
    return type(value).__name__

def _short(val) -> str:
    try:
        s = json.dumps(val, ensure_ascii=False)
    except Exception:
        s = str(val)
    if len(s) > MAX_SAMPLE_LEN:
        s = s[:MAX_SAMPLE_LEN] + "…"
    return s

def flatten(obj, prefix=""):
    """
    Yield tuples of (path, sample_value, type).
    For arrays, we show [0] path and sample the first element (if any).
    """
    q = deque([(prefix, obj)])
    seen = set()
    while q:
        path, val = q.popleft()
        t = infer_type(val)

        if isinstance(val, dict):
            if path:
                yield (path, _short(val), t)
            for k, v in val.items():
                key = f"{path}.{k}" if path else k
                q.append((key, v))

        elif isinstance(val, list):
            yield (path or "[]", _short(val[:1]), "array")
            if val:
                head = val[0]
                key = f"{path}[0]" if path else "[0]"
                q.append((key, head))

        else:
            yield (path or "$", _short(val), t)

def write_md(outfile: str, title: str, source_line: str, data: dict | list):
    rows = []
    for p, sample, t in flatten(data):
        rows.append((p, t, sample))

    rows_sorted = sorted(rows, key=lambda r: r[0])

    lines = []
    lines.append(f"# {title}")
    lines.append("")
    lines.append(source_line)
    lines.append("")
    lines.append("| Field path | Type | Sample value |")
    lines.append("|---|---|---|")
    for p, t, s in rows_sorted:
        # escape pipes in sample
        s = str(s).replace("|", "\\|")
        lines.append(f"| `{p}` | `{t}` | {s} |")
    lines.append("")
    lines.append(f"_Total unique paths: {len(rows_sorted)}_")
    os.makedirs(os.path.dirname(outfile), exist_ok=True)
    with open(outfile, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))