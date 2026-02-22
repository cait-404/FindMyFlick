import { useEffect, useState } from "react";

export default function About() {
  const [movieCount, setMovieCount] = useState(null);
  const [genreCount, setGenreCount] = useState(null);
  const [status, setStatus] = useState("Connecting...");

  useEffect(() => {
    Promise.all([
      fetch("http://localhost:5135/Movies").then(res => res.json()),
      fetch("http://localhost:5135/Tags").then(res => res.json())
    ])
      .then(([movies, tags]) => {
        setMovieCount(movies.length);
        setGenreCount(tags.length);
        setStatus("Connected");
      })
      .catch(() => {
        setStatus("");
      });
  }, []);

  return (
    <div className="p-6 max-w-4xl mx-auto text-white">
      <h2 className="text-4xl font-bold neon-text mb-6">
        About FindMyFlick
      </h2>

      <p className="text-lg mb-4 opacity-90">
        FindMyFlick is a movie discovery platform designed to help users explore,
        search, and discover movies with ease. From trending titles to browsing
        by genre, the goal is to simplify how users find something great to watch.
      </p>

      <p className="text-lg mb-6 opacity-90">
        This application is built as a full-stack project, combining a modern
        React frontend with a backend API that delivers movie data, genres,
        and search results in real time.
      </p>

      {/* Live Backend Stats */}
      <div className="bg-black/40 rounded-xl p-6 shadow-lg mb-8">
        <h3 className="text-2xl font-semibold neon-text mb-4">
          Live Platform Stats
        </h3>

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-6 text-center">
          <div>
            <p className="text-3xl font-bold neon-text">
              {movieCount !== null ? movieCount : "--"}
            </p>
            <p className="opacity-80">Movies Available</p>
          </div>

          <div>
            <p className="text-3xl font-bold neon-text">
              {genreCount !== null ? genreCount : "--"}
            </p>
            <p className="opacity-80">Genres</p>
          </div>

          <div>
            <p
              className={`text-3xl font-bold ${
                status === "Connected" ? "text-green-400" : "text-red-400"
              }`}
            >
              {status}
            </p>
            <p className="opacity-80">API Status</p>
          </div>
        </div>
      </div>

      <h3 className="text-2xl font-semibold neon-text mb-3">
        Our Vision
      </h3>

      <p className="text-lg opacity-90">
        FindMyFlick aims to bridge the gap between overwhelming streaming
        platforms and users who just want a straightforward way to find their
        next movie. The focus is on usability, clarity, and a visually engaging
        experience.
      </p>

      <p className="text-lg mt-4 opacity-90">
        Future improvements may include personalized recommendations, user
        profiles, saved watchlists, and integrations with external movie APIs.
      </p>
    </div>
  );
}
