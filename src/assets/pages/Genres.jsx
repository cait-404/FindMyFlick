import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaChevronRight } from "react-icons/fa";
 
const genreColors = {
  action: "from-red-600/90 to-red-900/90",
  adventure: "from-amber-500/90 to-orange-800/90",
  animation: "from-orange-400/90 to-orange-700/90",
  comedy: "from-yellow-400/90 to-yellow-700/90",
  crime: "from-gray-600/90 to-gray-900/90",
  documentary: "from-green-600/90 to-green-900/90",
  drama: "from-purple-600/90 to-purple-900/90",
  family: "from-sky-400/90 to-sky-700/90",
  fantasy: "from-violet-500/90 to-violet-900/90",
  history: "from-yellow-700/90 to-yellow-900/90",
  horror: "from-zinc-900/90 to-black",
  music: "from-pink-400/90 to-pink-700/90",
  mystery: "from-emerald-600/90 to-emerald-900/90",
  romance: "from-pink-500/90 to-rose-800/90",
  "science fiction": "from-blue-500/90 to-indigo-900/90",
  thriller: "from-indigo-600/90 to-indigo-900/90",
  "tv movie": "from-cyan-600/90 to-cyan-900/90",
  war: "from-stone-600/90 to-stone-900/90",
  western: "from-orange-700/90 to-red-900/90",
};
 
const genreTaglines = {
  action: "High-octane thrills and epic battles",
  adventure: "Bold journeys and daring exploits",
  animation: "Fun, colorful, and imaginative worlds",
  comedy: "Laughs, jokes, and feel-good moments",
  crime: "Heists, investigations, and dark deeds",
  documentary: "Real stories that inspire and inform",
  drama: "Emotional stories that hit deep",
  family: "Stories for everyone to enjoy together",
  fantasy: "Magic, myth, and otherworldly adventures",
  history: "Epic tales from the pages of history",
  horror: "Scares, suspense, and dark mysteries",
  music: "Rhythm, passion, and musical journeys",
  mystery: "Intriguing puzzles and hidden secrets",
  romance: "Love stories that warm the heart",
  "science fiction": "Futuristic worlds and mind-bending adventures",
  thriller: "Edge-of-your-seat tension and suspense",
  "tv movie": "Made-for-screen stories worth watching",
  war: "Courage, conflict, and human resilience",
  western: "Outlaws, deserts, and frontier justice",
};
 
export default function Genres() {
  const [genres, setGenres] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
 
  useEffect(() => {
    fetch("https://localhost:5002/api/genres")
      .then((r) => {
        if (!r.ok) throw new Error("Failed to fetch genres");
        return r.json();
      })
      .then((data) => {
        // data is [{tmdbGenreId, name}, ...]
        setGenres(data.map((g) => ({ name: g.name, tmdbGenreId: g.tmdbGenreId })));
      })
      .catch((err) => console.error("Failed to load genres:", err))
      .finally(() => setLoading(false));
  }, []);
 
  if (loading) {
    return <p className="text-center mt-20 text-gray-400">Loading genres...</p>;
  }
 
  return (
<div className="min-h-screen px-6 py-12 bg-linear-to-b from-black via-[#12001a] to-black text-white">
 
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
 
      {genres.length === 0 && (
<p className="text-center text-gray-400 mt-20">No genres found.</p>
      )}
 
      <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8">
        {genres.map((genre) => {
          const key = genre.name.toLowerCase();
          return (
<button
              key={genre.tmdbGenreId}
              onClick={() => navigate(`/discover?genre=${genre.name}`)}
              className={`
                group relative rounded-2xl h-52 p-6 text-left
                bg-linear-to-br ${genreColors[key] || "from-fuchsia-600/90 to-purple-900/90"}
                shadow-xl transition-transform duration-300
                hover:scale-105 hover:shadow-2xl hover:-translate-y-1
                focus:outline-none overflow-hidden
              `}
>
<div className="absolute inset-0 rounded-2xl bg-black/30 backdrop-blur-sm transition-all group-hover:bg-black/20"></div>
<div className="relative z-10 flex flex-col justify-between h-full">
<h2 className="text-3xl font-bold capitalize tracking-wide transition-colors group-hover:text-pink-400">
                  {genre.name}
</h2>
<p className="text-sm text-gray-200/80 mt-1 max-w-[90%]">
                  {genreTaglines[key] || "Discover amazing movies."}
</p>
<div className="flex items-center justify-between mt-4">
<FaChevronRight className="opacity-0 group-hover:opacity-100 group-hover:translate-x-1 transition-all duration-300 text-pink-400" />
</div>
</div>
<div className="absolute -inset-1 rounded-2xl bg-linear-to-r from-pink-500 to-purple-600 opacity-30 blur-2xl animate-pulse pointer-events-none"></div>
</button>
          );
        })}
</div>
 
      <div className="max-w-4xl mx-auto mt-20 text-center text-gray-400">
<p>More genres, smarter filters, and personalized recommendations coming soon!</p>
</div>
</div>
  );
}