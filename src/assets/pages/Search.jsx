import { useEffect, useState } from "react";
import { useLocation, Link } from "react-router-dom";
 
function Search() {
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");
 
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchedQuery, setSearchedQuery] = useState("");
 
  useEffect(() => {
    if (!query) return;
 
    setLoading(true);
    setError(null);
    setMovies([]);
 
    const searchMovies = async () => {
      try {
        // Step 1: Title search first
        const titleRes = await fetch("https://localhost:5002/api/MovieSearch", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            take: 20,
            minMatches: 1,
            enableApiFallback: true,
            alwaysAddFromApis: false,  // ✅ faster — no external API calls
            titleContains: query,
            genreNames: [],
            keywordNames: [],
            personNames: [],
            personRoles: [],
            streamingProviderNames: []
          }),
        });
 
        if (!titleRes.ok) throw new Error("Failed to fetch movies");
        const titleData = await titleRes.json();
        const titleResults = titleData.results || [];
 
        // ✅ If title search found results, use them
        if (titleResults.length > 0) {
          setMovies(titleResults);
          setSearchedQuery(query);
          return;
        }
 
        // Step 2: Fallback — keyword search only
        const keywordRes = await fetch("https://localhost:5002/api/MovieSearch", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            take: 20,
            minMatches: 1,
            enableApiFallback: true,
            alwaysAddFromApis: false,
            titleContains: null,
            keywordNames: [query],
            genreNames: [],
            personNames: [],
            personRoles: [],
            streamingProviderNames: []
          }),
        });
 
        if (!keywordRes.ok) throw new Error("Failed to fetch movies");
        const keywordData = await keywordRes.json();
        const keywordResults = keywordData.results || [];
 
        // Step 3: If still nothing, try genre
        if (keywordResults.length > 0) {
          setMovies(keywordResults);
        } else {
          const genreRes = await fetch("https://localhost:5002/api/MovieSearch", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
              take: 20,
              minMatches: 1,
              enableApiFallback: true,
              alwaysAddFromApis: false,
              titleContains: null,
              keywordNames: [],
              genreNames: [query],
              personNames: [],
              personRoles: [],
              streamingProviderNames: []
            }),
          });
 
          if (!genreRes.ok) throw new Error("Failed to fetch movies");
          const genreData = await genreRes.json();
          setMovies(genreData.results || []);
        }
 
        setSearchedQuery(query);
 
      } catch (err) {
        console.error("Search error:", err);
        setError("Failed to fetch movies. Try again later.");
      } finally {
        setLoading(false);
      }
    };
 
    searchMovies();
  }, [query]);
 
  return (
<div className="min-h-screen p-8 text-white bg-black">
<h2 className="text-3xl font-bold mb-6 neon-text">
        Results for: <span className="text-pink-400">{query}</span>
</h2>
 
      {loading && <p className="text-gray-400">Searching...</p>}
      {error && <p className="text-red-400">{error}</p>}
      {!loading && movies.length === 0 && !error && (
<p className="text-gray-400">No results found for "{query}".</p>
      )}
 
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {movies.map((movie, index) => (
<Link
            key={movie.imdbId || movie.tmdbId || `result-${index}`}
            to={`/movie/${movie.imdbId || movie.tmdbId}`}
>
<div className="bg-black/70 rounded-xl overflow-hidden shadow-lg p-4 transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer">
<div className="h-64 bg-black mb-4">
<img
                  src={movie.posterUrl || "https://via.placeholder.com/300x450?text=No+Poster"}
                  alt={movie.title || "Movie Poster"}
                  className="w-full h-full object-contain"
                />
</div>
<h3 className="font-bold text-lg truncate neon-text">
                {movie.title || "Unknown Title"}
</h3>
<p className="text-sm opacity-70 mt-1">{movie.releaseYear || "N/A"}</p>
</div>
</Link>
        ))}
</div>
</div>
  );
}
 
export default Search;