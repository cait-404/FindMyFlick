// import { useState, useEffect } from "react";
// import { useSearchParams, Link } from "react-router-dom";
// import API_URL from "../../config.js";
// import { useMovies } from "../../context/MovieContext.jsx"; 

// export default function Discover() {
//   const [movies, setMovies] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState(null);

//   const [searchParams] = useSearchParams();
//   const [selectedLetter, setSelectedLetter] = useState("");
//   const genre = searchParams.get("genre");
//   const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");

//   // Filtered movies after fetching (optional client-side filtering)
//   let filteredMovies = movies || [];

//   // Client-side genre filter
//   if (genre) {
//     filteredMovies = filteredMovies.filter((movie) =>
//       movie.genre?.some((g) =>
//         g.toLowerCase().includes(genre.toLowerCase())
//       )
//     );
//   }

//   // Client-side letter filter
//   if (selectedLetter) {
//     filteredMovies = filteredMovies.filter((movie) =>
//       movie.title?.toUpperCase().startsWith(selectedLetter)
//     );
//   }

//   // Sort alphabetically
//   const sortedMovies = [...filteredMovies].sort((a, b) =>
//     (a.title || "").localeCompare(b.title || "")
//   );

//   useEffect(() => {
//     const fetchMovies = async () => {
//       try {
//         setLoading(true);
//         setError(null);

//         let url = "";

//         // Choose correct API endpoint
//         if (selectedLetter) {
//           url = `${API_URL}/api/movies/getby/starts-with/${selectedLetter}`;
//         } else if (genre) {
//           url = `${API_URL}/api/movies/getby/genre/${genre}`;
//         } else {
//           url = `${API_URL}/api/movies/getby/starts-with/A`;
//         }

//         const res = await fetch(url);
//         if (!res.ok) {
//           const text = await res.text();
//           throw new Error(text || "Failed to fetch movies");
//         }

//         const data = await res.json();
//         setMovies(data);
//       } catch (err) {
//         console.error("Fetch error:", err);
//         setError("Failed to load movies");
//       } finally {
//         setLoading(false);
//       }
//     };

//     fetchMovies();
//   }, [selectedLetter, genre]);

//   return (
//     <div className="min-h-screen p-8 text-white bg-gradient-to-b from-black via-[#12001a] to-black">
//       {/* Header */}
//       <div className="max-w-6xl mx-auto mb-8">
//         <h1 className="text-4xl font-extrabold neon-text capitalize">
//           {genre ? `${genre} Movies` : "Discover Movies"}
//         </h1>
//         <p className="opacity-80 mt-2">
//           {genre
//             ? `Browsing movies in the ${genre} genre`
//             : "Explore movies from every genre"}
//         </p>
//       </div>

//       {/* A-Z Filter */}
//       <div className="max-w-6xl mx-auto mb-8 overflow-x-auto px-4">
//         <div className="flex justify-center items-center gap-3 whitespace-nowrap py-2">
//           <button
//             onClick={() => setSelectedLetter("")}
//             className={`w-9 h-9 rounded border border-pink-500 text-pink-400 font-semibold flex items-center justify-center transition
//               ${selectedLetter === ""
//                 ? "bg-pink-500/20 shadow-[0_0_12px_#ff6ed0]"
//                 : "hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
//               }`}
//           >
//             All
//           </button>
//           {alphabet.map((letter) => (
//             <button
//               key={letter}
//               onClick={() => setSelectedLetter(letter)}
//               className={`w-9 h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition
//                 ${selectedLetter === letter
//                   ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"
//                   : "hover:text-pink-400 hover:bg-pink-500/10 hover:shadow-[0_0_12px_#ff6ed0]"
//                 }`}
//             >
//               {letter}
//             </button>
//           ))}
//         </div>
//       </div>

