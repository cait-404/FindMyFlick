import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

function Search() {
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");

  const [movies, setMovies] = useState([]);

  useEffect(() => {
    if (!query) return;

    fetch(`http://localhost:5135/api/movies/search?query=${query}`)
      .then(res => res.json())
      .then(data => setMovies(data.results || data))
      .catch(err => console.error("Error fetching movies:", err));

  }, [query]);

//   return (
//     <div>
//       <h2>Results for: {query}</h2>

//       {movies.map(movie => (
//         <div key={movie.id}>
//           <h3>{movie.name}</h3>
//         </div>
//       ))}
//     </div>
//   );

return (
<div>
<h2>Results for: {query}</h2>

    {movies.length === 0 && <p>No results found.</p>}

    {movies.map(movie => (
<div key={movie.id}>
<img src={movie.poster} alt={movie.name} />
<h3>{movie.name}</h3>
<p>{movie.Year}</p>
<p>{movie["age rating"]}</p>
<p>{movie.summary}</p>
<p>Rating: {movie["user ratings"]}</p>
<p>Streaming: {movie["streaming services"]?.join(", ")}</p>
</div>

    ))}
</div>

);
 
}

export default Search;