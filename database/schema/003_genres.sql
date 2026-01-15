/*
003_genres.sql
--------------
Purpose:
  Add TMDB genres and the many-to-many bridge to movies.

Tables:
  - genres (one row per TMDB genre)
  - movie_genres (bridge: which genres apply to which imdb_id)
*/

BEGIN;

CREATE TABLE IF NOT EXISTS public.genres (
    tmdb_genre_id  INTEGER PRIMARY KEY,
    genre_name     TEXT NOT NULL UNIQUE,
    created_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at     TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS public.movie_genres (
    imdb_id        VARCHAR(16) NOT NULL,
    tmdb_genre_id  INTEGER NOT NULL,
    created_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    PRIMARY KEY (imdb_id, tmdb_genre_id),

    CONSTRAINT movie_genres_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_genres_genre_fk
        FOREIGN KEY (tmdb_genre_id) REFERENCES public.genres (tmdb_genre_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_genres_genre_id ON public.movie_genres (tmdb_genre_id);

COMMIT;