//       {/* Loading / Error */}
//       {loading && <p className="text-center mt-20 opacity-70">Loading movies...</p>}
//       {error && <p className="text-center mt-20 text-red-400">{error}</p>}

//       {/* Empty */}
//       {!loading && sortedMovies.length === 0 && (
//         <p className="text-center mt-20 opacity-80 text-lg neon-text">
//           {selectedLetter
//             ? `No movies starting with "${selectedLetter}".`
//             : "No movies found."}
//         </p>
//       )}

//       {/* Movie Grid */}
//       <div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">
//         {sortedMovies.map((movie) => {
//           const poster = movie.posterUrl; // working property
//           const year = movie.releaseYear || movie.release_year;
//           const hasId = !!movie.id;

//           return hasId ? (
//             <Link
//               key={`${movie.id}`} // unique key
//               to={`/movie/${movie.id}`}
//             >
//               <div className="rounded-xl overflow-hidden bg-black/60 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer">
//                 <div className="h-64 bg-black flex items-center justify-center">
//                   {poster ? (
//                     <img
//                       src={movie.posterUrl}
//                       alt={movie.title}
//                       className="w-full h-full object-contain"
//                     />
//                   ) : (
//                     <div className="text-gray-400">No Image</div>
//                   )}
//                 </div>
//                 <div className="p-4">
//                   <h3 className="font-bold text-lg neon-text truncate">{movie.title}</h3>
//                   <p className="text-sm opacity-70 mt-1">{year || "N/A"}</p>
//                 </div>
//               </div>
//             </Link>
//           ) : (
            
             
              
              
            
//             <div

//            key={`${movie.id}-${movie.title}`} // unique key
//               to={`/movie/${movie.id}`} 
//               className="rounded-xl overflow-hidden bg-gray-700 opacity-60"
//             >
              
//               <div className="h-64 bg-black flex items-center justify-center text-gray-400">
//                 {poster ? (
//                   <img
//                     src={poster} // using fallback poster variable
//                     alt={movie.title}
//                     className="w-full h-full object-contain"
//                   />
//                 ) : (
//                   <div className="text-gray-400">No Image</div>
//                 )}

                
//               </div>
//               <div className="p-4">
//                   <h3 className="font-bold text-lg neon-text truncate">{movie.title}</h3>
//                   <p className="text-sm opacity-70 mt-1">{year || "N/A"}</p>
//                 </div> 
              
//             </div>
            
            
            
//           );
//         })}
//       </div>
//     </div>
    
//   );
// }



import { useState, useEffect, useRef } from "react";

import { useSearchParams, Link } from "react-router-dom";

import { useInfiniteMovies } from "../../hooks/useInfiniteMovies";
 
