-- Generated 20260117_104044
BEGIN;

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  230, 'The Godfather Collection', 'https://image.tmdb.org/t/p/w500/e5iVtjkjM30znn86JsvkBYtvEo1.jpg', 'https://image.tmdb.org/t/p/w500/dIqp8unLVLykgDAlHNalrRVOKjI.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  328, 'Jurassic Park Collection', 'https://image.tmdb.org/t/p/w500/qIm2nHXLpBBdMxi8dvfrnDkBUDh.jpg', 'https://image.tmdb.org/t/p/w500/njFixYzIxX8jsn6KMSEtAzi4avi.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  399, 'Predator Collection', 'https://image.tmdb.org/t/p/w500/mm2t5dd1QFxtX6X56Z9U5ucsIb1.jpg', 'https://image.tmdb.org/t/p/w500/u1CTqTYRUVyLwlEd1z1VJrVBp0J.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1565, '28 Days/Weeks/Years Later Collection', 'https://image.tmdb.org/t/p/w500/2DMKMinwjGb45rC2nA4nr1JsT37.jpg', 'https://image.tmdb.org/t/p/w500/liImaqVpPGo3ddLWkJPeJxvaqCI.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  52785, 'xXx Collection', 'https://image.tmdb.org/t/p/w500/s13xGJSH3eEWPjSjDvI1iCqkwVg.jpg', 'https://image.tmdb.org/t/p/w500/jSfCi0onZRDxbeg2dPJgB5iAoxX.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  63043, 'TRON Collection', 'https://image.tmdb.org/t/p/w500/l0iZbJXY8MPLSqibet4M2rzrBw4.jpg', 'https://image.tmdb.org/t/p/w500/4INQEOQ96EvYmANsCZaad9VvRWW.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  86311, 'The Avengers Collection', 'https://image.tmdb.org/t/p/w500/yFSIUVTCvgYrpalUktulvk3Gi5Y.jpg', 'https://image.tmdb.org/t/p/w500/zuW6fOiusv4X9nnW3paHGfXcSll.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  87096, 'Avatar Collection', 'https://image.tmdb.org/t/p/w500/3C5brXxnBxfkeKWwA1Fh4xvy4wr.jpg', 'https://image.tmdb.org/t/p/w500/6qkJLRCZp9Y3ovXti5tSuhH0DpO.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  87359, 'Mission: Impossible Collection', 'https://image.tmdb.org/t/p/w500/geEjCGfdmRAA1skBPwojcdvnZ8A.jpg', 'https://image.tmdb.org/t/p/w500/mroWh717g0Ah2c0rrPGW6f3EWMM.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  275402, 'SpongeBob Collection', 'https://image.tmdb.org/t/p/w500/caA0tTR0ye1DEnCAUIolrWajmfO.jpg', 'https://image.tmdb.org/t/p/w500/82aBKm6QfMzNYl7jk9VG4mMPOYu.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  382685, 'Now You See Me Collection', 'https://image.tmdb.org/t/p/w500/n2B51uCX5r960pFbEl9jMbTMuvq.jpg', 'https://image.tmdb.org/t/p/w500/scBzORmLqP00h8hPKS19TLtyRQm.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  722971, 'Knives Out Collection', 'https://image.tmdb.org/t/p/w500/pkjW3K0UDnwYWDcUR83VzfHNQA4.jpg', 'https://image.tmdb.org/t/p/w500/G7qYINSq5xyDd0I0zn3DpAssA0.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  840465, 'Greenland Collection', 'https://image.tmdb.org/t/p/w500/naNSFJ2bQqGPNpO7ZC71vxkDhr4.jpg', 'https://image.tmdb.org/t/p/w500/bmfsqt3VZgXE0KRGxalFa9NpBYO.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  925155, 'Demon Slayer: Kimetsu no Yaiba Collection', 'https://image.tmdb.org/t/p/w500/3exjjYTseefny9nYjSbkIblZZdK.jpg', 'https://image.tmdb.org/t/p/w500/oHx043lHsysn8klll1nPvKMBHLf.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  968080, 'Wicked Collection', 'https://image.tmdb.org/t/p/w500/b9xo966oVIrFtpevhLQ9XILcXTh.jpg', 'https://image.tmdb.org/t/p/w500/oXIhFH4m4ONWScQx6TrbUdPzMv9.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1041720, 'School of the Magical Animals Collection', 'https://image.tmdb.org/t/p/w500/bBiUtPFrKMJpmuwCBczdDD0IwCL.jpg', 'https://image.tmdb.org/t/p/w500/9NEzHx2ygjEsICTRLZeG0V4Hgl7.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1084247, 'Zootopia Collection', 'https://image.tmdb.org/t/p/w500/u8597pdwSHOvcJQhP2cLi7a67ZW.jpg', 'https://image.tmdb.org/t/p/w500/1WDssJDYInLA4Avg45lgy3WM6Ly.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1149170, 'Cash Out - Saga', 'https://image.tmdb.org/t/p/w500/kn9NvcP3bbfRFq6HPNKoSclsidZ.jpg', 'https://image.tmdb.org/t/p/w500/6gsGMzrBNLQX46zerTUB2g2kUCQ.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1156666, 'Fault Collection', 'https://image.tmdb.org/t/p/w500/fR3AsfNeAmgGCy2CMFaOjIaIuA7.jpg', 'https://image.tmdb.org/t/p/w500/7N9p8D39MXJDfm14zlBwW7mBh7g.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1180834, 'Troll (2022) Collection', 'https://image.tmdb.org/t/p/w500/A5kWJJaBeFPTVqAW4peaubw1JqA.jpg', 'https://image.tmdb.org/t/p/w500/o8pjPgPsAleCTJr93FMmQOUZ05q.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1240438, 'Five Nights at Freddy''s Collection', 'https://image.tmdb.org/t/p/w500/mziuu7XpcnsOb6eRvHDi666U6Nc.jpg', 'https://image.tmdb.org/t/p/w500/537TH9PnmUk5X0FZaRP0wH1PJqV.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1262937, 'Sisu Collection', 'https://image.tmdb.org/t/p/w500/vMJdvbDvISk2GtbsbGi1vodkusW.jpg', 'https://image.tmdb.org/t/p/w500/dEiZ4HMNCXNr1abZIv6dfiyAd2Q.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1363124, 'The Family Plan Collection', 'https://image.tmdb.org/t/p/w500/etf8OuJwaVYCa9NfQTrpPoTvvbj.jpg', 'https://image.tmdb.org/t/p/w500/1nCczfmL9rNDOBJ53w4NrSqJu6F.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1449595, 'Ne Zha Collection', 'https://image.tmdb.org/t/p/w500/uY4ICaCkGIbDgiu7quRz5kfZXda.jpg', 'https://image.tmdb.org/t/p/w500/kazABXcuhF5AdpdjlWQQCH8iUyR.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1452167, 'The Myth Collection', 'https://image.tmdb.org/t/p/w500/6TRuMYz8mmbBxucQbyksbOFfieY.jpg', 'https://image.tmdb.org/t/p/w500/5GC4cUCAk5RSqVVZWsbCFh6KbEa.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1458864, 'How to Train Your Dragon (Live-Action) Collection', 'https://image.tmdb.org/t/p/w500/5uiXWKH2dcWcQDP2GVFLGTmcGRp.jpg', 'https://image.tmdb.org/t/p/w500/eKpWn8DwS6xpAKs4eLb4PmrXnhk.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1516488, 'Kantara Collection', NULL, NULL
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1529614, 'Jujutsu Kaisen Collection', 'https://image.tmdb.org/t/p/w500/9RJ4SC2nXfeUyGbnCXQ1lPAXnbA.jpg', 'https://image.tmdb.org/t/p/w500/wa3t5d9antb8arhLbkeODw7CsmU.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1540907, 'Superman (DCU) Collection', 'https://image.tmdb.org/t/p/w500/2L0dVc82ZWUgo6Fe2lnJ5qqUDId.jpg', 'https://image.tmdb.org/t/p/w500/6xQWbcYRHoVDRkyspTy64x8l22m.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1570181, 'Muzzle Collection', 'https://image.tmdb.org/t/p/w500/iU124bwXVhS5MmN1rTt0qgLIACM.jpg', NULL
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1593311, 'Dhurandhar Collection', NULL, NULL
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1611146, 'The Housemaid Collection', 'https://image.tmdb.org/t/p/w500/nzfd4GSmANTyJVVryteFU3Xjuje.jpg', 'https://image.tmdb.org/t/p/w500/lu7VARBXX1B5XjwYh3KhcV7WlGW.jpg'
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.collections (
  tmdb_collection_id, collection_name, poster_url, backdrop_url
) VALUES (
  1614429, 'The RajaSaab Collection', NULL, NULL
)
ON CONFLICT (tmdb_collection_id) DO UPDATE SET
  collection_name = EXCLUDED.collection_name,
  poster_url = EXCLUDED.poster_url,
  backdrop_url = EXCLUDED.backdrop_url,
  updated_at = NOW();

INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt31227572', 399
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt1757678', 87096
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt26443597', 1084247
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt27543632', 1611146
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt32820897', 925155
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt30274401', 1240438
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt4712810', 382685
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt6604188', 63043
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt32395062', 1570181
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt38121182', 1529614
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt1630029', 87096
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt0295701', 52785
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt31844586', 1262937
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt0499549', 87096
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt14850054', 840465
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt0848228', 86311
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt19847976', 968080
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt9603208', 87359
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt34956443', 1449595
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt14364480', 722971
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt26439764', 1516488
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt2948356', 1084247
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt31036941', 328
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt23790696', 1614429
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt32141377', 1565
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt32241578', 1041720
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt29232158', 1180834
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt34276058', 1363124
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt21909764', 1156666
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt26743210', 1458864
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt27589902', 1452167
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt23572848', 275402
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt5950044', 1540907
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt33014583', 1593311
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt27829510', 1149170
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;
INSERT INTO public.movie_collections (
  imdb_id, tmdb_collection_id
) VALUES (
  'tt0068646', 230
)
ON CONFLICT (imdb_id) DO UPDATE SET
  tmdb_collection_id = EXCLUDED.tmdb_collection_id;

COMMIT;
