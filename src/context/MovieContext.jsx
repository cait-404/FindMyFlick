import { createContext, useContext, useState, useEffect } from "react";
import API_URL from "../config.js";

const MovieContext = createContext();

export function MovieProvider({ children }) {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedLetter, setSelectedLetter] = useState("");
  const [genre, setGenre] = useState("");

  const fetchMovies = async () => {
  try {
    setLoading(true);

    let url = "";

    if (selectedLetter) {
      url = `${API_URL}/api/movies/getby/starts-with/${selectedLetter}?limit=100`;
    } else if (genre) {
      url = `${API_URL}/api/movies/getby/genre/${genre}?limit=200`;
    } else {
      url = `${API_URL}/api/movies/getby/starts-with/a?limit=100`;
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