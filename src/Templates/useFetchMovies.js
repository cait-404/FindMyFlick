// src/hooks/useFetchMovies.js
import { useState, useEffect } from "react";
import API_URL from "../config.js";

export default function useFetchMovies(endpoint) {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!endpoint) return;

    setLoading(true);

    // Use API_URL from config.js
    fetch(`${API_URL}/${endpoint}`)
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