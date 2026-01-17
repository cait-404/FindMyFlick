/*
_apply_views.sql
----------------
Run from psql:
  \i database/views/_apply_views.sql
*/

\set ON_ERROR_STOP on

-- Drop dependent views first (order matters)
DROP VIEW IF EXISTS public.v_movie_credits_agg;
DROP VIEW IF EXISTS public.v_movie_people_credits;
DROP VIEW IF EXISTS public.v_movies_streamable_us_agg;
DROP VIEW IF EXISTS public.v_movies_streamable_us;

-- Now recreate from files
\ir v_movies_streamable_us.sql
\ir v_movies_streamable_us_agg.sql
\ir v_movie_credits_agg.sql
\ir v_movie_people_credits.sql