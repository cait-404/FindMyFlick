import { createContext, useContext, useState, useEffect } from "react";
import API_URL from "../config.js";

const MovieContext = createContext();

export function MovieProvider({ children }) {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedLetter, setSelectedLetter] = useState("a"); // default to "a"
  const [genre, setGenre] = useState("");

  const fetchMovies = async () => {
    try {
      setLoading(true);

     let url = "";

// If genre is selected, prioritize it; otherwise use letter
if (genre) {
  url = `${API_URL}/api/movies/getby/genre/${genre}`;
} else if (selectedLetter) {
  url = `${API_URL}/api/movies/getby/starts-with/${selectedLetter}`;
} else {
  url = `${API_URL}/api/movies/getby/starts-with/a`; // fallback
}

const res = await fetch(url);

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || "Failed to fetch movies");
      }

      const data = await res.json();
      setMovies(data);
    } catch (err) {
      console.error("Fetch error:", err);
      setError(err.message || "Server error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMovies();
  }, [selectedLetter, genre]);

  return (
    <MovieContext.Provider
      value={{
        movies,
        loading,
        error,
        selectedLetter,
        setSelectedLetter,
        genre,
        setGenre,
      }}
    >
      {children}
    </MovieContext.Provider>
  );
}

export function useMovies() {
  return useContext(MovieContext);
}