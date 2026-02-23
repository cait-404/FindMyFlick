import { useState, useEffect } from "react";
import fmy from "../images/fmy.png";


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
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {movies.map((movie) => (
            <div
              key={movie.id}
              className="rounded-xl overflow-hidden bg-gray-900/80 
                         hover:scale-105 transform transition duration-300 
                         shadow-lg"
            >
              <img
                src={movie.poster} 
                alt={movie.name}
                className="w-full h-56 object-cover"
              />

              <div className="p-3 text-center">
                <h4 className="font-semibold text-lg truncate">{movie.name}</h4>
                <p className="text-sm text-gray-400 mt-1">{movie.Year}</p>
                <p className="text-sm text-gray-300 mt-1 line-clamp-3">
                  {movie.summary}
                </p>
                <p className="text-xs text-gray-400 mt-1">
                  Genres: {movie.genre ? movie.genre.join(", ") : "N/A"}
                </p>
                <p className="text-xs text-gray-400">
                  Age Rating: {movie["age rating"]}
                </p>
                {movie["streaming services"] && movie["streaming services"].length > 0 && (
                  <p className="text-xs text-gray-400 mt-1">
                    Available on: {movie["streaming services"].join(", ")}
                  </p>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}


export default function Home() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch("https://localhost:5002/api/Movies")
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch movies");
        return res.json();
      })
      .then((data) => {
        console.log("API response:", data)
        setMovies(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setError(err.message);
        setLoading(false);
      });
  }, []);

  return (
    <div className="text-white">

      
      <header className="relative h-[300px] md:h-[450px] w-full mt-6 
                         rounded-xl overflow-hidden shadow-xl 
                         mx-auto max-w-6xl">
        <img
          src={fmy}
          alt="Find My Flick banner"
          className="w-full h-full object-cover"
        />
      </header>

     
      <section className="mt-8 px-6 max-w-6xl mx-auto text-center">
        <h1 className="text-4xl md:text-5xl font-extrabold neon-text">
          Find Your Next Flick
        </h1>

        <p className="mt-4 text-lg text-gray-300 max-w-2xl mx-auto">
          Discover movies by genre, tags, triggers, and what actually matters to you — not just what’s trending.
        </p>

        <div className="mt-8 flex justify-center gap-4 flex-wrap">
          <a
            href="/discover"
            className="px-6 py-3 rounded-full bg-pink-600 
                       hover:bg-pink-500 transition 
                       font-semibold shadow-lg"
          >
            Explore Movies →
          </a>

          <a
            href="/genres"
            className="px-6 py-3 rounded-full border 
                       border-pink-500 text-pink-400 
                       hover:bg-pink-500/10 transition 
                       font-semibold"
          >
            Browse Genres
          </a>
        </div>
      </section>

     
      <section className="mt-16 px-6 max-w-6xl mx-auto">
        {loading && <p className="text-gray-400">Loading movies...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {!loading && !error && <MovieGrid movies={movies} title="Trending Now" />}
      </section>

     
      <div className="h-20" />
    </div>
  );
}
