BEGIN;

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2, 'Apple TV', '/SPnB1qiCkYfirS2it3hZORwGVn.jpg', 7
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  3, 'Google Play Movies', '/8z7rC8uIDaTM91X0ZfkRf04ydj2.jpg', 16
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  7, 'Fandango At Home', '/19fkcOz0xeUgCVW8tO85uOYnYK9.jpg', 35
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  8, 'Netflix', '/pbpMk2JmcoNnQwx5JGpXngfoWtp.jpg', 1
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  9, 'Amazon Prime Video', '/pvske1MyAoymrs5bguRfVqYiM9a.jpg', 5
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  10, 'Amazon Video', '/qR6FKvnPBx2O37FDg8PNM7efwF3.jpg', 6
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  15, 'Hulu', '/bxBlRPEPpMVDc4jMhSrTf2339DW.jpg', 8
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  34, 'MGM Plus', '/ctiRpS16dlaTXQBSsiFncMrgWmh.jpg', 50
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  73, 'Tubi TV', '/zLYr7OPvpskMA4S79E3vlCi71iC.jpg', 364
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  99, 'Shudder', '/vEtdiYRPRbDCp1Tcn3BEPF1Ni76.jpg', 55
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  191, 'Kanopy', '/rcBwnERpNfPfWB5DaSTyEMCZbCA.jpg', 33
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  192, 'YouTube', '/pTnn5JwWr4p3pG8H6VrpiQo7Vs0.jpg', 17
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  204, 'Shudder Amazon Channel', '/qb6Lj5BhNJavdmRVDzAqAjd4Tj3.jpg', 84
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  207, 'The Roku Channel', '/wQzSN83BnWVgO7xEh0SeTVqtrFv.jpg', 25
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  212, 'Hoopla', '/j7D006Uy3UWwZ6G0xH6BMgIWTzH.jpg', 34
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  257, 'fuboTV', '/9BgaNQRMDvVlji1JBZi6tcfxpKx.jpg', 10
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  331, 'FlixFling', '/yFGu4sSzwUMfhwmSsZgez8QhaVl.jpg', 95
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  337, 'Disney Plus', '/97yvRBw1GzX7fXprcF80er19ot.jpg', 3
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  350, 'Apple TV', '/mcbz1LgtErU9p4UdbZ0rG6RTWHX.jpg', 4
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  386, 'Peacock Premium', '/2aGrp1xw3qhwCYvNGAJZPdjfeeX.jpg', 15
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  387, 'Peacock Premium Plus', '/drPlq5beqXtBaP7MNs8W616YRhm.jpg', 217
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  486, 'Spectrum On Demand', '/aAb9CUHjFe9Y3O57qnrJH0KOF1B.jpg', 121
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  526, 'AMC+', '/ovmu6uot1XVvsemM2dDySXLiX57.jpg', 29
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  528, 'AMC+ Amazon Channel', '/2ino0WmHA4GROB7NYKzT6PGqLcb.jpg', 24
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  538, 'Plex', '/vLZKlXUNDcZR7ilvfY9Wr9k80FZ.jpg', 127
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  575, 'OnDemandKorea', '/gR8rrj71VCLjlF1LJpplo72MXf0.jpg', 138
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  582, 'Paramount+ Amazon Channel', '/hExO4PtimLIYn3kBOrzsejNv7cT.jpg', 22
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  583, 'MGM+ Amazon Channel', '/efu1Cqc63XrPBoreYnf2mn0Nizj.jpg', 13
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  633, 'Paramount+ Roku Premium Channel', '/ywIoxSjoYJGUIbR6BfxUiCHdPi3.jpg', 26
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  1796, 'Netflix Standard with Ads', '/dpR8r13zWDeUR0QkzWidrdMxa56.jpg', 155
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  1825, 'HBO Max Amazon Channel', '/5AkEgpCNaBP3Fbwd2m1GdQIk0vv.jpg', 12
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  1854, 'AMC Plus Apple TV Channel ', '/oTQdXIqM9iewlN4MC2nhKB0gHw.jpg', 20
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  1855, 'Starz Apple TV Channel', '/1C5EVCWyQD798CE1DFfcm6oAbxP.jpg', 19
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  1899, 'HBO Max', '/jbe4gVSfRlbPTdESXhEKpornsfu.jpg', 11
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2049, 'Shudder Apple TV Channel', '/kLfq0I2MwiUFUY9yI1GwOeKxX8f.jpg', 189
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2077, 'Plex Channel', '/27WMfRN7pQE3j5Khm8fPM7vYyLV.jpg', 128
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2100, 'Amazon Prime Video with Ads', '/8aBqoNeGGr0oSA85iopgNZUOTOc.jpg', 206
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2243, 'Apple TV Amazon Channel', '/mHrYMgnZIp6lgW2aXg7ix9zGOnA.jpg', 220
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2285, 'JustWatch TV', '/g2IaWyo6jCY0rIFjb4qgZ0bSmm3.jpg', 42
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2303, 'Paramount Plus Premium', '/fts6X10Jn4QT0X6ac3udKEn2tJA.jpg', 367
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2383, 'Philo', '/ptmbGSttkyzawLbxx9MElmxKuVo.jpg', 52
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2409, 'Fawesome', '/pSUa7lMYLoQAU00ikXoHxmOfTZ9.jpg', 146
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2478, 'FOUND TV', '/mD3KV7olfMJOBxJqKKPjvNrpVJF.jpg', 338
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2528, 'YouTube TV', '/x9zOHTUkQzt3PgPVKbMH9CKBwLK.jpg', 36
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2555, 'Bloodstream', '/qlsnWCrmcisQH5JnEDxPV2v2vlb.jpg', 356
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();

