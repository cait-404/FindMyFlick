import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";

export default function Discover() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchParams] = useSearchParams();

  const genre = searchParams.get("genre");

  useEffect(() => {
    setLoading(true);

    let url = "http://localhost:5135/api/Movies/search";

    if (genre) {
      url += `?genres=${genre}`;
    }

    fetch(url)
      .then(res => res.json())
      .then(data => {
        setMovies(data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  }, [genre]);

  return (
    <div className="min-h-screen p-8 text-white bg-gradient-to-b from-black via-[#12001a] to-black">
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

      {/* Loading */}
      {loading && (
        <p className="text-center mt-20 opacity-70">
          Loading movies...
        </p>
      )}

      {/* Empty State */}
      {!loading && movies.length === 0 && (
        <p className="text-center mt-20 opacity-70">
          No movies found for this genre.
        </p>
      )}

      {/* Movies Grid */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">
        {movies.map(movie => (
          <div
            key={movie.id}
            className="
              rounded-xl overflow-hidden bg-black/60
              shadow-lg transform transition duration-300
              hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0]
            "
          >
            {/* Poster */}
            <div className="h-64 bg-black">
              <img
                src={movie.poster || "https://via.placeholder.com/300x450?text=No+Poster"}
                alt={movie.name}
                className="w-full h-full object-contain"
              />
            </div>

            {/* Info */}
            <div className="p-4">
              <h3 className="font-bold text-lg neon-text truncate">
                {movie.name}
              </h3>

              <p className="text-sm opacity-70 mt-1">
                {movie.year} • {movie.ageRating?.toUpperCase()}
              </p>

              <div className="flex flex-wrap gap-2 mt-2">
                {movie.genre.map((g, i) => (
                  <span
                    key={i}
                    className="text-xs px-2 py-1 rounded-full bg-pink-700/70"
                  >
                    {g}
                  </span>
                ))}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
