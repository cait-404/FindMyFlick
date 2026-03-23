import { useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import { useMovies } from "../../context/MovieContext";

export default function Discover() {

  const { movies, loading, error } = useMovies();
  const [searchParams] = useSearchParams();
  const [selectedLetter, setSelectedLetter] = useState("");

  const genre = searchParams.get("genre");

  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  let filteredMovies = movies || [];

  // 🎯 GENRE FILTER
  if (genre) {
    filteredMovies = filteredMovies.filter((movie) =>
      movie.genre?.some(g =>
        g.toLowerCase().includes(genre.toLowerCase())
      )
    );
  }

  // 🔤 LETTER FILTER
  if (selectedLetter) {
    filteredMovies = filteredMovies.filter((movie) =>
      movie.title?.toUpperCase().startsWith(selectedLetter)
    );
  }

  // 🔤 SORT
  const sortedMovies = [...filteredMovies].sort((a, b) =>
    (a.title || "").localeCompare(b.title || "")
  );

  return (
    <div className="min-h-screen p-8 text-white bg-linear-to-b from-black via-[#12001a] to-black">

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

      {/* A-Z Filter */}
      <div className="max-w-6xl mx-auto mb-8 overflow-x-auto px-4">
        <div className="flex justify-center items-center gap-3 whitespace-nowrap py-2">

          <button
            onClick={() => setSelectedLetter("")}
            className={`w-9 h-9 rounded border border-pink-500 text-pink-400 font-semibold flex items-center justify-center transition
              ${selectedLetter === ""
                ? "bg-pink-500/20 shadow-[0_0_12px_#ff6ed0]"
                : "hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
              }`}
          >
            All
          </button>

          {alphabet.map((letter) => (
            <button
              key={letter}
              onClick={() => setSelectedLetter(letter)}
              className={`w-9 h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition
                ${selectedLetter === letter
                  ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"
                  : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
                }`}
            >
              {letter}
            </button>
          ))}

        </div>
      </div>

      {/* Loading */}
      {loading && (
        <p className="text-center mt-20 opacity-70">
          Loading movies...
        </p>
      )}

      {/* Error */}
      {error && (
        <p className="text-center mt-20 text-red-400">
          {error}
        </p>
      )}

      {/* Empty */}
      {!loading && sortedMovies.length === 0 && (
        <p className="text-center mt-20 opacity-80 text-lg neon-text">
          {selectedLetter
            ? `No movies starting with "${selectedLetter}".`
            : "No movies found."}
        </p>
      )}

      {/* GRID */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">

        {sortedMovies.map((movie) => {

          const poster = movie.poster_url || movie.posterUrl;
          const year = movie.release_year || movie.releaseYear;
          const hasId = !!movie.id;

          return hasId ? (

            /* ✅ CLICKABLE (NOW USES movie.id) */
            <Link
              key={movie.id}
              to={`/movie/${movie.id}`}
            >
              <div className="rounded-xl overflow-hidden bg-black/60 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer">

                <div className="h-64 bg-black">
                  {poster ? (
                    <img
                      src={poster}
                      alt={movie.title}
                      className="w-full h-full object-contain"
                    />
                  ) : (
                    <div className="w-full h-full flex items-center justify-center text-gray-400">
                      No Image
                    </div>
                  )}
                </div>

                <div className="p-4">
                  <h3 className="font-bold text-lg neon-text truncate">
                    {movie.title}
                  </h3>

                  <p className="text-sm opacity-70 mt-1">
                    {year || "N/A"}
                  </p>
                </div>

              </div>
            </Link>

          ) : (

            /* 🚫 fallback (should rarely happen now) */
            <div
              key={movie.title}
              className="rounded-xl overflow-hidden bg-gray-700 opacity-60"
            >

              <div className="h-64 bg-black flex items-center justify-center text-gray-400">
                No Image
              </div>

              <div className="p-4">
                <h3 className="font-bold text-lg truncate">
                  {movie.title}
                </h3>

                <p className="text-sm opacity-70 mt-1">
                  {year || "N/A"}
                </p>
              </div>

            </div>

          );

        })}

      </div>
    </div>
  );
}