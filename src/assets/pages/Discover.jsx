import { useState, useEffect } from "react";
import { useSearchParams, Link } from "react-router-dom";
import API_URL from "../../config.js";

const PAGE_SIZE = 24;

export default function Discover() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);

  const [searchParams] = useSearchParams();
  const [selectedLetter, setSelectedLetter] = useState("A");

  const genre = searchParams.get("genre");
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  const filteredMovies = genre && selectedLetter
    ? movies.filter(m => m.title?.toUpperCase().startsWith(selectedLetter))
    : movies;

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

  const totalPages = Math.ceil(sortedMovies.length / PAGE_SIZE);
  const paginatedMovies = sortedMovies.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  );

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedLetter, genre]);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        setLoading(true);
        setError(null);

        let url;
        if (genre) {
          url = `${API_URL}/api/movies/getby/genre/${encodeURIComponent(genre)}?limit=500`;
        } else if (selectedLetter === "0-9") {
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
  }, [genre, selectedLetter]);

  return (
    <div className="min-h-screen p-4 sm:p-6 md:p-8 text-white bg-gradient-to-b from-black via-[#12001a] to-black overflow-x-hidden">

      {/* Header */}
      <div className="max-w-6xl mx-auto mb-8">
        <h1 className="text-4xl font-extrabold neon-text capitalize">
          {genre ? `${genre} Movies` : "Discover Movies"}
        </h1>
        <p className="opacity-80 mt-2">
          {genre
            ? `Browsing movies in the ${genre} genre`
            : "Explore movies from every genre"}
        </p>
      </div>

      {/* Alphabet filter buttons */}
      <div className="max-w-6xl mx-auto mb-8 overflow-x-auto px-4">
        <div className="flex justify-start sm:justify-center items-center gap-2 whitespace-nowrap py-2 px-2">
          {alphabet.map((letter) => (
            <button
              key={letter}
              onClick={() => setSelectedLetter(letter)}
              className={`w-8 h-8 sm:w-9 sm:h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition
                ${selectedLetter === letter
                  ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"
                  : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
                }`}
            >
              {letter}
            </button>
          ))}
          <button
            onClick={() => setSelectedLetter("0-9")}
            className={`w-12 h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition
              ${selectedLetter === "0-9"
                ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"
                : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
              }`}
          >
            0-9
          </button>
        </div>
      </div>

      {/* Loading / Error */}
      {loading && <p className="text-center mt-20 opacity-70">Loading movies...</p>}
      {error && <p className="text-center mt-20 text-red-400">{error}</p>}

      {/* Movie count */}
      {!loading && !error && (
        <p className="text-gray-400 text-sm mb-3 max-w-6xl mx-auto text-center">
          Showing {paginatedMovies.length} of {sortedMovies.length} movies
          {totalPages > 1 && ` — Page ${currentPage} of ${totalPages}`}
        </p>
      )}

      {/* Movie cards */}
      <div className="max-w-6xl mx-auto grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4 sm:gap-6">
        {paginatedMovies.map((movie) => {
          const poster = movie.poster_url || movie.posterUrl;
          const year = movie.release_year || movie.releaseYear;
          const id = movie.id || movie.imdbId;

          return id ? (
            <Link
              key={id}
              to={`/movie/${id}`}
              className="flex flex-col rounded-xl overflow-hidden bg-gray-800/80 border border-gray-700 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer"
            >
              {/* Poster */}
<div className="w-full h-79.9 bg-black flex items-center justify-center overflow-hidden">
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

              {/* Title & Year Box */}
        <div className="p-3 flex flex-col h-24">
      <h3 className="font-bold text-sm neon-text break-words text-center flex-grow">
      {movie.title}
     </h3>
     <p className="text-xs opacity-70 text-center mt-2">
    {year || "N/A"}
  </p>
</div>
            </Link>
          ) : (
            <div
              key={movie.title}
              className="flex flex-col rounded-xl overflow-hidden bg-gray-700/60 border border-gray-600"
            >
              <div className="relative w-full h-64 sm:h-72 md:h-80 bg-black flex items-center justify-center overflow-hidden">
                No Image
              </div>
              <div className="p-3">
                <h3 className="font-bold text-lg break-words">{movie.title}</h3>
                <p className="text-sm opacity-70 mt-1">{year || "N/A"}</p>
              </div>
            </div>
          );
        })}
      </div>

      {/* Pagination controls */}
      {!loading && !error && totalPages > 1 && (
        <div className="flex justify-center items-center gap-4 mt-8">
          <button
            onClick={() => { setCurrentPage(p => Math.max(1, p - 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === 1}
            className="px-4 py-2 rounded bg-pink-500 text-white disabled:opacity-40 hover:bg-pink-600 transition"
          >
            ← Previous
          </button>

          <span className="text-gray-300 text-sm">
            Page {currentPage} of {totalPages}
          </span>

          <button
            onClick={() => { setCurrentPage(p => Math.min(totalPages, p + 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === totalPages}
            className="px-4 py-2 rounded bg-pink-500 text-white disabled:opacity-40 hover:bg-pink-600 transition"
          >
            Next →
          </button>
        </div>
      )}
    </div>
  );
}