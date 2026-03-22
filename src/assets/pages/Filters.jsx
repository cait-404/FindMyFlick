import { useState } from "react";
import { Link } from "react-router-dom";

export default function Filters({ movies }) {

  const [genre, setGenre] = useState("");
  const [includeTag, setIncludeTag] = useState("");
  const [excludeTag, setExcludeTag] = useState("");

  const filteredMovies = movies.filter(movie => {

    if (genre && movie.genre !== genre) return false;
    if (includeTag && !movie.tags?.includes(includeTag)) return false;
    if (excludeTag && movie.tags?.includes(excludeTag)) return false;

    return true;
  });

  return (
    <div className="min-h-screen text-white p-10">

      {/* PAGE TITLE */}
      <h1 className="text-4xl font-bold text-center mb-10 text-pink-400">
        Advanced Movie Filters
      </h1>

      {/* FILTER PANEL */}
      <div className="bg-black/70 p-8 rounded-xl shadow-lg max-w-4xl mx-auto mb-12">

        <div className="grid md:grid-cols-3 gap-6">

          {/* GENRE */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Genre
            </label>

            <select
              className="w-full p-3 rounded-md text-black"
              value={genre}
              onChange={(e) => setGenre(e.target.value)}
            >
              <option value="">All Genres</option>
              <option>Horror</option>
              <option>Comedy</option>
              <option>Drama</option>
              <option>Action</option>
              <option>Thriller</option>
            </select>
          </div>

          {/* INCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Include Trigger
            </label>

            <input
              type="text"
              placeholder="violence, gore..."
              className="w-full p-3 rounded-md text-black"
              value={includeTag}
              onChange={(e) => setIncludeTag(e.target.value)}
            />
          </div>

          {/* EXCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">
              Exclude Trigger
            </label>

            <input
              type="text"
              placeholder="spiders, death..."
              className="w-full p-3 rounded-md text-black"
              value={excludeTag}
              onChange={(e) => setExcludeTag(e.target.value)}
            />
          </div>

        </div>
      </div>

      {/* RESULTS */}
      <h2 className="text-2xl font-bold mb-6 text-center text-pink-300">
        Filter Results
      </h2>

      {filteredMovies.length === 0 ? (
        <p className="text-center text-gray-300">
          No movies match your filters.
        </p>
      ) : (

        <div className="grid md:grid-cols-3 lg:grid-cols-4 gap-8">

          {filteredMovies.map(movie => (

            <Link key={movie.id} to={`/movie/${movie.id}`}>

              <div className="bg-black/60 p-6 rounded-xl hover:bg-pink-500/20 transition hover:scale-105 shadow-lg">

                <h3 className="text-xl font-semibold mb-2">
                  {movie.title}
                </h3>

                <p className="text-sm text-gray-300 mb-2">
                  {movie.genre}
                </p>

                <p className="text-xs text-gray-400">
                  {movie.tags?.join(", ")}
                </p>

              </div>

            </Link>

          ))}

        </div>

      )}

    </div>
  );
}