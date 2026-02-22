/*
v_movie_warnings_agg.sql
Purpose:
  One row per movie with DTDD topics aggregated by answer bucket.
*/

DROP VIEW IF EXISTS public.v_movie_warnings_agg;

CREATE VIEW public.v_movie_warnings_agg AS
SELECT
  m.imdb_id,
  m.tmdb_id,
  m.title,
  m.release_year,

  array_agg(mw.dtdd_topic_id ORDER BY mw.dtdd_topic_id)
    FILTER (WHERE mw.answer = 'yes') AS yes_topic_ids,

  array_agg(mw.dtdd_topic_id ORDER BY mw.dtdd_topic_id)
    FILTER (WHERE mw.answer = 'no') AS no_topic_ids,

  array_agg(mw.dtdd_topic_id ORDER BY mw.dtdd_topic_id)
    FILTER (WHERE mw.answer = 'unknown') AS unknown_topic_ids,

  string_agg(w.topic_name, ', ' ORDER BY w.topic_name)
    FILTER (WHERE mw.answer = 'yes') AS yes_topics,

  string_agg(w.topic_name, ', ' ORDER BY w.topic_name)
    FILTER (WHERE mw.answer = 'no') AS no_topics,

  string_agg(w.topic_name, ', ' ORDER BY w.topic_name)
    FILTER (WHERE mw.answer = 'unknown') AS unknown_topics

FROM public.movies m
LEFT JOIN public.movie_warnings mw ON mw.imdb_id = m.imdb_id
LEFT JOIN public.warnings w ON w.dtdd_topic_id = mw.dtdd_topic_id
GROUP BY m.imdb_id, m.tmdb_id, m.title, m.release_year;