INSERT INTO streaming_providers (
  tmdb_provider_id, provider_name, logo_path, display_priority
) VALUES (
  2616, 'Paramount Plus Essential', '/5wym1C0jAvJeGirPdgVpcW0CCuy.jpg', 366
)
ON CONFLICT (tmdb_provider_id) DO UPDATE SET
  provider_name = EXCLUDED.provider_name,
  logo_path = COALESCE(EXCLUDED.logo_path, streaming_providers.logo_path),
  display_priority = COALESCE(EXCLUDED.display_priority, streaming_providers.display_priority),
  updated_at = NOW();


INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31227572', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29780866', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31495504', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31495504', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10799690', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37893389', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13186306', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13186306', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22740896', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22740896', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt22261722', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 191, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 575, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30254719', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30274401', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt4712810', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37268550', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29927663', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29927663', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 1899, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 1825, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30144839', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6604188', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 257, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0137523', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 538, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 2077, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt35851026', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32395062', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 331, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 2555, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 2409, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 207, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 73, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14940390', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34509472', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29544253', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1630029', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29538620', 331, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 2528, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 191, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0295701', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14142060', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31844586', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt11344566', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt11344566', 15, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0499549', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0848228', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14205554', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14205554', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 1854, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 528, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 526, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 2383, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 99, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 204, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 2049, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32263401', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 583, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 582, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 633, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 34, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2383, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2616, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2303, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2285, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14107334', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30219247', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 15, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt6300910', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt19847976', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1312221', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt1312221', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30472557', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32537226', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32537226', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32537226', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32537226', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32537226', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 582, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 633, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 2616, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 2303, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 2285, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt9603208', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 350, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 2243, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt16311594', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 583, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 582, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 633, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 34, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 2383, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 2616, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 2303, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 2285, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 486, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0816692', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 1899, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 1825, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34956443', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14364480', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt14364480', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26439764', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26439764', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt2948356', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 386, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 387, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31036941', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 337, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt10676052', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 15, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27196021', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt39307872', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt39307872', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt20254992', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30343021', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30343021', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30343021', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt30343021', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29232158', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29232158', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 1899, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 1825, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt31193180', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 386, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 387, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 538, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 2077, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 207, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 73, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt13176330', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34276058', 350, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt34276058', 2243, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 34, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt36153493', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt21909764', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt21909764', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 386, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 387, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt26743210', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32831759', 9, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32831759', 2100, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32164798', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32164798', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32164798', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32164798', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 73, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27589902', 331, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 386, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 387, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt12300742', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 1899, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 1825, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt5950044', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 386, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 387, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 486, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0111161', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 15, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27829510', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 191, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 207, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 2285, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 331, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 486, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0068646', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 1855, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 582, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 633, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 34, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 2616, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 2303, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 191, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 212, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 486, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 538, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt0120338', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29567915', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 2, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 7, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 2, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt32123395', 7, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt27208407', 73, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 2409, 'free'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 2478, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 73, 'free_with_ads'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 10, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 3, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 192, 'rent'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 10, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 3, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt29954526', 192, 'buy'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37961727', 8, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

INSERT INTO movie_streaming (
  imdb_id, tmdb_provider_id, offer_type
) VALUES (
  'tt37961727', 1796, 'subscription'
)
ON CONFLICT (imdb_id, tmdb_provider_id, offer_type) DO NOTHING;

COMMIT;
