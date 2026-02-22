/*
005_search_extensions_and_indexes.sql
------------------------------------
Purpose:
  Improve partial-name search performance for people (ILIKE '%...%').

Notes:
  - Requires pg_trgm extension.
  - CREATE EXTENSION may require elevated privileges depending on the Postgres install.
  - Consider running this after seeding for faster bulk inserts.
*/

BEGIN;

-- Enable trigram support (needed for gin_trgm_ops)
CREATE EXTENSION IF NOT EXISTS pg_trgm;

-- Trigram GIN index for fast ILIKE '%name%' searches
CREATE INDEX IF NOT EXISTS idx_people_person_name_trgm
ON public.people
USING gin (person_name gin_trgm_ops);

COMMIT;