/*
v_movie_credits_agg.sql
-----------------------
Purpose:
  One row per movie with aggregated credits for UI "movie card" display:
    - top_billed_cast: top N cast names (by cast_order)
    - directors: all directors (job = 'Director')
    - writers: common TMDB writer labels (Writer, Screenplay, Story)

Notes:
  - This view is for display, not strict searching.
  - It does NOT remove any rows from your tables. It only limits the
    cast names shown in the aggregated string.
  - To change how many cast names appear, update top_cast_n in params.
*/

BEGIN;

CREATE OR REPLACE VIEW public.v_movie_credits_agg AS
WITH
params AS (
    SELECT 10::INT AS top_cast_n
),

-- Top-billed cast names (limit to top N per movie by cast_order)
cast_ranked AS (
    SELECT
        mc.imdb_id,
        mc.tmdb_person_id,
        mc.cast_order,
        ROW_NUMBER() OVER (
            PARTITION BY mc.imdb_id
            ORDER BY mc.cast_order NULLS LAST, mc.tmdb_credit_id
        ) AS rn
    FROM public.movie_cast mc
),
cast_agg AS (
    SELECT
        cr.imdb_id,
        STRING_AGG(p.person_name, ', ' ORDER BY cr.cast_order, p.person_name) AS top_billed_cast
    FROM cast_ranked cr
    JOIN params pr ON TRUE
    JOIN public.people p
        ON p.tmdb_person_id = cr.tmdb_person_id
    WHERE cr.rn <= pr.top_cast_n
    GROUP BY cr.imdb_id
),

-- Directors (crew job = Director)
directors AS (
    SELECT
        mc.imdb_id,
        STRING_AGG(DISTINCT p.person_name, ', ' ORDER BY p.person_name) AS directors
    FROM public.movie_crew mc
    JOIN public.people p
        ON p.tmdb_person_id = mc.tmdb_person_id
    WHERE mc.job = 'Director'
    GROUP BY mc.imdb_id
),

-- Writers (common TMDB labels)
writers AS (
    SELECT
        mc.imdb_id,
        STRING_AGG(DISTINCT p.person_name, ', ' ORDER BY p.person_name) AS writers
    FROM public.movie_crew mc
    JOIN public.people p
        ON p.tmdb_person_id = mc.tmdb_person_id
    WHERE mc.job IN ('Writer', 'Screenplay', 'Story')
    GROUP BY mc.imdb_id
)

SELECT
    m.imdb_id,
    m.tmdb_id,
    m.title,
    m.release_year,
    COALESCE(ca.top_billed_cast, '') AS top_billed_cast,
    COALESCE(d.directors, '') AS directors,
    COALESCE(w.writers, '') AS writers
FROM public.movies m
LEFT JOIN cast_agg ca
    ON ca.imdb_id = m.imdb_id
LEFT JOIN directors d
    ON d.imdb_id = m.imdb_id
LEFT JOIN writers w
    ON w.imdb_id = m.imdb_id
;

COMMIT;