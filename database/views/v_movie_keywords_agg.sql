/*
v_movie_keywords_agg.sql
------------------------
Purpose:
  One row per movie with keywords aggregated (useful for movie card display).
*/

CREATE OR REPLACE VIEW public.v_movie_keywords_agg AS
SELECT
  m.imdb_id,
  m.tmdb_id,
  m.title,
  m.release_year,
  ARRAY_AGG(DISTINCT k.keyword_name ORDER BY k.keyword_name) AS keywords
FROM public.movies m
LEFT JOIN public.movie_keywords mk
  ON mk.imdb_id = m.imdb_id
LEFT JOIN public.keywords k
  ON k.tmdb_keyword_id = mk.tmdb_keyword_id
GROUP BY
  m.imdb_id, m.tmdb_id, m.title, m.release_year;