import MovieCard from "./MovieCard";

// Poster URLs mapping
const posterMap = {
  "tt22740896": "https://image.tmdb.org/t/p/w500/kOrJqRyt1pklNgDwJMjzN1GuNXS.jpg",
  "tt12300742": "https://image.tmdb.org/t/p/w500/oxgsAQDAAxA92mFGYCZllgWkH9J.jpg",
  "tt29954526": "https://image.tmdb.org/t/p/w500/puPN6uC5NpbAQW8dDovGkJ79arn.jpg",
  "tt16311594": "https://image.tmdb.org/t/p/w500/vqBmyAj0Xm9LnS1xe1MSlMAJyHq.jpg",
  "tt1312221": "https://image.tmdb.org/t/p/w500/g4JtvGlQO7DByTI6frUobqvSL3R.jpg",
  "tt26743210": "https://image.tmdb.org/t/p/w500/q5pXRYTycaeW6dEgsCrd4mYPmxM.jpg",
  "tt31036941": "https://image.tmdb.org/t/p/w500/1RICxzeoNCAO5NpcRMIgg1XT6fm.jpg",
  "tt14205554": "https://image.tmdb.org/t/p/w500/zT7Lhw3BhJbMkRqm9Zlx2YGMsY0.jpg",
  "tt9603208": "https://image.tmdb.org/t/p/w500/z53D72EAOxGRqdr7KXXWp9dJiDe.jpg",
  "tt34956443": "https://image.tmdb.org/t/p/w500/cb5NyNrqiCNNoDkA8FfxHAtypdG.jpg",
  
};

function MovieGrid({ movies, title, onMovieClick }) {
  if (!movies) return null;

  return (
    <div className="mt-14">
      {title && (
        <h3 className="text-2xl md:text-3xl font-bold neon-text mb-6">
          {title}
        </h3>
      )}

      {movies.length === 0 ? (
        <p className="text-gray-400">No movies found.</p>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
          {movies.map((movie) => (
            <button
              key={movie.id}
              onClick={() => onMovieClick(movie)}
              className="group relative rounded-xl overflow-hidden 
                         bg-gray-900 shadow-lg 
                         hover:scale-105 transition-transform duration-300"
            >
              <img
                src={movie.poster}
                alt={movie.name}
                className="w-full h-72 object-cover"
              />

              {/* Hover overlay */}
              <div className="absolute inset-0 bg-black/60 
                              opacity-0 group-hover:opacity-100 
                              transition flex items-end p-3">
                <h4 className="text-sm font-semibold text-left">
                  {movie.name}
                </h4>
              </div>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}

