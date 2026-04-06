import { useState, useEffect, useCallback } from "react";
 
const PAGE_SIZE = 20; // how many to show at a time
 
export function useInfiniteMovies(genreFilter = "") {

  const [allMovies, setAllMovies] = useState([]); // full list from API

  const [displayed, setDisplayed] = useState([]);  // what's shown on screen

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState(null);

  const [hasMore, setHasMore] = useState(true);

  const [page, setPage] = useState(1);
 
  // Fetch all movies once (or when genre changes)

  useEffect(() => {

    setAllMovies([]);

    setDisplayed([]);

    setPage(1);

    setHasMore(true);

    setError(null);
 
    const fetchAll = async () => {

      setLoading(true);

      try {

        const res = await fetch("https://localhost:5002/api/MovieSearch", {

          method: "POST",

          headers: { "Content-Type": "application/json" },

          body: JSON.stringify({

            take: 500,

            minMatches: 1,

            enableApiFallback: false,

            alwaysAddFromApis: false,

            genreNames: genreFilter ? [genreFilter] : [],

            keywordNames: [],

            personNames: [],

            personRoles: [],

            streamingProviderNames: []

          }),

        });
 
        if (!res.ok) throw new Error("Failed to fetch movies");

        const data = await res.json();

        const all = data.results || [];
 
        setAllMovies(all);

        // Show first page immediately

        setDisplayed(all.slice(0, PAGE_SIZE));

        setHasMore(all.length > PAGE_SIZE);
 
      } catch (err) {

        setError(err.message);

      } finally {

        setLoading(false);

      }

    };
 
    fetchAll();

  }, [genreFilter]);
 
  // Load next page from already-fetched data — no extra API call

  const loadMore = useCallback(() => {

    setPage((prev) => {

      const nextPage = prev + 1;

      const nextSlice = allMovies.slice(0, nextPage * PAGE_SIZE);

      setDisplayed(nextSlice);

      setHasMore(nextSlice.length < allMovies.length);

      return nextPage;

    });

  }, [allMovies]);
 
  return { movies: displayed, loading, error, hasMore, loadMore };

}
 