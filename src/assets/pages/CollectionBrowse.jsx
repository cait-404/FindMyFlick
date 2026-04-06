import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import API_URL from "../../config.js";

// CollectionBrowse page — shows all movies in a collection
// Added with Claude (April 2026)

function CollectionBrowse() {
  const { collectionName } = useParams();
  const navigate = useNavigate();
  const decoded = decodeURIComponent(collectionName);

  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!collectionName) return;

    setLoading(true);
    setError(null);

    fetch(`${API_URL}/api/movies/getby/collection/${encodeURIComponent(decoded)}`)
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch collection");
        return res.json();
      })
      .then((data) => setMovies(data))
      .catch((err) => {
        console.error("Error fetching collection:", err);
        setError("Failed to load collection. Try again later.");
      })
      .finally(() => setLoading(false));
  }, [collectionName]);

  return (
    <div className="min-h-screen p-8 text-white bg-black">
      <h2 className="text-3xl font-bold mb-6 neon-text">{decoded}</h2>

      {loading && <p>Loading movies...</p>}
      {error && <p className="text-red-400">{error}</p>}
      {!loading && movies.length === 0 && !error && (
        <p>No movies found for this collection.</p>
      )}

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {movies.map((movie) => (
          <div
            key={movie.imdbId}
            onClick={() => navigate(`/movie/${movie.imdbId}`)}
            className="bg-black/70 rounded-xl overflow-hidden shadow-lg p-4 transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer"
          >
            <div className="h-64 bg-black mb-4">
              <img
                src={movie.posterUrl || "https://via.placeholder.com/300x450?text=No+Poster"}
                alt={movie.title || "Movie Poster"}
                className="w-full h-full object-contain"
              />
            </div>
            <h3 className="font-bold text-lg truncate neon-text">{movie.title || "Unknown Title"}</h3>
            <p className="text-sm opacity-70 mt-1">{movie.releaseYear || "N/A"}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default CollectionBrowse;