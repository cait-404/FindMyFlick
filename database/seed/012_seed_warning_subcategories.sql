\encoding UTF8
-- Generated 20260128_145509
-- Source: python_scripts/content_warnings/taxonomy/claude_taxonomy.yml
-- Builds: warning_subcategories, warning_subcategory_topics
BEGIN;

CREATE TEMP TABLE _allowed_subcategories (
  category_name text NOT NULL,
  subcategory_name text NOT NULL,
  PRIMARY KEY (category_name, subcategory_name)
) ON COMMIT DROP;
INSERT INTO _allowed_subcategories (category_name, subcategory_name) VALUES
  ('Accidents, Disasters & Situations', 'Accidents & Disasters'),
  ('Accidents, Disasters & Situations', 'Meta/Narrative'),
  ('Accidents, Disasters & Situations', 'Miscellaneous'),
  ('Accidents, Disasters & Situations', 'Settings & Scenes'),
  ('Accidents, Disasters & Situations', 'Supernatural & Horror'),
  ('Animal Harm & Death', 'Specific Animals'),
  ('Animal Harm & Death', 'Types of Harm'),
  ('Child Harm & Family Trauma', 'Items'),
  ('Death & Dying', 'By Relationship'),
  ('Death & Dying', 'Manner of Death'),
  ('Discrimination & Hate', 'Disability/Body'),
  ('Discrimination & Hate', 'LGBTQ+'),
  ('Discrimination & Hate', 'Racism'),
  ('Medical & Hospital Content', 'Items'),
  ('Mental Health & Illness', 'Conditions'),
  ('Mental Health & Illness', 'Episodes & Phenomena'),
  ('Mental Health & Illness', 'Institutional/Treatment'),
  ('Mental Health & Illness', 'Stigma'),
  ('Phobias & Sensory Triggers', 'Animals/Creatures'),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects'),
  ('Phobias & Sensory Triggers', 'Body-Related'),
  ('Phobias & Sensory Triggers', 'Spaces/Environment'),
  ('Phobias & Sensory Triggers', 'Visual/Objects'),
  ('Psychological & Emotional Trauma', 'Items'),
  ('Self-Harm & Suicide', 'Items'),
  ('Sexual Content & Assault', 'Assault'),
  ('Sexual Content & Assault', 'Content'),
  ('Sexual Content & Assault', 'Harmful Content'),
  ('Substance Use & Addiction', 'Items'),
  ('Violence & Gore', 'Actions'),
  ('Violence & Gore', 'General Violence'),
  ('Violence & Gore', 'Specific Injuries');

