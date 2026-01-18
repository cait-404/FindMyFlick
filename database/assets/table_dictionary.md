# FindMyFlick database: table dictionary (readable)

This document describes the Postgres tables in `database/schema/`, what each table is used for, how the tables link together for search, and which tables are populated by the seed scripts in `database/seed/`.

## How the tables link together

### Main keys

- **Core title key (join hub):** `movies.imdb_id` (IMDb `tt...` id)
- **TMDB ids:**
  - `movies.tmdb_id` (movie id in TMDB)
  - `people.tmdb_person_id`
  - `genres.tmdb_genre_id`
  - `keywords.tmdb_keyword_id`
  - `collections.tmdb_collection_id`
  - `streaming_providers.tmdb_provider_id`
- **DTDD ids:**
  - `warnings.dtdd_topic_id`
  - `movie_dtdd_titles.dtdd_title_id` and `movie_dtdd_titles.dtdd_media_id` (DTDD match / identity bridge)

### Relationship summary (search-relevant)

- `movies` 1-to-many `movie_streaming` (availability by provider + offer type)
- `movies` many-to-many `genres` through `movie_genres`
- `movies` many-to-many `keywords` through `movie_keywords`
- `movies` 1-to-0/1 `movie_collections` (a movie can belong to at most one collection)
- `movies` 1-to-many `movie_cast` and `movie_crew` (credits), with `people` as the shared dimension
- `movies` many-to-many `warnings` through `movie_warnings` (DTDD content warning answers per topic)

**Practical search note:** for “include/exclude” behavior (include Tom Cruise, exclude Helen Hunt), it is usually easiest to start from `movies` and use `EXISTS (...)` and `NOT EXISTS (...)` subqueries against bridge tables, instead of joining everything into one wide row (which multiplies rows fast).

## Seed coverage (what works out of the box)

Your seed folder is designed to make the project usable immediately without populating every possible table from the APIs.

The following tables are populated by the provided seed scripts:

- `movies` (`001_seed_us_popular_100_movies.sql`)  
- `streaming_providers`, `movie_streaming` (`002_seed_us_popular_100_movie_streaming.sql`)  
- `genres`, `movie_genres` (`003_seed_us_popular_100_movie_genres.sql`)  
- `people`, `movie_cast`, `movie_crew` (`004_seed_us_popular_100_people_and_credits.sql`)  
- `keywords`, `movie_keywords` (`006_seed_us_popular_100_movie_keywords.sql`)  
- `collections`, `movie_collections` (`007_seed_us_popular_100_movie_collections.sql`)  
- `warnings` (`000_seed_warnings_catalog.sql`)  
- `movie_warnings` and `movie_dtdd_titles` (DTDD results + matches) (`008_seed_us_popular_100_movie_warnings.sql`, `009_seed_us_streamable_dtdd_media_map.sql`)  

So, yes: the database includes **seed data intended to support searches immediately**, at least for the seeded movie subset.

---

## Table index

- Core titles: `movies`
- Streaming availability: `streaming_providers`, `movie_streaming`
- Genres: `genres`, `movie_genres`
- People + credits: `people`, `movie_cast`, `movie_crew`
- Keywords: `keywords`, `movie_keywords`
- Collections: `collections`, `movie_collections`
- Content warnings: `warnings`, `movie_warnings`
- DTDD matching / identity bridge: `movie_dtdd_titles`

---

## `movies`

Defined in: `database/schema/001_init.sql`

Used for:
- One row per title returned in search results.
- Stores core movie attributes (title, year, runtime, summary, poster, rating).

Primary key:
- `imdb_id`

Foreign keys:
- None (this is the hub table).

