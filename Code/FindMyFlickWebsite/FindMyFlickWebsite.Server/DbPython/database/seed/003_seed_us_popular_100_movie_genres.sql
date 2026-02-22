BEGIN;

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (12, 'Adventure')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (14, 'Fantasy')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (16, 'Animation')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (18, 'Drama')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (27, 'Horror')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (28, 'Action')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (35, 'Comedy')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (36, 'History')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (37, 'Western')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (53, 'Thriller')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (80, 'Crime')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (99, 'Documentary')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (878, 'Science Fiction')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (9648, 'Mystery')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (10402, 'Music')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (10749, 'Romance')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (10751, 'Family')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.genres (tmdb_genre_id, genre_name)
VALUES (10752, 'War')
ON CONFLICT (tmdb_genre_id) DO UPDATE SET
  genre_name = EXCLUDED.genre_name,
  updated_at = now();

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0068646', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0068646', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0093949', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0111161', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0111161', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0120338', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0120338', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0137523', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0137523', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0178627', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0178627', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0178627', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0199007', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0199007', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0295701', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0295701', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0295701', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0295701', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0295701', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0499549', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0499549', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0499549', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0499549', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0816692', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0816692', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0816692', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0848228', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0848228', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt0848228', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10210064', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10210064', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10210064', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10676052', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10676052', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10676052', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10799690', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10799690', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt10799690', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt11344566', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt11344566', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt12300742', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt12300742', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt12300742', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1312221', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1312221', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1312221', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13176330', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13176330', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13176330', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13176330', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13186306', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt13186306', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14107334', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14107334', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14107334', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14142060', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14142060', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14205554', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14205554', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14205554', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14205554', 10402)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14264694', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14264694', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14264694', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14364480', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14364480', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14364480', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14850054', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14850054', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14850054', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14850054', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14905854', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14905854', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14940390', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14940390', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt14940390', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1527793', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1527793', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1527793', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1630029', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1630029', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1630029', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt16311594', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt16311594', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1757678', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1757678', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt1757678', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt19847976', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt19847976', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt19847976', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt20254992', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt21909764', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt21909764', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt21909764', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22261722', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22261722', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22261722', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22740896', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22740896', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt22740896', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23572848', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23572848', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23572848', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23572848', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23572848', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23790696', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23790696', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt23790696', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26439764', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26439764', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26439764', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26443597', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26443597', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26443597', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26443597', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26443597', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26743210', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26743210', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26743210', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt26743210', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27196021', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27208407', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27208407', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27543632', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27543632', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27589902', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27589902', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27589902', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27829510', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27829510', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt27829510', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt2806834', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29232158', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29232158', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29232158', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt2948356', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt2948356', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt2948356', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt2948356', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29538620', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29538620', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29544253', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29544253', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29544253', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29544253', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29544253', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29567915', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29567915', 36)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29780866', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29780866', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29780866', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29845247', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29845247', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29927663', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29927663', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29927663', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29954526', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt29954526', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30144839', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30144839', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30144839', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30219247', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30219247', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30254719', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30254719', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30254719', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30274401', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30274401', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30343021', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30343021', 10402)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30343021', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30472557', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30472557', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30472557', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt30472557', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31036941', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31036941', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31036941', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31193180', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31193180', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31193180', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31227572', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31227572', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31227572', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31434030', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31434030', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31434030', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31495504', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31495504', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31495504', 10752)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31844586', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt31844586', 10752)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32123395', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32123395', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32141377', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32141377', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32141377', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32164798', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32241578', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32241578', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32241578', 10751)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32263401', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32263401', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32263401', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32395062', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32395062', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32395062', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32537226', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32537226', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32537226', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32820897', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32820897', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32820897', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32831759', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32831759', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32916440', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt32916440', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33014583', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33014583', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33019528', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33028778', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33028778', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33244668', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33244668', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt33244668', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34276058', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34276058', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34509472', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34509472', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34509472', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34610311', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34610311', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34610311', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34956443', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34956443', 14)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34956443', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt34956443', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35222590', 27)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35222590', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35222590', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35851026', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35851026', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35851026', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35851026', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt35851026', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt36153493', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt36153493', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt36153493', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt36585628', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt36585628', 10749)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37268550', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37268550', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37268550', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37893389', 18)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37893389', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37961727', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt37961727', 35)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt38121182', 16)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt38121182', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt39307872', 99)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt4712810', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt4712810', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt4712810', 9648)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt5950044', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt5950044', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt5950044', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6300910', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6300910', 37)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6300910', 80)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6604188', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6604188', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt6604188', 878)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt9603208', 12)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt9603208', 28)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;
INSERT INTO public.movie_genres (imdb_id, tmdb_genre_id)
VALUES ('tt9603208', 53)
ON CONFLICT (imdb_id, tmdb_genre_id) DO NOTHING;

COMMIT;
