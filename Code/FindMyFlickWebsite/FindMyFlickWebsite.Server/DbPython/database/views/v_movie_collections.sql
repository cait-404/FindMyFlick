/*
v_movie_collections.sql
Purpose:
  One row per movie that belongs to a TMDB collection.
*/

DROP VIEW IF EXISTS public.v_movie_collections;

CREATE VIEW public.v_movie_collections AS
SELECT
  m.imdb_id,
  m.tmdb_id,
  m.title,
  m.release_year,
  mc.tmdb_collection_id,
  c.collection_name,
  c.poster_url AS collection_poster_url,
  c.backdrop_url AS collection_backdrop_url
FROM public.movies m
JOIN public.movie_collections mc ON mc.imdb_id = m.imdb_id
JOIN public.collections c ON c.tmdb_collection_id = mc.tmdb_collection_id;