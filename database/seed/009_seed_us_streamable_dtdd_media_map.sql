/*
009_seed_us_streamable_dtdd_media_map.sql
Purpose: Map movies.imdb_id to DTDD media ids for DTDD warnings.
Also inserts placeholder rows for unmatched movies for spot checking.
*/

\set ON_ERROR_STOP on
BEGIN;

INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt27589902', 487458, '', 1116465, 'the legend', 2024, 'tmdb_from_search', 8.778, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt37961727', 1608249, '', 1589891, 'A Time For Bravery', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0499549', 928891, 'tt0499549', 19995, 'Avatar', 2009, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt1630029', 983966, '', 76600, 'Avatar: The Way of Water', 2022, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt37893389', 1530475, '', 1511417, 'Bahubali : The Epic', 2025, 'tmdb_from_search', 8.919, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt12300742', 1555703, '', 0, 'Bugonia', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt29954526', 1124684, '', 1206083, 'The Conjuring Tapes', 2025, 'tmdb_from_search', 8.882, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt11344566', 1475413, '', 1457131, 'Don''t Follow Me', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt16311594', 1518310, '', 0, 'F1', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0137523', 9593, 'tt0137523', 550, 'Fight Club', 1999, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt1312221', 1574247, '', 1062722, 'Frankenstein', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt27829510', 1030013, '', 1149167, 'Cash Out 2: High Rollers', 2025, 'tmdb_from_search', 8.667, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt13176330', 718577, '', 756397, 'Hitpig', 2024, 'tmdb_from_search', 8.923, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt26743210', 981061, '', 1087192, 'How to Train Your Dragon', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0816692', 10103, 'tt0816692', 157336, 'Interstellar', 2014, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt31036941', 1146398, '', 1234821, 'Jurassic World Rebirth', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt26439764', NULL, '', NULL, '', NULL, 'unmatched_no_results', 0.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt14205554', 1520470, '', 0, 'KPop Demon Hunters', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt32831759', 1358077, '', 1403735, 'Laila', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt9603208', 1479413, '', 0, 'Mission Impossible: The Final Reckoning ', 2025, 'title_year_fallback', 6.950, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt21909764', 1025206, '', 0, 'My Fault', 2023, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt34956443', 1409918, '', 0, 'Ne Zha 2', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt29544253', 1124055, '', 1205229, 'Night of the Zoopocalypse', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt30254719', NULL, '', NULL, '', NULL, 'unmatched_no_results', 0.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt30144839', 1537151, '', 1054867, 'One Battle After Another', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt39307872', 1627755, '', 1610413, 'One Last Adventure: The Making of Stranger Things 5', 2026, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt22740896', 941560, '', 1034716, 'People We Meet on Vacation', 2026, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt27208407', 1526340, '', 0, 'Please Don''t Feed the Children', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt32263401', 1607501, '', 0, 'Reflection in a Dead Diamond', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt31193180', 1495841, '', 1233413, 'Sinners', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt27196021', 1116923, '', 1102493, 'Small Things Like These', 2024, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt5950044', 1223215, '', 1061474, 'Superman', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0848228', 770520, 'tt0848228', 24428, 'The Avengers', 2012, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt34276058', 1314044, '', 1363123, 'The Family Plan 2', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt10676052', 153809, 'tt10676052', 617126, 'The Fantastic Four', 2025, 'imdb_tt', 10.696, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0068646', 11800, 'tt0068646', 238, 'The Godfather', 1972, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt29927663', 1619565, '', 0, 'The Great Flood', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt14107334', 754886, '', 798645, 'The Running Man', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0111161', 12188, 'tt0111161', 278, 'The Shawshank Redemption', 1994, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt31495504', 1514856, '', 1495445, 'Tank Boy', 2025, 'title_year_fallback', 6.500, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt6300910', 1068281, '', 1195518, 'Unholy Trinity', 2024, 'tmdb_from_search', 8.875, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0120338', 12371, 'tt0120338', 597, 'Titanic', 1997, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt29232158', 12431, '', 1180831, 'Troll 2', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt6604188', 171917, 'tt6604188', 533533, 'TRON: Ares', 2025, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt14940390', 1096491, '', 1327881, 'Voice of Shadows', 2024, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt14364480', 1623844, '', 812583, 'Wake Up Dead Man: A Knives Out Mystery', 2025, 'tmdb_from_search', 9.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt13186306', 1541185, '', 0, 'War of the Worlds', 2025, 'title_year_fallback', 7.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt36153493', NULL, '', NULL, '', NULL, 'unmatched_year_mismatch', 0.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt0295701', 52211, 'tt0295701', 7451, 'xXx', 2002, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt35851026', NULL, '', NULL, '', NULL, 'unmatched_no_results', 0.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();
INSERT INTO public.movie_dtdd_titles
(imdb_id, dtdd_media_id, dtdd_imdb_id, dtdd_tmdb_id, dtdd_title, dtdd_release_year, match_method, match_score, updated_at)
VALUES
('tt2948356', 811680, 'tt2948356', 269149, 'Zootopia', 2016, 'imdb_tt', 11.000, now())
ON CONFLICT (imdb_id) DO UPDATE SET
  dtdd_media_id     = EXCLUDED.dtdd_media_id,
  dtdd_imdb_id      = EXCLUDED.dtdd_imdb_id,
  dtdd_tmdb_id      = EXCLUDED.dtdd_tmdb_id,
  dtdd_title        = EXCLUDED.dtdd_title,
  dtdd_release_year = EXCLUDED.dtdd_release_year,
  match_method      = EXCLUDED.match_method,
  match_score       = EXCLUDED.match_score,
  updated_at        = now();

COMMIT;

