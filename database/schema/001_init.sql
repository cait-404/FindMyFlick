/*
001_init.sql
------------
Purpose:
  Initialize the core Postgres schema for FindMyFlick.

Core concept:
  - One row per title you plan to show as a result.
  - Use imdb_id as the primary key for movies (since DTDD uses IMDb ids).
  - Store DTDD topics (warnings) separately, and link them via a bridge table.

Tables:
  - movies
  - warnings
  - movie_warnings

Convenience:
  - view: movies_with_warnings (only movies that have at least one warning)
*/

BEGIN;

-- =========================
-- movies (core title record)
-- =========================
CREATE TABLE IF NOT EXISTS public.movies (
    imdb_id            VARCHAR(16) PRIMARY KEY,
    tmdb_id            INTEGER UNIQUE,
    title              TEXT NOT NULL,
    release_year       INTEGER NOT NULL CHECK (release_year BETWEEN 1878 AND 2100),
    mpaa_rating        VARCHAR(10),               -- US only (e.g., G, PG, PG-13, R, NC-17)
    runtime_minutes    INTEGER CHECK (runtime_minutes IS NULL OR runtime_minutes BETWEEN 1 AND 600),
    plot_summary       TEXT,
    poster_url         TEXT,                      -- store full URL for simplicity
    original_language  CHAR(2),                   -- e.g., en
    media_type         VARCHAR(10) NOT NULL DEFAULT 'movie' CHECK (media_type IN ('movie', 'tv')),
    tagline            TEXT,
    status             TEXT,
    created_at         TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at         TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movies_imdb_id_format_chk
        CHECK (imdb_id ~ '^tt[0-9]+$')
);

CREATE INDEX IF NOT EXISTS idx_movies_title ON public.movies (title);
CREATE INDEX IF NOT EXISTS idx_movies_release_year ON public.movies (release_year);
CREATE INDEX IF NOT EXISTS idx_movies_tmdb_id ON public.movies (tmdb_id);

-- =======================================
-- warnings (DTDD topic catalog / taxonomy)
-- =======================================
CREATE TABLE IF NOT EXISTS public.warnings (
    dtdd_topic_id          INTEGER PRIMARY KEY,  -- DTDD "topic id" from your catalog
    topic_name             TEXT NOT NULL,
    topic_type             TEXT,                 -- e.g., trigger/plot/person/etc (whatever your catalog uses)
    parent_dtdd_topic_id   INTEGER NULL,         -- parent topic id if applicable
    tier                   SMALLINT NULL CHECK (tier IS NULL OR tier BETWEEN 1 AND 4),
    created_at             TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at             TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT warnings_parent_fk
        FOREIGN KEY (parent_dtdd_topic_id) REFERENCES public.warnings (dtdd_topic_id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS idx_warnings_topic_name ON public.warnings (topic_name);
CREATE INDEX IF NOT EXISTS idx_warnings_parent ON public.warnings (parent_dtdd_topic_id);

-- ==================================================
-- movie_warnings (bridge: which warnings apply where)
-- ==================================================
CREATE TABLE IF NOT EXISTS public.movie_warnings (
    imdb_id        VARCHAR(16) NOT NULL,
    dtdd_topic_id  INTEGER NOT NULL,
    source         TEXT NOT NULL DEFAULT 'DTDD',
    created_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    PRIMARY KEY (imdb_id, dtdd_topic_id),

    CONSTRAINT movie_warnings_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_warnings_topic_fk
        FOREIGN KEY (dtdd_topic_id) REFERENCES public.warnings (dtdd_topic_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_warnings_topic ON public.movie_warnings (dtdd_topic_id);

-- ===========================================
-- Convenience view: only movies with warnings
-- ===========================================
CREATE OR REPLACE VIEW public.movies_with_warnings AS
SELECT m.*
FROM public.movies m
WHERE EXISTS (
    SELECT 1
    FROM public.movie_warnings mw
    WHERE mw.imdb_id = m.imdb_id
);

COMMIT;