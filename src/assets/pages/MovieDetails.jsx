import { useParams } from "react-router-dom";

export default function MovieDetails({ movies }) {

  const { id } = useParams();

  const movie = movies.find((m) => String(m.id) === id);

  if (!movie) {
    return (
      <div className="p-10 text-center text-white">
        Movie not found
      </div>
    );
  }

  return (
    <div className="p-10 text-white">

      <h1 className="text-4xl font-bold mb-4">
        {movie.title}
      </h1>

      <p className="mb-4">
        {movie.description || "No description available."}
      </p>

      <p className="mb-2">
        <strong>Genre:</strong> {movie.genre || "Unknown"}
      </p>

      <p className="mb-2">
        <strong>Plot:</strong> {movie.plot || "No plot summary available."}
      </p>

      <p className="mb-2">
        <strong>Streaming:</strong> Netflix / Prime / Hulu
      </p>

      <p className="mb-2">
        <strong>Triggers:</strong> Violence, Gore
      </p>

    </div>
  );
}