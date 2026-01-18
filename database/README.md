# FindMyFlick Database (Postgres)

This folder contains everything needed to build the **local Postgres database** used by FindMyFlick:
- `schema/` → tables, constraints, indexes, and extensions
- `seed/` → starter dataset (so the frontend/backend can build against real rows)
- `views/` → developer-friendly query surfaces (aggregations and joins)

---

## Folder Layout

This README lives in `database/` and documents only the database folder (schema, seed, and views).

```text
database/
├── notes/                                                  # Working notes and scratch files (not used by build scripts)
│
├── schema/                                                 # Table definitions, constraints, indexes (run in order)
│   ├── 001_init.sql                                        # Core tables: movies + baseline foundation tables
│   ├── 002_streaming_providers.sql                         # Streaming providers + movie-to-provider tables
│   ├── 003_genres.sql                                      # Genres + movie-to-genre tables
│   ├── 004_people_and_credits.sql                          # People + credits tables (cast and crew) and relationships
│   ├── 005_search_extensions_and_indexes.sql               # Search extensions and indexes for faster lookups
│   ├── 006_keywords.sql                                    # Keywords + movie-to-keyword tables
│   ├── 007_collections.sql                                 # Collections + movie-to-collection tables
│   ├── 008_dtdd.sql                                        # DTDD topic and warning tables (warnings catalog and mappings)
│   └── 009_dtdd_match_bridge.sql                           # DTDD match bridge table (movies.imdb_id to DTDD media ids)
│
├── seed/                                                   # Seed data for a starter dataset (run after schema)
│   ├── 000_seed_warnings_catalog.sql                       # Seed the local content warning catalog (baseline taxonomy list)
│   ├── 001_seed_us_popular_100_movies.sql                  # Seed starter movie list (US popular 100)
│   ├── 002_seed_us_popular_100_movie_streaming.sql         # Seed streaming availability for starter movies
│   ├── 003_seed_us_popular_100_movie_genres.sql            # Seed genre mappings for starter movies
│   ├── 004_seed_us_popular_100_people_and_credits.sql      # Seed cast/crew for starter movies
│   ├── 006_seed_us_popular_100_movie_keywords.sql          # Seed keywords for starter movies
│   ├── 007_seed_us_popular_100_movie_collections.sql       # Seed collections for starter movies
│   ├── 008_seed_us_popular_100_movie_warnings.sql          # Seed warning triggers for starter movies (non-DTDD baseline)
│   └── 009_seed_us_streamable_dtdd_media_map.sql           # Seed DTDD media id mapping for streamable US movies
│
├── views/                                                  # Read-only helper views for the application
│   ├── _apply_views.sql                                    # Drop and recreate all views in the correct dependency order
│   ├── v_movie_collections.sql                             # Base view: movie to collections
│   ├── v_movie_collections_agg.sql                         # Aggregated collections per movie (app-friendly)
│   ├── v_movie_credits_agg.sql                             # Aggregated credits per movie (cast/crew summary)
│   ├── v_movie_keywords.sql                                # Base view: movie to keywords
│   ├── v_movie_keywords_agg.sql                            # Aggregated keywords per movie (app-friendly)
│   ├── v_movie_people_credits.sql                          # Base view: people and their movie credit rows
│   ├── v_movie_warnings.sql                                # Base view: movie to warnings (includes DTDD-derived rows when present)
│   ├── v_movie_warnings_agg.sql                            # Aggregated warnings per movie (app-friendly)
│   ├── v_movies_streamable_us.sql                          # Base view: movies available to stream in the US
│   └── v_movies_streamable_us_agg.sql                      # Aggregated streamable US view (app-friendly)
│
└── README.md                                               # This file

---

## Build and Use the Database (schema → seed → views)

Run these from the repo root.

### 1. Create the database (if needed)
If `findmyflick` already exists, skip this.

psql -h localhost -p 5432 -U postgres -c "CREATE DATABASE findmyflick;"

### 2. Apply schema files (creates tables, constraints, indexes)
Run in order:

psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/001_init.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/002_streaming_providers.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/003_genres.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/004_people_and_credits.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/005_search_extensions_and_indexes.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/006_keywords.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/007_collections.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/008_dtdd.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/schema/009_dtdd_match_bridge.sql

### 3. Apply seed files (loads a starter dataset)
Run in order:

psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/000_seed_warnings_catalog.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/001_seed_us_popular_100_movies.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/002_seed_us_popular_100_movie_streaming.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/003_seed_us_popular_100_movie_genres.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/004_seed_us_popular_100_people_and_credits.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/006_seed_us_popular_100_movie_keywords.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/007_seed_us_popular_100_movie_collections.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/008_seed_us_popular_100_movie_warnings.sql
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/seed/009_seed_us_streamable_dtdd_media_map.sql

### 4. Apply views (rebuild all views in dependency order)
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/views/_apply_views.sql

### 5. Verify the build worked (quick checks)

psql -h localhost -p 5432 -U postgres -d findmyflick -c "\dt public.*"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "\dv public.v_*"

# Row counts (starter dataset)
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT COUNT(*) AS movies FROM public.movies;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT COUNT(*) AS stream_rows FROM public.movie_streaming;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT COUNT(*) AS warnings FROM public.movie_warnings;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT COUNT(*) AS dtdd_map_rows, COUNT(*) FILTER (WHERE dtdd_media_id IS NOT NULL) AS mapped FROM public.movie_dtdd_titles;"

# These should run without errors and return some rows.
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT * FROM public.v_movies_streamable_us_agg LIMIT 10;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT * FROM public.v_movie_warnings_agg LIMIT 10;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT * FROM public.v_movie_credits_agg LIMIT 10;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT * FROM public.v_movie_keywords_agg LIMIT 10;"
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT * FROM public.v_movie_collections_agg LIMIT 10;"

### 6. Notes for teammates (so nobody has to guess)

- Schema files under `database/schema/` define the long-term database structure.
- Seed files under `database/seed/` load a starter dataset for development and UI work.
- Views under `database/views/` provide “frontend-friendly” query shapes (aggregated lists, filtered US-streamable set, etc.).
- If someone changes any view SQL files, re-run `database/views/_apply_views.sql` to refresh them.

## 7. Common queries (helpful when building UI/API)

### List seeded movies that are streamable in the US
SELECT * FROM public.v_movies_streamable_us_agg
ORDER BY title
LIMIT 50;

### Look up one movie by IMDb id
SELECT *
FROM public.movies
WHERE imdb_id = 'tt31193180';

### Get warnings for a movie (if DTDD warnings were seeded)
SELECT *
FROM public.v_movie_warnings
WHERE imdb_id = 'tt31193180'
ORDER BY warning_key;

### Get aggregated warnings per movie
SELECT *
FROM public.v_movie_warnings_agg
WHERE imdb_id = 'tt31193180';

### Get credits (people + roles) for a movie
SELECT *
FROM public.v_movie_people_credits
WHERE imdb_id = 'tt31193180'
ORDER BY person_name, job;

### Get keywords and collections for a movie
SELECT *
FROM public.v_movie_keywords
WHERE imdb_id = 'tt31193180';

SELECT *
FROM public.v_movie_collections
WHERE imdb_id = 'tt31193180';


## 8. Troubleshooting

### psql prompts for a password and you do not have one
Set a local password for your postgres user (or update your local Postgres auth config), then put DB_* values in your own .env.
This repo does not include a shared .env.

### Views fail to build
Re-run the view script, because it drops and recreates views in dependency order:
psql -h localhost -p 5432 -U postgres -d findmyflick -f database/views/_apply_views.sql

### Tables exist but you have no data
Re-run the seed scripts in order from `database/seed/`.
If you skipped the warnings catalog, the DTDD warnings seed will not have lookup rows to join.

### You want to completely rebuild locally
Drop and recreate the database, then re-run schema, seed, and views.
(Exact commands depend on your local Postgres setup.)


## 9. What is safe to change

- Add new schema files only by appending a new numbered file under `database/schema/` (do not edit old numbered files without telling the team).
- Add new views by creating a new `database/views/v_*.sql` file and updating `_apply_views.sql`.
- Add new seed files by appending a new numbered file under `database/seed/`.

If you change a table name or column name, you must update:
- dependent seed files
- dependent views
- any Python seed script that writes SQL for that table

---

## References

- Portions of the Python scripts were written and debugged with assistance from IntelliSense and integrated AI development tools in Visual Studio Code.


