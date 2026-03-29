import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

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
        // 🎬 MAIN MOVIE
        const res = await fetch(`https://localhost:5002/api/Movies/${id}`);
        if (!res.ok) throw new Error(`Server error: ${res.status}`);

        const data = await res.json();
        console.log("Movie Data:", data);

        // ✅ FIX 1: API RETURNS ARRAY → TAKE FIRST ITEM
        const movieData = Array.isArray(data) ? data[0] : data;
        setMovie(movieData);

        // ⚡ PARALLEL FETCHES (safe)
        const responses = await Promise.allSettled([
          fetch(`https://localhost:5002/api/Movies/${id}/plot-tags`),
          fetch(`https://localhost:5002/api/Movies/${id}/genres`),
          fetch(`https://localhost:5002/api/Movies/${id}/streaming-providers`),
          fetch(`https://localhost:5002/api/Movies/${id}/cast`),
          fetch(`https://localhost:5002/api/Movies/${id}/crew`),
          fetch(`https://localhost:5002/api/Movies/${id}/warnings`),
          fetch(`https://localhost:5002/api/Movies/${id}/collections`)
        ]);

        const parse = async (res) =>
          res.status === "fulfilled" && res.value.ok
            ? await res.value.json()
            : [];

        const [
          plotTagsData,
          genresData,
          streamingData,
          castData,
          crewData,
          warningsData,
          collectionsData
        ] = await Promise.all(responses.map(parse));

        // 🎯 MAPPINGS
        setTriggers(plotTagsData.map(t => t.tagName || "").filter(Boolean));
        setGenres(genresData.map(g => g.genreName || "").filter(Boolean));
        setProviders(streamingData.map(p => p.providerName || "").filter(Boolean));
        setCast(castData.map(c => c.personName || "").filter(Boolean));
        setCrew(
          crewData
            .map(c => `${c.personName || ""} (${c.jobs?.join(", ") || ""})`)
            .filter(Boolean)
        );
        setWarnings(
          warningsData.map(w => `${w.topicName}: ${w.answer ? "Yes" : "No"}`)
        );
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

  // ✅ FIX 2: CORRECT FIELD NAMES FROM YOUR API
  const poster = movie.posterUrl || movie.poster;

  return (
    <div className="min-h-screen text-white p-10 bg-linear-to-b from-black via-[#12001a] to-black">
      <div className="max-w-5xl mx-auto bg-black/70 p-8 rounded-xl shadow-lg">
        <div className="grid md:grid-cols-2 gap-8">

          {/* 🎬 POSTER */}
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

          {/* 🎬 INFO */}
          <div>
            <h1 className="text-4xl font-bold mb-4 text-pink-400">
              {movie.title || "Untitled"}
            </h1>

            <p className="mb-2 text-gray-300">
              Year: {movie.releaseYear || "N/A"}
            </p>

            <p className="mb-4 text-gray-300">
              Rating: {movie.mpaaRating || "N/A"}
            </p>

            <p className="mb-6">
              {movie.plotSummary || "No description available."}
            </p>

            <p className="mb-4">
              <strong>Genres:</strong> {genres.join(", ") || "Unknown"}
            </p>

            <p className="mb-4">
              <strong>Triggers:</strong> {triggers.join(", ") || "None"}
            </p>

            <p className="mb-4">
              <strong>Streaming:</strong> {providers.join(", ") || "Unknown"}
            </p>

            <p className="mb-4">
              <strong>Cast:</strong> {cast.join(", ") || "Unknown"}
            </p>

            <p className="mb-4">
              <strong>Crew:</strong> {crew.join(", ") || "Unknown"}
            </p>

            <p className="mb-4">
              <strong>Warnings:</strong> {warnings.join(", ") || "None"}
            </p>

            <p className="mb-4">
              <strong>Collections:</strong> {collections.join(", ") || "None"}
            </p>
          </div>

        </div>
      </div>
    </div>
  );
}