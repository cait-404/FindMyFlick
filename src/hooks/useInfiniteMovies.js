import { useState, useEffect, useCallback } from "react";
 
const BATCH_SIZE = 20;
 
export function useInfiniteMovies(genreFilter = "") {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [skip, setSkip] = useState(0);
  const [hasMore, setHasMore] = useState(true);
 
  // Reset everything when genre filter changes
  useEffect(() => {
    setMovies([]);
    setSkip(0);
    setHasMore(true);
  }, [genreFilter]);
 
  const fetchBatch = useCallback(async (currentSkip, genre) => {
    setLoading(true);
    try {
      const res = await fetch("https://localhost:5002/api/MovieSearch", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          take: BATCH_SIZE,
          skip: currentSkip,
          minMatches: 1,
          enableApiFallback: false,
          alwaysAddFromApis: false,
          genreNames: genre ? [genre] : [],
          keywordNames: [],
          personNames: [],
          personRoles: [],
          streamingProviderNames: []
        }),
      });
 
      if (!res.ok) throw new Error("Failed to fetch movies");
      const data = await res.json();
      const batch = data.results || data;
 
      setMovies((prev) => [...prev, ...batch]);
      setSkip(currentSkip + BATCH_SIZE);
      if (batch.length < BATCH_SIZE) setHasMore(false);
 
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, []);
 
  // Load first batch on mount or when genre changes
  useEffect(() => {
    fetchBatch(0, genreFilter);
  }, [genreFilter]);
 
  const loadMore = useCallback(() => {
    if (!loading && hasMore) fetchBatch(skip, genreFilter);
  }, [loading, hasMore, skip, genreFilter, fetchBatch]);
 
  return { movies, loading, error, hasMore, loadMore };
}