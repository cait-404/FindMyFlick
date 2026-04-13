import { useState, useEffect } from "react"; 
import { useParams, Link } from "react-router-dom";
import API_URL from "../../config.js";

const PAGE_SIZE = 24;

export default function GenreBrowse() {
  const { genreName } = useParams();
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedLetter, setSelectedLetter] = useState("");

  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  const stripArticle = (title) => {
    if (!title) return "";
    if (title.match(/^the /i)) return title.substring(4).trimStart();
    if (title.match(/^an /i)) return title.substring(3).trimStart();
    if (title.match(/^a /i)) return title.substring(2).trimStart();
    return title;
  };

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        setLoading(true);
        setError(null);
        const res = await fetch(
          `${API_URL}/api/movies/getby/genre/${encodeURIComponent(
            genreName
          )}?limit=2000`
        );
        if (!res.ok) throw new Error("Failed to fetch movies");
        const data = await res.json();
        setMovies(data);
      } catch (err) {
        console.error("Fetch error:", err);
        setError("Failed to load movies for this genre.");
      } finally {
        setLoading(false);
      }
    };
    fetchMovies();
  }, [genreName]);

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedLetter]);

  const filteredMovies = selectedLetter
    ? movies.filter((m) =>
        stripArticle(m.title || "").toUpperCase().startsWith(selectedLetter)
      )
    : movies;

  const sortedMovies = [...filteredMovies].sort((a, b) =>
    stripArticle(a.title || "").localeCompare(stripArticle(b.title || ""))
  );

  const totalPages = Math.ceil(sortedMovies.length / PAGE_SIZE);
  const paginatedMovies = sortedMovies.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  );

  const displayName = genreName
    ? genreName.charAt(0).toUpperCase() + genreName.slice(1).toLowerCase()
    : "";

  return (
    <div className="min-h-screen p-6 text-white bg-gradient-to-b from-black via-[#12001a] to-black">

      {/* Header */}
      <div className="max-w-6xl mx-auto mb-6">
        <Link
          to="/genres"
          className="text-pink-400 hover:text-pink-300 text-sm mb-2 inline-block"
        >
          ← Back to Genres
        </Link>
        <h1 className="text-3xl md:text-4xl font-bold neon-text">{displayName} Movies</h1>
        {!loading && !error && (
          <p className="text-gray-400 text-sm mt-1">
            {sortedMovies.length} movies available
          </p>
        )}
      </div>

      {/* Alphabet Filter */}
      <div className="flex justify-start md:justify-center gap-2 whitespace-nowrap py-3 overflow-x-auto px-2">
        <div className="flex justify-center gap-2 whitespace-nowrap py-3">
          <button
            className={`w-14 h-10 md:w-12 md:h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition active:scale-95
              ${selectedLetter === "" ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]" : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"}`}
            onClick={() => setSelectedLetter("")}
          >
            All
          </button>
          {alphabet.map((letter) => (
            <button
              key={letter}
              className={`w-10 h-10 md:w-9 md:h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition active:scale-95
                ${selectedLetter === letter ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]" : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"}`}
              onClick={() => setSelectedLetter(letter)}
            >
              {letter}
            </button>
          ))}
        </div>
      </div>

      {/* Loading / Error */}
      {loading && <p className="text-center mt-20 opacity-70">Loading movies...</p>}
      {error && <p className="text-center mt-20 text-red-400">{error}</p>}

      {/* Movie Count */}
      {!loading && !error && (
        <p className="text-gray-400 text-sm mb-3 max-w-6xl mx-auto text-center">
          Showing {paginatedMovies.length} of {sortedMovies.length} movies
          {totalPages > 1 && ` — Page ${currentPage} of ${totalPages}`}
        </p>
      )}

      {/* Movie Cards */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
        {paginatedMovies.map((movie) => {
          const poster = movie.poster_url || movie.posterUrl;
          const year = movie.release_year || movie.releaseYear;
          const id = movie.id || movie.imdbId;

          return id ? (
            <Link
              key={id}
              to={`/movie/${id}`}
              className="flex flex-col rounded-xl overflow-hidden bg-gray-900/70 border border-gray-700 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer"
            >
              {/* Poster */}
              <div className="relative w-full bg-black flex items-center justify-center">
                {poster ? (
                  <img
                    src={poster}
                    alt={movie.title}
                    className="w-full max-h-96 object-contain"
                  />
                ) : (
                  <div className="w-full max-h-96 flex items-center justify-center text-gray-400">
                    No Image
                  </div>
                )}
              </div>

              {/* Title & Year Box */}
              <div className="p-3 flex flex-col h-28">
                <h3 className="font-bold text-sm neon-text break-words text-center flex-grow">
                  {movie.title}
                </h3>
                <p className="text-xs opacity-70 text-center mt-2">{year || "N/A"}</p>
              </div>
            </Link>
          ) : null;
        })}
      </div>

      {/* Pagination */}
      {!loading && !error && totalPages > 1 && (
        <div className="flex justify-center items-center gap-4 mt-8">
          <button
            onClick={() => { setCurrentPage(p => Math.max(1, p - 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === 1}
            className="px-5 py-3 rounded bg-pink-500 text-white text-sm md:text-base active:scale-95 hover:bg-pink-600 transition"
          >
            ← Previous
          </button>
          <span className="text-gray-300 text-sm">
            Page {currentPage} of {totalPages}
          </span>
          <button
            onClick={() => { setCurrentPage(p => Math.min(totalPages, p + 1)); window.scrollTo(0, 0); }}
            disabled={currentPage === totalPages}
            className="px-5 py-3 rounded bg-pink-500 text-white text-sm md:text-base active:scale-95 hover:bg-pink-600 transition"
          >
            Next →
          </button>
        </div>
      )}
    </div>
  );
}