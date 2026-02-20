/*
009_dtdd_match_bridge.sql
Purpose:
  Expand movie_dtdd_titles into a proper DTDD match bridge table.
  Stores DTDD media id plus the external ids DTDD reports (which may be blank),
  and how the match was made.
*/

BEGIN;

-- If your table already exists, just add what we're missing.
ALTER TABLE public.movie_dtdd_titles
  ADD COLUMN IF NOT EXISTS dtdd_media_id integer,
  ADD COLUMN IF NOT EXISTS dtdd_imdb_id text,
  ADD COLUMN IF NOT EXISTS dtdd_tmdb_id integer,
  ADD COLUMN IF NOT EXISTS dtdd_title text,
  ADD COLUMN IF NOT EXISTS dtdd_release_year integer,
  ADD COLUMN IF NOT EXISTS match_method text,
  ADD COLUMN IF NOT EXISTS match_score numeric(6,3),
  ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone NOT NULL DEFAULT now();

-- dtdd_media_id should be unique when present.
DO $$
BEGIN
  IF NOT EXISTS (
    SELECT 1
    FROM pg_constraint
    WHERE conname = 'movie_dtdd_titles_dtdd_media_id_key'
  ) THEN
    ALTER TABLE public.movie_dtdd_titles
      ADD CONSTRAINT movie_dtdd_titles_dtdd_media_id_key UNIQUE (dtdd_media_id);
  END IF;
END $$;

-- Helpful indexes
CREATE INDEX IF NOT EXISTS idx_movie_dtdd_titles_dtdd_tmdb_id
  ON public.movie_dtdd_titles (dtdd_tmdb_id);

CREATE INDEX IF NOT EXISTS idx_movie_dtdd_titles_match_method
  ON public.movie_dtdd_titles (match_method);

COMMIT;