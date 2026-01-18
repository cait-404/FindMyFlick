/*
v_movie_keywords.sql
--------------------
Purpose:
  One row per movie-keyword, joined with core movie fields for easy filtering.
*/

CREATE OR REPLACE VIEW public.v_movie_keywords AS
SELECT
  m.imdb_id,
  m.tmdb_id,
  m.title,
  m.release_year,
  k.tmdb_keyword_id,
  k.keyword_name
FROM public.movie_keywords mk
JOIN public.movies m
  ON m.imdb_id = mk.imdb_id
JOIN public.keywords k
  ON k.tmdb_keyword_id = mk.tmdb_keyword_id;