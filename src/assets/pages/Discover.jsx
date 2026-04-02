import { useState, useEffect } from "react";
import { useSearchParams, Link } from "react-router-dom";
import API_URL from "../../config.js";

// Pagination, 0-9 filter, and article-stripping sort added with Claude (April 2026)

const PAGE_SIZE = 24;

export default function Discover() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);

  const [searchParams] = useSearchParams();
  // Default to "A" instead of "All"
  const [selectedLetter, setSelectedLetter] = useState("A");

  const genre = searchParams.get("genre");
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  // When a genre is active, apply letter filter client-side on fetched results
  const filteredMovies = genre && selectedLetter
    ? movies.filter(m => m.title?.toUpperCase().startsWith(selectedLetter))
    : movies;

  // Strip leading articles (A, An, The) for sorting — matches backend behavior
  const stripArticle = (title) => {
    if (!title) return "";
    if (title.match(/^the /i)) return title.substring(4).trimStart();
    if (title.match(/^an /i)) return title.substring(3).trimStart();
    if (title.match(/^a /i)) return title.substring(2).trimStart();
    return title;
  };

  const sortedMovies = [...filteredMovies].sort((a, b) =>
    stripArticle(a.title || "").localeCompare(stripArticle(b.title || ""))
  );

  // Pagination
  const totalPages = Math.ceil(sortedMovies.length / PAGE_SIZE);
  const paginatedMovies = sortedMovies.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  );

  // Reset to page 1 when letter or genre changes
  useEffect(() => {
    setCurrentPage(1);
  }, [selectedLetter, genre]);

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
        } else if (selectedLetter === "0-9") {
          // Fetch movies starting with numbers or symbols
          url = `${API_URL}/api/movies/getby/non-alpha?limit=500`;
        } else {
          url = `${API_URL}/api/movies/getby/starts-with/${encodeURIComponent(selectedLetter)}?limit=500`;
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
        {/* 0-9 button at the end for numbers and symbols */}
        <button
          className={`px-3 py-1 rounded ${
            selectedLetter === "0-9"
              ? "bg-purple-700 text-white"
              : "bg-gray-200 text-gray-800"
          }`}
          onClick={() => setSelectedLetter("0-9")}
        >
          0-9
        </button>
      </div>

      {/* Loading / Error */}
      {loading && <p>Loading movies...</p>}
      {error && <p className="text-red-500">{error}</p>}

      {/* Movie count and page info */}
      {!loading && !error && (
        <p className="text-gray-400 text-sm mb-3">
          Showing {paginatedMovies.length} of {sortedMovies.length} movies
          {totalPages > 1 && ` — Page ${currentPage} of ${totalPages}`}
        </p>
      )}

      {/* Cards */}
      <div className="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-7 xl:grid-cols-8 gap-3">
        {paginatedMovies.map((movie) => (
          <Link
            key={movie.imdbId || movie.id}
            to={`/movie/${movie.imdbId || movie.id}`}
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

      {/* Pagination controls */}
      {!loading && !error && totalPages > 1 && (
        <div className="flex justify-center items-center gap-4 mt-8">
          <button
            onClick={() => { setCurrentPage(p => Math.max(1, p - 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === 1}
            className="px-4 py-2 rounded bg-purple-700 text-white disabled:opacity-40 hover:bg-purple-600 transition"
          >
            ← Previous
          </button>

          <span className="text-gray-300 text-sm">
            Page {currentPage} of {totalPages}
          </span>

          <button
            onClick={() => { setCurrentPage(p => Math.min(totalPages, p + 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === totalPages}
            className="px-4 py-2 rounded bg-purple-700 text-white disabled:opacity-40 hover:bg-purple-600 transition"
          >
            Next →
          </button>
        </div>
      )}
    </div>
  );
}