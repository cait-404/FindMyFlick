import { useState, useEffect } from "react";

export default function Discover() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:5135/api/movies")
      .then((res) => res.json())
      .then((data) => {
        setMovies(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching movies:", err);
        setLoading(false);
      });
  }, []);

  if (loading) {
    return (
      <div className="p-6 max-w-6xl mx-auto text-white">
        <h2 className="text-3xl font-bold neon-text mb-6">Discover Movies</h2>
        <p>Loading movies...</p>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-6xl mx-auto text-white">
      <h2 className="text-3xl font-bold neon-text mb-6">Discover Movies</h2>
      <p className="mb-6 opacity-80">
        Explore trending, recommended, and new release movies!
      </p>

      <section className="flex gap-4 overflow-x-scroll scrollbar-hide pb-4">
        {movies.map((movie) => (
          <div
            key={movie.id}
            className="relative min-w-[180px] h-[260px] rounded-xl overflow-hidden transform transition duration-300 hover:scale-105 hover:shadow-[0_0_20px_#ff6ed0]"
          >
            <img
              src={movie.poster || "https://via.placeholder.com/180x260"} 
              alt={movie.name}
              className="absolute inset-0 w-full h-full object-cover"
            />
            <div className="absolute bottom-0 left-0 w-full bg-black/40 text-center p-2 neon-text font-semibold">
              {movie.name}
            </div>
          </div>
        ))}
      </section>
    </div>
  );
}
