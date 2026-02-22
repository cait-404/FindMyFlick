BEGIN;

CREATE TABLE IF NOT EXISTS public.warning_categories (
  category_id   SERIAL PRIMARY KEY,
  category_name TEXT NOT NULL UNIQUE,
  created_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at    TIMESTAMPTZ NOT NULL DEFAULT now()
);

-- Many-to-many: one DTDD topic can belong to multiple umbrella categories
CREATE TABLE IF NOT EXISTS public.warning_category_topics (
  category_id   INT NOT NULL REFERENCES public.warning_categories(category_id) ON DELETE CASCADE,
  dtdd_topic_id INT NOT NULL REFERENCES public.warnings(dtdd_topic_id) ON DELETE CASCADE,
  created_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
  PRIMARY KEY (category_id, dtdd_topic_id)
);

CREATE INDEX IF NOT EXISTS idx_warning_category_topics_topic
  ON public.warning_category_topics(dtdd_topic_id);

COMMIT;