import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import API_URL from "../../config.js";

export default function Filters() {

  const [genre, setGenre] = useState("");
  const [includeTag, setIncludeTag] = useState("");
  const [excludeTag, setExcludeTag] = useState("");
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {

    const fetchMovies = async () => {
      setLoading(true);

      try {

        // 🔥 NORMALIZE INPUT (fixes your issue)
        const normalizeTag = (tag) => {
          if (!tag) return [];

          const lower = tag.toLowerCase();

          if (lower === "violence") {
            return ["gun violence", "violence"];
          }

          if (lower === "gore") {
            return ["blood/gore", "gore"];
          }

          return [tag];
        };

<<<<<<< HEAD
        const formatGenre = (g) => {
  if (!g) return [];
  return [g.charAt(0).toUpperCase() + g.slice(1).toLowerCase()];
};

const body = {
  genreNames: formatGenre(genre),
  includeWarningNames: normalizeTag(includeTag),
  excludeWarningNames: normalizeTag(excludeTag),
  take: 20
};

        const res = await fetch("https://localhost:5002/api/MovieSearch", {
=======
        const res = await fetch('${API_URL}/api/MovieSearch', {
>>>>>>> a9957c9bc61f89ee8f0651b70fc8121e7f015324
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(body)
        });

        const data = await res.json();

        setMovies(data.results || []);

      } catch (err) {
        console.error("Error fetching movies:", err);
      }

      setLoading(false);
    };

    fetchMovies();

  }, [genre, includeTag, excludeTag]);

  return (
    <div className="min-h-screen text-white p-10">

      {/* TITLE */}
      <h1 className="text-4xl font-bold text-center mb-10 text-pink-400">
        Advanced Movie Filters
      </h1>

      {/* FILTER PANEL */}
      <div className="bg-black/70 p-8 rounded-xl shadow-lg max-w-4xl mx-auto mb-12">

        <div className="grid md:grid-cols-3 gap-6">

          {/* GENRE */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Genre
            </label>

            <select
              className="w-full p-3 rounded-md text-black"
              value={genre}
              onChange={(e) => setGenre(e.target.value)}
            >
              <option value="">All Genres</option>
              <option>Horror</option>
              <option>Comedy</option>
              <option>Drama</option>
              <option>Action</option>
              <option>Thriller</option>
            </select>
          </div>

          {/* INCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Include Trigger
            </label>

            <input
              type="text"
              placeholder="violence, gore..."
              className="w-full p-3 rounded-md text-black"
              value={includeTag}
              onChange={(e) => setIncludeTag(e.target.value)}
            />
          </div>

          {/* EXCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Exclude Trigger
            </label>

            <input
              type="text"
              placeholder="spiders, death..."
              className="w-full p-3 rounded-md text-black"
              value={excludeTag}
              onChange={(e) => setExcludeTag(e.target.value)}
            />
          </div>

        </div>
      </div>

      {/* RESULTS */}
      <h2 className="text-2xl font-bold mb-6 text-center text-pink-300">
        Filter Results
      </h2>

      {loading ? (
        <p className="text-center text-gray-300">Loading...</p>
      ) : movies.length === 0 ? (
        <p className="text-center text-gray-300">
          No movies match your filters.
        </p>
      ) : (

        <div className="grid md:grid-cols-3 lg:grid-cols-4 gap-8">

          {movies.map((movie, index) => {

            const hasImdb = !!movie.imdbId;

            return hasImdb ? (

              <Link key={index} to={`/movie/${movie.imdbId}`}>

                <div className="bg-black/60 p-6 rounded-xl hover:bg-pink-500/20 transition hover:scale-105 shadow-lg cursor-pointer">

                  {movie.posterUrl ? (
                    <img
                      src={movie.posterUrl}
                      alt={movie.title}
                      className="w-full h-64 object-cover rounded-md mb-4"
                    />
                  ) : (
                    <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">
                      No Image
                    </div>
                  )}

                  <h3 className="text-xl font-semibold mb-2">
                    {movie.title}
                  </h3>

                  <p className="text-sm text-gray-300">
                    {movie.releaseYear || "N/A"}
                  </p>

                </div>

              </Link>

            ) : (

              <div
                key={index}
                className="bg-gray-700 p-6 rounded-xl opacity-60 cursor-not-allowed"
              >

                {movie.posterUrl ? (
                  <img
                    src={movie.posterUrl}
                    alt={movie.title}
                    className="w-full h-64 object-cover rounded-md mb-4"
                  />
                ) : (
                  <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">
                    No Image
                  </div>
                )}

                <h3 className="text-xl font-semibold mb-2">
                  {movie.title}
                </h3>

                <p className="text-sm text-gray-300">
                  {movie.releaseYear || "N/A"}
                </p>

                <p className="text-xs text-red-400 mt-2">
                  No details available
                </p>

              </div>

            );

          })}

        </div>

      )}

    </div>
  );
}