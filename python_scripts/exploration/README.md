## FindMyFlick - exploration folder

These exploration scripts were used to test how alternate editions of the same movie (extended cuts, final cuts, director’s cuts, unrated versions) appear across our source APIs (TMDB/OMDb) before we finalized our database structure.

**What we found:** The APIs do not represent editions as separate titles with distinct IDs. Instead, edition information may appear as a note (for example, in TMDB release date notes), while core fields like runtime, poster, rating, and external IDs remain tied to the base title. OMDb also did not consistently return distinct results for edition-style title queries.

**Decision:** The team chose to exclude edition markers from our search results. From a user experience standpoint, including edition labels based only on notes is unreliable, and the existence of an edition does not mean it is actually available to stream on a given service. Excluding edition markers avoids misleading users and prevents incorrect runtime/poster/rating details from being presented as if they were edition-specific.