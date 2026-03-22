import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaChevronRight } from "react-icons/fa";
import { useMovies } from "../../context/MovieContext";

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
  const { movies, loading } = useMovies();
  const [genres, setGenres] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!movies.length) return;

    const genreMap = {};

    movies.forEach((movie) => {
      const movieGenres = movie.genre || [];

      movieGenres.forEach((g) => {
        if (g) {
          const key = g.toLowerCase();
          genreMap[key] = (genreMap[key] || 0) + 1;
        }
      });
    });

    setGenres(
      Object.entries(genreMap)
        .sort((a, b) => b[1] - a[1])
        .map(([name, count]) => ({ name, count }))
    );
  }, [movies]);

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
            onClick={() => navigate(`/discover?genre=${genre.name}`)}
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

                <span className="text-sm text-gray-200/90 font-semibold">
                  {genre.count} movies
                </span>

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