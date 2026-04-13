import { useEffect, useState } from "react";
import API_URL from "../../config.js";
import EmmanImg from "../images/emman.JPG";
import MichelleImg from "../images/michelle.png";
import IsabelleImg from "../images/isabelle.png";
import CaitlinImg from "../images/caitlin.jpg";

export default function About() {
  const [movieCount, setMovieCount] = useState(null);
  const [genreCount, setGenreCount] = useState(null);
  const [status, setStatus] = useState("Connecting...");

  const team = [
    {
      name: "Emmanuella Asamoah",
      role: "Frontend Developer",
      img: EmmanImg,
      bio: "Designing smooth, interactive, and responsive UI for the movie discovery platform."
    }
    ,
    {
      name: "Michelle Yau",
      role: "Data Technology",
      img: MichelleImg,
      bio: "Focused on robust API design and ensuring smooth data delivery for the platform."
    },
    {
      name: "Isabelle Kramer",
      role: "Backend Developer",
      img: IsabelleImg,
      bio: "Crafting intuitive and visually engaging experiences for all users."
    },
    {
      name: "Caitlin Hemmert",
      role: "Cybersecurity",
      img: CaitlinImg || "/images/placeholder.jpg",
      bio: "Managing the team and enhancing security to provide a safe experience for all users."
    },
  ];

  useEffect(() => {
    Promise.all([
      fetch(`${API_URL}/api/Movies?page=1`),
      fetch(`${API_URL}/api/Genres`).then(res => res.json())
    ])
      .then(async ([moviesRes, genres]) => {
        const total = parseInt(moviesRes.headers.get("X-Total-Count") || "0", 10);
        setMovieCount(total || null);
        setGenreCount(genres.length);
        setStatus("Connected");
      })
      .catch(() => {
        setStatus("Offline");
      });
  }, []);

  return (
    <div className="px-4 sm:px-6 py-6 max-w-5xl mx-auto text-white">

      {/* Title */}
      <h2 className="text-3xl sm:text-4xl font-bold neon-text mb-6">
        About FindMyFlick
      </h2>

      {/* Description */}
      <p className="text-base sm:text-lg mb-4 opacity-90">
        FindMyFlick is a movie discovery platform designed to help users explore,
        search, and discover movies with ease. From trending titles to browsing
        by genre, the goal is to simplify how users find something great to watch.
      </p>

      <p className="text-base sm:text-lg mb-10 opacity-90">
        This application is built as a full-stack project, combining a modern
        React frontend with a backend API that delivers movie data and genres
        in real time.
      </p>

      {/* Divider */}
      <div className="h-px bg-linear-to-r from-transparent via-purple-500 to-transparent my-10"></div>

      {/* Meet the Team */}
      <h3 className="text-2xl font-semibold neon-text mb-4">Meet the Team</h3>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6 sm:gap-8 mb-10">
        {team.map((member) => (
          <div key={member.name} className="bg-purple-900/40 p-4 rounded-xl shadow-lg text-center hover:scale-105 transition">
            <img src={member.img} alt={member.name} className="w-32 h-32 sm:w-40 sm:h-40 md:w-48 md:h-48 object-cover mx-auto mb-4 rounded-full" />
            <h4 className="font-bold text-lg neon-text">{member.name}</h4>
            <p className="opacity-80 mb-2">{member.role}</p>
            <p className="text-sm opacity-90">{member.bio}</p>
          </div>
        ))}
      </div>
      {/* Divider */}
      <div className="h-px bg-linear-to-r from-transparent via-purple-500 to-transparent my-10"></div>

      {/* Live Backend Stats */}
      <h3 className="text-2xl font-semibold neon-text mb-6">
        Live Platform Stats
      </h3>

      <div className="grid grid-cols-1 sm:grid-cols-3 gap-6 sm:gap-8 text-center mb-10">

        {/* Movies */}
        <div className="bg-purple-900/40 p-6 rounded-xl shadow-lg hover:scale-105 transition">
          <p className="text-4xl font-bold neon-text">
            {movieCount ?? "--"}
          </p>
          <p className="opacity-80 mt-2">Movies Available</p>
        </div>

        {/* Genres */}
        <div className="bg-purple-900/40 p-6 rounded-xl shadow-lg hover:scale-105 transition">
          <p className="text-4xl font-bold neon-text">
            {genreCount ?? "--"}
          </p>
          <p className="opacity-80 mt-2">Genres</p>
        </div>

        {/* API Status */}
        <div className="bg-purple-900/40 p-6 rounded-xl shadow-lg hover:scale-105 transition">

          <div className="flex justify-center items-center gap-2 mb-2">
            <span
              className={`w-3 h-3 rounded-full ${
                status === "Connected"
                  ? "bg-green-400 animate-pulse"
                  : status === "Offline"
                  ? "bg-red-400"
                  : "bg-yellow-400 animate-pulse"
              }`}
            ></span>
            <p
              className={`text-3xl font-bold ${
                status === "Connected"
                  ? "text-green-400"
                  : status === "Offline"
                  ? "text-red-400"
                  : "text-yellow-400"
              }`}
            >
              {status}
            </p>
          </div>
          <p className="opacity-80">API Status</p>
        </div>

      </div>

      {/* Divider */}
      <div className="h-px bg-linear-to-r from-transparent via-purple-500 to-transparent my-10"></div>

      {/* Features */}
      <h3 className="text-2xl font-semibold neon-text mb-4">
        Key Features
      </h3>

      <ul className="space-y-2 opacity-90 mb-10">
        <li>🎬 Browse movies by genre</li>
        <li>🔍 Search movies instantly</li>
        <li>⚡ Real-time API powered movie data</li>
        <li>📱 Responsive modern UI</li>
      </ul>

      {/* Divider */}
      <div className="h-px bg-linear-to-r from-transparent via-purple-500 to-transparent my-10"></div>

      {/* Vision */}
      <h3 className="text-2xl font-semibold neon-text mb-3">
        Our Vision
      </h3>

      <p className="text-lg opacity-90 mb-4">
        FindMyFlick aims to bridge the gap between overwhelming streaming
        platforms and users who just want a straightforward way to find their
        next movie. The focus is on usability, clarity, and a visually engaging
        experience.
      </p>

      <p className="text-lg opacity-90">
        The project continues to evolve as new features and improvements are
        explored.
      </p>

      {/* Divider */}
      <div className="h-px bg-linear-to-r from-transparent via-purple-500 to-transparent my-10"></div>

      {/* Fun Section */}
      <h3 className="text-2xl font-semibold neon-text mb-4">
        Why FindMyFlick? 🌟
      </h3>

      <p className="text-lg opacity-90 mb-2">
        Because discovering your next favorite movie should be fun, simple, and visually exciting!  
      </p>
      <ul className="space-y-2 opacity-90">
        <li>💡 Simplified search experience</li>
        <li>🎨 Interactive movie browsing</li>
        <li>🧩 Personalized recommendations in the future</li>
      </ul>

    </div>
  );
}
