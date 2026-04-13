import { Link } from "react-router-dom";
import fmy from "../images/fmy.png";

// Poster display and title truncation fixed with Claude (April 2026)
function MovieGrid({ movies, title }) {
  if (!movies) return null;

  return (
    <div className="mt-12">
      {title && (
        <h3 className="text-xl sm:text-2xl md:text-3xl font-bold neon-text mb-6">
          {title}
        </h3>
      )}

      {movies.length === 0 ? (
        <p className="text-gray-400">No movies found.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-5 xl:grid-cols-6 gap-4 sm:gap-6">
          {movies.map((movie) => (
            <Link
              key={movie.imdbId || movie.id}
              to={`/movie/${movie.imdbId || movie.id}`}
              className="flex flex-col rounded-xl overflow-hidden bg-black/70 shadow-lg transform transition duration-300 hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer"
            >
              <div className="w-full h-52 sm:h-64 bg-black flex items-center justify-center">
                {movie.poster_url || movie.posterUrl ? (
                  <img
                    src={movie.poster_url || movie.posterUrl}
                    alt={movie.title}
                    className="w-full h-full object-contain"
                  />
                ) : (
                  <div className="w-full h-full flex items-center justify-center text-gray-400 text-sm">
                    No Image
                  </div>
                )}
              </div>
              <div className="p-3 flex flex-col h-auto sm:h-28">
                <h4 className="font-bold text-sm neon-text break-words text-center flex-grow line-clamp-2">
                   {movie.title}
                </h4>
                <p className="text-xs opacity-70 text-center mt-2">
                  {movie.release_year || movie.releaseYear || "N/A"}
                </p>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}

export default function Home({ movies, loading, error }) {
  return (
    <div className="text-white">
      <header className="relative w-full mt-4 sm:mt-6 px-2 sm:px-0 rounded-xl overflow-hidden shadow-xl mx-auto max-w-6xl">
        <img src={fmy} alt="Find My Flick banner" className="w-full h-auto object-contain" />
      </header>

      <section className="mt-8 px-4 sm:px-6 max-w-6xl mx-auto text-center">
        <h1 className="text-2xl sm:text-3xl md:text-5xl font-extrabold neon-text">
          Find Your Next Flick
        </h1>

        <p className="mt-4 text-base sm:text-lg text-gray-300 max-w-2xl mx-auto">
          Discover movies by genre, tags, triggers, and what actually matters to you.
        </p>

        <div className="mt-8 flex flex-col sm:flex-row justify-center gap-4">
          
          <Link to="/discover"
            className="px-6 py-3.5 rounded-full bg-pink-600 hover:bg-pink-500 transition font-semibold shadow-lg"
          >
            Explore Movies →
          </Link>
          
          <Link to="/genres"
            className="px-6 py-3.5 rounded-full border border-pink-500 text-pink-400 hover:bg-pink-500/10 transition font-semibold"
          >
            Browse Genres
          </Link>
        </div>
      </section>

      <section className="mt-16 px-6 max-w-6xl mx-auto">
        {loading && <p className="text-gray-400">Loading movies...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {!loading && !error && (
          <MovieGrid
            movies={movies.slice(0, 12)}
            title="Trending Now"
          />
        )}
      </section>

      <div className="h-12 sm:h-20" />
    </div>
  );
}