-- 0015_normalize_mpaa_trigger.sql
-- Automatically normalizes mpaa_rating on insert or update to prevent
-- inconsistent values like "Unrated", "NOT RATED", "NR" from entering the DB.

CREATE OR REPLACE FUNCTION normalize_mpaa_rating()
RETURNS TRIGGER AS $$
BEGIN
    NEW.mpaa_rating = CASE
        WHEN NEW.mpaa_rating IS NULL THEN NULL
        WHEN TRIM(UPPER(NEW.mpaa_rating)) IN ('NOT RATED', 'UNRATED', 'NR') THEN 'Not Rated'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'G' THEN 'G'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'PG' THEN 'PG'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'PG-13' THEN 'PG-13'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'R' THEN 'R'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) = 'NC-17' THEN 'NC-17'
        WHEN TRIM(UPPER(NEW.mpaa_rating)) LIKE 'TV-%' THEN TRIM(NEW.mpaa_rating)
        ELSE NULL
    END;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trg_normalize_mpaa_rating
BEFORE INSERT OR UPDATE ON movies
FOR EACH ROW
EXECUTE FUNCTION normalize_mpaa_rating();