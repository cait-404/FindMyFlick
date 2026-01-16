-- Apply all views (relative includes)
\set ON_ERROR_STOP on

\ir v_movies_streamable_us.sql
\ir v_movies_streamable_us_agg.sql
\ir v_movie_credits_agg.sql
\ir v_movie_people_credits.sql