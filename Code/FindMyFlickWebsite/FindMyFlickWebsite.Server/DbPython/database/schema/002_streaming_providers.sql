/*
002_streaming_providers.sql
---------------------------
Purpose:
  Store where a movie is available to watch in the US, using TMDB watch providers.

Tables:
  - streaming_providers: provider catalog (TMDB provider_id)
  - movie_streaming: bridge table between movies and providers + offer type
*/

BEGIN;

CREATE TABLE IF NOT EXISTS public.streaming_providers (
    tmdb_provider_id     INTEGER PRIMARY KEY,
    provider_name        TEXT NOT NULL,
    logo_path            TEXT,
    display_priority     INTEGER,
    created_at           TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_streaming_providers_name
ON public.streaming_providers (provider_name);

-- offer_type maps to TMDB buckets:
-- flatrate -> subscription
-- free     -> free
-- ads      -> free_with_ads
-- rent     -> rent
-- buy      -> buy
CREATE TABLE IF NOT EXISTS public.movie_streaming (
    imdb_id             VARCHAR(16) NOT NULL,
    tmdb_provider_id    INTEGER NOT NULL,
    offer_type          TEXT NOT NULL,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    PRIMARY KEY (imdb_id, tmdb_provider_id, offer_type),

    CONSTRAINT movie_streaming_movie_fk
        FOREIGN KEY (imdb_id) REFERENCES public.movies (imdb_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_streaming_provider_fk
        FOREIGN KEY (tmdb_provider_id) REFERENCES public.streaming_providers (tmdb_provider_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT movie_streaming_offer_type_chk
        CHECK (offer_type IN ('subscription', 'free', 'free_with_ads', 'rent', 'buy'))
);

CREATE INDEX IF NOT EXISTS idx_movie_streaming_provider
ON public.movie_streaming (tmdb_provider_id);

CREATE INDEX IF NOT EXISTS idx_movie_streaming_offer_type
ON public.movie_streaming (offer_type);

COMMIT;