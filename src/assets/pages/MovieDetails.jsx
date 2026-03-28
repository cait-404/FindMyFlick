import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import API_URL from "../../config.js";

export default function MovieDetails() {
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);

  const [triggers, setTriggers] = useState([]);
  const [genres, setGenres] = useState([]);
  const [providers, setProviders] = useState([]);
  const [cast, setCast] = useState([]);
  const [crew, setCrew] = useState([]);
  const [warnings, setWarnings] = useState([]);
  const [collections, setCollections] = useState([]);

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        // Main movie
        const res = await fetch(`${API_URL}/api/Movies/${id}`);
        if (!res.ok) throw new Error(`Server error: ${res.status}`);
        const data = await res.json();
        setMovie(data);

        // Parallel fetches
        const [
          plotTagsRes,
          genresRes,
          streamingRes,
          castRes,
          crewRes,
          warningsRes,
          collectionsRes
        ] = await Promise.all([
          fetch(`${API_URL}/api/movies/${id}/plot-tags`),
          fetch(`${API_URL}/api/movies/${id}/genres`),
          fetch(`${API_URL}/api/movies/${id}/streaming-providers`),
          fetch(`${API_URL}/api/movies/${id}/cast`),
          fetch(`${API_URL}/api/movies/${id}/crew`),
          fetch(`${API_URL}/api/movies/${id}/warnings`),
          fetch(`${API_URL}/api/movies/${id}/collections`)
        ]);

        // Convert responses to JSON
        const plotTagsData = plotTagsRes.ok ? await plotTagsRes.json() : [];
        const genresData = genresRes.ok ? await genresRes.json() : [];
        const streamingData = streamingRes.ok ? await streamingRes.json() : [];
        const castData = castRes.ok ? await castRes.json() : [];
        const crewData = crewRes.ok ? await crewRes.json() : [];
        const warningsData = warningsRes.ok ? await warningsRes.json() : [];
        const collectionsData = collectionsRes.ok ? await collectionsRes.json() : [];

        // Map to readable strings
        setTriggers(plotTagsData.map(t => t.keywordName || t.name || "").filter(Boolean));
        setGenres(genresData.map(g => g.genreName || g.name || "").filter(Boolean));
        setProviders(streamingData.map(p => p.providerName || "").filter(Boolean));
        setCast(castData.map(c => c.personName || "").filter(Boolean));
        setCrew(crewData.map(c => `${c.personName} (${c.jobs?.join(", ")})`).filter(Boolean));
        setWarnings(warningsData.map(w => `${w.topicName}: ${w.answer ? "Yes" : "No"}`));
        setCollections(collectionsData.map(c => c.name || "").filter(Boolean));

      } catch (err) {
        console.error("Error fetching movie:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMovieDetails();
  }, [id]);

  if (error) {
    return (
      <div className="text-center mt-10 text-red-400">
        <p>Could not load movie details.</p>
        <p className="text-sm text-gray-400">Try another movie.</p>
      </div>
    );
  }

  if (loading) {
    return (
      <p className="text-white text-center mt-10">
        Loading movie details...
      </p>
    );
  }

  if (!movie) return null;

  const poster = movie.poster_url || movie.posterUrl;

  return (
    <div className="min-h-screen text-white p-10 bg-linear-to-b from-black via-[#12001a] to-black">
      <div className="max-w-5xl mx-auto bg-black/70 p-8 rounded-xl shadow-lg">
        <div className="grid md:grid-cols-2 gap-8">

          {/* POSTER */}
          <div>
            {poster ? (
              <img
                src={poster}
                alt={movie.title}
                className="w-full rounded-lg shadow-lg object-contain"
              />
            ) : (
              <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-lg">
                No Image
              </div>
            )}
          </div>

          {/* INFO */}
          <div>
            <h1 className="text-4xl font-bold mb-4 text-pink-400">
              {movie.title || "Untitled"}
            </h1>

            <p className="mb-2 text-gray-300">
              Year: {movie.release_year || movie.releaseYear || "N/A"}
            </p>

            <p className="mb-4 text-gray-300">
              Rating: {movie.mpaa_rating || "N/A"}
            </p>

            <p className="mb-6">
              {movie.plot_summary || "No description available."}
            </p>

            <p className="mb-4"><strong>Genres:</strong> {genres.join(", ") || "Unknown"}</p>
            <p className="mb-4"><strong>Triggers:</strong> {triggers.join(", ") || "None"}</p>
            <p className="mb-4"><strong>Streaming:</strong> {providers.join(", ") || "Unknown"}</p>
            <p className="mb-4"><strong>Cast:</strong> {cast.join(", ") || "Unknown"}</p>
            <p className="mb-4"><strong>Crew:</strong> {crew.join(", ") || "Unknown"}</p>
            <p className="mb-4"><strong>Warnings:</strong> {warnings.join(", ") || "None"}</p>
            <p className="mb-4"><strong>Collections:</strong> {collections.join(", ") || "None"}</p>

          </div>
        </div>
      </div>
    </div>
  );
}