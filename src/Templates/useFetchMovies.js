// src/hooks/useFetchMovies.js
import { useState, useEffect } from "react";

export default function useFetchMovies(endpoint) {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!endpoint) return;

    setLoading(true);
    fetch(`${import.meta.env.VITE_API_URL}/api/${endpoint}`)
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch data");
        return res.json();
      })
      .then((data) => {
        setMovies(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setError(err.message);
        setLoading(false);
      });
  }, [endpoint]);

  return { movies, loading, error };
}
