/*
v_movie_people_credits.sql
--------------------------
Purpose:
  Flat, searchable view: one row per person-credit per movie.
  Supports role-based filtering (actor vs director vs writer, etc.)
  while allowing the same person to appear multiple times for a movie.

Notes:
  - CAST rows have character_name and cast_order
  - CREW rows have department and job
*/

BEGIN;

CREATE OR REPLACE VIEW public.v_movie_people_credits AS
SELECT
    m.imdb_id,
    m.tmdb_id,
    m.title,
    m.release_year,
    p.tmdb_person_id,
    p.person_name,
    'CAST'::TEXT AS credit_type,
    NULL::TEXT AS department,
    NULL::TEXT AS job,
    mc.character_name,
    mc.cast_order,
    mc.tmdb_credit_id
FROM public.movies m
JOIN public.movie_cast mc
    ON mc.imdb_id = m.imdb_id
JOIN public.people p
    ON p.tmdb_person_id = mc.tmdb_person_id

UNION ALL

SELECT
    m.imdb_id,
    m.tmdb_id,
    m.title,
    m.release_year,
    p.tmdb_person_id,
    p.person_name,
    'CREW'::TEXT AS credit_type,
    mcr.department,
    mcr.job,
    NULL::TEXT AS character_name,
    NULL::INTEGER AS cast_order,
    mcr.tmdb_credit_id
FROM public.movies m
JOIN public.movie_crew mcr
    ON mcr.imdb_id = m.imdb_id
JOIN public.people p
    ON p.tmdb_person_id = mcr.tmdb_person_id
;

COMMIT;