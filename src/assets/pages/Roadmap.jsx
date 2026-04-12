// Roadmap.jsx
export default function Roadmap() {
  return (
    <div className="min-h-screen px-6 py-12 text-white max-w-5xl mx-auto">

      {/* Header */}
      <div className="text-center mb-10">
        <h1 className="text-4xl font-extrabold neon-text mb-4">
          Future Roadmap
        </h1>
        <p className="text-gray-300 max-w-2xl mx-auto">
          FindMyFlick isn’t just about finding movies — it’s about making
          discovery faster, more personal, and actually enjoyable.
        </p>
      </div>

      {/* WHY SECTION */}
      <div className="mb-14">
        <h2 className="text-2xl font-bold neon-text mb-6 text-center">
          Why FindMyFlick?
        </h2>

        <div className="grid md:grid-cols-2 gap-6">

          <div className="bg-black/40 border border-gray-700 rounded-xl p-5 hover:shadow-[0_0_20px_#ff52d9] transition">
            <h3 className="font-semibold text-lg mb-2">🎯 No More Endless Scrolling</h3>
            <p className="text-gray-300 text-sm">
              Stop wasting time searching through hundreds of movies. FindMyFlick
              helps you narrow things down instantly so you can actually pick something to watch.
            </p>
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-5 hover:shadow-[0_0_20px_#ff52d9] transition">
            <h3 className="font-semibold text-lg mb-2">⚡ Built for Speed</h3>
            <p className="text-gray-300 text-sm">
              Browse by letter, genre, or filters without lag or clutter.
              Everything is designed to get you from “I don’t know” → “I found it.”
            </p>
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-5 hover:shadow-[0_0_20px_#ff52d9] transition">
            <h3 className="font-semibold text-lg mb-2">🧠 Smarter Discovery</h3>
            <p className="text-gray-300 text-sm">
              Instead of generic recommendations, FindMyFlick focuses on what you
              actually care about — themes, tags, and specific elements.
            </p>
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-5 hover:shadow-[0_0_20px_#ff52d9] transition">
            <h3 className="font-semibold text-lg mb-2">🎬 Made for Real Users</h3>
            <p className="text-gray-300 text-sm">
              This isn’t just another movie database. It’s built with real user
              frustration in mind — making discovery feel simple and intentional.
            </p>
          </div>

        </div>
      </div>

      {/* DIVIDER */}
      <div className="my-12 h-px bg-linear-to-r from-transparent via-gray-700 to-transparent" />

      {/* ROADMAP SECTION */}
      <div>
        <h2 className="text-2xl font-bold neon-text mb-6 text-center">
          What’s Coming Next
        </h2>

        <div className="space-y-4 text-gray-300">

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Advanced filtering (multiple genres, include/exclude tags)
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Personalized recommendations based on user behavior
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Watchlist and favorites system
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Improved movie detail pages (cast, crew, streaming info)
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Mobile optimization and performance improvements
          </div>

          <div className="bg-black/40 border border-gray-700 rounded-xl p-4">
            Social features (sharing, reviews, ratings)
          </div>

        </div>
      </div>

    </div>
  );
}