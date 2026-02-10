-- 012_plot_tags.sql
-- User-submitted plot tags + per-movie linking + per-user voting

BEGIN;

-- 1) plot_tags (unique tag text)
CREATE TABLE IF NOT EXISTS public.plot_tags (
  plot_tag_id    integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  tag_text       text NOT NULL,
  tag_text_norm  text NOT NULL,
  created_at     timestamptz NOT NULL DEFAULT now(),
  UNIQUE (tag_text_norm)
);

CREATE INDEX IF NOT EXISTS idx_plot_tags_norm
  ON public.plot_tags (tag_text_norm);

-- 2) movie_plot_tags (tag applied to a movie)
CREATE TABLE IF NOT EXISTS public.movie_plot_tags (
  imdb_id            varchar(16) NOT NULL
                    REFERENCES public.movies(imdb_id)
                    ON UPDATE CASCADE ON DELETE CASCADE,
  plot_tag_id        integer NOT NULL
                    REFERENCES public.plot_tags(plot_tag_id)
                    ON UPDATE CASCADE ON DELETE CASCADE,
  created_at         timestamptz NOT NULL DEFAULT now(),
  created_by_user_id integer NULL,   -- backend will wire to users table later
  status             text NOT NULL DEFAULT 'approved',
  PRIMARY KEY (imdb_id, plot_tag_id),
  CONSTRAINT movie_plot_tags_status_check
    CHECK (status IN ('pending','approved','rejected'))
);

CREATE INDEX IF NOT EXISTS idx_movie_plot_tags_tag
  ON public.movie_plot_tags (plot_tag_id);

CREATE INDEX IF NOT EXISTS idx_movie_plot_tags_status
  ON public.movie_plot_tags (status);

-- 3) movie_plot_tag_votes (one vote per user per movie-tag pair)
CREATE TABLE IF NOT EXISTS public.movie_plot_tag_votes (
  imdb_id      varchar(16) NOT NULL,
  plot_tag_id  integer NOT NULL,
  user_id      integer NOT NULL,     -- backend will wire to users table later
  vote         smallint NOT NULL,
  created_at   timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (imdb_id, plot_tag_id, user_id),
  CONSTRAINT movie_plot_tag_votes_vote_check
    CHECK (vote IN (-1, 1)),
  CONSTRAINT movie_plot_tag_votes_pair_fk
    FOREIGN KEY (imdb_id, plot_tag_id)
    REFERENCES public.movie_plot_tags(imdb_id, plot_tag_id)
    ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_movie_plot_tag_votes_pair
  ON public.movie_plot_tag_votes (imdb_id, plot_tag_id);

COMMIT;