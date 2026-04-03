import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import API_URL from "../../config.js";

// Advanced Search page — rebuilt with Claude (April 2026)
// Supports genre, streaming, cast, crew, MPAA rating, content warnings, and plot tags
// Warning taxonomy uses category/subcategory structure from /api/WarningTaxonomy

const MPAA_RATINGS = ["G", "PG", "PG-13", "R", "NC-17"];

const MAJOR_PROVIDERS = [
  { name: "Netflix", id: 8 },
  { name: "Hulu", id: 15 },
  { name: "Disney Plus", id: 337 },
  { name: "Amazon Prime Video", id: 9 },
  { name: "HBO Max", id: 1899 },
  { name: "Paramount+", id: 2303 },
  { name: "Apple TV", id: 350 },
  { name: "Peacock Premium", id: 386 },
  { name: "Starz", id: 43 },
  { name: "MGM Plus", id: 34 },
  { name: "Tubi TV", id: 73 },
  { name: "Pluto TV", id: 300 },
  { name: "Shudder", id: 99 },
  { name: "Criterion Channel", id: 258 },
  { name: "BritBox", id: 151 },
  { name: "AMC+", id: 526 },
  { name: "Fandango At Home", id: 7 },
  { name: "Plex", id: 538 },
];

// Collapsible section wrapper — AO3-style
function CollapsibleSection({ title, children, defaultOpen = false }) {
  const [open, setOpen] = useState(defaultOpen);
  return (
    <div className="border border-purple-800 rounded-lg overflow-hidden mb-4">
      <button
        onClick={() => setOpen(o => !o)}
        className="w-full flex justify-between items-center px-4 py-3 bg-purple-900/40 hover:bg-purple-900/60 transition text-left"
      >
        <span className="font-semibold text-pink-300">{title}</span>
        <span className="text-gray-400">{open ? "▲" : "▼"}</span>
      </button>
      {open && <div className="px-4 py-4 bg-black/40">{children}</div>}
    </div>
  );
}

// Include/Exclude toggle for a single warning topic
function WarningTopicRow({ topic, includeIds, excludeIds, onInclude, onExclude }) {
  const included = includeIds.has(topic.dtddTopicId);
  const excluded = excludeIds.has(topic.dtddTopicId);

  return (
    <div className="flex items-center justify-between py-1 border-b border-purple-900/30 last:border-0">
      <span className="text-sm text-gray-300 flex-1">{topic.topicName}</span>
      <div className="flex gap-2 ml-2">
        <button
          onClick={() => onInclude(topic.dtddTopicId)}
          className={`px-2 py-0.5 rounded text-xs font-semibold transition ${
            included
              ? "bg-green-600 text-white"
              : "border border-green-600 text-green-400 hover:bg-green-600/20"
          }`}
        >
          Include
        </button>
        <button
          onClick={() => onExclude(topic.dtddTopicId)}
          className={`px-2 py-0.5 rounded text-xs font-semibold transition ${
            excluded
              ? "bg-red-600 text-white"
              : "border border-red-600 text-red-400 hover:bg-red-600/20"
          }`}
        >
          Exclude
        </button>
      </div>
    </div>
  );
}

