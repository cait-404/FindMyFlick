/*
v_movie_collections_agg.sql
Purpose:
  One row per collection, with member movies aggregated (ids + titles).
*/

DROP VIEW IF EXISTS public.v_movie_collections_agg;

CREATE VIEW public.v_movie_collections_agg AS
SELECT
  c.tmdb_collection_id,
  c.collection_name,
  c.poster_url AS collection_poster_url,
  c.backdrop_url AS collection_backdrop_url,

  COUNT(*) AS movie_count,

  string_agg(DISTINCT m.imdb_id, ', ' ORDER BY m.imdb_id) AS imdb_ids,
  string_agg(DISTINCT m.title, ' | ' ORDER BY m.title) AS titles
FROM public.collections c
JOIN public.movie_collections mc ON mc.tmdb_collection_id = c.tmdb_collection_id
JOIN public.movies m ON m.imdb_id = mc.imdb_id
GROUP BY
  c.tmdb_collection_id,
  c.collection_name,
  c.poster_url,
  c.backdrop_url;