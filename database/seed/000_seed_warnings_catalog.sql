\encoding UTF8
-- Generated 20260114_122750
BEGIN;

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (153, 'a dog dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (158, 'a kid dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (161, 'there are jump scares', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (164, 'someone is burned alive', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (165, 'there are spiders', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (167, 'there''s flashing lights or images', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (168, 'a parent dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (171, 'there''s finger/toe mutilation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (174, 'there are clowns', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (176, 'there are shower scenes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (177, 'there''s shaving/cutting', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (180, 'there''s spitting', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (181, 'shaky cam is used', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (182, 'someone is sexually assaulted', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (183, 'a horse dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (184, 'a car crashes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (186, 'a cat dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (187, 'someone dies by suicide', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (188, 'there''s blood/gore', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (189, 'an animal dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (190, 'needles/syringes are used', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (191, 'someone drowns', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (192, 'there''s a hospital scene', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (193, 'someone uses drugs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (194, 'an LGBT person dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (195, 'there''s body dysmorphia', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (196, 'a dragon dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (197, 'there is sexual content', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (198, 'a plane crashes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (199, 'someone self harms', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (200, 'there''s eye mutilation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (201, 'someone vomits', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (202, 'there''s a claustrophobic scene', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (203, 'there''s torture', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (204, 'someone has cancer', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (205, 'electro-therapy is used', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (206, 'someone has a seizure', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (207, 'there''s ghosts', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (208, 'a person is hit by a car', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (209, 'Santa (et al) is spoiled', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (210, 'teeth are damaged', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (211, 'someone falls to their death', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (212, 'someone speaks hate speech', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (213, 'there are bugs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (214, 'there are snakes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (215, 'someone miscarries', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (216, 'someone breaks a bone', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (217, 'someone has an eating disorder', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (218, 'there''s child abuse', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (219, 'there''s domestic violence', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (220, 'there''s a mental institution scene', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (221, 'there''s a nuclear explosion', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (222, 'the ending is sad', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (223, 'heads get squashed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (224, 'someone is possessed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (225, 'alcohol abuse', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (226, 'someone is misgendered', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (227, 'there are hangings', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (228, 'there''s childbirth', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (229, 'animals are abused', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (230, 'there''s addiction', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (231, 'there''s dog fighting', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (232, 'there''s gun violence', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (233, 'there''s fat jokes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (234, 'the black guy dies first', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (235, 'someone has an anxiety attack', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (236, 'there are incestuous relationships', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (237, 'someone gets gaslighted', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (238, 'someone has an abortion', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (239, 'a pregnant person dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (240, 'someone is buried alive', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (241, 'someone cheats', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (242, 'someone is stalked', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (243, 'someone is kidnapped', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (244, 'there''s ableist language or behavior', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (245, 'someone struggles to breathe', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (246, 'there''s antisemitism', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (247, 'there are homophobic slurs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (248, 'Autism specific abuse', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (250, 'there''s amputation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (251, 'someone says the n-word', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (252, 'there''s a dead animal', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (253, 'A child''s dear toy is destroyed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (254, 'there''s cannibalism', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (255, 'There''s audio gore', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (256, 'there is copaganda', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (257, 'someone wets/soils themselves', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (258, 'there''s genital trauma/mutilation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (259, 'someone says "I''ll kill myself"', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (260, 'there''s misophonia', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (261, 'a baby cries', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (262, 'there are "Man in a dress" jokes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (263, 'a mentally ill person is violent', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (264, 'a baby is stillborn', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (265, 'someone suffers from PTSD', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (266, 'there is a baby or unborn child', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (267, 'there''s excessive gore', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (268, 'the fourth wall is broken', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (269, 'a car honks or tires screech', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (270, 'someone is homeless', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (271, 'someone falls down stairs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (272, 'the r-slur is used', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (273, 'a male character is ridiculed for crying', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (274, 'someone is restrained', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (275, 'someone overdoses', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (276, 'someone is sexually objectified', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (277, 'there''s a large age gap', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (278, 'someone has a stroke', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (279, 'there are nude scenes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (280, 'there''s Achilles Tendon injury', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (281, 'someone asphyxiates', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (282, 'someone is crushed to death', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (283, 'there are 9/11 depictions', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (284, 'an infant is abducted', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (285, 'a pet dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (286, 'Someone attempts suicide', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (287, 'a minor is sexualized', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (288, 'someone has a chronic illness', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (289, 'someone sacrifices themselves', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (290, 'there is obscene language/gestures', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (291, 'someone has dementia/Alzheimer''s', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (292, 'someone is sexually assaulted onscreen', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (293, 'a child is abandoned by a parent or guardian', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (294, 'a minority is misrepresented', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (295, 'there are mannequins', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (296, 'there''s body horror', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (297, 'there are razors', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (298, 'Someone becomes unconscious', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (299, 'someone is drugged', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (300, 'there''s fat suits', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (301, 'there''s bisexual cheating', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (302, 'D.I.D. Misrepresentation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (303, 'there''s aphobia', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (304, 'the abused becomes the abuser', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (305, 'a non-human character dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (306, 'there''s body dysphoria', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (307, 'there''s bestiality', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (308, 'someone is held under water', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (309, 'somebody is choked', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (310, 'there''s deadnaming or birthnaming', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (311, 'someone dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (312, 'trypophobic content is shown', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (313, 'a family member dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (314, 'there are transphobic slurs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (315, 'sexual assault on men is a joke', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (316, 'there''s anti-abortion themes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (317, 'someone loses their virginity', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (318, 'someone is watched without knowing', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (319, 'animals were harmed in the making', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (320, 'there''s pedophilia', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (321, 'someone is beaten up by a bully', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (322, 'someone is eaten', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (323, 'there''s ABA therapy', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (324, 'there''s farting', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (325, 'there''s blackface', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (326, 'rape is mentioned', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (327, 'someone is terminally ill', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (328, 'a major character dies', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (329, 'a priceless artifact is destroyed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (330, 'there''s abusive parents', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (331, 'there''s decapitation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (332, 'there''s an alligator/crocodile', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (334, 'reality is unstable or unhinged', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (335, 'there''s natural bodies of water', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (336, 'someone has a mental illness', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (337, 'there are sharks', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (338, 'rabbits are harmed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (339, 'there are sudden loud noises', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (341, 'existentialism is debated', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (342, 'a woman gets slapped', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (343, 'someone is stabbed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (345, 'there''s incarceration', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (346, 'there''s end credits scenes?', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (347, 'the abused forgives their abuser', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (348, 'someone has a meltdown', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (349, 'autism is misrepresented', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (350, 'an animal is abandoned', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (351, 'religion is discussed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (352, 'hands are damaged', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (353, 'someone disabled played by able-bodied', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (354, 'someone poops on-screen', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (355, 'an animal is sad', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (356, 'there''s underwater scenes', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (357, 'there''s bedbugs', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (358, 'there''s menstruation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (359, 'a trans person is depicted predatorily', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (360, 'someone''s mouth is covered', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (361, 'someone''s throat is mutilated', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (362, 'someone dislocates something', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (363, 'someone leaves without saying goodbye', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (364, 'there''s BDSM', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (365, 'someone is abused with a belt', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (366, 'there''s screaming', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (367, 'a woman is brutalized for spectacle', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (368, 'an LGBT+ person is outed', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (369, 'there''s demons or Hell', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (370, 'there''s dissociation, depersonalization, or derealization', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (371, 'there''s audience participation', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (372, 'there''s smoke or haze', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

INSERT INTO warnings (dtdd_topic_id, topic_name, topic_type, parent_dtdd_topic_id, tier)
VALUES (375, 'someone has a heart attack', NULL, NULL, NULL)
ON CONFLICT (dtdd_topic_id) DO UPDATE SET
  topic_name = EXCLUDED.topic_name,
  topic_type = COALESCE(EXCLUDED.topic_type, warnings.topic_type),
  parent_dtdd_topic_id = COALESCE(EXCLUDED.parent_dtdd_topic_id, warnings.parent_dtdd_topic_id),
  tier = COALESCE(EXCLUDED.tier, warnings.tier),
  updated_at = now();

COMMIT;
