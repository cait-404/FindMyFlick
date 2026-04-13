import { useEffect, useState } from "react";
import { useLocation, Link } from "react-router-dom";
import API_URL from "../../config.js";

function Search() {
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");
  const navigate = useNavigate();

  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
  if (!query) return;
 
  setLoading(true);
  setError(null);
 
  // Step 1: Search by title first
  fetch(`${API_URL}/api/MovieSearch`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      take: 20,
      minMatches: 1,
      enableApiFallback: true,
      alwaysAddFromApis: false,
      titleContains: query,
      genreNames: [],
      keywordNames: [],
      personNames: [],
      personRoles: [],
      streamingProviderNames: []
    }),
  })
    .then((res) => {
      if (!res.ok) throw new Error("Failed to fetch movies");
      return res.json();
    })
    .then((data) => {
      const titleResults = data.results || data;
 
      // Step 2: If no title matches, fall back to keyword/genre search
      if (titleResults.length > 0) {
        setMovies(titleResults);
        setLoading(false);
        return;
      }
 
      // Fallback: search by keyword and genre
      return fetch(`${API_URL}/api/MovieSearch`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          take: 20,
          minMatches: 1,
          enableApiFallback: true,
          alwaysAddFromApis: true,
          titleContains: null,
          keywordNames: [query],   // ✅ fallback to keyword
          genreNames: [query],     // ✅ fallback to genre
          personNames: [],
          personRoles: [],
          streamingProviderNames: []
        }),
      })
        .then((res) => {
          if (!res.ok) throw new Error("Failed to fetch movies");
          return res.json();
        })
        .then((data) => setMovies(data.results || data));
    })
    .catch((err) => {
      console.error("Error fetching movies:", err);
      if (movies.length === 0) {
        setError("Failed to fetch movies. Try again later.");
      }
    })
    .finally(() => setLoading(false));
}, [query]);

  return (
    <div className="min-h-screen p-4 sm:p-6 md:p-8 text-white bg-gradient-to-b from-black via-[#12001a] to-black">
      <div className="max-w-6xl mx-auto mb-8">
        <h2 className="text-4xl font-extrabold neon-text">Results for: {query}</h2>
      </div>

      {loading && <p className="text-center mt-20 opacity-70">Loading movies...</p>}
      {error && <p className="text-center mt-20 text-red-400">{error}</p>}
      {!loading && movies.length === 0 && !error && (
        <p className="text-center mt-20 text-gray-400">No results found for "{query}".</p>
      )}

      <div className="max-w-6xl mx-auto grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4 sm:gap-6">
        {movies.map((movie) => {
          const poster = movie.posterUrl || movie.poster_url;
          const year = movie.releaseYear || movie.release_year;
          const id = movie.imdbId || movie.tmdbId || movie.id;

          return (
            <Link
              key={id}
              to={`/movie/${id}`}
              className="flex flex-col rounded-xl overflow-hidden bg-gray-800/80 border border-gray-700 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer"
            >
              <div className="w-full h-80 bg-black flex items-center justify-center overflow-hidden">
                {poster ? (
                  <img
                    src={poster}
                    alt={movie.title}
                    className="max-h-full max-w-full object-contain"
                  />
                ) : (
                  <div className="w-full h-full flex items-center justify-center text-gray-400">
                    No Image
                  </div>
                )}
              </div>
              <div className="p-3 flex flex-col h-24">
                <h3 className="font-bold text-sm neon-text break-words text-center flex-grow">
                  {movie.title || "Unknown Title"}
                </h3>
                <p className="text-xs opacity-70 text-center mt-2">
                  {year || "N/A"}
                </p>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
}

export default Search;