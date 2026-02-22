/*
004_people_and_credits.sql
--------------------------
Purpose:
  Store TMDB people and per-movie credits (cast + crew) in a way that supports:
  - People who have multiple roles (actor/director/writer)
  - Multiple crew jobs per person per movie (e.g., Director + Writer)
  - Stable de-duplication using TMDB credit_id

Tables:
  - people
  - movie_cast
  - movie_crew
*/

BEGIN;

-- =========================
-- people (TMDB person record)
-- =========================
CREATE TABLE IF NOT EXISTS public.people (
    tmdb_person_id        INTEGER PRIMARY KEY,
    person_name           TEXT NOT NULL,
    known_for_department  TEXT,
    profile_url           TEXT,
    created_at            TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at            TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_people_name ON public.people (person_name);

-- ==================================
-- movie_cast (TMDB cast credits)
-- ==================================
CREATE TABLE IF NOT EXISTS public.movie_cast (
    tmdb_credit_id   TEXT PRIMARY KEY,         -- TMDB credit_id (stable unique id)
    imdb_id          VARCHAR(16) NOT NULL,
    tmdb_person_id   INTEGER NOT NULL,
    character_name   TEXT,
    cast_order       INTEGER,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movie_cast_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_cast_person_fk
        FOREIGN KEY (tmdb_person_id) REFERENCES public.people (tmdb_person_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_cast_imdb_id ON public.movie_cast (imdb_id);
CREATE INDEX IF NOT EXISTS idx_movie_cast_person ON public.movie_cast (tmdb_person_id);

-- ==================================
-- movie_crew (TMDB crew credits)
-- ==================================
CREATE TABLE IF NOT EXISTS public.movie_crew (
    tmdb_credit_id   TEXT PRIMARY KEY,         -- TMDB credit_id (stable unique id)
    imdb_id          VARCHAR(16) NOT NULL,
    tmdb_person_id   INTEGER NOT NULL,
    department       TEXT,
    job              TEXT,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movie_crew_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_crew_person_fk
        FOREIGN KEY (tmdb_person_id) REFERENCES public.people (tmdb_person_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_crew_imdb_id ON public.movie_crew (imdb_id);
CREATE INDEX IF NOT EXISTS idx_movie_crew_person ON public.movie_crew (tmdb_person_id);
CREATE INDEX IF NOT EXISTS idx_movie_crew_job ON public.movie_crew (job);

COMMIT;