-- 0015_normalize_mpaa_trigger.sql
-- Automatically normalizes mpaa_rating on insert or update to prevent
-- inconsistent values from entering the DB.
--
-- Legacy rating mappings:
--   GP, M/PG, M  -> PG  (pre-1972 MPAA ratings)
--   X            -> NC-17 (pre-1990 MPAA rating)
--   AO, 13+, etc -> Not Rated (non-US or non-standard ratings)

CREATE OR REPLACE FUNCTION normalize_mpaa_rating()
RETURNS TRIGGER AS $$
BEGIN
    NEW.mpaa_rating = CASE
        WHEN NEW.mpaa_rating IS NULL THEN NULL
        WHEN TRIM(UPPER(NEW.mpaa_rating)) IN ('NOT RATED', 'UNRATED', 'NR') THEN 'Not Rated'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'G' THEN 'G'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'PG' THEN 'PG'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) IN ('GP', 'M/PG', 'M') THEN 'PG'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'PG-13' THEN 'PG-13'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'R' THEN 'R'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) IN ('NC-17', 'X') THEN 'NC-17'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) LIKE 'TV-%' THEN TRIM(NEW.mpaa_rating)
        ELSE 'Not Rated'
    END;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trg_normalize_mpaa_rating
BEFORE INSERT OR UPDATE ON movies
FOR EACH ROW
EXECUTE FUNCTION normalize_mpaa_rating();