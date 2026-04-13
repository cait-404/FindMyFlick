import { useState } from "react";

function MovieCard({ movie }) {
  const [flipped, setFlipped] = useState(false);

  return (
    <div
      className="movie-card-wrapper cursor-pointer w-full max-w-[180px] h-[260px]"
      onClick={() => setFlipped(!flipped)}
    >
      <div className={`movie-card-inner ${flipped ? "flipped" : ""}`}>
        {/* FRONT */}
        <div className="movie-card-front">
          <img
            src={movie.poster}
            alt={movie.name}
            className="w-full h-48 sm:h-56 object-cover"
          />
          <div className="p-3 text-center">
            <h4 className="font-semibold text-base sm:text-lg truncate">{movie.name}</h4>
            <p className="text-sm text-gray-400">{movie.Year}</p>
            <p className="text-[10px] sm:text-xs text-pink-400 mt-2">Click for details →</p>
          </div>
        </div>

        {/* BACK */}
        <div className="movie-card-back overflow-y-auto">
          <h4 className="font-semibold text-lg mb-1">{movie.name}</h4>
          <p className="text-gray-400 mb-2">{movie.Year}</p>
          <p className="text-gray-300 mb-2">{movie.summary}</p>
          <p className="text-gray-400 text-xs mb-1">
            Genres: {movie.genre.join(", ")}
          </p>
          <p className="text-gray-400 text-xs mb-1">
            Age Rating: {movie["age rating"]}
          </p>
          {movie["streaming services"]?.length > 0 && (
            <p className="text-gray-400 text-xs">
              Available on: {movie["streaming services"].join(", ")}
            </p>
          )}
          <p className="text-[10px] sm:text-xs text-pink-400 mt-3">← Click to flip back</p>
        </div>
      </div>
    </div>
  );
}

export default MovieCard;
