import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useMovies } from "../../context/MovieContext";

export default function Discover() {

  const { movies, loading, error } = useMovies();
  const [searchParams] = useSearchParams();
  const [selectedLetter, setSelectedLetter] = useState("");
  const [selectedMovie, setSelectedMovie] = useState(null);

  const genre = searchParams.get("genre");

  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

  let filteredMovies = movies;

  if (genre) {
    filteredMovies = filteredMovies.filter((movie) =>
      movie.genre?.map((g) => g.toLowerCase()).includes(genre.toLowerCase())
    );
  }

  if (selectedLetter) {
    filteredMovies = filteredMovies.filter((movie) =>
      movie.title?.toUpperCase().startsWith(selectedLetter)
    );
  }

  const sortedMovies = [...filteredMovies].sort((a, b) =>
    a.title.localeCompare(b.title)
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

          {/* All Button */}
          <button
            onClick={() => setSelectedLetter("")}
            className={`
              w-9 h-9 rounded border border-pink-500
              text-pink-400 font-semibold
              flex items-center justify-center
              transition-all duration-200
              ${
                selectedLetter === ""
                  ? "bg-pink-500/20 shadow-[0_0_12px_#ff6ed0]"
                  : "hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
              }
            `}
          >
            All
          </button>

          {alphabet.map((letter) => (
            <button
              key={letter}
              onClick={() => setSelectedLetter(letter)}
              className={`
                w-9 h-9 rounded border border-pink-500
                text-white font-semibold
                flex items-center justify-center
                transition-all duration-200
                ${
                  selectedLetter === letter
                    ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"
                    : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
                }
              `}
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

      {/* Empty State */}
      {!loading && sortedMovies.length === 0 && (
        <p className="text-center mt-20 opacity-80 text-lg neon-text">
          {selectedLetter
            ? `No movies starting with "${selectedLetter}".`
            : "No movies found."}
        </p>
      )}

      {/* Movies Grid */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">

      {sortedMovies.map((movie) => (
  <div
    key={movie.imdbId}
    onClick={() => setSelectedMovie(movie.imdbId)}
    className={`
      rounded-xl overflow-hidden bg-black/60
      shadow-lg transform transition duration-300 cursor-pointer
      hover:scale-105
      ${selectedMovie === movie.imdbId 
        ? "shadow-[0_0_25px_#ff6ed0]" // glow when clicked
        : "hover:shadow-[0_0_25px_#ff6ed0]"} // normal hover
    `}
  >

            {/* Poster */}
            <div className="h-64 bg-black">
              <img
                src={
                  movie.poster_url ||
                  "https://via.placeholder.com/300x450?text=No+Poster"
                }
                alt={movie.title}
                className="w-full h-full object-contain"
              />
            </div>

            {/* Info */}
            <div className="p-4">
              <h3 className="font-bold text-lg neon-text truncate">
                {movie.title}
              </h3>

              <p className="text-sm opacity-70 mt-1">
                {movie.release_year || "N/A"}
              </p>
            </div>

          </div>
        ))}

      </div>
    </div>
  );
}