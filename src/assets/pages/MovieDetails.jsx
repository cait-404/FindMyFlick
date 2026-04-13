import { useParams, Link } from "react-router-dom";
import { useEffect, useState, useRef } from "react";
import API_URL from "../../config.js";

// MovieDetails page — layout redesign, plot tag voting, warnings by category
// Rebuilt with Claude (April 2026)

export default function MovieDetails() {
  const { id } = useParams();

  const [movie, setMovie] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);

  const [plotTags, setPlotTags] = useState([]);
  const [genres, setGenres] = useState([]);
  const [providers, setProviders] = useState([]);
  const [cast, setCast] = useState([]);
  const [crew, setCrew] = useState([]);
  const [warnings, setWarnings] = useState([]);
  const [warningTaxonomy, setWarningTaxonomy] = useState([]);
  const [collections, setCollections] = useState([]);

  const [showAllCast, setShowAllCast] = useState(false);
  const [showAllCrew, setShowAllCrew] = useState(false);

  // Plot tag voting state
  const [votes, setVotes] = useState({});
  const [voteLoading, setVoteLoading] = useState({});

  // Add plot tag state
  const [allPlotTags, setAllPlotTags] = useState([]);
  const [tagQuery, setTagQuery] = useState("");
  const [tagSuggestions, setTagSuggestions] = useState([]);
  const [showTagSuggestions, setShowTagSuggestions] = useState(false);
  const [addingTag, setAddingTag] = useState(false);
  const [addTagMessage, setAddTagMessage] = useState("");
  const tagInputRef = useRef(null);

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        // 🎬 MAIN MOVIE
        const res = await fetch(`${API_URL}/api/Movies/${id}`);
        if (!res.ok) throw new Error(`Server error: ${res.status}`);
        const data = await res.json();

        // ✅ FIX 1: API RETURNS ARRAY → TAKE FIRST ITEM
        const movieData = Array.isArray(data) ? data[0] : data;
        setMovie(movieData);

        // ⚡ PARALLEL FETCHES (safe)
        const responses = await Promise.allSettled([
          fetch(`${API_URL}/api/Movies/${id}/plot-tags`),
          fetch(`${API_URL}/api/Movies/${id}/genres`),
          fetch(`${API_URL}/api/Movies/${id}/streaming-providers`),
          fetch(`${API_URL}/api/Movies/${id}/cast`),
          fetch(`${API_URL}/api/Movies/${id}/crew`),
          fetch(`${API_URL}/api/Movies/${id}/warnings`),
          fetch(`${API_URL}/api/Movies/${id}/collections`),
          fetch(`${API_URL}/api/WarningTaxonomy`),
          fetch(`${API_URL}/api/movies/plot-tags/getall`)
        ]);

        const parse = async (res) =>
          res.status === "fulfilled" && res.value.ok
            ? await res.value.json()
            : [];

        const [
          plotTagsData,
          genresData,
          streamingData,
          castData,
          crewData,
          warningsData,
          collectionsData,
          taxonomyData,
          allTagsData
        ] = await Promise.all(responses.map(parse));

        // 🎯 MAPPINGS
        setPlotTags(plotTagsData.filter(t => t.tagName));
        setGenres(genresData.map(g => g.genreName || "").filter(Boolean));
        setProviders(streamingData.map(p => p.providerName || "").filter(Boolean));
        setCast(castData);
        setCrew(crewData);
        // Only show warnings where answer is "yes"
        setWarnings(warningsData.filter(w =>
          w.answer?.toLowerCase() === "yes" || w.answer === true
        ));
        setCollections(collectionsData.map(c => c.collectionName || c.name || "").filter(Boolean));
        setWarningTaxonomy(taxonomyData);
        setAllPlotTags(allTagsData.map(t => t.tagText));

      } catch (err) {
        console.error("Error fetching movie:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMovieDetails();
  }, [id]);

  // Handle plot tag vote
  const handleVote = async (tagId, vote) => {
    setVoteLoading(prev => ({ ...prev, [tagId]: true }));
    try {
      await fetch(`${API_URL}/api/movies/${id}/plot-tags/${tagId}/vote`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ vote })
      });
      setVotes(prev => ({ ...prev, [tagId]: vote }));
    } catch (err) {
      console.error("Vote failed:", err);
    } finally {
      setVoteLoading(prev => ({ ...prev, [tagId]: false }));
    }
  };

  // Handle plot tag search/add
  const handleTagQueryChange = (e) => {
    const q = e.target.value;
    setTagQuery(q);
    setAddTagMessage("");
    if (q.trim().length < 1) {
      setTagSuggestions([]);
      setShowTagSuggestions(false);
      return;
    }
    const filtered = allPlotTags.filter(t =>
      t.toLowerCase().includes(q.toLowerCase()) &&
      !plotTags.some(pt => pt.tagName?.toLowerCase() === t.toLowerCase())
    ).slice(0, 8);
    setTagSuggestions(filtered);
    setShowTagSuggestions(true);
  };

  const handleAddTag = async (tagName) => {
    setTagQuery("");
    setShowTagSuggestions(false);
    setAddingTag(true);
    setAddTagMessage("");

    try {
      // Check login first
      const token = localStorage.getItem("token");
      if (!token) {
        setAddTagMessage("Please log in to suggest plot tags.");
        return;
      }

      // Find the tag ID from allPlotTags data
      const res = await fetch(`${API_URL}/api/movies/plot-tags/getbyname/${encodeURIComponent(tagName)}`);
      if (!res.ok) {
        const errText = await res.text();
        throw new Error(`Tag lookup failed (${res.status}): ${errText}`);
      }
      const tagData = await res.json();
      const tagId = tagData.plotTagId;

      // Vote upvote to "add" the tag
      const voteRes = await fetch(`${API_URL}/api/movies/${id}/plot-tags/${tagId}/vote`, {
        method: "POST",
        headers: { "Content-Type": "application/json", "Authorization": `Bearer ${token}` },
        body: JSON.stringify({ vote: 1 })
      });
      if (!voteRes.ok) {
        const voteErr = await voteRes.text();
        throw new Error(`Vote failed (${voteRes.status}): ${voteErr}`);
      }

      // Refresh plot tags
      const refreshRes = await fetch(`${API_URL}/api/Movies/${id}/plot-tags`);
      if (refreshRes.ok) {
        const refreshed = await refreshRes.json();
        setPlotTags(refreshed.filter(t => t.tagName));
      }
      setAddTagMessage(`"${tagName}" added successfully!`);
    } catch (err) {
      console.error("Plot tag error:", err);
      setAddTagMessage(err.message || "Could not add tag. Please try again.");
    } finally {
      setAddingTag(false);
    }
  };

  // Build warnings organized by category
  const warningsByCategory = warningTaxonomy.map(category => {
    const matchingTopics = warnings.filter(w =>
      category.subcategories.some(sub =>
        sub.topics.some(t => t.dtddTopicId === w.dtddTopicId)
      )
    );
    return { ...category, matchingTopics };
  }).filter(c => c.matchingTopics.length > 0);

  if (error) {
    return (
      <div className="text-center mt-10 text-red-400">
        <p>Could not load movie details.</p>
        <p className="text-sm text-gray-400">Try another movie.</p>
      </div>
    );
  }

  if (loading) {
    return (
      <p className="text-white text-center mt-10">Loading movie details...</p>
    );
  }

  if (!movie) return null;

  // ✅ FIX 2: CORRECT FIELD NAMES FROM YOUR API
  const poster = movie.posterUrl || movie.poster;
  const displayedCast = showAllCast ? cast : cast.slice(0, 10);
  const displayedCrew = showAllCrew ? crew : crew.slice(0, 5);

  return (
    <div className="min-h-screen text-white p-6 bg-linear-to-b from-black via-[#12001a] to-black">
      <div className="max-w-6xl mx-auto">

        {/* TOP SECTION: Poster + Core Info */}
        <div className="grid md:grid-cols-3 gap-8 mb-8">

          {/* POSTER */}
          <div className="md:col-span-1">
            {poster ? (
              <img
                src={poster}
                alt={movie.title}
                className="w-full rounded-xl shadow-2xl object-contain"
              />
            ) : (
              <div className="w-full h-64 bg-gray-800 flex items-center justify-center rounded-xl text-gray-400">
                No Image
              </div>
            )}
          </div>

          {/* CORE INFO */}
          <div className="md:col-span-2">
            <h1 className="text-4xl font-bold mb-2 text-pink-400">
              {movie.title || "Untitled"}
            </h1>

            <div className="flex gap-4 mb-4 text-gray-400 text-sm">
              <span>{movie.releaseYear || "N/A"}</span>
              {movie.mpaaRating && (
                <span className="border border-gray-500 px-2 py-0.5 rounded text-xs">
                  {movie.mpaaRating}
                </span>
              )}
            </div>

            {/* Genres */}
            <div className="flex flex-wrap gap-2 mb-4">
              {genres.map(g => (
                <span key={g} className="px-3 py-1 rounded-full bg-purple-800/60 text-sm text-purple-200">
                  {g}
                </span>
              ))}
            </div>

            {/* Plot Summary */}
            <p className="text-gray-300 leading-relaxed mb-4">
              {movie.plotSummary || "No description available."}
            </p>

            {/* Streaming */}
            {providers.length > 0 && (
              <div className="mb-4">
                <h3 className="text-sm font-semibold text-pink-300 mb-2">Available On</h3>
                <div className="flex flex-wrap gap-2">
                  {providers.map(p => (
                    <span key={p} className="px-3 py-1 rounded-full bg-green-900/60 text-green-300 text-sm">
                      {p}
                    </span>
                  ))}
                </div>
              </div>
            )}

            {/* PLOT TAGS */}
        <div className="bg-black/40 rounded-xl p-6 mb-6">
          <h2 className="text-xl font-bold text-pink-400 mb-4">Plot Tags</h2>

          {plotTags.length === 0 ? (
            <p className="text-gray-400 text-sm mb-4">No plot tags yet — be the first to add one!</p>
          ) : (
            <div className="flex flex-wrap gap-3 mb-4">
              {plotTags.map(tag => (
                <div key={tag.tagID}
                  className="flex items-center gap-2 bg-purple-900/60 rounded-full px-3 py-1.5">
                  <span className="text-sm text-white">{tag.tagName}</span>
                  <button
                    onClick={() => handleVote(tag.tagID, 1)}
                    disabled={voteLoading[tag.tagID]}
                    className={`text-xs px-1.5 py-0.5 rounded transition ${
                      votes[tag.tagID] === 1
                        ? "bg-green-600 text-white"
                        : "text-green-400 hover:bg-green-600/30"
                    }`}
                    title="Upvote — this tag fits"
                  >
                    👍
                  </button>
                  <button
                    onClick={() => handleVote(tag.tagID, -1)}
                    disabled={voteLoading[tag.tagID]}
                    className={`text-xs px-1.5 py-0.5 rounded transition ${
                      votes[tag.tagID] === -1
                        ? "bg-red-600 text-white"
                        : "text-red-400 hover:bg-red-600/30"
                    }`}
                    title="Downvote — this tag doesn't fit"
                  >
                    👎
                  </button>
                </div>
              ))}
            </div>
          )}

          {/* Add a plot tag */}
          <div className="relative">
            <p className="text-sm text-gray-400 mb-2">Suggest a plot tag:</p>
            <input
              ref={tagInputRef}
              type="text"
              placeholder="Search plot tags..."
              className="w-full md:w-72 p-2 rounded-md text-black text-sm"
              value={tagQuery}
              onChange={handleTagQueryChange}
              onFocus={() => tagQuery && setShowTagSuggestions(true)}
              onBlur={() => setTimeout(() => setShowTagSuggestions(false), 150)}
              disabled={addingTag}
            />
            {showTagSuggestions && tagSuggestions.length > 0 && (
              <div className="absolute z-20 w-72 mt-1 rounded-md shadow-lg max-h-48 overflow-y-auto"
                style={{ background: '#1a0033', border: '2px solid #550088' }}>
                {tagSuggestions.map(tag => (
                  <button
                    key={tag}
                    onMouseDown={() => handleAddTag(tag)}
                    className="w-full text-left px-3 py-2 text-sm text-white hover:bg-purple-900/50"
                  >
                    {tag}
                  </button>
                ))}
              </div>
            )}
            {addTagMessage && (
              <p className={`text-sm mt-2 ${addTagMessage.includes("success") ? "text-green-400" : "text-red-400"}`}>
                {addTagMessage}
              </p>
            )}
          </div>

            </div>
        </div>

        </div>

        {/* COLLECTIONS */}
        {collections.length > 0 && collections[0] !== "None" && (
          <div className="bg-black/40 rounded-xl p-6 mb-6">
            <h2 className="text-xl font-bold text-pink-400 mb-4">Collection</h2>
            <div className="flex flex-wrap gap-2">
              {collections.map(c => (
                <Link
                  key={c}
                  to={`/collection/${encodeURIComponent(c)}`}
                  className="px-3 py-1.5 rounded-full bg-purple-800/60 text-purple-200 text-sm hover:bg-purple-700/60 transition cursor-pointer"
                >
                  {c}
                </Link>
              ))}
            </div>
          </div>
        )}

        {/* CAST */}
        <div className="bg-black/40 rounded-xl p-6 mb-6">
          <h2 className="text-xl font-bold text-pink-400 mb-4">Cast</h2>
          <div className="flex flex-col gap-1">
            <div className="grid grid-cols-2 text-xs text-purple-300 font-semibold uppercase mb-1 px-1">
              <span>Character</span>
              <span>Actor</span>
            </div>
            {displayedCast.map(c => (
              <div key={c.tmdbPersonId} className="grid grid-cols-2 text-sm py-1.5 px-1 border-b border-purple-900/30">
                <span className="text-gray-400">{c.characterNames?.[0] || "—"}</span>
                <span className="text-white font-medium">{c.personName}</span>
              </div>
            ))}
          </div>
          {cast.length > 10 && (
            <button
              onClick={() => setShowAllCast(prev => !prev)}
              className="mt-3 text-sm text-pink-400 hover:text-pink-300 transition"
            >
              {showAllCast ? "Show less ▲" : `Show all ${cast.length} cast members ▼`}
            </button>
          )}
        </div>

        {/* CREW */}
        <div className="bg-black/40 rounded-xl p-6 mb-6">
          <h2 className="text-xl font-bold text-pink-400 mb-4">Crew</h2>
          <div className="flex flex-col gap-1">
            <div className="grid grid-cols-2 text-xs text-purple-300 font-semibold uppercase mb-1 px-1">
              <span>Role</span>
              <span>Name</span>
            </div>
            {displayedCrew.map(c => (
              <div key={c.tmdbPersonId} className="grid grid-cols-2 text-sm py-1.5 px-1 border-b border-purple-900/30">
                <span className="text-gray-400">{c.jobs?.[0] || "—"}</span>
                <span className="text-white font-medium">{c.personName}</span>
              </div>
            ))}
          </div>
          {crew.length > 5 && (
            <button
              onClick={() => setShowAllCrew(prev => !prev)}
              className="mt-3 text-sm text-pink-400 hover:text-pink-300 transition"
            >
              {showAllCrew ? "Show less ▲" : `Show all ${crew.length} crew members ▼`}
            </button>
          )}
        </div>

        {/* CONTENT WARNINGS */}
        <div className="bg-black/40 rounded-xl p-6 mb-6">
          <h2 className="text-xl font-bold text-pink-400 mb-4">Content Warnings</h2>
          {warningsByCategory.length === 0 ? (
            <p className="text-gray-400 text-sm">No content warnings recorded for this movie.</p>
          ) : (
            <div className="space-y-2">
              {warningsByCategory.map(category => (
                <WarningCategory key={category.categoryId} category={category} />
              ))}
            </div>
          )}
        </div>

      </div>
    </div>
  );
}

// Collapsible warning category
function WarningCategory({ category }) {
  const [open, setOpen] = useState(false);
  return (
    <div className="border border-purple-800 rounded-lg overflow-hidden">
      <button
        onClick={() => setOpen(o => !o)}
        className="w-full flex justify-between items-center px-4 py-3 bg-purple-900/40 hover:bg-purple-900/60 transition text-left"
      >
        <span className="font-semibold text-pink-300 text-sm">{category.categoryName}</span>
        <span className="text-gray-400 text-xs">{open ? "▲" : `▼ ${category.matchingTopics.length} warning${category.matchingTopics.length !== 1 ? "s" : ""}`}</span>
      </button>
      {open && (
        <div className="px-4 py-3 bg-black/40">
          <ul className="space-y-1">
            {category.matchingTopics.map(w => (
              <li key={w.dtddTopicId} className="text-sm text-gray-300 flex items-center gap-2">
                <span className="text-yellow-400">⚠</span>
                {w.topicName}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}