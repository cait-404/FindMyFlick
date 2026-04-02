import { Link } from "react-router-dom";
import fmy from "../images/fmy.png";

// Poster display and title truncation fixed with Claude (April 2026)
function MovieGrid({ movies, title }) {
  if (!movies) return null;

  return (
    <div className="mt-12">
      {title && (
        <h3 className="text-2xl md:text-3xl font-bold neon-text mb-6">
          {title}
        </h3>
      )}

      {movies.length === 0 ? (
        <p className="text-gray-400">No movies found.</p>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
          {movies.map((movie) => (
            <Link
              key={movie.id}
              to={`/movie/${movie.id}`}
              className="flex flex-col rounded-xl overflow-hidden bg-gray-900/80 hover:scale-105 transform transition duration-300 shadow-lg"
            >
              {movie.poster_url ? (
                <img
                  src={movie.poster_url}
                  alt={movie.title}
                  className="w-full object-contain"
                />
              ) : (
                <div className="w-full h-48 bg-gray-800 flex items-center justify-center text-gray-400 text-sm">
                  No Image
                </div>
              )}
              <div className="p-2 flex flex-col gap-1">
                <h4 className="font-semibold text-sm text-white leading-snug">
                  {movie.title}
                </h4>
                <p className="text-sm text-gray-400 mt-1">{movie.release_year}</p>
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
      <header className="relative h-[300px] md:h-[450px] w-full mt-6 rounded-xl overflow-hidden shadow-xl mx-auto max-w-6xl">
        <img src={fmy} alt="Find My Flick banner" className="w-full h-full object-cover" />
      </header>

      <section className="mt-8 px-6 max-w-6xl mx-auto text-center">
        <h1 className="text-4xl md:text-5xl font-extrabold neon-text">
          Find Your Next Flick
        </h1>

        <p className="mt-4 text-lg text-gray-300 max-w-2xl mx-auto">
          Discover movies by genre, tags, triggers, and what actually matters to you.
        </p>

        <div className="mt-8 flex justify-center gap-4 flex-wrap">
          
          <a  href="/discover"
            className="px-6 py-3 rounded-full bg-pink-600 hover:bg-pink-500 transition font-semibold shadow-lg"
          >
            Explore Movies →
          </a>

          
          <a href="/genres"
            className="px-6 py-3 rounded-full border border-pink-500 text-pink-400 hover:bg-pink-500/10 transition font-semibold"
          >
            Browse Genres
          </a>
        </div>
      </section>

      <section className="mt-16 px-6 max-w-6xl mx-auto">
        {loading && <p className="text-gray-400">Loading movies...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {!loading && !error && (
          <MovieGrid
            movies={movies
              .filter(m => m.release_year && m.release_year <= new Date().getFullYear())
              .slice(0, 12)}
            title="Trending Now"
          />
        )}
      </section>

      <div className="h-20" />
    </div>
  );
}