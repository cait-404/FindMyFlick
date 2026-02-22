import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaChevronRight } from "react-icons/fa";

const genreColors = {
  action: "from-red-600/90 to-red-900/90",
  comedy: "from-yellow-400/90 to-yellow-700/90",
  drama: "from-purple-600/90 to-purple-900/90",
  horror: "from-zinc-900/90 to-black",
  "sci-fi": "from-blue-500/90 to-indigo-900/90",
  romance: "from-pink-500/90 to-rose-800/90",
  thriller: "from-indigo-600/90 to-indigo-900/90",
  mystery: "from-emerald-600/90 to-emerald-900/90",
  crime: "from-gray-600/90 to-gray-900/90",
  animation: "from-orange-400/90 to-orange-700/90"
};

// Optional: Add short tagline per genre
const genreTaglines = {
  action: "High-octane thrills and epic battles",
  comedy: "Laughs, jokes, and feel-good moments",
  drama: "Emotional stories that hit deep",
  horror: "Scares, suspense, and dark mysteries",
  "sci-fi": "Futuristic worlds and mind-bending adventures",
  romance: "Love stories that warm the heart",
  thriller: "Edge-of-your-seat tension and suspense",
  mystery: "Intriguing puzzles and secrets",
  crime: "Heists, investigations, and dark deeds",
  animation: "Fun, colorful, and imaginative worlds"
};

export default function Genres() {
  const [genres, setGenres] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("http://localhost:5135/api/Movies")
      .then(res => res.json())
      .then(movies => {
        const genreMap = {};

        movies.forEach(movie => {
          movie.genre.forEach(g => {
            const key = g.toLowerCase();
            genreMap[key] = (genreMap[key] || 0) + 1;
          });
        });

        setGenres(
          Object.entries(genreMap).map(([name, count]) => ({
            name,
            count
          }))
        );
      });
  }, []);

  return (
    <div className="min-h-screen px-6 py-12 bg-gradient-to-b from-black via-[#12001a] to-black text-white">
      
      {/* Header */}
      <div className="max-w-6xl mx-auto mb-12 text-center relative">
        <h1 className="text-4xl md:text-5xl font-extrabold mb-4 tracking-wide">
          Explore Movies by Genre
        </h1>
        <p className="text-lg md:text-xl text-gray-300 max-w-3xl mx-auto leading-relaxed">
          Browse curated genres and discover films that match your mood.
          From epic adventures to heartwarming comedies, every story awaits.
        </p>
        {/* Optional particle/glow effect */}
        <div className="absolute inset-0 -z-10 bg-[radial-gradient(circle,_rgba(255,0,255,0.1)_0%,_transparent_80%)] animate-pulse opacity-20"></div>
      </div>

      {/* Genre Grid */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8">
        {genres.map((genre, i) => (
          <button
            key={i}
            onClick={() => navigate(`/discover?genre=${genre.name}`)}
            className={`
              group relative rounded-2xl h-52 p-6 text-left
              bg-gradient-to-br ${genreColors[genre.name] || "from-fuchsia-600/90 to-purple-900/90"}
              shadow-xl transition-transform duration-300
              hover:scale-105 hover:shadow-2xl hover:-translate-y-1
              focus:outline-none
              overflow-hidden
            `}
          >
            {/* Overlay for readability */}
            <div className="absolute inset-0 rounded-2xl bg-black/30 backdrop-blur-sm transition-all group-hover:bg-black/20"></div>

            {/* Content */}
            <div className="relative z-10 flex flex-col justify-between h-full">
              
              {/* Genre Name */}
              <h2 className="text-3xl font-bold capitalize tracking-wide transition-colors group-hover:text-pink-400">
                {genre.name}
              </h2>

              {/* Tagline */}
              <p className="text-sm text-gray-200/80 mt-1 max-w-[90%]">
                {genreTaglines[genre.name] || "Discover amazing movies."}
              </p>

              {/* Bottom Row */}
              <div className="flex items-center justify-between mt-4">
                <span className="text-sm text-gray-200/90 font-semibold">
                  {genre.count} movies
                </span>
                <FaChevronRight className="opacity-0 translate-x-0 group-hover:opacity-100 group-hover:translate-x-1 transition-all duration-300 text-pink-400" />
              </div>
            </div>

            {/* Decorative glow effect */}
            <div className="absolute -inset-1 rounded-2xl bg-gradient-to-r from-pink-500 to-purple-600 opacity-30 blur-2xl animate-pulse pointer-events-none"></div>
          </button>
        ))}
      </div>

      {/* Footer */}
      <div className="max-w-4xl mx-auto mt-20 text-center text-gray-400">
        <p>
          More genres, smarter filters, and personalized recommendations
          coming soon — stay tuned!
        </p>
      </div>
    </div>
  );
}