CREATE TEMP TABLE _allowed_subcategory_topics (
  category_name text NOT NULL,
  subcategory_name text NOT NULL,
  dtdd_topic_id int NOT NULL,
  PRIMARY KEY (category_name, subcategory_name, dtdd_topic_id)
) ON COMMIT DROP;
INSERT INTO _allowed_subcategory_topics (category_name, subcategory_name, dtdd_topic_id) VALUES
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 184),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 198),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 208),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 221),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 271),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 283),
  ('Accidents, Disasters & Situations', 'Accidents & Disasters', 373),
  ('Accidents, Disasters & Situations', 'Meta/Narrative', 209),
  ('Accidents, Disasters & Situations', 'Meta/Narrative', 268),
  ('Accidents, Disasters & Situations', 'Meta/Narrative', 346),
  ('Accidents, Disasters & Situations', 'Meta/Narrative', 371),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 227),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 256),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 273),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 277),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 290),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 329),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 347),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 351),
  ('Accidents, Disasters & Situations', 'Miscellaneous', 357),
  ('Accidents, Disasters & Situations', 'Settings & Scenes', 176),
  ('Accidents, Disasters & Situations', 'Settings & Scenes', 270),
  ('Accidents, Disasters & Situations', 'Settings & Scenes', 345),
  ('Accidents, Disasters & Situations', 'Settings & Scenes', 372),
  ('Accidents, Disasters & Situations', 'Supernatural & Horror', 207),
  ('Accidents, Disasters & Situations', 'Supernatural & Horror', 224),
  ('Accidents, Disasters & Situations', 'Supernatural & Horror', 369),
  ('Animal Harm & Death', 'Specific Animals', 153),
  ('Animal Harm & Death', 'Specific Animals', 183),
  ('Animal Harm & Death', 'Specific Animals', 186),
  ('Animal Harm & Death', 'Specific Animals', 196),
  ('Animal Harm & Death', 'Specific Animals', 231),
  ('Animal Harm & Death', 'Specific Animals', 285),
  ('Animal Harm & Death', 'Specific Animals', 305),
  ('Animal Harm & Death', 'Types of Harm', 189),
  ('Animal Harm & Death', 'Types of Harm', 229),
  ('Animal Harm & Death', 'Types of Harm', 231),
  ('Animal Harm & Death', 'Types of Harm', 252),
  ('Animal Harm & Death', 'Types of Harm', 319),
  ('Animal Harm & Death', 'Types of Harm', 338),
  ('Animal Harm & Death', 'Types of Harm', 350),
  ('Animal Harm & Death', 'Types of Harm', 355),
  ('Child Harm & Family Trauma', 'Items', 158),
  ('Child Harm & Family Trauma', 'Items', 218),
  ('Child Harm & Family Trauma', 'Items', 219),
  ('Child Harm & Family Trauma', 'Items', 228),
  ('Child Harm & Family Trauma', 'Items', 236),
  ('Child Harm & Family Trauma', 'Items', 253),
  ('Child Harm & Family Trauma', 'Items', 264),
  ('Child Harm & Family Trauma', 'Items', 266),
  ('Child Harm & Family Trauma', 'Items', 284),
  ('Child Harm & Family Trauma', 'Items', 287),
  ('Child Harm & Family Trauma', 'Items', 293),
  ('Child Harm & Family Trauma', 'Items', 330),
  ('Death & Dying', 'By Relationship', 158),
  ('Death & Dying', 'By Relationship', 168),
  ('Death & Dying', 'By Relationship', 194),
  ('Death & Dying', 'By Relationship', 239),
  ('Death & Dying', 'By Relationship', 313),
  ('Death & Dying', 'By Relationship', 328),
  ('Death & Dying', 'Manner of Death', 164),
  ('Death & Dying', 'Manner of Death', 187),
  ('Death & Dying', 'Manner of Death', 191),
  ('Death & Dying', 'Manner of Death', 211),
  ('Death & Dying', 'Manner of Death', 282),
  ('Death & Dying', 'Manner of Death', 286),
  ('Death & Dying', 'Manner of Death', 311),
  ('Death & Dying', 'Manner of Death', 322),
  ('Discrimination & Hate', 'Disability/Body', 233),
  ('Discrimination & Hate', 'Disability/Body', 244),
  ('Discrimination & Hate', 'Disability/Body', 248),
  ('Discrimination & Hate', 'Disability/Body', 272),
  ('Discrimination & Hate', 'Disability/Body', 300),
  ('Discrimination & Hate', 'Disability/Body', 353),
  ('Discrimination & Hate', 'LGBTQ+', 226),
  ('Discrimination & Hate', 'LGBTQ+', 247),
  ('Discrimination & Hate', 'LGBTQ+', 262),
  ('Discrimination & Hate', 'LGBTQ+', 301),
  ('Discrimination & Hate', 'LGBTQ+', 303),
  ('Discrimination & Hate', 'LGBTQ+', 310),
  ('Discrimination & Hate', 'LGBTQ+', 314),
  ('Discrimination & Hate', 'LGBTQ+', 359),
  ('Discrimination & Hate', 'LGBTQ+', 368),
  ('Discrimination & Hate', 'Racism', 212),
  ('Discrimination & Hate', 'Racism', 234),
  ('Discrimination & Hate', 'Racism', 246),
  ('Discrimination & Hate', 'Racism', 251),
  ('Discrimination & Hate', 'Racism', 294),
  ('Discrimination & Hate', 'Racism', 325),
  ('Medical & Hospital Content', 'Items', 190),
  ('Medical & Hospital Content', 'Items', 192),
  ('Medical & Hospital Content', 'Items', 201),
  ('Medical & Hospital Content', 'Items', 204),
  ('Medical & Hospital Content', 'Items', 205),
  ('Medical & Hospital Content', 'Items', 215),
  ('Medical & Hospital Content', 'Items', 238),
  ('Medical & Hospital Content', 'Items', 278),
  ('Medical & Hospital Content', 'Items', 288),
  ('Medical & Hospital Content', 'Items', 317),
  ('Medical & Hospital Content', 'Items', 327),
  ('Medical & Hospital Content', 'Items', 358),
  ('Medical & Hospital Content', 'Items', 375),
  ('Mental Health & Illness', 'Conditions', 195),
  ('Mental Health & Illness', 'Conditions', 217),
  ('Mental Health & Illness', 'Conditions', 235),
  ('Mental Health & Illness', 'Conditions', 265),
  ('Mental Health & Illness', 'Conditions', 291),
  ('Mental Health & Illness', 'Conditions', 302),
  ('Mental Health & Illness', 'Conditions', 306),
  ('Mental Health & Illness', 'Conditions', 336),
  ('Mental Health & Illness', 'Conditions', 349),
  ('Mental Health & Illness', 'Episodes & Phenomena', 206),
  ('Mental Health & Illness', 'Episodes & Phenomena', 224),
  ('Mental Health & Illness', 'Episodes & Phenomena', 334),
  ('Mental Health & Illness', 'Episodes & Phenomena', 348),
  ('Mental Health & Illness', 'Episodes & Phenomena', 370),
  ('Mental Health & Illness', 'Institutional/Treatment', 205),
  ('Mental Health & Illness', 'Institutional/Treatment', 220),
  ('Mental Health & Illness', 'Institutional/Treatment', 323),
  ('Mental Health & Illness', 'Stigma', 263),
  ('Phobias & Sensory Triggers', 'Animals/Creatures', 165),
  ('Phobias & Sensory Triggers', 'Animals/Creatures', 213),
  ('Phobias & Sensory Triggers', 'Animals/Creatures', 214),
  ('Phobias & Sensory Triggers', 'Animals/Creatures', 332),
  ('Phobias & Sensory Triggers', 'Animals/Creatures', 337),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 161),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 167),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 181),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 260),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 261),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 269),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 339),
  ('Phobias & Sensory Triggers', 'Audio/Visual Effects', 366),
  ('Phobias & Sensory Triggers', 'Body-Related', 180),
  ('Phobias & Sensory Triggers', 'Body-Related', 201),
  ('Phobias & Sensory Triggers', 'Body-Related', 257),
  ('Phobias & Sensory Triggers', 'Body-Related', 324),
  ('Phobias & Sensory Triggers', 'Body-Related', 354),
  ('Phobias & Sensory Triggers', 'Spaces/Environment', 202),
  ('Phobias & Sensory Triggers', 'Spaces/Environment', 211),
  ('Phobias & Sensory Triggers', 'Spaces/Environment', 335),
  ('Phobias & Sensory Triggers', 'Spaces/Environment', 356),
  ('Phobias & Sensory Triggers', 'Visual/Objects', 174),
  ('Phobias & Sensory Triggers', 'Visual/Objects', 295),
  ('Phobias & Sensory Triggers', 'Visual/Objects', 312),
  ('Psychological & Emotional Trauma', 'Items', 222),
  ('Psychological & Emotional Trauma', 'Items', 237),
  ('Psychological & Emotional Trauma', 'Items', 240),
  ('Psychological & Emotional Trauma', 'Items', 241),
  ('Psychological & Emotional Trauma', 'Items', 242),
  ('Psychological & Emotional Trauma', 'Items', 243),
  ('Psychological & Emotional Trauma', 'Items', 245),
  ('Psychological & Emotional Trauma', 'Items', 281),
  ('Psychological & Emotional Trauma', 'Items', 289),
  ('Psychological & Emotional Trauma', 'Items', 298),
  ('Psychological & Emotional Trauma', 'Items', 308),
  ('Psychological & Emotional Trauma', 'Items', 316),
  ('Psychological & Emotional Trauma', 'Items', 318),
  ('Psychological & Emotional Trauma', 'Items', 341),
  ('Psychological & Emotional Trauma', 'Items', 360),
  ('Psychological & Emotional Trauma', 'Items', 363),
  ('Psychological & Emotional Trauma', 'Items', 365),
  ('Self-Harm & Suicide', 'Items', 177),
  ('Self-Harm & Suicide', 'Items', 187),
  ('Self-Harm & Suicide', 'Items', 199),
  ('Self-Harm & Suicide', 'Items', 259),
  ('Self-Harm & Suicide', 'Items', 275),
  ('Self-Harm & Suicide', 'Items', 286),
  ('Self-Harm & Suicide', 'Items', 297),
  ('Self-Harm & Suicide', 'Items', 304),
  ('Sexual Content & Assault', 'Assault', 182),
  ('Sexual Content & Assault', 'Assault', 292),
  ('Sexual Content & Assault', 'Assault', 299),
  ('Sexual Content & Assault', 'Assault', 315),
  ('Sexual Content & Assault', 'Assault', 326),
  ('Sexual Content & Assault', 'Content', 197),
  ('Sexual Content & Assault', 'Content', 276),
  ('Sexual Content & Assault', 'Content', 279),
  ('Sexual Content & Assault', 'Content', 364),
  ('Sexual Content & Assault', 'Harmful Content', 287),
  ('Sexual Content & Assault', 'Harmful Content', 307),
  ('Sexual Content & Assault', 'Harmful Content', 320),
  ('Substance Use & Addiction', 'Items', 193),
  ('Substance Use & Addiction', 'Items', 225),
  ('Substance Use & Addiction', 'Items', 230),
  ('Substance Use & Addiction', 'Items', 275),
  ('Substance Use & Addiction', 'Items', 299),
  ('Violence & Gore', 'Actions', 274),
  ('Violence & Gore', 'Actions', 309),
  ('Violence & Gore', 'Actions', 321),
  ('Violence & Gore', 'Actions', 342),
  ('Violence & Gore', 'Actions', 343),
  ('Violence & Gore', 'Actions', 365),
  ('Violence & Gore', 'General Violence', 188),
  ('Violence & Gore', 'General Violence', 203),
  ('Violence & Gore', 'General Violence', 219),
  ('Violence & Gore', 'General Violence', 232),
  ('Violence & Gore', 'General Violence', 254),
  ('Violence & Gore', 'General Violence', 255),
  ('Violence & Gore', 'General Violence', 267),
  ('Violence & Gore', 'General Violence', 296),
  ('Violence & Gore', 'General Violence', 367),
  ('Violence & Gore', 'General Violence', 378),
  ('Violence & Gore', 'Specific Injuries', 171),
  ('Violence & Gore', 'Specific Injuries', 200),
  ('Violence & Gore', 'Specific Injuries', 210),
  ('Violence & Gore', 'Specific Injuries', 216),
  ('Violence & Gore', 'Specific Injuries', 223),
  ('Violence & Gore', 'Specific Injuries', 250),
  ('Violence & Gore', 'Specific Injuries', 258),
  ('Violence & Gore', 'Specific Injuries', 280),
  ('Violence & Gore', 'Specific Injuries', 331),
  ('Violence & Gore', 'Specific Injuries', 352),
  ('Violence & Gore', 'Specific Injuries', 361),
  ('Violence & Gore', 'Specific Injuries', 362);

