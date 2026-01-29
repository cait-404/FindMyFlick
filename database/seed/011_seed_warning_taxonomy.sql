\encoding UTF8
-- Generated 20260128_145509
-- Source: python_scripts/content_warnings/taxonomy/claude_taxonomy.yml
-- Builds: warning_categories, warning_category_topics
BEGIN;

-- Upsert categories
INSERT INTO public.warning_categories (category_name)
VALUES ('Animal Harm & Death')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Death & Dying')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Violence & Gore')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Sexual Content & Assault')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Child Harm & Family Trauma')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Mental Health & Illness')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Self-Harm & Suicide')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Substance Use & Addiction')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Medical & Hospital Content')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Discrimination & Hate')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Phobias & Sensory Triggers')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Psychological & Emotional Trauma')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

INSERT INTO public.warning_categories (category_name)
VALUES ('Accidents, Disasters & Situations')
ON CONFLICT (category_name) DO UPDATE SET updated_at = now();

-- Allowed category-topic mappings (from YAML)
CREATE TEMP TABLE _allowed_category_topics (
  category_name text NOT NULL,
  dtdd_topic_id int NOT NULL,
  PRIMARY KEY (category_name, dtdd_topic_id)
) ON COMMIT DROP;
INSERT INTO _allowed_category_topics (category_name, dtdd_topic_id) VALUES
  ('Accidents, Disasters & Situations', 176),
  ('Accidents, Disasters & Situations', 184),
  ('Accidents, Disasters & Situations', 198),
  ('Accidents, Disasters & Situations', 207),
  ('Accidents, Disasters & Situations', 208),
  ('Accidents, Disasters & Situations', 209),
  ('Accidents, Disasters & Situations', 221),
  ('Accidents, Disasters & Situations', 224),
  ('Accidents, Disasters & Situations', 227),
  ('Accidents, Disasters & Situations', 256),
  ('Accidents, Disasters & Situations', 268),
  ('Accidents, Disasters & Situations', 270),
  ('Accidents, Disasters & Situations', 271),
  ('Accidents, Disasters & Situations', 273),
  ('Accidents, Disasters & Situations', 277),
  ('Accidents, Disasters & Situations', 283),
  ('Accidents, Disasters & Situations', 290),
  ('Accidents, Disasters & Situations', 329),
  ('Accidents, Disasters & Situations', 345),
  ('Accidents, Disasters & Situations', 346),
  ('Accidents, Disasters & Situations', 347),
  ('Accidents, Disasters & Situations', 351),
  ('Accidents, Disasters & Situations', 357),
  ('Accidents, Disasters & Situations', 369),
  ('Accidents, Disasters & Situations', 371),
  ('Accidents, Disasters & Situations', 372),
  ('Accidents, Disasters & Situations', 373),
  ('Animal Harm & Death', 153),
  ('Animal Harm & Death', 183),
  ('Animal Harm & Death', 186),
  ('Animal Harm & Death', 189),
  ('Animal Harm & Death', 196),
  ('Animal Harm & Death', 229),
  ('Animal Harm & Death', 231),
  ('Animal Harm & Death', 252),
  ('Animal Harm & Death', 285),
  ('Animal Harm & Death', 305),
  ('Animal Harm & Death', 319),
  ('Animal Harm & Death', 338),
  ('Animal Harm & Death', 350),
  ('Animal Harm & Death', 355),
  ('Child Harm & Family Trauma', 158),
  ('Child Harm & Family Trauma', 218),
  ('Child Harm & Family Trauma', 219),
  ('Child Harm & Family Trauma', 228),
  ('Child Harm & Family Trauma', 236),
  ('Child Harm & Family Trauma', 253),
  ('Child Harm & Family Trauma', 264),
  ('Child Harm & Family Trauma', 266),
  ('Child Harm & Family Trauma', 284),
  ('Child Harm & Family Trauma', 287),
  ('Child Harm & Family Trauma', 293),
  ('Child Harm & Family Trauma', 330),
  ('Death & Dying', 158),
  ('Death & Dying', 164),
  ('Death & Dying', 168),
  ('Death & Dying', 187),
  ('Death & Dying', 191),
  ('Death & Dying', 194),
  ('Death & Dying', 211),
  ('Death & Dying', 239),
  ('Death & Dying', 282),
  ('Death & Dying', 286),
  ('Death & Dying', 311),
  ('Death & Dying', 313),
  ('Death & Dying', 322),
  ('Death & Dying', 328),
  ('Discrimination & Hate', 212),
  ('Discrimination & Hate', 226),
  ('Discrimination & Hate', 233),
  ('Discrimination & Hate', 234),
  ('Discrimination & Hate', 244),
  ('Discrimination & Hate', 246),
  ('Discrimination & Hate', 247),
  ('Discrimination & Hate', 248),
  ('Discrimination & Hate', 251),
  ('Discrimination & Hate', 262),
  ('Discrimination & Hate', 272),
  ('Discrimination & Hate', 294),
  ('Discrimination & Hate', 300),
  ('Discrimination & Hate', 301),
  ('Discrimination & Hate', 303),
  ('Discrimination & Hate', 310),
  ('Discrimination & Hate', 314),
  ('Discrimination & Hate', 325),
  ('Discrimination & Hate', 353),
  ('Discrimination & Hate', 359),
  ('Discrimination & Hate', 368),
  ('Medical & Hospital Content', 190),
  ('Medical & Hospital Content', 192),
  ('Medical & Hospital Content', 201),
  ('Medical & Hospital Content', 204),
  ('Medical & Hospital Content', 205),
  ('Medical & Hospital Content', 215),
  ('Medical & Hospital Content', 238),
  ('Medical & Hospital Content', 278),
  ('Medical & Hospital Content', 288),
  ('Medical & Hospital Content', 317),
  ('Medical & Hospital Content', 327),
  ('Medical & Hospital Content', 358),
  ('Medical & Hospital Content', 375),
  ('Mental Health & Illness', 195),
  ('Mental Health & Illness', 205),
  ('Mental Health & Illness', 206),
  ('Mental Health & Illness', 217),
  ('Mental Health & Illness', 220),
  ('Mental Health & Illness', 224),
  ('Mental Health & Illness', 235),
  ('Mental Health & Illness', 263),
  ('Mental Health & Illness', 265),
  ('Mental Health & Illness', 291),
  ('Mental Health & Illness', 302),
  ('Mental Health & Illness', 306),
  ('Mental Health & Illness', 323),
  ('Mental Health & Illness', 334),
  ('Mental Health & Illness', 336),
  ('Mental Health & Illness', 348),
  ('Mental Health & Illness', 349),
  ('Mental Health & Illness', 370),
  ('Phobias & Sensory Triggers', 161),
  ('Phobias & Sensory Triggers', 165),
  ('Phobias & Sensory Triggers', 167),
  ('Phobias & Sensory Triggers', 174),
  ('Phobias & Sensory Triggers', 180),
  ('Phobias & Sensory Triggers', 181),
  ('Phobias & Sensory Triggers', 201),
  ('Phobias & Sensory Triggers', 202),
  ('Phobias & Sensory Triggers', 211),
  ('Phobias & Sensory Triggers', 213),
  ('Phobias & Sensory Triggers', 214),
  ('Phobias & Sensory Triggers', 257),
  ('Phobias & Sensory Triggers', 260),
  ('Phobias & Sensory Triggers', 261),
  ('Phobias & Sensory Triggers', 269),
  ('Phobias & Sensory Triggers', 295),
  ('Phobias & Sensory Triggers', 312),
  ('Phobias & Sensory Triggers', 324),
  ('Phobias & Sensory Triggers', 332),
  ('Phobias & Sensory Triggers', 335),
  ('Phobias & Sensory Triggers', 337),
  ('Phobias & Sensory Triggers', 339),
  ('Phobias & Sensory Triggers', 354),
  ('Phobias & Sensory Triggers', 356),
  ('Phobias & Sensory Triggers', 366),
  ('Psychological & Emotional Trauma', 222),
  ('Psychological & Emotional Trauma', 237),
  ('Psychological & Emotional Trauma', 240),
  ('Psychological & Emotional Trauma', 241),
  ('Psychological & Emotional Trauma', 242),
  ('Psychological & Emotional Trauma', 243),
  ('Psychological & Emotional Trauma', 245),
  ('Psychological & Emotional Trauma', 281),
  ('Psychological & Emotional Trauma', 289),
  ('Psychological & Emotional Trauma', 298),
  ('Psychological & Emotional Trauma', 308),
  ('Psychological & Emotional Trauma', 316),
  ('Psychological & Emotional Trauma', 318),
  ('Psychological & Emotional Trauma', 341),
  ('Psychological & Emotional Trauma', 360),
  ('Psychological & Emotional Trauma', 363),
  ('Psychological & Emotional Trauma', 365),
  ('Self-Harm & Suicide', 177),
  ('Self-Harm & Suicide', 187),
  ('Self-Harm & Suicide', 199),
  ('Self-Harm & Suicide', 259),
  ('Self-Harm & Suicide', 275),
  ('Self-Harm & Suicide', 286),
  ('Self-Harm & Suicide', 297),
  ('Self-Harm & Suicide', 304),
  ('Sexual Content & Assault', 182),
  ('Sexual Content & Assault', 197),
  ('Sexual Content & Assault', 276),
  ('Sexual Content & Assault', 279),
  ('Sexual Content & Assault', 287),
  ('Sexual Content & Assault', 292),
  ('Sexual Content & Assault', 299),
  ('Sexual Content & Assault', 307),
  ('Sexual Content & Assault', 315),
  ('Sexual Content & Assault', 320),
  ('Sexual Content & Assault', 326),
  ('Sexual Content & Assault', 364),
  ('Substance Use & Addiction', 193),
  ('Substance Use & Addiction', 225),
  ('Substance Use & Addiction', 230),
  ('Substance Use & Addiction', 275),
  ('Substance Use & Addiction', 299),
  ('Violence & Gore', 171),
  ('Violence & Gore', 188),
  ('Violence & Gore', 200),
  ('Violence & Gore', 203),
  ('Violence & Gore', 210),
  ('Violence & Gore', 216),
  ('Violence & Gore', 219),
  ('Violence & Gore', 223),
  ('Violence & Gore', 232),
  ('Violence & Gore', 250),
  ('Violence & Gore', 254),
  ('Violence & Gore', 255),
  ('Violence & Gore', 258),
  ('Violence & Gore', 267),
  ('Violence & Gore', 274),
  ('Violence & Gore', 280),
  ('Violence & Gore', 296),
  ('Violence & Gore', 309),
  ('Violence & Gore', 321),
  ('Violence & Gore', 331),
  ('Violence & Gore', 342),
  ('Violence & Gore', 343),
  ('Violence & Gore', 352),
  ('Violence & Gore', 361),
  ('Violence & Gore', 362),
  ('Violence & Gore', 365),
  ('Violence & Gore', 367),
  ('Violence & Gore', 378);

-- Prune mappings no longer present in YAML
DELETE FROM public.warning_category_topics wct
USING public.warning_categories wc
WHERE wct.category_id = wc.category_id
  AND NOT EXISTS (
    SELECT 1
    FROM _allowed_category_topics a
    WHERE a.category_name = wc.category_name
      AND a.dtdd_topic_id = wct.dtdd_topic_id
  );

-- Insert allowed mappings
INSERT INTO public.warning_category_topics (category_id, dtdd_topic_id)
SELECT wc.category_id, a.dtdd_topic_id
FROM _allowed_category_topics a
JOIN public.warning_categories wc
  ON wc.category_name = a.category_name
ON CONFLICT DO NOTHING;

COMMIT;
