import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import API_URL from "../../config.js";

const MPAA_RATINGS = ["G", "PG", "PG-13", "R", "NC-17"];

export default function Filters() {

  const [genres, setGenres] = useState([]);
  const [selectedGenres, setSelectedGenres] = useState([]);
  const [genreOpen, setGenreOpen] = useState(false);
  const [actor, setActor] = useState("");
  const [selectedRatings, setSelectedRatings] = useState([]);
  const [includeTag, setIncludeTag] = useState("");
  const [excludeTag, setExcludeTag] = useState("");
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searched, setSearched] = useState(false);

  // Load genres for dropdown
  useEffect(() => {
    fetch(`${API_URL}/api/Genres`)
      .then(res => res.json())
      .then(data => setGenres(data))
      .catch(() => {});
  }, []);

  const toggleRating = (rating) => {
    setSelectedRatings(prev => prev.includes(rating) ? [] : [rating]);
  };

  const handleSearch = async () => {
    setLoading(true);
    setSearched(true);

    try {
      const normalizeTag = (tag) => {
        if (!tag) return [];
        const lower = tag.toLowerCase();
        if (lower === "violence") return ["gun violence", "violence"];
        if (lower === "gore") return ["blood/gore", "gore"];
        return [tag];
      };

      const body = {
        genreNames: selectedGenres.map(g => g.charAt(0).toUpperCase() + g.slice(1).toLowerCase()),
        personNames: actor.trim() ? [actor.trim()] : [],
        mpaaRatings: selectedRatings,
        includeWarningNames: normalizeTag(includeTag),
        excludeWarningNames: normalizeTag(excludeTag),
        take: 50
      };

      const res = await fetch(`${API_URL}/api/MovieSearch`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
      });

      const text = await res.text();
      let data;
      try {
        data = JSON.parse(text);
      } catch {
        throw new Error("Server not returning JSON (is backend running?)");
      }

      setMovies(data.results || []);
    } catch (err) {
      console.error("Error fetching movies:", err);
    }

    setLoading(false);
  };

  return (
    <div className="min-h-screen text-white p-10">

      {/* TITLE */}
      <h1 className="text-4xl font-bold text-center mb-10 text-pink-400">
        Advanced Movie Filters
      </h1>

      {/* FILTER PANEL */}
      <div className="bg-black/70 p-8 rounded-xl shadow-lg max-w-4xl mx-auto mb-8">

        <div className="grid md:grid-cols-2 gap-6 mb-6">

          {/* GENRE */}
          <div className="relative">
            <label className="block mb-2 text-pink-300 font-semibold">Genre</label>
            <button
              type="button"
              onClick={() => setGenreOpen(prev => !prev)}
              className="w-full p-3 text-left flex justify-between items-center transition-all duration-300"
              style={{ background: '#1a0033', border: `2px solid ${genreOpen ? '#ff39e1' : '#550088'}`, borderRadius: '6px', boxShadow: genreOpen ? '0 0 10px #ff39e1, 0 0 20px #ff6ed0' : 'none' }}
            >
              <span style={{ color: selectedGenres.length === 0 ? '#9ca3af' : 'white' }} className="truncate">
                {selectedGenres.length === 0 ? "All Genres" : selectedGenres.join(", ")}
              </span>
              <span style={{ color: '#9ca3af' }} className="ml-2">{genreOpen ? "▲" : "▼"}</span>
            </button>
            {genreOpen && (
              <div className="absolute z-10 w-full mt-1 max-h-56 overflow-y-auto" style={{ background: '#1a0033', border: '2px solid #550088', borderRadius: '6px' }}>
                {genres.map(g => (
                  <label key={g.tmdbGenreId} className="flex items-center gap-2 px-3 py-2 cursor-pointer text-sm capitalize text-white hover:bg-purple-900/50">
                    <input
                      type="checkbox"
                      value={g.name}
                      checked={selectedGenres.includes(g.name)}
                      onChange={(e) =>
                        setSelectedGenres(prev =>
                          e.target.checked ? [...prev, g.name] : prev.filter(x => x !== g.name)
                        )
                      }
                      className="accent-pink-500"
                    />
                    {g.name}
                  </label>
                ))}
              </div>
            )}
          </div>

          {/* ACTOR */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">Actor / Director</label>
            <input
              type="text"
              placeholder="e.g. Tom Hanks"
              className="w-full p-3 rounded-md text-black"
              value={actor}
              onChange={(e) => setActor(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && handleSearch()}
            />
          </div>

          {/* INCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">Include Keyword</label>
            <input
              type="text"
              placeholder="coming of age, bugs, gore..."
              className="w-full p-3 rounded-md text-black"
              value={includeTag}
              onChange={(e) => setIncludeTag(e.target.value)}
            />
          </div>

          {/* EXCLUDE TAG */}
          <div>
            <label className="block mb-2 text-pink-300 font-semibold">Exclude Keyword</label>
            <input
              type="text"
              placeholder="heist, drugs, death..."
              className="w-full p-3 rounded-md text-black"
              value={excludeTag}
              onChange={(e) => setExcludeTag(e.target.value)}
            />
          </div>

        </div>

        {/* MPAA RATINGS */}
        <div className="mb-6">
          <label className="block mb-2 text-pink-300 font-semibold">MPAA Rating</label>
          <div className="flex gap-3 flex-wrap">
            {MPAA_RATINGS.map(r => (
              <button
                key={r}
                onClick={() => toggleRating(r)}
                className={`px-4 py-2 rounded-full font-semibold border transition ${
                  selectedRatings.includes(r)
                    ? "bg-pink-500 border-pink-500 text-white"
                    : "border-gray-500 text-gray-300 hover:border-pink-400"
                }`}
              >
                {r}
              </button>
            ))}
          </div>
        </div>

        {/* SEARCH BUTTON */}
        <div className="text-center">
          <button
            onClick={handleSearch}
            className="px-8 py-3 rounded-full font-bold bg-pink-500 hover:bg-pink-600 transition text-white shadow-lg"
          >
            Search
          </button>
        </div>

      </div>

      {/* RESULTS */}
      {searched && (
        <>
          <h2 className="text-2xl font-bold mb-6 text-center text-pink-300">
            Filter Results
          </h2>

          {loading ? (
            <p className="text-center text-gray-300">Loading...</p>
          ) : movies.length === 0 ? (
            <p className="text-center text-gray-300">No movies match your filters.</p>
          ) : (
            <div className="grid md:grid-cols-3 lg:grid-cols-4 gap-8">
              {movies.map((movie, index) => {
                const hasImdb = !!movie.imdbId;
                return hasImdb ? (
                  <Link key={index} to={`/movie/${movie.imdbId}`}>
                    <div className="bg-black/60 p-6 rounded-xl hover:bg-pink-500/20 transition hover:scale-105 shadow-lg cursor-pointer">
                      {movie.posterUrl ? (
                        <img
                          src={movie.posterUrl}
                          alt={movie.title}
                          className="w-full h-64 object-cover rounded-md mb-4"
                        />
                      ) : (
                        <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">
                          No Image
                        </div>
                      )}
                      <h3 className="text-xl font-semibold mb-1">{movie.title}</h3>
                      <p className="text-sm text-gray-300">{movie.releaseYear || "N/A"}</p>
                      {movie.mpaaRating && (
                        <span className="text-xs border border-gray-500 px-2 py-0.5 rounded mt-1 inline-block text-gray-400">
                          {movie.mpaaRating}
                        </span>
                      )}
                    </div>
                  </Link>
                ) : (
                  <div key={index} className="bg-gray-700 p-6 rounded-xl opacity-60 cursor-not-allowed">
                    {movie.posterUrl ? (
                      <img src={movie.posterUrl} alt={movie.title} className="w-full h-64 object-cover rounded-md mb-4" />
                    ) : (
                      <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-md mb-4">No Image</div>
                    )}
                    <h3 className="text-xl font-semibold mb-1">{movie.title}</h3>
                    <p className="text-sm text-gray-300">{movie.releaseYear || "N/A"}</p>
                    <p className="text-xs text-red-400 mt-2">No details available</p>
                  </div>
                );
              })}
            </div>
          )}
        </>
      )}

    </div>
  );
}
