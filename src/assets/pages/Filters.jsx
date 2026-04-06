import { useState, useEffect } from "react";

import { Link } from "react-router-dom";
 
export default function Filters() {

  const [genre, setGenre] = useState("");

  const [includeTag, setIncludeTag] = useState("");

  const [excludeTag, setExcludeTag] = useState("");

  const [cast, setCast] = useState("");

  const [crew, setCrew] = useState("");

  const [plotTag, setPlotTag] = useState("");

  const [mpaaRating, setMpaaRating] = useState("");

  const [movies, setMovies] = useState([]);

  const [loading, setLoading] = useState(false);
 
  useEffect(() => {

    const fetchMovies = async () => {

      setLoading(true);
 
      try {

        const normalizeTag = (tag) => {

          if (!tag) return [];

          const lower = tag.toLowerCase();

          if (lower === "violence") return ["gun violence", "violence"];

          if (lower === "gore") return ["blood/gore", "gore"];

          return [tag];

        };
 
        const body = {

          take: 20,

          minMatches: 1,                                    // ✅ needed

          genreNames: genre ? [genre] : [],

          includeWarningNames: normalizeTag(includeTag),

          excludeWarningNames: normalizeTag(excludeTag),

          personNames: [cast, crew].filter(Boolean),        // ✅ correct field for cast+crew

          keywordNames: plotTag ? [plotTag] : [],           // ✅ correct field for plot tags

          mpaaRatings: mpaaRating ? [mpaaRating] : [],      // ✅ correct field for rating

          personRoles: [],

          streamingProviderNames: []

        };
 
        const res = await fetch("https://localhost:5002/api/MovieSearch", {

          method: "POST",

          headers: { "Content-Type": "application/json" },

          body: JSON.stringify(body)

        });
 
        if (!res.ok) throw new Error("Server error");

        const data = await res.json();

        setMovies(data.results || []);
 
      } catch (err) {

        console.error("Error fetching movies:", err);

      } finally {

        setLoading(false);

      }

    };
 
    fetchMovies();

  }, [genre, includeTag, excludeTag, cast, crew, plotTag, mpaaRating]); // ✅ all filters as deps
 
  return (
<div className="min-h-screen text-white p-10">
 
      <h1 className="text-4xl font-bold text-center mb-10 text-pink-400">

        Advanced Movie Filters
</h1>
 
      <div className="bg-black/70 p-8 rounded-xl shadow-lg max-w-4xl mx-auto mb-12">
<div className="grid md:grid-cols-3 gap-6">
 
          {/* GENRE */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Genre</label>
<select

              className="w-full p-3 rounded-md text-black"

              value={genre}

              onChange={(e) => setGenre(e.target.value)}
>
<option value="">All Genres</option>
<option>Action</option>
<option>Adventure</option>
<option>Animation</option>
<option>Comedy</option>
<option>Crime</option>
<option>Documentary</option>
<option>Drama</option>
<option>Family</option>
<option>Fantasy</option>
<option>History</option>
<option>Horror</option>
<option>Music</option>
<option>Mystery</option>
<option>Romance</option>
<option>Science Fiction</option>
<option>Thriller</option>
<option>TV Movie</option>
<option>War</option>
<option>Western</option>
</select>
</div>
 
          {/* MPAA RATING */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Rating</label>
<select

              className="w-full p-3 rounded-md text-black"

              value={mpaaRating}

              onChange={(e) => setMpaaRating(e.target.value)}
>
<option value="">Any Rating</option>
<option value="G">G</option>
<option value="PG">PG</option>
<option value="PG-13">PG-13</option>
<option value="R">R</option>
<option value="NC-17">NC-17</option>
</select>
</div>
 
          {/* INCLUDE TRIGGER */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Include Trigger</label>
<input

              type="text"

              placeholder="violence, gore..."

              className="w-full p-3 rounded-md text-black"

              value={includeTag}

              onChange={(e) => setIncludeTag(e.target.value)}

            />
</div>
 
          {/* EXCLUDE TRIGGER */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Exclude Trigger</label>
<input

              type="text"

              placeholder="spiders, death..."

              className="w-full p-3 rounded-md text-black"

              value={excludeTag}

              onChange={(e) => setExcludeTag(e.target.value)}

            />
</div>
 
          {/* CAST */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Cast</label>
<input

              type="text"

              placeholder="Tom Hanks..."

              className="w-full p-3 rounded-md text-black"

              value={cast}

              onChange={(e) => setCast(e.target.value)}

            />
</div>
 
          {/* CREW */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Director / Crew</label>
<input

              type="text"

              placeholder="Christopher Nolan..."

              className="w-full p-3 rounded-md text-black"

              value={crew}

              onChange={(e) => setCrew(e.target.value)}

            />
</div>
 
          {/* PLOT TAG */}
<div>
<label className="block mb-2 text-pink-300 font-semibold">Plot Tag</label>
<input

              type="text"

              placeholder="time travel, revenge..."

              className="w-full p-3 rounded-md text-black"

              value={plotTag}

              onChange={(e) => setPlotTag(e.target.value)}

            />
</div>
 
        </div>
</div>
 
      <h2 className="text-2xl font-bold mb-6 text-center text-pink-300">

        Filter Results
</h2>
 
      {loading ? (
<p className="text-center text-gray-300">Loading...</p>

      ) : movies.length === 0 ? (
<p className="text-center text-gray-300">No movies match your filters.</p>

      ) : (
<div className="grid md:grid-cols-3 lg:grid-cols-4 gap-8">

          {movies.map((movie, index) => (

            movie.imdbId ? (
<Link key={movie.imdbId || index} to={`/movie/${movie.imdbId}`}>
<div className="bg-black/60 p-6 rounded-xl hover:bg-pink-500/20 transition hover:scale-105 shadow-lg cursor-pointer">

                  {movie.posterUrl ? (
<img src={movie.posterUrl} alt={movie.title} className="w-full h-64 object-cover rounded-md mb-4" />

                  ) : (
<div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">No Image</div>

                  )}
<h3 className="text-xl font-semibold mb-2">{movie.title}</h3>
<p className="text-sm text-gray-300">{movie.releaseYear || "N/A"}</p>
</div>
</Link>

            ) : (
<div key={index} className="bg-gray-700 p-6 rounded-xl opacity-60 cursor-not-allowed">

                {movie.posterUrl ? (
<img src={movie.posterUrl} alt={movie.title} className="w-full h-64 object-cover rounded-md mb-4" />

                ) : (
<div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">No Image</div>

                )}
<h3 className="text-xl font-semibold mb-2">{movie.title}</h3>
<p className="text-sm text-gray-300">{movie.releaseYear || "N/A"}</p>
<p className="text-xs text-red-400 mt-2">No details available</p>
</div>

            )

          ))}
</div>

      )}
</div>

  );

}
 