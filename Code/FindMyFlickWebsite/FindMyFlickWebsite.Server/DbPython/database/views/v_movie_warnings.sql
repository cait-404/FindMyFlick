/*
v_movie_warnings.sql
Purpose:
  One row per (movie, DTDD topic) with answer + topic name.
*/

DROP VIEW IF EXISTS public.v_movie_warnings;

CREATE VIEW public.v_movie_warnings AS
SELECT
  m.imdb_id,
  m.tmdb_id,
  m.title,
  m.release_year,
  mw.dtdd_topic_id,
  w.topic_name,
  mw.answer,
  mw.is_spoiler,
  mw.warning_comment,
  mw.source,
  mw.updated_at
FROM public.movie_warnings mw
JOIN public.movies m ON m.imdb_id = mw.imdb_id
JOIN public.warnings w ON w.dtdd_topic_id = mw.dtdd_topic_id;