export default function Filters() {
  const [genres, setGenres] = useState([]);
  const [selectedGenres, setSelectedGenres] = useState([]);
  const [genreOpen, setGenreOpen] = useState(false);

  const [selectedProviders, setSelectedProviders] = useState([]);
  const [otherProvider, setOtherProvider] = useState("");

  const [includeCast, setIncludeCast] = useState("");
  const [includeCrew, setIncludeCrew] = useState("");
 
  const [selectedRatings, setSelectedRatings] = useState([]);

  const [taxonomy, setTaxonomy] = useState([]);
  const [includeWarningIds, setIncludeWarningIds] = useState(new Set());
  const [excludeWarningIds, setExcludeWarningIds] = useState(new Set());

  const [includePlotTag, setIncludePlotTag] = useState("");
  const [excludePlotTag, setExcludePlotTag] = useState("");

  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searched, setSearched] = useState(false);
  const [error, setError] = useState(null);

  // Load genres
  useEffect(() => {
    fetch(`${API_URL}/api/Genres`)
      .then(res => res.json())
      .then(data => setGenres(data))
      .catch(() => {});
  }, []);

  // Load warning taxonomy
  useEffect(() => {
    fetch(`${API_URL}/api/WarningTaxonomy`)
      .then(res => res.json())
      .then(data => setTaxonomy(data))
      .catch(() => {});
  }, []);

  const toggleRating = (rating) => {
    setSelectedRatings(prev =>
      prev.includes(rating) ? prev.filter(r => r !== rating) : [...prev, rating]
    );
  };

  const toggleProvider = (id) => {
    setSelectedProviders(prev =>
      prev.includes(id) ? prev.filter(p => p !== id) : [...prev, id]
    );
  };

  const toggleIncludeWarning = (id) => {
    setIncludeWarningIds(prev => {
      const next = new Set(prev);
      if (next.has(id)) { next.delete(id); return next; }
      const excl = new Set(excludeWarningIds);
      excl.delete(id);
      setExcludeWarningIds(excl);
      next.add(id);
      return next;
    });
  };

  const toggleExcludeWarning = (id) => {
    setExcludeWarningIds(prev => {
      const next = new Set(prev);
      if (next.has(id)) { next.delete(id); return next; }
      const incl = new Set(includeWarningIds);
      incl.delete(id);
      setIncludeWarningIds(incl);
      next.add(id);
      return next;
    });
  };

  const handleSearch = async () => {
    setLoading(true);
    setSearched(true);
    setError(null);

    try {
      // Combine selected major providers with any typed "other" provider
      const streamingProviderNames = [];
      if (otherProvider.trim()) streamingProviderNames.push(otherProvider.trim());

      // Build person names arrays
      const personNames = [];
      if (includeCast.trim()) personNames.push(includeCast.trim());
      if (includeCrew.trim()) personNames.push(includeCrew.trim());

      const body = {
        genreNames: selectedGenres,
        streamingProviderIds: selectedProviders,
        streamingProviderNames,
        personNames,
        mpaaRatings: selectedRatings,
        includeWarningTopicIds: [...includeWarningIds],
        excludeWarningTopicIds: [...excludeWarningIds],
        tagNamesInclude: includePlotTag.trim() ? [includePlotTag.trim()] : [],
        tagNamesExclude: excludePlotTag.trim() ? [excludePlotTag.trim()] : [],
        take: 50,
        enableApiFallback: false
      };

      const res = await fetch(`${API_URL}/api/MovieSearch`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
      });

      if (!res.ok) throw new Error("Search failed");

      const data = await res.json();
      setMovies(data.results || []);
    } catch (err) {
      console.error("Search error:", err);
      setError("Search failed. Please try again.");
    }

    setLoading(false);
  };

  const handleClear = () => {
    setSelectedGenres([]);
    setSelectedProviders([]);
    setOtherProvider("");
    setIncludeCast("");
    setExcludeCast("");
    setIncludeCrew("");
    setExcludeCrew("");
    setSelectedRatings([]);
    setIncludeWarningIds(new Set());
    setExcludeWarningIds(new Set());
    setIncludePlotTag("");
    setExcludePlotTag("");
    setMovies([]);
    setSearched(false);
    setError(null);
  };

  const activeWarningCount = includeWarningIds.size + excludeWarningIds.size;

  return (
    <div className="min-h-screen text-white p-6 max-w-5xl mx-auto">

      {/* TITLE */}
      <h1 className="text-4xl font-bold text-center mb-2 text-pink-400">
        Advanced Movie Search
      </h1>
      <p className="text-center text-gray-400 mb-8 text-sm">
        Filter by genre, streaming service, cast, crew, ratings, content warnings, and plot tags.
      </p>

      {/* FILTER PANEL */}
      <div className="bg-black/70 p-6 rounded-xl shadow-lg mb-8">

        {/* GENRE */}
        <CollapsibleSection title="Genre" defaultOpen={true}>
          <div className="flex flex-wrap gap-2">
            {genres.map(g => (
              <button
                key={g.tmdbGenreId}
                onClick={() =>
                  setSelectedGenres(prev =>
                    prev.includes(g.name) ? prev.filter(x => x !== g.name) : [...prev, g.name]
                  )
                }
                className={`px-3 py-1.5 rounded-full text-sm font-semibold border transition capitalize ${
                  selectedGenres.includes(g.name)
                    ? "bg-pink-500 border-pink-500 text-white"
                    : "border-gray-500 text-gray-300 hover:border-pink-400"
                }`}
              >
                {g.name}
              </button>
            ))}
          </div>
        </CollapsibleSection>

        {/* STREAMING SERVICE */}
        <CollapsibleSection title="Streaming Service">
          <div className="flex flex-wrap gap-2 mb-3">
            {MAJOR_PROVIDERS.map(p => (
              <button
                key={p.id}
                onClick={() => toggleProvider(p.id)}
                className={`px-3 py-1.5 rounded-full text-sm font-semibold border transition ${
                  selectedProviders.includes(p.id)
                    ? "bg-pink-500 border-pink-500 text-white"
                    : "border-gray-500 text-gray-300 hover:border-pink-400"
                }`}
              >
                {p.name}
              </button>
            ))}
          </div>
          <div className="flex gap-2 items-center mt-2">
            <span className="text-sm text-gray-400 whitespace-nowrap">Other:</span>
            <input
              type="text"
              placeholder="Type a streaming service name..."
              className="flex-1 p-2 rounded-md text-black text-sm"
              value={otherProvider}
              onChange={(e) => setOtherProvider(e.target.value)}
            />
          </div>
        </CollapsibleSection>

        {/* CAST & CREW */}
        <CollapsibleSection title="Cast & Crew">
          <div className="grid md:grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm text-green-400 font-semibold">Include Cast Member</label>
              <input
                type="text"
                placeholder="e.g. Tom Hanks"
                className="w-full p-2 rounded-md text-black text-sm"
                value={includeCast}
                onChange={(e) => setIncludeCast(e.target.value)}
              />
            </div>
            <div>
              <label className="block mb-1 text-sm text-green-400 font-semibold">Include Crew Member</label>
              <input
                type="text"
                placeholder="e.g. Christopher Nolan"
                className="w-full p-2 rounded-md text-black text-sm"
                value={includeCrew}
                onChange={(e) => setIncludeCrew(e.target.value)}
              />
            </div>
            
          </div>
        </CollapsibleSection>

        {/* MPAA RATING */}
        <CollapsibleSection title="MPAA Rating">
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
          <p className="text-xs text-gray-400 mt-2">Select multiple ratings to include all of them.</p>
        </CollapsibleSection>

        {/* CONTENT WARNINGS */}
        <CollapsibleSection title={`Content Warnings${activeWarningCount > 0 ? ` (${activeWarningCount} selected)` : ""}`}>
          <p className="text-xs text-gray-400 mb-4">
            Use <span className="text-green-400 font-semibold">Include</span> to require a warning be present,
            or <span className="text-red-400 font-semibold">Exclude</span> to filter out movies containing it.
          </p>
          {taxonomy.map(category => (
            <CollapsibleSection key={category.categoryId} title={category.categoryName}>
              {category.subcategories.map(sub => (
                <div key={sub.subcategoryId} className="mb-4">
                  <h4 className="text-xs font-semibold text-purple-300 uppercase tracking-wide mb-2">
                    {sub.subcategoryName}
                  </h4>
                  {/* Deduplicate topics within subcategory */}
                  {[...new Map(sub.topics.map(t => [t.dtddTopicId, t])).values()].map(topic => (
                    <WarningTopicRow
                      key={topic.dtddTopicId}
                      topic={topic}
                      includeIds={includeWarningIds}
                      excludeIds={excludeWarningIds}
                      onInclude={toggleIncludeWarning}
                      onExclude={toggleExcludeWarning}
                    />
                  ))}
                </div>
              ))}
            </CollapsibleSection>
          ))}
        </CollapsibleSection>

        {/* PLOT TAGS */}
        <CollapsibleSection title="Plot Tags">
          <div className="grid md:grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm text-green-400 font-semibold">Include Plot Tag</label>
              <input
                type="text"
                placeholder="e.g. coming of age, heist..."
                className="w-full p-2 rounded-md text-black text-sm"
                value={includePlotTag}
                onChange={(e) => setIncludePlotTag(e.target.value)}
              />
            </div>
            <div>
              <label className="block mb-1 text-sm text-red-400 font-semibold">Exclude Plot Tag</label>
              <input
                type="text"
                placeholder="e.g. drugs, death..."
                className="w-full p-2 rounded-md text-black text-sm"
                value={excludePlotTag}
                onChange={(e) => setExcludePlotTag(e.target.value)}
              />
            </div>
          </div>
        </CollapsibleSection>

        {/* BUTTONS */}
        <div className="flex justify-center gap-4 mt-6">
          <button
            onClick={handleSearch}
            className="px-8 py-3 rounded-full font-bold bg-pink-500 hover:bg-pink-600 transition text-white shadow-lg"
          >
            Search
          </button>
          <button
            onClick={handleClear}
            className="px-8 py-3 rounded-full font-bold border border-gray-500 text-gray-300 hover:border-pink-400 hover:text-pink-400 transition"
          >
            Clear Filters
          </button>
        </div>

      </div>

      {/* RESULTS */}
      {searched && (
        <>
          <h2 className="text-2xl font-bold mb-6 text-center text-pink-300">
            Search Results
          </h2>

          {loading ? (
            <p className="text-center text-gray-300">Searching...</p>
          ) : error ? (
            <p className="text-center text-red-400">{error}</p>
          ) : movies.length === 0 ? (
            <p className="text-center text-gray-300">No movies match your filters. Try broadening your search.</p>
          ) : (
            <>
              <p className="text-center text-gray-400 text-sm mb-6">{movies.length} movies found</p>
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
                {movies.map((movie, index) => (
                  <Link
                    key={index}
                    to={`/movie/${movie.imdbId}`}
                    className="flex flex-col rounded-lg overflow-hidden bg-gray-900/80 hover:scale-105 transform transition duration-200 shadow-lg"
                  >
                    {movie.posterUrl ? (
                      <img
                        src={movie.posterUrl}
                        alt={movie.title}
                        className="w-full object-contain"
                      />
                    ) : (
                      <div className="w-full h-48 bg-gray-800 flex items-center justify-center text-gray-400 text-sm">
                        No Image
                      </div>
                    )}
                    <div className="p-2 flex flex-col gap-1">
                      <h3 className="font-semibold text-sm text-white leading-snug">{movie.title}</h3>
                      <p className="text-gray-400 text-xs">{movie.releaseYear || "N/A"}</p>
                      {movie.mpaaRating && (
                        <span className="text-xs border border-gray-500 px-1.5 py-0.5 rounded text-gray-400 w-fit">
                          {movie.mpaaRating}
                        </span>
                      )}
                    </div>
                  </Link>
                ))}
              </div>
            </>
          )}
        </>
      )}

    </div>
  );
}