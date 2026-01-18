/*
007_collections.sql
-------------------
Purpose:
  Store TMDB collections (franchises/series) and map movies to a collection.

Notes:
  - In TMDB, a movie belongs to at most one collection via belongs_to_collection.
  - We model that as: one row per movie in movie_collections (imdb_id is PK).
*/

BEGIN;

-- =========================
-- collections (TMDB collection record)
-- =========================
CREATE TABLE IF NOT EXISTS public.collections (
    tmdb_collection_id   INTEGER PRIMARY KEY,
    collection_name      TEXT NOT NULL,
    poster_url           TEXT,
    backdrop_url         TEXT,
    created_at           TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_collections_name
ON public.collections (collection_name);

-- ==================================
-- movie_collections (movie -> collection)
-- ==================================
CREATE TABLE IF NOT EXISTS public.movie_collections (
    imdb_id              VARCHAR(16) PRIMARY KEY,
    tmdb_collection_id   INTEGER NOT NULL,
    created_at           TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movie_collections_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_collections_collection_fk
        FOREIGN KEY (tmdb_collection_id) REFERENCES public.collections (tmdb_collection_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_collections_collection
ON public.movie_collections (tmdb_collection_id);

COMMIT;