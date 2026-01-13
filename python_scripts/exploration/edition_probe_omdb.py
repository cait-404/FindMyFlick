"""
edition_probe_omdb.py
---------------------
Purpose:
  Test whether OMDb distinguishes "standard" vs "extended cut" as separate items.

How it works:
  - Calls OMDb by title (t=...) for two queries
  - Prints Title/Year/Runtime/imdbID/Type
  - Also prints Response/Error so we can see if a query fails

Run (from project root):
  python -m python_scripts.exploration.edition_probe_omdb
"""

from python_scripts.shared.schema_utils import load_api_key, fetch_json


def fetch_by_title(title: str, year: str | None = None):
    api_key = load_api_key("OMDB_API_KEY")
    url = "http://www.omdbapi.com/"
    params = {
        "t": title,
        "plot": "short",
        "r": "json",
        "apikey": api_key,
    }
    if year:
        params["y"] = year
    return fetch_json(url, params=params)


def summarize(label: str, data: dict):
    print(f"\n--- {label} ---")
    print("Response:", data.get("Response"))
    if data.get("Response") == "False":
        print("Error:", data.get("Error"))
        return

    for k in ["Title", "Year", "Runtime", "Rated", "imdbID", "Type", "Poster"]:
        print(f"{k}: {data.get(k)}")


def main():
    print("\nEnter two OMDb title queries to compare (standard vs extended).")
    t1 = input("Query 1 (standard): ").strip()
    t2 = input("Query 2 (alternate/extended): ").strip()
    y = input("Optional year (press Enter to skip): ").strip() or None

    d1 = fetch_by_title(t1, y)
    d2 = fetch_by_title(t2, y)

    summarize(t1, d1)
    summarize(t2, d2)

    if d1.get("Response") == "True" and d2.get("Response") == "True":
        print("\n=== Quick comparison ===")
        print("IMDb match:", "YES" if d1.get("imdbID") == d2.get("imdbID") else "NO")
        print("Runtime match:", "YES" if d1.get("Runtime") == d2.get("Runtime") else "NO")


if __name__ == "__main__":
    main()