export default function Discover() {

  const [searchParams] = useSearchParams();

  const [selectedLetter, setSelectedLetter] = useState("");

  const genre = searchParams.get("genre") || "";

  const loaderRef = useRef(null);
 
  const { movies, loading, error, hasMore, loadMore } = useInfiniteMovies(genre);
 
  const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");
 
  // Letter filter applied client-side on already-loaded movies

  const filteredMovies = selectedLetter

    ? movies.filter((movie) => movie.title?.toUpperCase().startsWith(selectedLetter))

    : movies;
 
  const sortedMovies = [...filteredMovies].sort((a, b) =>

    (a.title || "").localeCompare(b.title || "")

  );
 
  // Infinite scroll observer

  useEffect(() => {

    const observer = new IntersectionObserver(

      (entries) => { if (entries[0].isIntersecting) loadMore(); },

      { threshold: 1.0 }

    );

    if (loaderRef.current) observer.observe(loaderRef.current);

    return () => observer.disconnect();

  }, [loadMore]);
 
  return (
<div className="min-h-screen p-8 text-white bg-linear-to-b from-black via-[#12001a] to-black">
 
      {/* Header */}
<div className="max-w-6xl mx-auto mb-8">
<h1 className="text-4xl font-extrabold neon-text capitalize">

          {genre ? `${genre} Movies` : "Discover Movies"}
</h1>
<p className="opacity-80 mt-2">

          {genre ? `Browsing movies in the ${genre} genre` : "Explore movies from every genre"}
</p>
</div>
 
      {/* A-Z Filter */}
<div className="max-w-6xl mx-auto mb-8 overflow-x-auto px-4">
<div className="flex justify-center items-center gap-3 whitespace-nowrap py-2">
<button

            onClick={() => setSelectedLetter("")}

            className={`w-9 h-9 rounded border border-pink-500 text-pink-400 font-semibold flex items-center justify-center transition

              ${selectedLetter === "" ? "bg-pink-500/20 shadow-[0_0_12px_#ff6ed0]" : "hover:bg-pink-500/10"}`}
>

            All
</button>

          {alphabet.map((letter) => (
<button

              key={letter}

              onClick={() => setSelectedLetter(letter)}

              className={`w-9 h-9 rounded border border-pink-500 text-white font-semibold flex items-center justify-center transition

                ${selectedLetter === letter

                  ? "bg-pink-500/20 text-pink-400 shadow-[0_0_12px_#ff6ed0]"

                  : "hover:text-pink-400 hover:bg-pink-500/10"}`}
>

              {letter}
</button>

          ))}
</div>
</div>
 
      {error && <p className="text-center mt-10 text-red-400">{error}</p>}
 
      {!loading && sortedMovies.length === 0 && (
<p className="text-center mt-20 opacity-80 text-lg neon-text">

          {selectedLetter ? `No movies starting with "${selectedLetter}".` : "No movies found."}
</p>

      )}
 
      {/* Grid */}
<div className="max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">

        {sortedMovies.map((movie, index) => {

          // ✅ Use index as fallback to guarantee unique keys

          const key = movie.id ?? `${movie.title}-${index}`;

          const poster = movie.posterUrl || movie.poster_url;

          const year = movie.releaseYear || movie.release_year;
 
          // ✅ Log to confirm id exists — remove after debugging

          // console.log(movie.title, movie.id);
 
          const hasImdb = !!movie.imdbId;

            return hasImdb ? (
<Link key={movie.imdbId || String(movie.tmdbId)} to={`/movie/${movie.imdbId || movie.tmdbId}`}>
<div className="rounded-xl overflow-hidden bg-black/60 shadow-lg transform transition hover:scale-105 hover:shadow-[0_0_25px_#ff6ed0] cursor-pointer h-full">
<div className="h-64 bg-black">

                  {poster ? (
<img src={poster} alt={movie.title} className="w-full h-full object-contain" />

                  ) : (
<div className="w-full h-full flex items-center justify-center text-gray-400">No Image</div>

                  )}
</div>
<div className="p-4">
<h3 className="font-bold text-lg neon-text truncate">{movie.title}</h3>
<p className="text-sm opacity-70 mt-1">{year || "N/A"}</p>
</div>
</div>
</Link>

          ) : (

            // Fallback for movies without an id — not clickable
<div key={key} className="rounded-xl overflow-hidden bg-gray-700 opacity-60">
<div className="h-64 bg-black flex items-center justify-center">

                {poster ? (
<img src={poster} alt={movie.title} className="w-full h-full object-contain" />

                ) : (
<div className="text-gray-400">No Image</div>

                )}
</div>
<div className="p-4">
<h3 className="font-bold text-lg truncate">{movie.title}</h3>
<p className="text-sm opacity-70 mt-1">{year || "N/A"}</p>
</div>
</div>

          );

        })}
</div>
 
      {/* Infinite scroll trigger */}
<div ref={loaderRef} className="py-8 text-center text-gray-500">

        {loading && <p>Loading more movies...</p>}

        {!hasMore && movies.length > 0 && <p>You've seen all {movies.length} movies!</p>}
</div>
 
    </div>

  );

}
 




