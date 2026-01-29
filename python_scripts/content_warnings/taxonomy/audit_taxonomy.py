import json
import yaml
from pathlib import Path
from collections import defaultdict

HERE = Path(__file__).resolve().parent

FILES = {
    "claude_taxonomy_yml": HERE / "claude_taxonomy.yml",
    "expanded_json": HERE / "expanded.json",
    "umbrellas_json": HERE / "umbrellas.json",
    "dtdd_topics_catalog_json": HERE.parent / "data" / "dtdd" / "dtdd_topics_catalog.json",
}

def load_json(p: Path):
    return json.loads(p.read_text(encoding="utf-8"))

def load_yml(p: Path):
    return yaml.safe_load(p.read_text(encoding="utf-8"))

def norm(s):
    return str(s).strip().lower()

def pick_topic_key(obj: dict):
    """
    Try to extract a stable unique identifier.
    Prefer a numeric id if present, else slug-ish, else name.
    """
    for k in ["dtdd_id", "topic_id", "id", "topicId", "topicID"]:
        if k in obj and obj[k] not in (None, ""):
            return f"id:{obj[k]}"
    for k in ["slug", "key", "code"]:
        if k in obj and obj[k]:
            return f"slug:{norm(obj[k])}"
    for k in ["name", "label", "topic", "title"]:
        if k in obj and obj[k]:
            return f"name:{norm(obj[k])}"
    return None

def flatten_topics_from_claude(data):
    # data is dict: { "U01": {...}, "U02": {...}, ... }
    def collect_ints(obj):
        out = set()
        if isinstance(obj, int):
            out.add(obj)
        elif isinstance(obj, list):
            for x in obj:
                out |= collect_ints(x)
        elif isinstance(obj, dict):
            for v in obj.values():
                out |= collect_ints(v)
        return out

    umbrella_to_topics = {}
    all_topics = set()

    if isinstance(data, dict):
        for umb_id, subtree in data.items():
            ints = collect_ints(subtree)
            # store as id:<num> so it matches other parts of the script
            keys = {f"id:{n}" for n in ints}
            umbrella_to_topics[str(umb_id)] = sorted(keys)
            all_topics |= keys

    return umbrella_to_topics, all_topics

def flatten_topics_from_expanded(data):
    # expanded.json: data["topics"] is dict keyed by string topic ids
    topics = data.get("topics", {})
    all_topics = set()

    if isinstance(topics, dict):
        for tid, obj in topics.items():
            # prefer numeric id if present
            if isinstance(obj, dict) and obj.get("topic_id") is not None:
                all_topics.add(f"id:{obj['topic_id']}")
            else:
                all_topics.add(f"id:{tid}")

    return all_topics

def umbrella_map_from_umbrellas_json(data):
    # umbrellas.json has no topic lists, only umbrella definitions
    umbrella_to_topics = {}
    all_topics = set()
    return umbrella_to_topics, all_topics

def flatten_topics_from_catalog(data):
    """
    dtdd_topics_catalog.json could be list/dict.
    We treat any dict items as topics.
    """
    all_topics = set()

    if isinstance(data, list):
        for obj in data:
            if isinstance(obj, dict):
                all_topics.add(pick_topic_key(obj) or f"raw:{norm(str(obj))}")
            else:
                all_topics.add(f"raw:{norm(str(obj))}")

    elif isinstance(data, dict):
        # may be {id: {...}} or { "topics": [...] }
        if "topics" in data and isinstance(data["topics"], list):
            return flatten_topics_from_catalog(data["topics"])
        for k, v in data.items():
            if isinstance(v, dict):
                all_topics.add(pick_topic_key(v) or f"key:{norm(k)}")
            else:
                all_topics.add(f"key:{norm(k)}")

    return all_topics

def main():
    print("Loading files from:")
    for name, p in FILES.items():
        print(f"  {name}: {p}")

    claude = load_yml(FILES["claude_taxonomy_yml"])
    expanded = load_json(FILES["expanded_json"])
    umbrellas = load_json(FILES["umbrellas_json"])
    catalog = load_json(FILES["dtdd_topics_catalog_json"])

    claude_umb_map, claude_topics = flatten_topics_from_claude(claude)
    expanded_topics = flatten_topics_from_expanded(expanded)
    umb_map, umb_topics = umbrella_map_from_umbrellas_json(umbrellas)
    catalog_topics = flatten_topics_from_catalog(catalog)

    print("\nCounts")
    print(f"  claude_taxonomy.yml topics: {len(claude_topics)}")
    print(f"  expanded.json topics:       {len(expanded_topics)}")
    print(f"  umbrellas.json topics:      {len(umb_topics)}")
    print(f"  dtdd_topics_catalog.json:   {len(catalog_topics)}")

    def diff(a, b, a_name, b_name, limit=40):
        only_a = sorted(a - b)
        only_b = sorted(b - a)
        print(f"\nDiff: {a_name} vs {b_name}")
        print(f"  In {a_name} only: {len(only_a)}")
        for x in only_a[:limit]:
            print(f"    - {x}")
        if len(only_a) > limit:
            print(f"    ... ({len(only_a) - limit} more)")
        print(f"  In {b_name} only: {len(only_b)}")
        for x in only_b[:limit]:
            print(f"    - {x}")
        if len(only_b) > limit:
            print(f"    ... ({len(only_b) - limit} more)")

    diff(claude_topics, expanded_topics, "claude", "expanded")
    diff(claude_topics, umb_topics, "claude", "umbrellas")
    diff(claude_topics, catalog_topics, "claude", "dtdd_topics_catalog")

    # Umbrella name alignment check
    claude_umbrellas = set(claude_umb_map.keys())
    json_umbrellas = {u.get("umbrella_id") for u in umbrellas if isinstance(u, dict)}
    json_umbrellas = {x for x in json_umbrellas if x}
    print("\nUmbrella names")
    print(f"  claude umbrellas:    {len(claude_umbrellas)}")
    print(f"  umbrellas.json keys: {len(json_umbrellas)}")
    diff(claude_umbrellas, json_umbrellas, "claude_umbrellas", "umbrellas_json_keys", limit=80)

if __name__ == "__main__":
    main()