BEGIN;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31227572', 1242898, 'Predator: Badlands', 2025,
  NULL, 107, 'Cast out from his clan, a young Predator finds an unlikely ally in a damaged android and embarks on a treacherous journey in search of the ultimate adversary.', 'https://image.tmdb.org/t/p/w500/pHpq9yNUIo6aDoCXEBzjSolywgz.jpg',
  'en', 'movie', 'First hunt. Last chance.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29780866', 1242501, 'Icefall', 2025,
  NULL, 99, 'A young Indigenous game warden arrests an infamous poacher only to discover that the poacher knows the location of a plane carrying millions of dollars that has crashed in a frozen lake. When a group of criminals and dirty cops are alerted to the poacher’s whereabouts, the warden and the poacher team up to fight back and escape across the treacherous lake before the ice melts.', 'https://image.tmdb.org/t/p/w500/5Byv6nznAb2Izd0gHpODOXnuSbo.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt1757678', 83533, 'Avatar: Fire and Ash', 2025,
  NULL, 198, 'In the wake of the devastating war against the RDA and the loss of their eldest son, Jake Sully and Neytiri face a new threat on Pandora: the Ash People, a violent and power-hungry Na''vi tribe led by the ruthless Varang. Jake''s family must fight for their survival and the future of Pandora in a conflict that pushes them to their emotional and physical limits.', 'https://image.tmdb.org/t/p/w500/lE9KpVwgeWHMwgwkNaeH5nEFh20.jpg',
  'en', 'movie', 'The world of Pandora will change forever.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31495504', 1252037, 'The Tank', 2025,
  NULL, 117, 'A German Tiger tank crew is sent on a dangerous mission to rescue the missing officer Paul von Hardenburg from a top-secret bunker behind enemy lines. As they make their way through the lethal no-man''s land, they must confront not only the enemy, but also their own fears and inner demons. Fueled by the Wehrmacht''s methamphetamine, their mission increasingly becomes a journey into the heart of darkness.', 'https://image.tmdb.org/t/p/w500/65Jr1JAgWlu9em8zHhAfrNJJQBt.jpg',
  'de', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt26443597', 1084242, 'Zootopia 2', 2025,
  NULL, 107, 'After cracking the biggest case in Zootopia''s history, rookie cops Judy Hopps and Nick Wilde find themselves on the twisting trail of a great mystery when Gary De''Snake arrives and turns the animal metropolis upside down. To crack the case, Judy and Nick must go undercover to unexpected new parts of town, where their growing partnership is tested like never before.', 'https://image.tmdb.org/t/p/w500/bjUWGw0Ao0qVWxagN3VCwBJHVo6.jpg',
  'en', 'movie', 'They''re back with a twissst.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt27543632', 1368166, 'The Housemaid', 2025,
  NULL, 131, 'Trying to escape her past, Millie Calloway accepts a job as a live-in housemaid for the wealthy Nina and Andrew Winchester. But what begins as a dream job quickly unravels into something far more dangerous—a sexy, seductive game of secrets, scandal, and power.', 'https://image.tmdb.org/t/p/w500/cWsBscZzwu5brg9YjNkGewRUvJX.jpg',
  'en', 'movie', 'Discover what lies behind closed doors.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt10799690', 628847, 'Trap House', 2025,
  NULL, 102, 'An undercover DEA agent and his partner embark on a game of cat and mouse with an audacious, and surprising group of thieves - their own rebellious teenagers, who have begun robbing from a dangerous cartel, using their parents'' tactics and top-secret intel to do it.', 'https://image.tmdb.org/t/p/w500/6tpAPeuuqbVnYWWPoOLEDLSBU7a.jpg',
  'en', 'movie', 'This isn''t a raid. It''s a reckoning.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt37893389', 1511417, 'Bāhubali: The Epic', 2025,
  NULL, 224, 'When a mysterious child is found by a tribal couple near a roaring waterfall, they raise him as their own. As he grows, Sivudu is drawn to the world beyond the cliffs, where he discovers the ancient kingdom of Mahishmati, ruled by a cruel tyrant, haunted by rebellion, and bound to his past. What begins as a quest for love soon unravels a legacy of betrayal, sacrifice, and a forgotten prince.', 'https://image.tmdb.org/t/p/w500/z9YIo2qscyaXYgRqIdRJtND3bw8.jpg',
  'te', 'movie', 'Remastered & Re-Cut of Baahubali: The Beginning (2015) & Baahubali 2: The Conclusion (2017) into One Film', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32820897', 1311031, 'Demon Slayer: Kimetsu no Yaiba Infinity Castle', 2025,
  NULL, 156, 'The Demon Slayer Corps are drawn into the Infinity Castle, where Tanjiro, Nezuko, and the Hashira face terrifying Upper Rank demons in a desperate fight as the final battle against Muzan Kibutsuji begins.', 'https://image.tmdb.org/t/p/w500/fWVSwgjpT2D78VUh6X8UBd2rorW.jpg',
  'ja', 'movie', 'It''s time to have some fun.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt13186306', 755898, 'War of the Worlds', 2025,
  NULL, 91, 'Will Radford is a top analyst for Homeland Security who tracks potential threats through a mass surveillance program, until one day an attack by an unknown entity leads him to question whether the government is hiding something from him... and from the rest of the world.', 'https://image.tmdb.org/t/p/w500/yvirUYrva23IudARHn3mMGVxWqM.jpg',
  'en', 'movie', 'Your data is deadly.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt22740896', 1034716, 'People We Meet on Vacation', 2026,
  NULL, 116, 'Poppy''s a free spirit. Alex loves a plan. After years of summer vacations, these polar-opposite pals wonder if they could be a perfect romantic match.', 'https://image.tmdb.org/t/p/w500/kOrJqRyt1pklNgDwJMjzN1GuNXS.jpg',
  'en', 'movie', 'On vacation, you''re free to follow your heart.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt22261722', 1301122, 'Nazi Supersoldier', 2024,
  NULL, 78, 'A group of journalism students ventures into a remote forest and awakens a long-frozen World War II experiment a superhuman Nazi soldier. Hunted through cursed woods where trust crumbles and survival is slim, they soon learn that some sins demand blood.', 'https://image.tmdb.org/t/p/w500/3daBOc5ZcICXMzAVNhzJTSgweaP.jpg',
  'en', 'movie', 'They awakened a monster.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30254719', 1131759, 'Omniscient Reader: The Prophecy', 2025,
  NULL, 117, 'Kim Dok-ja, an ordinary man in his 20s, is the only reader of an obscure web novel titled "Three Ways to Survive the Apocalypse". After having read the last chapter, the novel suddenly becomes reality, and its omnipotent hero Yu Jung-hyeok appears before Kim. As the only person who knows how to survive in this world, Kim and his companions strive to save the world by writing his own, new ending.', 'https://image.tmdb.org/t/p/w500/3R3dXO2nm8JyR5NG7SEfii7RzlV.jpg',
  'ko', 'movie', 'A novel known only by me has become reality.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30274401', 1228246, 'Five Nights at Freddy''s 2', 2025,
  NULL, 104, 'One year since the supernatural nightmare at Freddy Fazbear''s Pizza, the stories about what transpired there have been twisted into a campy local legend, inspiring the town''s first ever Fazfest. With the truth kept from her, Abby sneaks out to reconnect with Freddy, Bonnie, Chica, and Foxy, setting into motion a terrifying series of events that will reveal dark secrets about the real origin of Freddy''s, and unleash a decades-hidden horror.', 'https://image.tmdb.org/t/p/w500/udAxQEORq2I5wxI97N2TEqdhzBE.jpg',
  'en', 'movie', 'You there?', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt4712810', 425274, 'Now You See Me: Now You Don''t', 2025,
  NULL, 113, 'The original Four Horsemen reunite with a new generation of illusionists to take on powerful diamond heiress Veronika Vanderberg, who leads a criminal empire built on money laundering and trafficking. The new and old magicians must overcome their differences to work together on their most ambitious heist yet.', 'https://image.tmdb.org/t/p/w500/oD3Eey4e4Z259XLm3eD3WGcoJAh.jpg',
  'en', 'movie', 'Unlock the illusion.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt37268550', 1512623, 'Risqué', 2025,
  NULL, 91, 'Fired from a strip club, a vengeful dancer masterminds a high-stakes heist with fellow strippers to take down the corrupt boss and the men who underestimate them.', 'https://image.tmdb.org/t/p/w500/h6OsRrDwbspLnKMvlFl57QFDP7d.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt34610311', 1419406, 'The Shadow''s Edge', 2025,
  NULL, 142, 'Macau Police brings the tracking expert police officer out of retirement to help catch a dangerous group of professional thieves.', 'https://image.tmdb.org/t/p/w500/e0RU6KpdnrqFxDKlI3NOqN8nHL6.jpg',
  'zh', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29927663', 982843, 'The Great Flood', 2025,
  NULL, 109, 'When a raging flood traps a researcher and her young son, a call to a crucial mission puts their escape — and the future of humanity — on the line.', 'https://image.tmdb.org/t/p/w500/1tUOZQDgZaGqZtrB21MieiXARL2.jpg',
  'ko', 'movie', 'The last day on earth. The one choice for survival.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30144839', 1054867, 'One Battle After Another', 2025,
  NULL, 162, 'Washed-up revolutionary Bob exists in a state of stoned paranoia, surviving off-grid with his spirited, self-reliant daughter, Willa. When his evil nemesis resurfaces after 16 years and she goes missing, the former radical scrambles to find her, father and daughter both battling the consequences of his past.', 'https://image.tmdb.org/t/p/w500/m1jFoahEbeQXtx4zArT2FKdbNIj.jpg',
  'en', 'movie', 'Some search for battle, others are born into it...', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt6604188', 533533, 'TRON: Ares', 2025,
  NULL, 119, 'A highly sophisticated Program called Ares is sent from the digital world into the real world on a dangerous mission, marking humankind''s first encounter with A.I. beings.', 'https://image.tmdb.org/t/p/w500/chpWmskl3aKm1aTZqUHRCtviwPy.jpg',
  'en', 'movie', 'No going back.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt33019528', 1445698, 'The Pearl Comb', 2025,
  NULL, 21, 'A doctor, hell-bent on proving a woman’s place is in the home and not practising medicine, is sent to investigate her miraculous claim - only to discover the source of her unearthly power...', 'https://image.tmdb.org/t/p/w500/737hppvYVz8Wx9I2McyQPHCD7PB.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0137523', 550, 'Fight Club', 1999,
  NULL, 139, 'A ticking-time-bomb insomniac and a slippery soap salesman channel primal male aggression into a shocking new form of therapy. Their concept catches on, with underground "fight clubs" forming in every town, until an eccentric gets in the way and ignites an out-of-control spiral toward oblivion.', 'https://image.tmdb.org/t/p/w500/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg',
  'en', 'movie', 'Mischief. Mayhem. Soap.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt35851026', 1137179, 'Yadang: The Snitch', 2025,
  NULL, 123, 'Navigating both the criminal underworld and law enforcement agencies, professional snitches called "yadang" provide covert information about the drug world to prosecutors and police. When a drug bust at a party attended by high-profile second-generation VIPs entangles those involved into a dangerous conspiracy, a seasoned yadang must do everything in his power not just to make it out on top, but alive.', 'https://image.tmdb.org/t/p/w500/y7dsDbG8zVWkrkyOhA6ckZXX1uC.jpg',
  'ko', 'movie', 'Blowing the nation''s  drug scene wide open.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32395062', 1439112, 'Muzzle: City of Wolves', 2025,
  NULL, 93, 'LAPD officer Jake Rosser endeavors to lead a peaceful life with his family and retired K-9 officer, Socks. However, tranquility dissolves into chaos when a gang targets them in a brutal attack. Alongside his new K-9 partner Argos, Jake launches into a relentless pursuit of justice, determined to protect his loved ones.', 'https://image.tmdb.org/t/p/w500/8qTMRmC07XCGidnKQFLbRM3FoDU.jpg',
  'en', 'movie', 'Revenge has a new breed.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14940390', 1327881, 'Voice of Shadows', 2024,
  NULL, 90, 'A young working class woman stands to inherit an estate if she and her boyfriend abide by a set of bizarre stipulations.', 'https://image.tmdb.org/t/p/w500/hffZQgrulC6jgneLuTANQrYgiui.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt38121182', 1539104, 'JUJUTSU KAISEN: Execution', 2025,
  NULL, 88, 'A veil abruptly descends over the busy Shibuya area amid the bustling Halloween crowds, trapping countless civilians inside. Satoru Gojo, the strongest jujutsu sorcerer, steps into the chaos. But lying in wait are curse users and spirits scheming to seal him away. Yuji Itadori, accompanied by his classmates and other top-tier jujutsu sorcerers, enters the fray in an unprecedented clash of curses — the Shibuya Incident. In the aftermath, ten colonies across Japan are transformed into dens of curses in a plan orchestrated by Noritoshi Kamo. As the deadly Culling Game starts, Special Grade sorcerer Yuta Okkotsu is assigned to carry out Yuji''s execution for his perceived crimes. A compilation movie of Shibuya Incident including the first two episodes of the Culling Games arc.', 'https://image.tmdb.org/t/p/w500/v0s3dx6am0RzfsuK3KdEy8ZoCDs.jpg',
  'ja', 'movie', 'Shibuya Incident × The Culling Game Begins', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt34509472', 1247002, 'The Old Woman with the Knife', 2025,
  NULL, 123, 'Aging assassin Hornclaw has seen it all, but she never expected to mentor a reckless rookie like Bullfight. As their unlikely bond deepens, cracks form in the underworld they navigate together. When Hornclaw discovers someone wants her dead, she''s thrust into a deadly game of deception. With enemies closing in and trust in short supply, survival means staying sharp—both blade and mind.', 'https://image.tmdb.org/t/p/w500/wbmxnsv41vsg5UEaNDlf203dOWw.jpg',
  'ko', 'movie', 'Now I see who you are.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29544253', 1205229, 'Night of the Zoopocalypse', 2025,
  NULL, 92, 'A wolf and mountain lion team up when a meteor unleashes a virus turning zoo animals into zombies. They join forces with other survivors to rescue the zoo and stop the deranged mutant leader from spreading the virus.', 'https://image.tmdb.org/t/p/w500/nCejOVZcOxKS27nnbh28NKXOdXF.jpg',
  'en', 'movie', 'Don''t feed the animals.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt1630029', 76600, 'Avatar: The Way of Water', 2022,
  NULL, 192, 'Set more than a decade after the events of the first film, learn the story of the Sully family (Jake, Neytiri, and their kids), the trouble that follows them, the lengths they go to keep each other safe, the battles they fight to stay alive, and the tragedies they endure.', 'https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg',
  'en', 'movie', 'Return to Pandora.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29538620', 1491902, 'Bang', 2025,
  NULL, NULL, 'Follows a ruthless hitman who, after narrowly escaping a fatal attempt on his life, betrays his gang in an effort to find forgiveness but ends up becoming a target himself.', 'https://image.tmdb.org/t/p/w500/8SUzKOqe2ectvhYdSnR7Vq2F3n1.jpg',
  'th', 'movie', 'Once a killer, now the mark.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0295701', 7451, 'xXx', 2002,
  NULL, 124, 'Xander Cage is your standard adrenaline junkie with no fear and a lousy attitude. When the US Government "recruits" him to go on a mission, he''s not exactly thrilled. His mission: to gather information on an organization that may just be planning the destruction of the world, led by the nihilistic Yorgi.', 'https://image.tmdb.org/t/p/w500/xeEw3eLeSFmJgXZzmF2Efww0q3s.jpg',
  'en', 'movie', 'A new breed of secret agent.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14142060', 1208348, 'Rental Family', 2025,
  NULL, 110, 'An American actor in Tokyo struggles to find purpose until he lands an unusual gig: working for a Japanese ''rental family'' agency, playing stand-in roles for strangers. As he immerses himself in his clients'' worlds, he begins to form genuine bonds that blur the lines between performance and reality.', 'https://image.tmdb.org/t/p/w500/h3zd2DFwrQWrE3zc7fTH0LrCFjs.jpg',
  'en', 'movie', 'Happiness tailored to you!', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31844586', 1223601, 'Sisu: Road to Revenge', 2025,
  NULL, 89, 'Returning to the house where his family was brutally murdered during the war, "the man who refuses to die" dismantles it, loads it on a truck, and is determined to rebuild it somewhere safe in their honor. When the commander who killed his family comes back hellbent on finishing the job, a relentless, eye-popping cross-country chase ensues.', 'https://image.tmdb.org/t/p/w500/jNsttCWZyPtW66MjhUozBzVsRb7.jpg',
  'fi', 'movie', 'When they took his family, he took revenge.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt11344566', 772462, 'Don''t Follow Me', 2025,
  NULL, 83, 'Obsessed with going viral with her videos of ghosts and paranormal events, aspiring influencer Carla moves into a famous haunted building. There, she begins faking ghostly apparitions to gain more followers, unaware that a real evil entity resides in her apartment and will torment her until no one, not even she, can distinguish what''s real and what isn''t.', 'https://image.tmdb.org/t/p/w500/wBYicLV0zfdtXweWXcoiou3moNu.jpg',
  'es', 'movie', 'Follow. Like. Die.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0499549', 19995, 'Avatar', 2009,
  NULL, 162, 'In the 22nd century, a paraplegic Marine is dispatched to the moon Pandora on a unique mission, but becomes torn between following orders and protecting an alien civilization.', 'https://image.tmdb.org/t/p/w500/gKY6q7SjCkAU6FqvqWybDYgUKIF.jpg',
  'en', 'movie', 'Enter the world of Pandora.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14850054', 840464, 'Greenland 2: Migration', 2026,
  NULL, 98, 'Having found the safety of the Greenland bunker after the comet Clarke decimated the Earth, the Garrity family must now risk everything to embark on a perilous journey across the wasteland of Europe to find a new home.', 'https://image.tmdb.org/t/p/w500/z2tqCJLsw6uEJ8nJV8BsQXGa3dr.jpg',
  'en', 'movie', 'Hope is uncharted territory.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14264694', 1196067, 'Worldbreaker', 2025,
  NULL, 95, 'Five years ago, a tear in the fabric of reality brought creatures to our world from an alternate dimension bent on our destruction. A father hides his daughter on an island to keep her safe while he prepares her for survival and the battles to come. But when the world is about to break, no place is safe.', 'https://image.tmdb.org/t/p/w500/7K8w6mdrJp0oaSoKWGyjSZ4Zv2z.jpg',
  'en', 'movie', 'The future belongs to those who fight.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0848228', 24428, 'The Avengers', 2012,
  NULL, 143, 'When an unexpected enemy emerges and threatens global safety and security, Nick Fury, director of the international peacekeeping agency known as S.H.I.E.L.D., finds himself in need of a team to pull the world back from the brink of disaster. Spanning the globe, a daring recruitment effort begins!', 'https://image.tmdb.org/t/p/w500/RYMX2wcKCBAr24UyPD7xwmjaTn.jpg',
  'en', 'movie', 'Some assembly required.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0199007', 252969, 'Secret Pleasures', 2002,
  NULL, 90, 'An attractive woman detective, expert in cases of conjugal infidelity is hired by a mysterious millionaire, unaware that she will get involved in a feverish love triangle to gather all the evidence she needs to finish the case.', 'https://image.tmdb.org/t/p/w500/j0Qu5opplDTot70kMqVqzURFXHY.jpg',
  'en', 'movie', 'Sometimes its better if the truth remains hidden.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt33244668', 1234731, 'Anaconda', 2025,
  NULL, 100, 'A group of friends facing mid-life crises head to the rainforest with the intention of remaking their favorite movie from their youth, only to find themselves in a fight for their lives against natural disasters, giant snakes and violent criminals.', 'https://image.tmdb.org/t/p/w500/5MDnvvkOqthhE5gQebzkcOhD1p5.jpg',
  'en', 'movie', 'A comedy so big it''ll leave you breathless.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14205554', 803796, 'KPop Demon Hunters', 2025,
  NULL, 96, 'When K-pop superstars Rumi, Mira and Zoey aren''t selling out stadiums, they''re using their secret powers to protect their fans from supernatural threats.', 'https://image.tmdb.org/t/p/w500/zT7Lhw3BhJbMkRqm9Zlx2YGMsY0.jpg',
  'en', 'movie', 'They sing. They dance. They battle demons.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32263401', 1013446, 'Reflection in a Dead Diamond', 2025,
  NULL, 87, 'When the mysterious woman in the room next door disappears, a debonair 70-year-old ex-spy living in a luxury hotel on the Côte d''Azur is confronted by the demons and darlings of a lurid past in which moviemaking, memories and madness collide.', 'https://image.tmdb.org/t/p/w500/5CdRxBu7jRDHjthkAL8vE0GdKUD.jpg',
  'fr', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14107334', 798645, 'The Running Man', 2025,
  NULL, 133, 'Desperate to save his sick daughter, working-class Ben Richards is convinced by The Running Man''s charming but ruthless producer to enter the deadly competition game as a last resort. But Ben''s defiance, instincts, and grit turn him into an unexpected fan favorite — and a threat to the entire system. As ratings skyrocket, so does the danger, and Ben must outwit not just the Hunters, but a nation addicted to watching him fall.', 'https://image.tmdb.org/t/p/w500/dKL78O9zxczVgjtNcQ9UkbYLzqX.jpg',
  'en', 'movie', 'Hunt him down.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt1527793', 639988, 'No Other Choice', 2025,
  NULL, 139, 'Looking for a job? When a happy family man is dismissed after twenty-five years of loyal service at a paper company, he finds the perfect solution to land his next role: truly eliminate the competition.', 'https://image.tmdb.org/t/p/w500/vc2S0dvgpsM0XfSiXZDMVkRCSSU.jpg',
  'ko', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30219247', 1336189, 'Delivery Run', 2025,
  NULL, 84, 'A food delivery driver gets caught in a deadly chase in the icy Minnesota wilderness, pursued by a crazed snowplow driver for unknown reasons, facing life-threatening situations and forced to outsmart his relentless pursuer alone.', 'https://image.tmdb.org/t/p/w500/eCA5maHDlZtXNTtiLXenwnEw7tc.jpg',
  'en', 'movie', 'Outsmart or be overtaken', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32916440', 1317288, 'Marty Supreme', 2025,
  NULL, 150, 'In 1950s New York, table tennis player Marty Mauser, a young man with a dream no one respects, goes to Hell and back in pursuit of greatness.', 'https://image.tmdb.org/t/p/w500/firAhZA0uQvRL2slp7v3AnOj0ZX.jpg',
  'en', 'movie', 'Dream big.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt2806834', 1433667, 'Merkin Penal', 2014,
  NULL, 24, 'Gary is falsely accused of beating his insane ex-girlfriend Charlotte, and he is sentenced to 30 days in Merkin Penal, a privately run prison. Once behind bars he runs into his old friend Smitty, who''s in the 2nd year of his 9 month stay, due to a run in with the TSA. Smitty is "insane Charlotte''s" older brother. Smitty and Gary met while Gary and Charlotte were an item. They are forever bonded together by their shared celebration of hatred for Charlotte. After a First Class, welcoming tour of the prison via Smitty, and a run-in with Emmett O''Donald, from upper Prison Management, Gary realizes his original sentence of 30 days was all Bulls@!t. He may never get out.', 'https://image.tmdb.org/t/p/w500/3bP7AyTuoT5mvBLeu8bLG1yFVN0.jpg',
  'en', 'movie', 'It''s like HBO''s "OZ" ...but even funnier.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt10210064', 1033462, 'Bureau 749', 2024,
  NULL, 123, 'A traumatized young man with physical abnormalities is forced to join a mysterious bureau to confront a disaster spreading across the earth caused by an unknown creature. He embarks on an adventure uncovering mysteries about his life.', 'https://image.tmdb.org/t/p/w500/flykCMw22y6yv8vKnBjmsW3pneo.jpg',
  'zh', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt6300910', 1195518, 'The Unholy Trinity', 2025,
  NULL, 95, 'Set against the turbulent backdrop of 1870s Montana, in the moments before the execution of Isaac Broadway, he gives his estranged son, Henry, an impossible task: murder the man who framed him for a crime he didn’t commit. Intent on fulfilling his promise, Henry travels to the remote town of Trinity, where an unexpected turn of events traps him in town and leaves him caught between Gabriel Dove, the town’s upstanding new sheriff, and a mysterious figure named St. Christopher.', 'https://image.tmdb.org/t/p/w500/waU3o5qRPNA9bIC59DIsDppll11.jpg',
  'en', 'movie', 'The past never stays buried.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt19847976', 967941, 'Wicked: For Good', 2025,
  NULL, 137, 'As an angry mob rises against the Wicked Witch, Glinda and Elphaba will need to come together one final time. With their singular friendship now the fulcrum of their futures, they will need to truly see each other, with honesty and empathy, if they are to change themselves, and all of Oz, for good.', 'https://image.tmdb.org/t/p/w500/si9tolnefLSUKaqQEGz1bWArOaL.jpg',
  'en', 'movie', 'You will be changed.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt33028778', 1315303, 'Primate', 2026,
  NULL, 89, 'Lucy, a college student, along with her friends, spend their vacation at her family''s home in Hawaii, which includes her pet chimpanzee, Ben. However, when Ben contracts rabies after being bitten by a rabid animal, the group must fight for their lives in order to avoid the now-violent chimp.', 'https://image.tmdb.org/t/p/w500/z97hrgI5wbGbZvSVkBfAeBnFKAg.jpg',
  'en', 'movie', 'Something''s wrong with Ben.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt1312221', 1062722, 'Frankenstein', 2025,
  NULL, 150, 'Dr. Victor Frankenstein, a brilliant but egotistical scientist, brings a creature to life in a monstrous experiment that ultimately leads to the undoing of both the creator and his tragic creation.', 'https://image.tmdb.org/t/p/w500/g4JtvGlQO7DByTI6frUobqvSL3R.jpg',
  'en', 'movie', 'Only monsters play God.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30472557', 1218925, 'Chainsaw Man - The Movie: Reze Arc', 2025,
  NULL, 100, 'In a brutal war between devils, hunters, and secret enemies, a mysterious girl named Reze has stepped into Denji''s world, and he faces his deadliest battle yet, fueled by love in a world where survival knows no rules.', 'https://image.tmdb.org/t/p/w500/pHyxb2RV5wLlboAwm9ZJ9qTVEDw.jpg',
  'ja', 'movie', 'EVERYBODY’S AFTER MY CHAINSAW HEART, WHAT ABOUT DENJI’S HEART?', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt35222590', 1408208, 'Exit 8', 2025,
  NULL, 95, 'A man gets lost in an underground passage. He follows the "guide" through the passage, but one after another, strange things happen to him. Is this space real? Or an illusion? Will the man be able to escape the passage?', 'https://image.tmdb.org/t/p/w500/nQXYTvm6AY4WmtcPskroqC7Skh.jpg',
  'ja', 'movie', 'Try your best to get out.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32537226', 1387382, 'Hunting Season', 2025,
  NULL, 93, 'When a reclusive survivalist and his daughter rescue a mysterious, wounded woman from a river, they become entangled in a deadly web of violence and revenge, forcing them to confront a brutal criminal to survive.', 'https://image.tmdb.org/t/p/w500/cbryTyaWdqrKpQCw6K7zm2jrB5v.jpg',
  'en', 'movie', 'It takes an awful lot to kill a person.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt9603208', 575265, 'Mission: Impossible - The Final Reckoning', 2025,
  NULL, 170, 'Ethan Hunt and team continue their search for the terrifying AI known as the Entity — which has infiltrated intelligence networks all over the globe — with the world''s governments and a mysterious ghost from Hunt''s past on their trail. Joined by new allies and armed with the means to shut the Entity down for good, Hunt is in a race against time to prevent the world as we know it from changing forever.', 'https://image.tmdb.org/t/p/w500/z53D72EAOxGRqdr7KXXWp9dJiDe.jpg',
  'en', 'movie', 'Our lives are the sum of our choices.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt16311594', 911430, 'F1', 2025,
  NULL, 156, 'Racing legend Sonny Hayes is coaxed out of retirement to lead a struggling Formula 1 team—and mentor a young hotshot driver—while chasing one more chance at glory.', 'https://image.tmdb.org/t/p/w500/vqBmyAj0Xm9LnS1xe1MSlMAJyHq.jpg',
  'en', 'movie', 'Let''s ride.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0816692', 157336, 'Interstellar', 2014,
  NULL, 169, 'The adventures of a group of explorers who make use of a newly discovered wormhole to surpass the limitations on human space travel and conquer the vast distances involved in an interstellar voyage.', 'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg',
  'en', 'movie', 'Mankind was born on Earth. It was never meant to die here.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt34956443', 980477, 'Ne Zha 2', 2025,
  NULL, 144, 'After a catastrophic event leaves their bodies destroyed, Ne Zha and Ao Bing are granted a fragile second chance at life. As tensions rise between the dragon clans and celestial forces, the two must undergo a series of perilous trials that will test their bond, challenge their identities, and decide the fate of both mortals and immortals.', 'https://image.tmdb.org/t/p/w500/cb5NyNrqiCNNoDkA8FfxHAtypdG.jpg',
  'zh', 'movie', 'Witness a hero reborn.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14364480', 812583, 'Wake Up Dead Man: A Knives Out Mystery', 2025,
  NULL, 145, 'When young priest Jud Duplenticy is sent to assist charismatic firebrand Monsignor Jefferson Wicks, it’s clear that all is not well in the pews. After a sudden and seemingly impossible murder rocks the town, the lack of an obvious suspect prompts local police chief Geraldine Scott to join forces with renowned detective Benoit Blanc to unravel a mystery that defies all logic.', 'https://image.tmdb.org/t/p/w500/qCOGGi8JBVEZMc3DVby8rUivyXz.jpg',
  'en', 'movie', 'He works in mysterious ways.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt26439764', 1083637, 'Kantara - A Legend: Chapter 1', 2025,
  NULL, 165, 'During the Kadamba reign, King Vijayendra, the ruler of the fictional feudatory land of Bangra, meets his final fate while venturing into the mystical forest of Kantara. Witnessing this, his son Rajashekara seals the borders of their realm. Later, Prince Kulashekara reopens them through a brutal massacre. The protagonist, Berme, in search of prosperity, crosses the divide and ignites a conflict of faith, power, and destiny between the Kingdom and Nature.', 'https://image.tmdb.org/t/p/w500/ehQPboTPaIMkMUOoNOh8e7pZ5Rp.jpg',
  'kn', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt2948356', 269149, 'Zootopia', 2016,
  NULL, 109, 'Determined to prove herself, Officer Judy Hopps, the first bunny on Zootopia''s police force, jumps at the chance to crack her first case - even if it means partnering with scam-artist fox Nick Wilde to solve the mystery.', 'https://image.tmdb.org/t/p/w500/hlK0e0wAQ3VLuJcsfIYPvb4JVud.jpg',
  'en', 'movie', 'Welcome to the urban jungle.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31036941', 1234821, 'Jurassic World Rebirth', 2025,
  NULL, 134, 'Five years after the events of Jurassic World Dominion, covert operations expert Zora Bennett is contracted to lead a skilled team on a top-secret mission to secure genetic material from the world''s three most massive dinosaurs. When Zora''s operation intersects with a civilian family whose boating expedition was capsized, they all find themselves stranded on an island where they come face-to-face with a sinister, shocking discovery that''s been hidden from the world for decades.', 'https://image.tmdb.org/t/p/w500/1RICxzeoNCAO5NpcRMIgg1XT6fm.jpg',
  'en', 'movie', 'A new era is born.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt10676052', 617126, 'The Fantastic 4: First Steps', 2025,
  NULL, 115, 'Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world, Marvel''s First Family is forced to balance their roles as heroes with the strength of their family bond, while defending Earth from a ravenous space god called Galactus and his enigmatic Herald, Silver Surfer.', 'https://image.tmdb.org/t/p/w500/abqOz6EL3yXyOOafCPZxjL1M5bQ.jpg',
  'en', 'movie', 'Welcome to the family.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0178627', 275039, 'Around the World in 80 Beds', 1977,
  NULL, 79, 'This German “mondo” movie has a TV host narrating and introducing various sexual acts from around the world. This includes S&M acts, a class teaching masturbation, a man who likes to pretend to be a baby and getting spanked by his mother and various other naughty tales.', NULL,
  'de', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt27196021', 1102493, 'Small Things Like These', 2024,
  NULL, 99, 'In 1985, while working as a coal merchant to support his family, Bill Furlong discovers disturbing secrets kept by the local convent and uncovers truths of his own; forcing him to confront his past and the complicit silence of a small Irish town controlled by the Catholic Church.', 'https://image.tmdb.org/t/p/w500/rdcO38cbWFg002nXg5QYtk7Tz4L.jpg',
  'en', 'movie', 'If you want to get on in life, there’s things you have to ignore so you can keep on.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt23790696', 1022453, 'The Rajasaab', 2026,
  NULL, 186, 'A young heir embraces both his royal heritage and rebellious spirit as he rises to power, establishing unprecedented rules during his reign as Raja Saab.', 'https://image.tmdb.org/t/p/w500/nvK6gYa4diCnQkDVN42uoYXPrdT.jpg',
  'te', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt39307872', 1610413, 'One Last Adventure: The Making of Stranger Things 5', 2026,
  NULL, 123, 'An inside look at the years of effort and craft that went into the final installment of the Duffer Brothers'' generation-defining series.', 'https://image.tmdb.org/t/p/w500/aZhrAx5CevLfjXcGIG3wBXxDiAL.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt20254992', 1090983, 'The Beldham', 2025,
  NULL, 85, 'At the edge of postpartum psychosis, new mother Harper moves in with her own mother at their family''s rural farmhouse. Soon she finds herself hunted by a threatening monster who has designs on her child.', 'https://image.tmdb.org/t/p/w500/suY2CNfGwq3kccMZuMA1sXAcNXm.jpg',
  'en', 'movie', 'Beware the witch.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt30343021', 1371185, 'Song Sung Blue', 2025,
  NULL, 133, 'Based on a true story, two down-on-their-luck musicians form a joyous Neil Diamond tribute band, proving it''s never too late to find love and follow your dreams.', 'https://image.tmdb.org/t/p/w500/dXVGVz3sGLVnJf7tiJ9zDHqiqFX.jpg',
  'en', 'movie', 'Inspired by a legend. Bound by a dream.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32141377', 1272837, '28 Years Later: The Bone Temple', 2026,
  NULL, 109, 'Dr. Kelson finds himself in a shocking new relationship - with consequences that could change the world as they know it - and Spike''s encounter with Jimmy Crystal becomes a nightmare he can''t escape.', 'https://image.tmdb.org/t/p/w500/kK1BGkG3KAvWB0WMV1DfOx9yTMZ.jpg',
  'en', 'movie', 'Fear is the new faith.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt14905854', 858024, 'Hamnet', 2025,
  NULL, 126, 'The powerful story of love and loss that inspired the creation of Shakespeare''s timeless masterpiece, Hamlet.', 'https://image.tmdb.org/t/p/w500/71KNp6RiOQTbs8Fn1DagrFQrZmx.jpg',
  'en', 'movie', 'Keep your heart open.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32241578', 1181765, 'School of Magical Animals 3', 2024,
  NULL, 100, 'Ida wants to perform with her class at the annual Forest Day to campaign for the protection of the local forest. Even Helene is there, as she hopes to use the footage of the performance to build up her influencer channel. What nobody knows is that Helene''s family is on the verge of bankruptcy and Helene urgently needs followers to avert the threat of bankruptcy. Helene is also under pressure from the high expectations of her magical animal, Karajan the cat from Paris, who imagines a life of pure luxury.', 'https://image.tmdb.org/t/p/w500/yuwPuKYdXUch49sEb757bNs4t0b.jpg',
  'de', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29232158', 1180831, 'Troll 2', 2025,
  NULL, 105, 'When a dangerous new troll unleashes devastation across their homeland, Nora, Andreas and Major Kris embark on their most perilous mission yet.', 'https://image.tmdb.org/t/p/w500/p6xAExLNFbHcLfvSuvLPoM8aqZU.jpg',
  'no', 'movie', 'A new troll has awakened!', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31434030', 1246049, 'Dracula', 2025,
  NULL, 130, 'When a 15th-century prince denounces God after the devastating loss of his wife, he inherits an eternal curse: he becomes Dracula. Condemned to wander the centuries, he defies fate and death itself, guided by a single hope — to be reunited with his lost love.', 'https://image.tmdb.org/t/p/w500/ykyRfv7JDofLxXLAwtLXaSuaFfM.jpg',
  'fr', 'movie', 'He renounced his faith to become immortal. Passion, anger, vengeance, and hatred will be unleashed into the modern world.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt31193180', 1233413, 'Sinners', 2025,
  NULL, 138, 'Trying to leave their troubled lives behind, twin brothers return to their hometown to start again, only to discover that an even greater evil is waiting to welcome them back.', 'https://image.tmdb.org/t/p/w500/qTvFWCGeGXgBRaINLY1zqgTPSpn.jpg',
  'en', 'movie', 'Dance with the devil.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt13176330', 756397, 'Hitpig!', 2024,
  NULL, 85, 'A porcine bounty hunter accepts his next hit: Pickles, a naive, ebullient elephant. Though he initially sets out to capture the perky pachyderm, the unlikely pair find themselves crisscrossing the globe on an adventure that brings out the best in both of them.', 'https://image.tmdb.org/t/p/w500/bYa20LYAZ3Q7lKXtLYybo0yWWTN.jpg',
  'en', 'movie', 'When you’re on the lam, they call the ham.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt34276058', 1363123, 'The Family Plan 2', 2025,
  NULL, 106, 'Now that Dan''s assassin days are behind him, all he wants for Christmas is quality time with his kids. But when he learns his daughter has her own plans, he books a family trip to London—putting them all in the crosshairs of an unexpected enemy.', 'https://image.tmdb.org/t/p/w500/semFxuYx6HcrkZzslgAkBqfJvZk.jpg',
  'en', 'movie', 'Deck the halls, dodge the bad guys.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt36153493', 1448560, 'Wildcat', 2025,
  NULL, 99, 'An ex-black ops team reunite to pull off a desperate heist in order to save the life of their leader’s eight-year-old daughter.', 'https://image.tmdb.org/t/p/w500/h893ImjM6Fsv5DFhKJdlZFZIJno.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt21909764', 1010581, 'My Fault', 2023,
  NULL, 117, 'Noah must leave her city, boyfriend, and friends to move into William Leister''s mansion, the flashy and wealthy husband of her mother Rafaela. As a proud and independent 17 year old, Noah resists living in a mansion surrounded by luxury. However, it is there where she meets Nick, her new stepbrother, and the clash of their strong personalities becomes evident from the very beginning.', 'https://image.tmdb.org/t/p/w500/w46Vw536HwNnEzOa7J24YH9DPRS.jpg',
  'es', 'movie', 'Love can only survive in the shadow of secrets.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt26743210', 1087192, 'How to Train Your Dragon', 2025,
  NULL, 125, 'On the rugged isle of Berk, where Vikings and dragons have been bitter enemies for generations, Hiccup stands apart, defying centuries of tradition when he befriends Toothless, a feared Night Fury dragon. Their unlikely bond reveals the true nature of dragons, challenging the very foundations of Viking society.', 'https://image.tmdb.org/t/p/w500/q5pXRYTycaeW6dEgsCrd4mYPmxM.jpg',
  'en', 'movie', 'The legend is real.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32831759', 1403735, 'Laila', 2025,
  NULL, 134, 'Sonu Model, a renowned beautician from the old city, is forced to disguise himself as Laila, leading to a series of comedic, romantic, and action-packed events. Chaos ensues in this hilarious laugh riot', 'https://image.tmdb.org/t/p/w500/aFogllaRGlAhk1nqvVGFpZpl4qU.jpg',
  'te', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32164798', 1266990, 'Kulong', 2024,
  NULL, 48, 'Three friends retreat to a resort to collaborate on a daring screenplay for a contest. Faced with the challenge of writing about passion despite their own dry spells, their journey soon blurs the line between imagination and experience.', 'https://image.tmdb.org/t/p/w500/AnzrE2WHg3DtZrtmB9AnEbAh17m.jpg',
  'tl', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt27589902', 1116465, 'A Legend', 2024,
  NULL, 129, 'An archeologist noticed that the texture of the relics discovered during the excavation of a glacier closely resembled a jade pendant seen in one of his dreams. He and his team then embark on an expedition into the depths of the glacier.', 'https://image.tmdb.org/t/p/w500/qbImUt1d3itXcB81BCItPZlfbyr.jpg',
  'zh', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt12300742', 701387, 'Bugonia', 2025,
  NULL, 119, 'Two conspiracy obsessed young men kidnap the high-powered CEO of a major company, convinced that she is an alien intent on destroying planet Earth.', 'https://image.tmdb.org/t/p/w500/oxgsAQDAAxA92mFGYCZllgWkH9J.jpg',
  'en', 'movie', 'Of all the abductions, this one is different.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt23572848', 991494, 'The SpongeBob Movie: Search for SquarePants', 2025,
  NULL, 88, 'Desperate to be a big guy, SpongeBob sets out to prove his bravery to Mr. Krabs by following The Flying Dutchman – a mysterious swashbuckling ghost pirate – on a seafaring adventure that takes him to the deepest depths of the deep sea, where no Sponge has gone before.', 'https://image.tmdb.org/t/p/w500/pDWYW9v8fmJdA7N0I1MOdQA3ETq.jpg',
  'en', 'movie', 'Ship''s about to go down.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt5950044', 1061474, 'Superman', 2025,
  NULL, 130, 'Superman, a journalist in Metropolis, embarks on a journey to reconcile his Kryptonian heritage with his human upbringing as Clark Kent.', 'https://image.tmdb.org/t/p/w500/ldyfo0BKmz5rWtJJKCvwaNS4cJT.jpg',
  'en', 'movie', 'Look up.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt33014583', 1291608, 'Dhurandhar', 2025,
  NULL, 212, 'After the hijacking of IC-814 in 1999 and the Parliament attack in 2001, India’s Intelligence Bureau Chief, Ajay Sanyal, devises an indomitable mission to intrude and rupture the terrorist network in Pakistan, by infiltrating the underworld mafia of Karachi with the help of a 20-year-old boy from Punjab, held captive for a revenge crime.', 'https://image.tmdb.org/t/p/w500/8FHOtUpNIk5ZPEay2N2EY5lrxkv.jpg',
  'hi', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0111161', 278, 'The Shawshank Redemption', 1994,
  NULL, 142, 'Imprisoned in the 1940s for the double murder of his wife and her lover, upstanding banker Andy Dufresne begins a new life at the Shawshank prison, where he puts his accounting skills to work for an amoral warden. During his long stretch in prison, Dufresne comes to be admired by the other inmates -- including an older prisoner named Red -- for his integrity and unquenchable sense of hope.', 'https://image.tmdb.org/t/p/w500/9cqNxx0GxF0bflZmeSMuL5tnGzr.jpg',
  'en', 'movie', 'Fear can hold you prisoner. Hope can set you free.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt36585628', 1495694, 'Beauty from Pain', 2025,
  NULL, 116, 'After a devastating heartbreak, an aspiring musician escapes to Australia. She agrees to a no-strings romance with a billionaire winemaker, but when she falls in love she must choose to walk away or risk her heart on a man afraid to love.', 'https://image.tmdb.org/t/p/w500/mJtBKvHQaFzgp0N6JFskSvjbTar.jpg',
  'en', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt27829510', 1149167, 'High Rollers', 2025,
  NULL, 101, 'A master thief must pull off a dangerous casino heist when his nemesis kidnaps his lover. Caught between rival criminals and FBI pursuit, he risks all to save her and score big.', 'https://image.tmdb.org/t/p/w500/hHowAaChDjwueySmwVbsjHmpWa.jpg',
  'en', 'movie', 'Everything on the line. Nothing off the table.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29845247', 1194963, 'Noise', 2025,
  NULL, 95, 'Ju-young is a young woman with a hearing impediment who decides to investigate her sister''s inexplicable disappearance, last seen in her apartment. Feeling more and more cornered, Ju-young begins to hear strange sounds and sense an evil presence in the apartment.', 'https://image.tmdb.org/t/p/w500/uU3NXe7QqjJNxVpYPdz8dscFWlj.jpg',
  'ko', 'movie', 'Could you be quiet before I rip your mouth out.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0068646', 238, 'The Godfather', 1972,
  NULL, 175, 'Spanning the years 1945 to 1955, a chronicle of the fictional Italian-American Corleone crime family. When organized crime family patriarch, Vito Corleone barely survives an attempt on his life, his youngest son, Michael steps in to take care of the would-be killers, launching a campaign of bloody revenge.', 'https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg',
  'en', 'movie', 'An offer you can''t refuse.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0120338', 597, 'Titanic', 1997,
  NULL, 194, '101-year-old Rose DeWitt Bukater tells the story of her life aboard the Titanic, 84 years later. A young Rose boards the ship with her mother and fiancé. Meanwhile, Jack Dawson and Fabrizio De Rossi win third-class tickets aboard the ship. Rose tells the whole story from Titanic''s departure through to its death—on its first and last voyage—on April 15, 1912.', 'https://image.tmdb.org/t/p/w500/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg',
  'en', 'movie', 'Nothing on earth could come between them.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29567915', 1214931, 'Nuremberg', 2025,
  NULL, 148, 'In postwar Germany, an American psychiatrist must determine whether Nazi prisoners are fit to go on trial for war crimes, and finds himself in a complex battle of intellect and ethics with Hermann Göring, Hitler''s right-hand man.', 'https://image.tmdb.org/t/p/w500/sxu8TsG7k06ymWL98ELEIE1EMZV.jpg',
  'en', 'movie', 'Judgment is coming.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt32123395', 1309012, 'Altered', 2025,
  NULL, 85, 'In an alternate present, genetically enhanced humans dominate society. Outcasts Leon and Chloe fight for justice against corrupt politicians exploiting genetic disparity, risking everything to challenge the oppressive system.', 'https://image.tmdb.org/t/p/w500/6QlAcGRaUrgHcZ4WTBh5lsPnzKx.jpg',
  'en', 'movie', 'Fight the system. Change the world.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt27208407', 1103437, 'Please Don''t Feed the Children', 2025,
  NULL, 94, 'After a viral outbreak ravaged the country’s adult population, a group of orphans heads south in search of a new life, only to find themselves at the mercy of a deranged woman harboring a dangerous secret.', 'https://image.tmdb.org/t/p/w500/GoIIdrjmXo587L2s2mqgMFvBIs.jpg',
  'en', 'movie', 'Don''t run. Don''t scream. Don''t break the rules', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt29954526', 1206083, 'Conjuring Tapes', 2025,
  NULL, 79, 'While sorting through their late friend''s belongings, two women discover VHS tapes showing them in haunting, unfamiliar scenes, each one drawing them into the grasp of a mysterious, malevolent entity.', 'https://image.tmdb.org/t/p/w500/puPN6uC5NpbAQW8dDovGkJ79arn.jpg',
  'en', 'movie', 'Their Footage. Your Nightmare.', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt37961727', 1589891, 'A Time for Bravery', 2025,
  NULL, 107, 'A psychoanalyst on community service aids an agent shattered by infidelity; together, they will confront danger and discover second chances.', 'https://image.tmdb.org/t/p/w500/qKKG22O5pFxqglG0oMjWTjQWCNl.jpg',
  'es', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

INSERT INTO movies (
  imdb_id, tmdb_id, title, release_year,
  mpaa_rating, runtime_minutes, plot_summary, poster_url,
  original_language, media_type, tagline, status
) VALUES (
  'tt0093949', 62201, 'Shahenshah', 1988,
  NULL, 180, 'Inspector Srivastav is framed by a wily and cunning gangster, J.K., and unable to prove his innocence, hangs himself, leaving behind his wife, and son, Vijay. Years later, Vijay has grown up and has joined the police force as an Inspector. Unlike his dad, he is corrupt and does accept bribes to turn a nelson''s eye to crime. The City Police are assigned the task of apprehending a customed man called "Shahenshah", who operates at night and targets, tries and kills gangsters in a "Judge and Executioner "style. No one knows the real identity of Shahenshah, as he is feared by the police department, and respected by the poor and middle-class.', 'https://image.tmdb.org/t/p/w500/2ZUpj73MzEh2YwVzJNVUqBSu2LZ.jpg',
  'hi', 'movie', '', 'Released'
)
ON CONFLICT (imdb_id) DO NOTHING;

COMMIT;
