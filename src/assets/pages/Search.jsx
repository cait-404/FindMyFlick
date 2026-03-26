import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import API_URL from "../../config.js";

function Search() {
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");

  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!query) return;

    setLoading(true);
    setError(null);

    fetch(`${API_URL}/api/Movies/search?name=${query}`)
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch");
        return res.json();
      })
      .then((data) => setMovies(data.results || data))
      .catch((err) => {
        console.error("Error fetching movies:", err);
        setError("Failed to fetch movies. Try again later.");
      })
      .finally(() => setLoading(false));
  }, [query]);

  return (
    <div className="min-h-screen p-8 text-white bg-black">
      <h2 className="text-3xl font-bold mb-6">Results for: {query}</h2>

      {loading && <p>Loading movies...</p>}

      {error && <p className="text-red-400">{error}</p>}

      {!loading && movies.length === 0 && !error && (
        <p>No results found for "{query}".</p>
      )}

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {movies.map((movie) => (
          <div
            key={movie.imdbId || movie.tmdbId}
            className="bg-black/70 rounded-xl overflow-hidden shadow-lg p-4 transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0]"
          >
            <div className="h-64 bg-black mb-4">
              <img
                src={movie.posterUrl || "https://via.placeholder.com/300x450?text=No+Poster"}
                alt={movie.title}
                className="w-full h-full object-contain"
              />
            </div>

            <h3 className="font-bold text-lg truncate neon-text">{movie.title}</h3>
            <p className="text-sm opacity-70 mt-1">
              {movie.releaseYear || "N/A"}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Search;
