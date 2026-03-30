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
  // Only re-fetch when genre changes or when letter changes without an active genre
  }, [genre, genre ? null : selectedLetter]);

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold mb-4">Discover Movies</h1>

      {/* Alphabet filter buttons */}
      <div className="flex flex-wrap gap-2 mb-4">
        {alphabet.map((letter) => (
          <button
            key={letter}
            className={`px-3 py-1 rounded ${
              selectedLetter === letter ? "bg-purple-700 text-white" : "bg-gray-200 text-gray-800"
            }`}
            onClick={() => setSelectedLetter(letter)}
          >
            {letter}
          </button>
        ))}
        <button
          className="px-3 py-1 rounded bg-gray-400 text-white"
          onClick={() => setSelectedLetter("")}
        >
          All
        </button>
      </div>

      {/* Loading / Error */}
      {loading && <p>Loading movies...</p>}
      {error && <p className="text-red-500">{error}</p>}

      {/* Movies list */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        {sortedMovies.map((movie) => (
          <Link
            key={movie.imdbId || movie.id}
            to={`/movie/${movie.imdbId || movie.id}`}
            className="border rounded-lg p-2 hover:shadow-lg hover:scale-105 transition-transform duration-200"
          >
            <img
              src={movie.poster_url || movie.posterUrl}
              alt={movie.title}
              className="w-full h-64 object-cover rounded"
            />
            <h2 className="mt-2 font-semibold text-lg">{movie.title}</h2>
            <p className="text-gray-400 text-sm">{movie.genre?.join(", ")}</p>
          </Link>
        ))}
      </div>
    </div>
  );
}