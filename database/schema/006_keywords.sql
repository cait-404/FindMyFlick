/*
006_keywords.sql
----------------
Purpose:
  Store TMDB keywords and map them to your seeded movies.

Tables:
  - keywords
  - movie_keywords
*/

BEGIN;

-- ======================
-- keywords (TMDB keyword)
-- ======================
CREATE TABLE IF NOT EXISTS public.keywords (
    tmdb_keyword_id  INTEGER PRIMARY KEY,
    keyword_name     TEXT NOT NULL,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_keywords_name
ON public.keywords (keyword_name);

-- ==================================
-- movie_keywords (movie <-> keyword)
-- ==================================
CREATE TABLE IF NOT EXISTS public.movie_keywords (
    imdb_id          VARCHAR(16) NOT NULL,
    tmdb_keyword_id  INTEGER NOT NULL,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movie_keywords_pkey PRIMARY KEY (imdb_id, tmdb_keyword_id),

    CONSTRAINT movie_keywords_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_keywords_keyword_fk
        FOREIGN KEY (tmdb_keyword_id) REFERENCES public.keywords (tmdb_keyword_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_keywords_keyword
ON public.movie_keywords (tmdb_keyword_id);

COMMIT;