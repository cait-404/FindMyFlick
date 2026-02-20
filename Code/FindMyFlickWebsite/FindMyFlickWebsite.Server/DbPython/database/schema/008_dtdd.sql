/*
008_dtdd.sql
------------
Purpose:
  Add the missing DTDD pieces needed to store per-movie warning results:
  1) movie_dtdd_titles: map imdb_id -> dtdd_title_id (stable bridge)
  2) Extend movie_warnings to store the answer and optional metadata
  3) Ensure (imdb_id, dtdd_topic_id) is unique (without assuming PK structure)
*/

BEGIN;

-- ------------------------------------------------------------
-- 1) Map our movie key (imdb_id) to DTDD's title id
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.movie_dtdd_titles (
    imdb_id        VARCHAR(16) PRIMARY KEY,
    dtdd_title_id  INTEGER UNIQUE NOT NULL,
    match_method   TEXT,                     -- e.g., 'imdb', 'tmdb', 'title_year'
    created_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT movie_dtdd_titles_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_dtdd_titles_dtdd_title_id
ON public.movie_dtdd_titles (dtdd_title_id);

-- ------------------------------------------------------------
-- 2) Extend movie_warnings to store DTDD results
-- ------------------------------------------------------------
ALTER TABLE public.movie_warnings
    ADD COLUMN IF NOT EXISTS answer TEXT,          -- 'yes'/'no'/'unknown'/'unsure' etc.
    ADD COLUMN IF NOT EXISTS is_spoiler BOOLEAN,
    ADD COLUMN IF NOT EXISTS warning_comment TEXT,
    ADD COLUMN IF NOT EXISTS updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW();

-- ------------------------------------------------------------
-- 3) Ensure (imdb_id, dtdd_topic_id) is unique
--    (don’t add a PK; the table already has one)
-- ------------------------------------------------------------
DO $$
BEGIN
    -- If there is already a UNIQUE or PRIMARY KEY constraint on (imdb_id, dtdd_topic_id),
    -- do nothing. Otherwise, add a UNIQUE constraint.
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'movie_warnings'
          AND c.contype IN ('p','u')
          AND pg_get_constraintdef(c.oid) ILIKE '%(imdb_id, dtdd_topic_id)%'
    ) THEN
        ALTER TABLE public.movie_warnings
            ADD CONSTRAINT movie_warnings_imdb_topic_uk UNIQUE (imdb_id, dtdd_topic_id);
    END IF;
END$$;

-- ------------------------------------------------------------
-- 4) Foreign keys (add only if missing)
-- ------------------------------------------------------------
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'movie_warnings_movie_fk'
          AND conrelid = 'public.movie_warnings'::regclass
    ) THEN
        ALTER TABLE public.movie_warnings
            ADD CONSTRAINT movie_warnings_movie_fk
            FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
            ON UPDATE CASCADE
            ON DELETE CASCADE;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'movie_warnings_topic_fk'
          AND conrelid = 'public.movie_warnings'::regclass
    ) THEN
        ALTER TABLE public.movie_warnings
            ADD CONSTRAINT movie_warnings_topic_fk
            FOREIGN KEY (dtdd_topic_id) REFERENCES public.warnings (dtdd_topic_id)
            ON UPDATE CASCADE
            ON DELETE CASCADE;
    END IF;
END$$;

CREATE INDEX IF NOT EXISTS idx_movie_warnings_topic
ON public.movie_warnings (dtdd_topic_id);

CREATE INDEX IF NOT EXISTS idx_movie_warnings_imdb
ON public.movie_warnings (imdb_id);

COMMIT;