-- Upsert subcategories
INSERT INTO public.warning_subcategories (category_id, subcategory_name)
SELECT wc.category_id, a.subcategory_name
FROM _allowed_subcategories a
JOIN public.warning_categories wc
  ON wc.category_name = a.category_name
ON CONFLICT (category_id, subcategory_name)
DO UPDATE SET updated_at = now();

-- Prune subcategory-topic mappings no longer present in YAML
DELETE FROM public.warning_subcategory_topics wst
USING public.warning_subcategories ws, public.warning_categories wc
WHERE wst.subcategory_id = ws.subcategory_id
  AND ws.category_id = wc.category_id
  AND NOT EXISTS (
    SELECT 1
    FROM _allowed_subcategory_topics a
    WHERE a.category_name = wc.category_name
      AND a.subcategory_name = ws.subcategory_name
      AND a.dtdd_topic_id = wst.dtdd_topic_id
  );

-- Insert allowed subcategory-topic mappings
INSERT INTO public.warning_subcategory_topics (subcategory_id, dtdd_topic_id)
SELECT ws.subcategory_id, a.dtdd_topic_id
FROM _allowed_subcategory_topics a
JOIN public.warning_categories wc
  ON wc.category_name = a.category_name
JOIN public.warning_subcategories ws
  ON ws.category_id = wc.category_id
 AND ws.subcategory_name = a.subcategory_name
ON CONFLICT DO NOTHING;

-- Prune subcategories no longer present in YAML
DELETE FROM public.warning_subcategories ws
USING public.warning_categories wc
WHERE ws.category_id = wc.category_id
  AND NOT EXISTS (
    SELECT 1
    FROM _allowed_subcategories a
    WHERE a.category_name = wc.category_name
      AND a.subcategory_name = ws.subcategory_name
  );

COMMIT;
