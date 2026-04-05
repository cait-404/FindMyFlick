import { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import API_URL from "../../config.js";

// Advanced Search page — rebuilt with Claude (April 2026)
// Supports genre, streaming, cast, crew, MPAA rating, content warnings, and plot tags
// Plot tag and streaming autocomplete added with Claude (April 2026)

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
function CollapsibleSection({ title, children, defaultOpen = false, headerExtra = null }) {
  const [open, setOpen] = useState(defaultOpen);
  return (
    <div className="border border-purple-800 rounded-lg overflow-visible mb-4">
      <div className="w-full flex justify-between items-center px-4 py-3 bg-purple-900/40 hover:bg-purple-900/60 transition">
        <button
          onClick={() => setOpen(o => !o)}
          className="flex-1 text-left"
        >
          <span className="font-semibold text-pink-300">{title}</span>
        </button>
        {headerExtra}
        <button onClick={() => setOpen(o => !o)}>
          <span className="text-gray-400">{open ? "▲" : "▼"}</span>
        </button>
      </div>
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
            included ? "bg-green-600 text-white" : "border border-green-600 text-green-400 hover:bg-green-600/20"
          }`}
        >
          Include
        </button>
        <button
          onClick={() => onExclude(topic.dtddTopicId)}
          className={`px-2 py-0.5 rounded text-xs font-semibold transition ${
            excluded ? "bg-red-600 text-white" : "border border-red-600 text-red-400 hover:bg-red-600/20"
          }`}
        >
          Exclude
        </button>
      </div>
    </div>
  );
}

// Reusable autocomplete tag input
function TagAutocomplete({ label, color, allOptions, selectedTags, onAdd, onRemove, placeholder }) {
  const [query, setQuery] = useState("");
  const [showSuggestions, setShowSuggestions] = useState(false);
  const inputRef = useRef(null);

  const filtered = query.trim().length < 1 ? [] : allOptions.filter(opt =>
    opt.toLowerCase().includes(query.toLowerCase()) &&
    !selectedTags.includes(opt)
  ).slice(0, 8);

  const handleSelect = (opt) => {
    onAdd(opt);
    setQuery("");
    setShowSuggestions(false);
    inputRef.current?.focus();
  };

  return (
    <div>
      <label className={`block mb-1 text-sm font-semibold ${color}`}>{label}</label>
      <div className="relative">
        <input
          ref={inputRef}
          type="text"
          placeholder={placeholder}
          className="w-full p-2 rounded-md text-black text-sm"
          value={query}
          onChange={(e) => { setQuery(e.target.value); setShowSuggestions(true); }}
          onFocus={() => setShowSuggestions(true)}
          onBlur={() => setTimeout(() => setShowSuggestions(false), 150)}
        />
        {showSuggestions && filtered.length > 0 && (
          <div className="absolute z-20 w-full mt-1 rounded-md shadow-lg max-h-48 overflow-y-auto"
            style={{ background: '#1a0033', border: '2px solid #550088' }}>
            {filtered.map(opt => (
              <button
                key={opt}
                onMouseDown={() => handleSelect(opt)}
                className="w-full text-left px-3 py-2 text-sm text-white hover:bg-purple-900/50"
              >
                {opt}
              </button>
            ))}
          </div>
        )}
      </div>
      {/* Selected tags as chips */}
      {selectedTags.length > 0 && (
        <div className="flex flex-wrap gap-2 mt-2">
          {selectedTags.map(tag => (
            <span key={tag}
              className="flex items-center gap-1 px-2 py-1 rounded-full text-xs font-semibold bg-purple-800 text-white">
              {tag}
              <button onClick={() => onRemove(tag)} className="ml-1 text-gray-300 hover:text-white">×</button>
            </span>
          ))}
        </div>
      )}
    </div>
  );
}

export default function Filters() {
  const [genres, setGenres] = useState([]);
  const [selectedGenres, setSelectedGenres] = useState([]);

  const [selectedProviders, setSelectedProviders] = useState([]);
  const [allProviders, setAllProviders] = useState([]);
  const [otherProviderIds, setOtherProviderIds] = useState([]);

  const [includeCast, setIncludeCast] = useState("");
  const [includeCrew, setIncludeCrew] = useState("");

  const [selectedRatings, setSelectedRatings] = useState([]);

  const [taxonomy, setTaxonomy] = useState([]);
  const [includeWarningIds, setIncludeWarningIds] = useState(new Set());
  const [excludeWarningIds, setExcludeWarningIds] = useState(new Set());
  const [includeCategoryIds, setIncludeCategoryIds] = useState(new Set());
  const [excludeCategoryIds, setExcludeCategoryIds] = useState(new Set());

  const [allPlotTags, setAllPlotTags] = useState([]);
  const [includePlotTags, setIncludePlotTags] = useState([]);
  const [excludePlotTags, setExcludePlotTags] = useState([]);

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

  // Load all plot tags for autocomplete
  useEffect(() => {
    fetch(`${API_URL}/api/movies/plot-tags/getall`)
      .then(res => res.json())
      .then(data => setAllPlotTags(data.map(t => t.tagText)))
      .catch(() => {});
  }, []);

  // Load all streaming providers for autocomplete
  useEffect(() => {
    fetch(`${API_URL}/api/Genres`) // placeholder — we'll use hardcoded for now
      .catch(() => {});
    // Build full provider list from hardcoded + any extras we know about
    setAllProviders(MAJOR_PROVIDERS.map(p => p.name));
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

  const toggleIncludeCategory = (id) => {
    setIncludeCategoryIds(prev => {
      const next = new Set(prev);
      if (next.has(id)) { next.delete(id); return next; }
      const excl = new Set(excludeCategoryIds);
      excl.delete(id);
      setExcludeCategoryIds(excl);
      next.add(id);
      return next;
    });
  };

  const toggleExcludeCategory = (id) => {
    setExcludeCategoryIds(prev => {
      const next = new Set(prev);
      if (next.has(id)) { next.delete(id); return next; }
      const incl = new Set(includeCategoryIds);
      incl.delete(id);
      setIncludeCategoryIds(incl);
      next.add(id);
      return next;
    });
  };

  const handleSearch = async () => {
    setLoading(true);
    setSearched(true);
    setError(null);

    try {
      const streamingProviderNames = [];
      // Add any "other" provider names selected via autocomplete
      otherProviderIds.forEach(name => streamingProviderNames.push(name));

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
        includeWarningCategoryIds: [...includeCategoryIds],
        excludeWarningCategoryIds: [...excludeCategoryIds],
        tagNamesInclude: includePlotTags,
        tagNamesExclude: excludePlotTags,
        take: 50,
        enableApiFallback: true,
        minMatches: 10,
        maxApiAdds: 5
      };

      const res = await fetch(`${API_URL}/api/MovieSearch`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
      });

      if (!res.ok) throw new Error("Search failed");

       const data = await res.json();
      
      // If person names were specified but none resolved, show no results
      // rather than returning 50 unrelated movies
      if (personNames.length > 0 && data.unresolvedNames?.length === personNames.length) {
        setMovies([]);
        setError(`Could not find "${personNames.join(", ")}" in our database. Please check your spelling.`);
        setLoading(false);
        return;
      }
      
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
    setOtherProviderIds([]);
    setIncludeCast("");
    setIncludeCrew("");
    setSelectedRatings([]);
    setIncludeWarningIds(new Set());
    setExcludeWarningIds(new Set());
    setIncludeCategoryIds(new Set());
    setExcludeCategoryIds(new Set());
    setIncludePlotTags([]);
    setExcludePlotTags([]);
    setMovies([]);
    setSearched(false);
    setError(null);
  };

  const activeWarningCount = includeWarningIds.size + excludeWarningIds.size + includeCategoryIds.size + excludeCategoryIds.size;

  // Provider names not in the major list for the "other" autocomplete
  const otherProviderOptions = MAJOR_PROVIDERS.map(p => p.name);

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
          {/* Other provider autocomplete */}
          <TagAutocomplete
            label="Other Streaming Service"
            color="text-pink-300"
            allOptions={otherProviderOptions}
            selectedTags={otherProviderIds}
            onAdd={(name) => setOtherProviderIds(prev => [...prev, name])}
            onRemove={(name) => setOtherProviderIds(prev => prev.filter(p => p !== name))}
            placeholder="Search for another streaming service..."
          />
        </CollapsibleSection>

        {/* CAST & CREW */}
        <CollapsibleSection title="Cast & Crew (Include)">
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
              <label className="block mb-1 text-sm text-green-400 font-semibold">Include Crew Member (Director, Writer, etc.)</label>
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
            <CollapsibleSection
              key={category.categoryId}
              title={category.categoryName}
              headerExtra={
                <div className="flex gap-2 mr-2" onClick={e => e.stopPropagation()}>
                  
                  <button
                    onClick={() => toggleExcludeCategory(category.categoryId)}
                    className={`px-2 py-0.5 rounded text-xs font-semibold transition ${
                      excludeCategoryIds.has(category.categoryId)
                        ? "bg-red-600 text-white"
                        : "border border-red-600 text-red-400 hover:bg-red-600/20"
                    }`}
                  >
                    Exclude All
                  </button>
                </div>
              }
            >
              {category.subcategories.map(sub => (
                <div key={sub.subcategoryId} className="mb-4">
                  <h4 className="text-xs font-semibold text-purple-300 uppercase tracking-wide mb-2">
                    {sub.subcategoryName}
                  </h4>
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
        <CollapsibleSection title={`Plot Tags${includePlotTags.length + excludePlotTags.length > 0 ? ` (${includePlotTags.length + excludePlotTags.length} selected)` : ""}`}>
          <div className="grid md:grid-cols-2 gap-4">
            <TagAutocomplete
              label="Include Plot Tag"
              color="text-green-400"
              allOptions={allPlotTags}
              selectedTags={includePlotTags}
              onAdd={(tag) => {
                if (!excludePlotTags.includes(tag))
                  setIncludePlotTags(prev => [...prev, tag]);
              }}
              onRemove={(tag) => setIncludePlotTags(prev => prev.filter(t => t !== tag))}
              placeholder="e.g. Heist, Betrayal, Coming of age..."
            />
            <TagAutocomplete
              label="Exclude Plot Tag"
              color="text-red-400"
              allOptions={allPlotTags}
              selectedTags={excludePlotTags}
              onAdd={(tag) => {
                if (!includePlotTags.includes(tag))
                  setExcludePlotTags(prev => [...prev, tag]);
              }}
              onRemove={(tag) => setExcludePlotTags(prev => prev.filter(t => t !== tag))}
              placeholder="e.g. Slasher, Cult, Villain origin story..."
            />
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