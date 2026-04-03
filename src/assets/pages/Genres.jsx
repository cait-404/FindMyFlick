import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaChevronRight } from "react-icons/fa";
import API_URL from "../../config.js";

// Genre taglines and navigation updated with Claude (April 2026)

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
  animation: "from-orange-400/90 to-orange-700/90",
  adventure: "from-green-600/90 to-green-900/90",
  documentary: "from-sky-600/90 to-sky-900/90",
  family: "from-teal-500/90 to-teal-800/90",
  fantasy: "from-violet-600/90 to-violet-900/90",
  history: "from-amber-600/90 to-amber-900/90",
  music: "from-fuchsia-500/90 to-fuchsia-900/90",
  "science fiction": "from-blue-500/90 to-indigo-900/90",
  "tv movie": "from-slate-600/90 to-slate-900/90",
  war: "from-stone-600/90 to-stone-900/90",
  western: "from-orange-700/90 to-orange-950/90"
};

const genreTaglines = {
  action: "High-octane thrills and epic battles",
  adventure: "Bold journeys into the unknown",
  animation: "Fun, colorful, and imaginative worlds",
  comedy: "Laughs, jokes, and feel-good moments",
  crime: "Heists, investigations, and dark deeds",
  documentary: "Real stories that inform and inspire",
  drama: "Emotional stories that hit deep",
  family: "Something for everyone to enjoy together",
  fantasy: "Magic, myth, and worlds beyond imagination",
  history: "True events that shaped the world",
  horror: "Scares, suspense, and dark mysteries",
  music: "Rhythm, performance, and the power of song",
  mystery: "Intriguing puzzles and secrets to unravel",
  romance: "Love stories that warm the heart",
  "science fiction": "Futuristic worlds and mind-bending adventures",
  thriller: "Edge-of-your-seat tension and suspense",
  "tv movie": "Feature-length stories made for the screen",
  war: "Courage, sacrifice, and the cost of conflict",
  western: "Outlaws, frontiers, and the wild west"
};

export default function Genres() {
  const [genres, setGenres] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetch(`${API_URL}/api/Genres`)
      .then(res => res.json())
      .then(data => setGenres(data.map(g => ({ name: g.name?.toLowerCase(), count: null }))))
      .catch(() => {})
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <p className="text-center mt-20 text-gray-400">
        Loading genres...
      </p>
    );
  }

  return (
    <div className="min-h-screen px-6 py-12 bg-linear-to-b from-black via-[#12001a] to-black text-white">

      {/* Header */}
      <div className="max-w-6xl mx-auto mb-12 text-center relative">

        <h1 className="text-4xl md:text-5xl font-extrabold mb-4 tracking-wide">
          Explore Movies by Genre
        </h1>

        <p className="text-lg md:text-xl text-gray-300 max-w-3xl mx-auto leading-relaxed">
          Browse curated genres and discover films that match your mood.
          From epic adventures to heartwarming comedies, every story awaits.
        </p>

        <div className="absolute inset-0 -z-10 bg-[radial-gradient(circle,rgba(255,0,255,0.1)_0%,transparent_80%)] animate-pulse opacity-20"></div>

      </div>

      {/* Empty State */}
      {genres.length === 0 && (
        <p className="text-center text-gray-400 mt-20">
          No genres found.
        </p>
      )}

      {/* Genre Cards */}
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8">

        {genres.map((genre, i) => (
          <button
            key={i}
            onClick={() => navigate(`/genre/${encodeURIComponent(genre.name)}`)}
            className={`
              group relative rounded-2xl h-52 p-6 text-left
              bg-linear-to-br ${genreColors[genre.name] || "from-fuchsia-600/90 to-purple-900/90"}
              shadow-xl transition-transform duration-300
              hover:scale-105 hover:shadow-2xl hover:-translate-y-1
              focus:outline-none overflow-hidden
            `}
          >

            {/* Card Overlay */}
            <div className="absolute inset-0 rounded-2xl bg-black/30 backdrop-blur-sm transition-all group-hover:bg-black/20"></div>

            {/* Card Content */}
            <div className="relative z-10 flex flex-col justify-between h-full">

              <h2 className="text-3xl font-bold capitalize tracking-wide transition-colors group-hover:text-pink-400">
                {genre.name}
              </h2>

              <p className="text-sm text-gray-200/80 mt-1 max-w-[90%]">
                {genreTaglines[genre.name] || "Discover amazing movies."}
              </p>

              <div className="flex items-center justify-between mt-4">

                {genre.count !== null && (
                  <span className="text-sm text-gray-200/90 font-semibold">
                    {genre.count} movies
                  </span>
                )}

                <FaChevronRight className="opacity-0 translate-x-0 group-hover:opacity-100 group-hover:translate-x-1 transition-all duration-300 text-pink-400" />

              </div>

            </div>

            {/* Glow Effect */}
            <div className="absolute -inset-1 rounded-2xl bg-linear-to-r from-pink-500 to-purple-600 opacity-30 blur-2xl animate-pulse pointer-events-none"></div>

          </button>
        ))}

      </div>

      {/* Footer Message */}
      <div className="max-w-4xl mx-auto mt-20 text-center text-gray-400">
        <p>
          More genres, smarter filters, and personalized recommendations coming soon — stay tuned!
        </p>
      </div>

    </div>
  );
}