Seeded:
- Yes (`001_seed_us_popular_100_movies.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK | Stable title id (IMDb `tt...`) used as the join hub. |
| tmdb_id | INTEGER | YES | UNIQUE | TMDB movie id for ingestion and crosswalks. |
| title | TEXT | NO |  | Display title. |
| release_year | INTEGER | NO | CHECK | Release year (1878–2100). |
| mpaa_rating | VARCHAR(10) | YES |  | US MPAA rating (G, PG, PG-13, R, NC-17). |
| runtime_minutes | INTEGER | YES | CHECK | Runtime in minutes. |
| plot_summary | TEXT | YES |  | Plot overview / description. |
| poster_url | TEXT | YES |  | Full poster URL (TMDB image base + path). |
| original_language | CHAR(2) | YES |  | Original language ISO code (e.g., `en`). |
| media_type | VARCHAR(10) | NO | CHECK | `movie` or `tv` (defaults to `movie`). |
| tagline | TEXT | YES |  | Tagline when available. |
| status | TEXT | YES |  | Release status from TMDB when available. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `streaming_providers`

Defined in: `database/schema/002_streaming_providers.sql`

Used for:
- Provider catalog (Netflix, Hulu, etc.) using TMDB watch provider ids.

Primary key:
- `tmdb_provider_id`

Seeded:
- Yes (`002_seed_us_popular_100_movie_streaming.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_provider_id | INTEGER | NO | PK | TMDB provider id. |
| provider_name | TEXT | NO |  | Display name. |
| logo_path | TEXT | YES |  | TMDB logo path (not a full URL). |
| display_priority | INTEGER | YES |  | TMDB display ordering hint. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_streaming`

Defined in: `database/schema/002_streaming_providers.sql`

Used for:
- Movie availability in the US (which provider, and what offer type).

Primary key:
- `(imdb_id, tmdb_provider_id, offer_type)`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_provider_id` → `streaming_providers(tmdb_provider_id)`

Seeded:
- Yes (`002_seed_us_popular_100_movie_streaming.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie being offered. |
| tmdb_provider_id | INTEGER | NO | PK/FK | Provider offering it. |
| offer_type | TEXT | NO | PK/CHECK | `subscription`, `free`, `free_with_ads`, `rent`, `buy`. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `genres`

Defined in: `database/schema/003_genres.sql`

Used for:
- TMDB genre catalog.

Primary key:
- `tmdb_genre_id`

Seeded:
- Yes (`003_seed_us_popular_100_movie_genres.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_genre_id | INTEGER | NO | PK | TMDB genre id. |
| genre_name | TEXT | NO | UNIQUE | Genre display name. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_genres`

Defined in: `database/schema/003_genres.sql`

Used for:
- Many-to-many bridge: which genres apply to which movies.

Primary key:
- `(imdb_id, tmdb_genre_id)`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_genre_id` → `genres(tmdb_genre_id)`

Seeded:
- Yes (`003_seed_us_popular_100_movie_genres.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie. |
| tmdb_genre_id | INTEGER | NO | PK/FK | Genre. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `people`

Defined in: `database/schema/004_people_and_credits.sql`

Used for:
- TMDB person catalog (actors, directors, writers, etc.).

Primary key:
- `tmdb_person_id`

Seeded:
- Yes (`004_seed_us_popular_100_people_and_credits.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_person_id | INTEGER | NO | PK | TMDB person id. |
| person_name | TEXT | NO |  | Person display name. |
| known_for_department | TEXT | YES |  | TMDB’s “known for” department. |
| profile_url | TEXT | YES |  | Full profile image URL (TMDB). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_cast`

Defined in: `database/schema/004_people_and_credits.sql`

Used for:
- TMDB cast credits for a movie (actors + roles), de-duplicated by `tmdb_credit_id`.

Primary key:
- `tmdb_credit_id`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_person_id` → `people(tmdb_person_id)`

Seeded:
- Yes (`004_seed_us_popular_100_people_and_credits.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_credit_id | TEXT | NO | PK | Stable unique credit id from TMDB. |
| imdb_id | VARCHAR(16) | NO | FK | Movie. |
| tmdb_person_id | INTEGER | NO | FK | Person. |
| character_name | TEXT | YES |  | Character name. |
| cast_order | INTEGER | YES |  | Billing order (lower is earlier). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `movie_crew`

Defined in: `database/schema/004_people_and_credits.sql`

Used for:
- TMDB crew credits for a movie (director, writer, producer, etc.).

Primary key:
- `tmdb_credit_id`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_person_id` → `people(tmdb_person_id)`

Seeded:
- Yes (`004_seed_us_popular_100_people_and_credits.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_credit_id | TEXT | NO | PK | Stable unique credit id from TMDB. |
| imdb_id | VARCHAR(16) | NO | FK | Movie. |
| tmdb_person_id | INTEGER | NO | FK | Person. |
| department | TEXT | YES |  | Department (e.g., Directing, Writing). |
| job | TEXT | YES |  | Job title (e.g., Director, Writer, Screenplay). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `keywords`

Defined in: `database/schema/006_keywords.sql`

Used for:
- TMDB keyword catalog.

Primary key:
- `tmdb_keyword_id`

Seeded:
- Yes (`006_seed_us_popular_100_movie_keywords.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_keyword_id | INTEGER | NO | PK | TMDB keyword id. |
| keyword_name | TEXT | NO |  | Keyword text. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_keywords`

Defined in: `database/schema/006_keywords.sql`

Used for:
- Many-to-many bridge: which keywords apply to which movies.

Primary key:
- `(imdb_id, tmdb_keyword_id)`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_keyword_id` → `keywords(tmdb_keyword_id)`

Seeded:
- Yes (`006_seed_us_popular_100_movie_keywords.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie. |
| tmdb_keyword_id | INTEGER | NO | PK/FK | Keyword. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `collections`

Defined in: `database/schema/007_collections.sql`

Used for:
- TMDB collection catalog (franchises/series like “The Godfather Collection”).

Primary key:
- `tmdb_collection_id`

Seeded:
- Yes (`007_seed_us_popular_100_movie_collections.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| tmdb_collection_id | INTEGER | NO | PK | TMDB collection id. |
| collection_name | TEXT | NO |  | Collection name. |
| poster_url | TEXT | YES |  | Full poster URL (TMDB). |
| backdrop_url | TEXT | YES |  | Full backdrop URL (TMDB). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_collections`

Defined in: `database/schema/007_collections.sql`

Used for:
- Map a movie to its collection (at most one collection per movie).

Primary key:
- `imdb_id` (enforces “0 or 1 collection per movie”)

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `tmdb_collection_id` → `collections(tmdb_collection_id)`

Seeded:
- Yes (`007_seed_us_popular_100_movie_collections.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie. |
| tmdb_collection_id | INTEGER | NO | FK | Collection. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |

---

## `warnings`

Defined in: `database/schema/001_init.sql`

Used for:
- DTDD warning/topic taxonomy catalog (“a dog dies”, “vomit”, etc.).

Primary key:
- `dtdd_topic_id`

Foreign keys:
- `parent_dtdd_topic_id` → `warnings(dtdd_topic_id)` (self-referential hierarchy)

Seeded:
- Yes (`000_seed_warnings_catalog.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| dtdd_topic_id | INTEGER | NO | PK | DTDD topic id. |
| topic_name | TEXT | NO |  | Human-readable topic name. |
| topic_type | TEXT | YES |  | DTDD topic type when available. |
| parent_dtdd_topic_id | INTEGER | YES | FK | Parent topic id (hierarchy). |
| tier | SMALLINT | YES | CHECK | Optional topic tier (1–4). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## `movie_warnings`

Defined in: `database/schema/001_init.sql` plus alterations in `database/schema/008_dtdd.sql`

Used for:
- Bridge table storing DTDD warning answers per movie per topic.

Primary key:
- `(imdb_id, dtdd_topic_id)`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`
- `dtdd_topic_id` → `warnings(dtdd_topic_id)`

Seeded:
- Yes (`008_seed_us_popular_100_movie_warnings.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie. |
| dtdd_topic_id | INTEGER | NO | PK/FK | Topic (warning). |
| source | TEXT | NO |  | Source system label (defaults to `DTDD`). |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| answer | TEXT | YES |  | DTDD answer bucket (commonly `yes`, `no`, `unknown`). |
| is_spoiler | BOOLEAN | YES |  | Whether the warning is marked as spoiler. |
| warning_comment | TEXT | YES |  | Optional comment / detail text. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp for answer/comment changes. |

---

## `movie_dtdd_titles`

Defined in: `database/schema/008_dtdd.sql` plus expansions in `database/schema/009_dtdd_match_bridge.sql`

Used for:
- Identity / match bridge between your `movies.imdb_id` and DTDD’s media/title identifiers.
- Stores how the match was made, and DTDD’s view of external ids.

Primary key:
- `imdb_id`

Foreign keys:
- `imdb_id` → `movies(imdb_id)`

Seeded:
- Yes (`008_seed_us_popular_100_movie_warnings.sql`, `009_seed_us_streamable_dtdd_media_map.sql`)

| Field | Type | Null? | Key | Purpose |
|---|---|---|---|---|
| imdb_id | VARCHAR(16) | NO | PK/FK | Movie. |
| dtdd_title_id | INTEGER | NO | UNIQUE | DTDD title id (older DTDD id field used in some endpoints). |
| dtdd_media_id | INTEGER | YES | UNIQUE | DTDD media id (preferred stable id when present). |
| dtdd_imdb_id | TEXT | YES |  | IMDb id as returned by DTDD (may be blank). |
| dtdd_tmdb_id | INTEGER | YES |  | TMDB id as returned by DTDD (may be 0/blank). |
| dtdd_title | TEXT | YES |  | Title string as returned by DTDD. |
| dtdd_release_year | INTEGER | YES |  | Release year as returned by DTDD. |
| match_method | TEXT | YES |  | How the match was made (`imdb_tt`, `tmdb_from_search`, `title_year_fallback`, etc.). |
| match_score | NUMERIC(6,3) | YES |  | Heuristic score used by your matching script. |
| created_at | TIMESTAMPTZ | NO |  | Insert timestamp. |
| updated_at | TIMESTAMPTZ | NO |  | Update timestamp. |

---

## Views (search-friendly helpers)

Views live under `database/views/` and are applied by `database/views/_apply_views.sql`.

Most relevant views for search and UI:

- `v_movies_streamable_us` and `v_movies_streamable_us_agg`  
  Streamable US titles and aggregated providers per movie.
- `v_movie_people_credits` and `v_movie_credits_agg`  
  Person-level credit rows (good for searching) and aggregated display fields (good for UI cards).
- `v_movie_warnings` and `v_movie_warnings_agg`  
  Warning rows and aggregated buckets (`yes/no/unknown`).
- `v_movie_keywords` and `v_movie_keywords_agg`
- `v_movie_collections` and `v_movie_collections_agg`

