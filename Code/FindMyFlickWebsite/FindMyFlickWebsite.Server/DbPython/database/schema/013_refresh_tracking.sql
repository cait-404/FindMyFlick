-- Tracks last time we checked external sources for refresh logic

CREATE TABLE IF NOT EXISTS public.movie_streaming_refresh (
  imdb_id         text PRIMARY KEY,
  last_checked_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS public.movie_warnings_refresh (
  imdb_id         text PRIMARY KEY,
  last_checked_at timestamptz NOT NULL DEFAULT now()
);