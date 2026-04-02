import { useState, useEffect } from "react";
import { useSearchParams, Link } from "react-router-dom";
import API_URL from "../../config.js";

export default function Discover() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [searchParams] = useSearchParams();
  const [selectedLetter, setSelectedLetter] = useState("");

  const genre = searchParams.get("genre");
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  // When a genre is active, apply letter filter client-side on fetched results
  const filteredMovies = genre && selectedLetter
    ? movies.filter(m => m.title?.toUpperCase().startsWith(selectedLetter))
    : movies;

  const sortedMovies = [...filteredMovies].sort((a, b) =>
    (a.title || "").localeCompare(b.title || "")
  );

  // Fetch movies from API
  useEffect(() => {
    const fetchMovies = async () => {
      try {
        setLoading(true);
        setError(null);

        let url;
        if (genre) {
          // Fetch all movies for this genre; letter filtering is applied client-side
          url = `${API_URL}/api/movies/getby/genre/${encodeURIComponent(genre)}?limit=500`;
        } else if (selectedLetter) {
          url = `${API_URL}/api/movies/getby/starts-with/${encodeURIComponent(selectedLetter)}?limit=500`;
        } else {
          url = `${API_URL}/api/Movies?page=1&order=title_asc`;
        }

        const res = await fetch(url);
        if (!res.ok) {
          const text = await res.text();
          throw new Error(text || "Failed to fetch movies");
        }

        const data = await res.json();
        setMovies(data);
      } catch (err) {
        console.error("Fetch error:", err);
        setError("Failed to load movies");
      } finally {
        setLoading(false);
      }
    };

    fetchMovies();
  // ✅ FIX: Proper dependency array so letter filter actually triggers a re-fetch
  }, [genre, selectedLetter]);

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold mb-4">Discover Movies</h1>

      {/* Alphabet filter buttons */}
      <div className="flex flex-wrap gap-2 mb-4">
        {alphabet.map((letter) => (
          <button
            key={letter}
            className={`px-3 py-1 rounded ${
              selectedLetter === letter
                ? "bg-purple-700 text-white"
                : "bg-gray-200 text-gray-800"
            }`}
            onClick={() => setSelectedLetter(letter)}
          >
            {letter}
          </button>
        ))}
        <button
          className={`px-3 py-1 rounded ${
            selectedLetter === ""
              ? "bg-purple-700 text-white"
              : "bg-gray-400 text-white"
          }`}
          onClick={() => setSelectedLetter("")}
        >
          All
        </button>
      </div>

      {/* Loading / Error */}
      {loading && <p>Loading movies...</p>}
      {error && <p className="text-red-500">{error}</p>}

      {/* ✅ FIX: 5-6 cards across, fixed poster height, full title shown */}
      <div className="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-7 xl:grid-cols-8 gap-3">
        {sortedMovies.map((movie) => (
          // ✅ FIX: Use movie.id (integer) so MovieDetails can fetch correctly
          <Link
            key={movie.id}
            to={`/movie/${movie.id}`}
            className="flex flex-col rounded-lg overflow-hidden bg-gray-900/80 hover:scale-105 transform transition duration-200 shadow-lg"
          >
            {movie.poster_url || movie.posterUrl ? (
              <img
                src={movie.poster_url || movie.posterUrl}
                alt={movie.title}
                className="w-full object-contain"
              />
            ) : (
              <div className="w-full h-48 bg-gray-800 flex items-center justify-center text-gray-400 text-sm">
                No Image
              </div>
            )}
            <div className="p-2 flex flex-col gap-1">
              {/* ✅ FIX: No truncation, title wraps fully */}
              <h2 className="font-semibold text-sm text-white leading-snug">
                {movie.title}
              </h2>
              <p className="text-gray-400 text-xs">
                {movie.releaseYear || movie.release_year}
              </p>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}