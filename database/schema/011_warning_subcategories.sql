-- warning subcategories (YAML group headings like "General Violence", "Actions", etc.)

CREATE TABLE IF NOT EXISTS public.warning_subcategories (
  subcategory_id   integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  category_id      integer NOT NULL
                  REFERENCES public.warning_categories(category_id)
                  ON DELETE CASCADE,
  subcategory_name text NOT NULL,
  created_at       timestamptz NOT NULL DEFAULT now(),
  updated_at       timestamptz NOT NULL DEFAULT now(),
  UNIQUE (category_id, subcategory_name)
);

CREATE TABLE IF NOT EXISTS public.warning_subcategory_topics (
  subcategory_id   integer NOT NULL
                  REFERENCES public.warning_subcategories(subcategory_id)
                  ON DELETE CASCADE,
  dtdd_topic_id    integer NOT NULL
                  REFERENCES public.warnings(dtdd_topic_id)
                  ON DELETE CASCADE,
  created_at       timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (subcategory_id, dtdd_topic_id)
);

CREATE INDEX IF NOT EXISTS idx_warning_subcategories_category
  ON public.warning_subcategories(category_id);

CREATE INDEX IF NOT EXISTS idx_warning_subcategory_topics_topic
  ON public.warning_subcategory_topics(dtdd_topic_id);