using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieSearchController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        // Hard stop so searches don't try to populate your whole DB.
        private const int DefaultMaxApiAdds = 25;

        // When a Person filter is used, we need credits. Crew can explode.
        private const int CrewLimit = 25;

        // How many days before we re-check warnings for a movie that already has them.
        // Movies with no warnings at all (but released) are always re-checked.
        private const int WarningStalenessDays = 30;

        // How many days before we re-check streaming availability.
        private const int StreamingStalenessDays = 7;

        private sealed record ApiFillStats(
            int Candidates,
            int SkippedMissingImdb,
            int SkippedAlreadyEligible,
            int SkippedStreamingEnrichFailed,
            int SkippedStillNotStreamable,
            int SkippedWarningsEnrichFailed,
            int SkippedStillNoWarnings,
            int Added,
            string? ExampleStillNoWarningsImdb,
            string? ExampleWarningsEnrichFailedImdb
        );

        public MovieSearchController(FindmyflickContext context)
        {
            _context = context;
        }

        public enum MatchMode
        {
            Any = 0,
            All = 1
        }

        // -------------------------------------------------------------------------
        // REQUEST
        // All name-based fields accept plain-language strings (case-insensitive).
        // ID-based fields still work for callers that already know the IDs.
        // -------------------------------------------------------------------------
        public sealed class MovieSearchRequest
        {
            public int Take { get; set; } = 25;
            public int MinMatches { get; set; } = 5;

            public bool EnableApiFallback { get; set; } = true;
            public bool AlwaysAddFromApis { get; set; } = false;
            public int MaxApiAdds { get; set; } = DefaultMaxApiAdds;

            public string WatchRegion { get; set; } = "US";

            // ------------------------------------------------------------------
            // TEXT-BASED filters (plain language, case-insensitive partial match)
            // ------------------------------------------------------------------

            /// <summary>Partial name match against streaming_providers.provider_name (e.g. "disney", "netflix").</summary>
            public List<string> StreamingProviderNames { get; set; } = new();

            /// <summary>Partial name match against genres.genre_name (e.g. "action", "rom").</summary>
            public List<string> GenreNames { get; set; } = new();

            /// <summary>Partial name match against keywords.keyword_name (e.g. "time travel", "heist").</summary>
            public List<string> KeywordNames { get; set; } = new();

            /// <summary>Partial name match against people.person_name (e.g. "nolan", "meryl streep").</summary>
            public List<string> PersonNames { get; set; } = new();

            /// <summary>
            /// Restricts which credits count for person filtering.
            /// Accepted values (case-insensitive):
            ///   "cast"               — only acting credits
            ///   "crew"               — all crew credits
            ///   "director"           — crew where job == Director
            ///   "writer"             — crew where job is Writer/Screenplay/Story
            ///   "producer"           — crew where job is Producer/Executive Producer
            /// Multiple values are combined with OR (e.g. ["cast", "director"]).
            /// When empty, both cast and all crew are included (original behaviour).
            /// </summary>
            public List<string> PersonRoles { get; set; } = new();

            /// <summary>Partial name match against warnings.topic_name (e.g. "dog", "suicide").</summary>
            public List<string> IncludeWarningNames { get; set; } = new();

            /// <summary>Partial name match against warning_categories.category_name (e.g. "violence", "animal").</summary>
            public List<string> IncludeWarningCategoryNames { get; set; } = new();

            /// <summary>Partial name match against warning_subcategories.subcategory_name.</summary>
            public List<string> IncludeWarningSubcategoryNames { get; set; } = new();

            /// <summary>Partial name match against warnings.topic_name for exclusion.</summary>
            public List<string> ExcludeWarningNames { get; set; } = new();

            /// <summary>Partial name match against warning_categories.category_name for exclusion.</summary>
            public List<string> ExcludeWarningCategoryNames { get; set; } = new();

            /// <summary>Partial name match against warning_subcategories.subcategory_name for exclusion.</summary>
            public List<string> ExcludeWarningSubcategoryNames { get; set; } = new();

            // ------------------------------------------------------------------
            // ID-BASED filters (kept for backward compatibility / internal use)
            // ------------------------------------------------------------------
            public List<int> StreamingProviderIds { get; set; } = new();
            public MatchMode ProviderMatchMode { get; set; } = MatchMode.Any;

            public List<int> GenreIds { get; set; } = new();
            public List<int> KeywordIds { get; set; } = new();
            public List<int> PersonIds { get; set; } = new();

            /// <summary>Plain-language title search (case-insensitive partial match).</summary>
            public string? TitleContains { get; set; }

            public List<int> IncludeWarningTopicIds { get; set; } = new();
            public List<int> IncludeWarningCategoryIds { get; set; } = new();
            public List<int> IncludeWarningSubcategoryIds { get; set; } = new();
            public MatchMode IncludeWarningMatchMode { get; set; } = MatchMode.Any;

            public List<int> ExcludeWarningTopicIds { get; set; } = new();
            public List<int> ExcludeWarningCategoryIds { get; set; } = new();
            public List<int> ExcludeWarningSubcategoryIds { get; set; } = new();
        }

        public sealed class MovieSearchResultCard
        {
            public string ImdbId { get; set; } = "";
            public int? TmdbId { get; set; }
            public string Title { get; set; } = "";
            public int? ReleaseYear { get; set; }
            public string? PosterUrl { get; set; }
        }

        public sealed class MovieSearchResponse
        {
            public int Returned { get; set; }
            public int MinMatchesTarget { get; set; }
            public int TakeTarget { get; set; }
            public List<string> RelaxedStepsUsed { get; set; } = new();
            public List<MovieSearchResultCard> Results { get; set; } = new();

            public int IncludedWarningTopicsExpandedCount { get; set; }
            public int ExcludedWarningTopicsExpandedCount { get; set; }

            public int AddedFromApis { get; set; } = 0;
            public int StaleWarningsRefreshed { get; set; } = 0;

            /// <summary>
            /// Names that were provided but could not be resolved even after
            /// attempting a TMDB lookup (e.g. typos, or people/genres genuinely
            /// absent from all streaming movies in the database).
            /// These filters are silently skipped so the search still returns results.
            /// </summary>
            public List<string> UnresolvedNames { get; set; } = new();
        }

        // =========================================================================
        // MAIN ENDPOINT
        // =========================================================================

        [HttpPost]
        public async Task<ActionResult<MovieSearchResponse>> Search([FromBody] MovieSearchRequest req)
        {
            if (req.Take <= 0) req.Take = 25;
            if (req.Take > 100) req.Take = 100;
            if (req.MinMatches <= 0) req.MinMatches = 5;
            if (req.MaxApiAdds <= 0) req.MaxApiAdds = DefaultMaxApiAdds;
            if (req.MaxApiAdds > 50) req.MaxApiAdds = 50;
            if (string.IsNullOrWhiteSpace(req.WatchRegion)) req.WatchRegion = "US";

            // ------------------------------------------------------------------
            // STEP 1: Resolve plain-language names → IDs and merge with any
            //         ID lists the caller already provided.
            // ------------------------------------------------------------------
            var unresolvedNames = await ResolveNamesToIdsAsync(req);

            // ------------------------------------------------------------------
            // STEP 2: Expand umbrella tiers (category/subcategory → topic IDs).
            // ------------------------------------------------------------------
            var expandedIncludeTopicIds = await ResolveTopicIdsAsync(
                req.IncludeWarningTopicIds,
                req.IncludeWarningCategoryIds,
                req.IncludeWarningSubcategoryIds
            );

            var expandedExcludeTopicIds = await ResolveTopicIdsAsync(
                req.ExcludeWarningTopicIds,
                req.ExcludeWarningCategoryIds,
                req.ExcludeWarningSubcategoryIds
            );

            var baseReq = Clone(req);
            baseReq.IncludeWarningTopicIds = expandedIncludeTopicIds;
            baseReq.ExcludeWarningTopicIds = expandedExcludeTopicIds;

            var relaxedSteps = new List<string>();

            // ------------------------------------------------------------------
            // STEP 3: Background refresh of stale/missing warnings (fire-and-
            //         forget-style — we kick it off but don't block the query).
            // ------------------------------------------------------------------
            int staleRefreshed = 0;
            var dtddKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (!string.IsNullOrWhiteSpace(dtddKey))
            {
                staleRefreshed = await RefreshStaleWarningsAsync(dtddKey);
                if (staleRefreshed > 0)
                    relaxedSteps.Add($"Stale warnings refreshed: {staleRefreshed} movie(s) updated");
            }

            // ------------------------------------------------------------------
            // STEP 4: Run the primary query and progressive relaxation.
            // ------------------------------------------------------------------
            var effectiveReq = Clone(baseReq);
            var results = await RunQuery(effectiveReq, take: req.Take);

            if (results.Count < req.MinMatches)
            {
                if (effectiveReq.KeywordIds.Count > 0)
                {
                    var clone = Clone(effectiveReq);
                    clone.KeywordIds.Clear();
                    var r = await RunQuery(clone, req.Take);
                    if (r.Count >= results.Count)
                    {
                        results = r;
                        effectiveReq = clone;
                        relaxedSteps.Add("Relax: removed KeywordIds");
                    }
                }

            }

            // ------------------------------------------------------------------
            // STEP 5: API fallback (same logic as before).
            // ------------------------------------------------------------------
            int addedFromApis = 0;

            var shouldApiFill =
                req.EnableApiFallback &&
                (
                    req.AlwaysAddFromApis
                        ? results.Count < req.Take
                        : results.Count < req.MinMatches
                );

            if (shouldApiFill)
            {
                var (addedCount, stats) = await TryApiFillAsync(effectiveReq);
                addedFromApis = addedCount;

                relaxedSteps.Add(
                    $"API fill stats: candidates={stats.Candidates}, added={stats.Added}, " +
                    $"skipMissingImdb={stats.SkippedMissingImdb}, skipAlreadyEligible={stats.SkippedAlreadyEligible}, " +
                    $"skipStreamingEnrichFailed={stats.SkippedStreamingEnrichFailed}, skipStillNotStreamable={stats.SkippedStillNotStreamable}, " +
                    $"skipWarningsEnrichFailed={stats.SkippedWarningsEnrichFailed}, skipStillNoWarnings={stats.SkippedStillNoWarnings}"
                );

                relaxedSteps.Add(
                    $"API fill examples: stillNoWarningsImdb={stats.ExampleStillNoWarningsImdb ?? "(none)"}, " +
                    $"warningsEnrichFailedImdb={stats.ExampleWarningsEnrichFailedImdb ?? "(none)"}"
                );

                if (addedFromApis > 0)
                {
                    relaxedSteps.Add($"API fill: added {addedFromApis} movies");
                    results = await RunQuery(effectiveReq, req.Take);
                }
                else
                {
                    relaxedSteps.Add("API fill: added 0 movies");
                }
            }

            var response = new MovieSearchResponse
            {
                Returned = results.Count,
                MinMatchesTarget = req.MinMatches,
                TakeTarget = req.Take,
                RelaxedStepsUsed = relaxedSteps,
                Results = results,
                IncludedWarningTopicsExpandedCount = expandedIncludeTopicIds.Count,
                ExcludedWarningTopicsExpandedCount = expandedExcludeTopicIds.Count,
                AddedFromApis = addedFromApis,
                StaleWarningsRefreshed = staleRefreshed,
                UnresolvedNames = unresolvedNames
            };

            return Ok(response);
        }

        // =========================================================================
        // NAME → ID RESOLUTION
        // Each name list does a case-insensitive partial match (ILike '%term%')
        // against the appropriate lookup table and appends the resolved IDs to the
        // corresponding ID list. Duplicates are removed before the query runs.
        // =========================================================================

        private async Task<List<string>> ResolveNamesToIdsAsync(MovieSearchRequest req)
        {
            var unresolved = new List<string>();
            // Streaming providers (hard constraint — track unresolved names)
            foreach (var name in req.StreamingProviderNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.StreamingProviders
                    .AsNoTracking()
                    .Where(sp => EF.Functions.ILike(sp.ProviderName, $"%{name.Trim()}%"))
                    .Select(sp => sp.TmdbProviderId)
                    .ToListAsync();
                if (ids.Count == 0)
                    unresolved.Add($"streaming provider: '{name.Trim()}'");
                req.StreamingProviderIds.AddRange(ids);
            }
            req.StreamingProviderIds = req.StreamingProviderIds.Distinct().ToList();

            // Genres (hard constraint — track unresolved names)
            foreach (var name in req.GenreNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.Genres
                    .AsNoTracking()
                    .Where(g => EF.Functions.ILike(g.GenreName, $"%{name.Trim()}%"))
                    .Select(g => g.TmdbGenreId)
                    .ToListAsync();
                if (ids.Count == 0)
                    unresolved.Add($"genre: '{name.Trim()}'");
                req.GenreIds.AddRange(ids);
            }
            req.GenreIds = req.GenreIds.Distinct().ToList();

            // Keywords
            foreach (var name in req.KeywordNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.Keywords
                    .AsNoTracking()
                    .Where(k => EF.Functions.ILike(k.KeywordName, $"%{name.Trim()}%"))
                    .Select(k => k.TmdbKeywordId)
                    .ToListAsync();
                req.KeywordIds.AddRange(ids);
            }
            req.KeywordIds = req.KeywordIds.Distinct().ToList();

            // People (hard constraint — fall back to TMDB person search if not in DB)
            var tmdbKeyForPeople = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            foreach (var name in req.PersonNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.People
                    .AsNoTracking()
                    .Where(p => EF.Functions.ILike(p.PersonName, $"%{name.Trim()}%"))
                    .Select(p => p.TmdbPersonId)
                    .ToListAsync();

                if (ids.Count == 0 && !string.IsNullOrWhiteSpace(tmdbKeyForPeople))
                {
                    // Not in our DB yet — search TMDB, upsert any matches, then retry
                    var tmdbPersonIds = await FetchAndUpsertTmdbPersonsByNameAsync(name.Trim(), tmdbKeyForPeople);
                    if (tmdbPersonIds.Count > 0)
                    {
                        // Re-query our DB now that we've upserted them
                        ids = await _context.People
                            .AsNoTracking()
                            .Where(p => EF.Functions.ILike(p.PersonName, $"%{name.Trim()}%"))
                            .Select(p => p.TmdbPersonId)
                            .ToListAsync();
                    }
                }

                if (ids.Count == 0)
                    unresolved.Add($"person: '{name.Trim()}'");

                req.PersonIds.AddRange(ids);
            }
            req.PersonIds = req.PersonIds.Distinct().ToList();

            // Include warning topic names → topic IDs
            foreach (var name in req.IncludeWarningNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.Warnings
                    .AsNoTracking()
                    .Where(w => EF.Functions.ILike(w.TopicName, $"%{name.Trim()}%"))
                    .Select(w => w.DtddTopicId)
                    .ToListAsync();
                req.IncludeWarningTopicIds.AddRange(ids);
            }
            req.IncludeWarningTopicIds = req.IncludeWarningTopicIds.Distinct().ToList();

            // Include warning category names → category IDs
            foreach (var name in req.IncludeWarningCategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                const string sql = @"
SELECT category_id FROM public.warning_categories
WHERE category_name ILIKE @name;";
                await using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    req.IncludeWarningCategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.IncludeWarningCategoryIds = req.IncludeWarningCategoryIds.Distinct().ToList();

            // Include warning subcategory names → subcategory IDs
            foreach (var name in req.IncludeWarningSubcategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                const string sql = @"
SELECT subcategory_id FROM public.warning_subcategories
WHERE subcategory_name ILIKE @name;";
                await using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    req.IncludeWarningSubcategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.IncludeWarningSubcategoryIds = req.IncludeWarningSubcategoryIds.Distinct().ToList();

            // Exclude warning topic names → topic IDs
            foreach (var name in req.ExcludeWarningNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.Warnings
                    .AsNoTracking()
                    .Where(w => EF.Functions.ILike(w.TopicName, $"%{name.Trim()}%"))
                    .Select(w => w.DtddTopicId)
                    .ToListAsync();
                req.ExcludeWarningTopicIds.AddRange(ids);
            }
            req.ExcludeWarningTopicIds = req.ExcludeWarningTopicIds.Distinct().ToList();

            // Exclude warning category names → category IDs
            foreach (var name in req.ExcludeWarningCategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                const string sql = @"
SELECT category_id FROM public.warning_categories
WHERE category_name ILIKE @name;";
                await using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    req.ExcludeWarningCategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.ExcludeWarningCategoryIds = req.ExcludeWarningCategoryIds.Distinct().ToList();

            // Exclude warning subcategory names → subcategory IDs
            foreach (var name in req.ExcludeWarningSubcategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                const string sql = @"
SELECT subcategory_id FROM public.warning_subcategories
WHERE subcategory_name ILIKE @name;";
                await using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    req.ExcludeWarningSubcategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.ExcludeWarningSubcategoryIds = req.ExcludeWarningSubcategoryIds.Distinct().ToList();

            return unresolved;
        }

        // =========================================================================
        // STALE WARNING REFRESH
        //
        // Two cases we fix:
        //   1. Released movies that have ZERO warnings at all → always re-check DTDD.
        //   2. Movies whose most-recent movie_warning.updated_at is older than
        //      WarningStalenessDays → re-check DTDD.
        //
        // We cap the batch per search call to avoid adding latency on busy searches.
        // =========================================================================

        private const int StaleRefreshBatchSize = 5;

        private async Task<int> RefreshStaleWarningsAsync(string dtddKey)
        {
            var now = DateTime.UtcNow;
            var staleThreshold = now.AddDays(-WarningStalenessDays);
            var refreshed = 0;

            // Case 1: Released movies with NO warning rows at all.
            // "Released" = release_year <= current year AND status is Released (or null, which we treat as released).
            var noWarningMovies = await _context.Movies
                .AsNoTracking()
                .Where(m =>
                    m.ReleaseYear <= now.Year &&
                    (m.Status == null || EF.Functions.ILike(m.Status, "released")) &&
                    !m.MovieWarnings.Any())
                .OrderBy(m => m.UpdatedAt) // oldest first
                .Take(StaleRefreshBatchSize)
                .ToListAsync();

            foreach (var movie in noWarningMovies)
            {
                try
                {
                    var ok = await TryEnrichWarningsFromDtddAsync(movie, dtddKey);
                    if (ok) refreshed++;
                }
                catch { /* never crash the search endpoint */ }
            }

            // Case 2: Movies whose warnings haven't been updated recently.
            // Find imdb_ids where the newest updated_at on movie_warnings is older than threshold.
            if (refreshed < StaleRefreshBatchSize)
            {
                var remaining = StaleRefreshBatchSize - refreshed;

                var staleImdbIds = await _context.MovieWarnings
                    .AsNoTracking()
                    .GroupBy(mw => mw.ImdbId)
                    .Where(g => g.Max(mw => mw.UpdatedAt) < staleThreshold)
                    .OrderBy(g => g.Max(mw => mw.UpdatedAt))
                    .Take(remaining)
                    .Select(g => g.Key)
                    .ToListAsync();

                foreach (var imdbId in staleImdbIds)
                {
                    var movie = await _context.Movies.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.ImdbId == imdbId);

                    if (movie == null) continue;

                    try
                    {
                        var ok = await TryEnrichWarningsFromDtddAsync(movie, dtddKey);
                        if (ok) refreshed++;
                    }
                    catch { /* never crash the search endpoint */ }
                }
            }

            return refreshed;
        }

        // =========================================================================
        // CLONE
        // =========================================================================

        private MovieSearchRequest Clone(MovieSearchRequest req) => new MovieSearchRequest
        {
            Take = req.Take,
            MinMatches = req.MinMatches,
            EnableApiFallback = req.EnableApiFallback,
            MaxApiAdds = req.MaxApiAdds,
            WatchRegion = req.WatchRegion,

            StreamingProviderIds = req.StreamingProviderIds.ToList(),
            ProviderMatchMode = req.ProviderMatchMode,

            GenreIds = req.GenreIds.ToList(),
            KeywordIds = req.KeywordIds.ToList(),
            PersonIds = req.PersonIds.ToList(),
            PersonRoles = req.PersonRoles.ToList(),
            TitleContains = req.TitleContains,

            IncludeWarningTopicIds = req.IncludeWarningTopicIds.ToList(),
            IncludeWarningCategoryIds = req.IncludeWarningCategoryIds.ToList(),
            IncludeWarningSubcategoryIds = req.IncludeWarningSubcategoryIds.ToList(),
            IncludeWarningMatchMode = req.IncludeWarningMatchMode,

            ExcludeWarningTopicIds = req.ExcludeWarningTopicIds.ToList(),
            ExcludeWarningCategoryIds = req.ExcludeWarningCategoryIds.ToList(),
            ExcludeWarningSubcategoryIds = req.ExcludeWarningSubcategoryIds.ToList(),

            // Name lists don't need to carry forward — IDs are already resolved.
            StreamingProviderNames = new(),
            GenreNames = new(),
            KeywordNames = new(),
            PersonNames = new(),
            IncludeWarningNames = new(),
            IncludeWarningCategoryNames = new(),
            IncludeWarningSubcategoryNames = new(),
            ExcludeWarningNames = new(),
            ExcludeWarningCategoryNames = new(),
            ExcludeWarningSubcategoryNames = new()
        };

        // =========================================================================
        // QUERY
        // TitleContains is case-insensitive via ILike (unchanged behaviour, now also
        // applies the same ILike approach consistently across all text comparisons).
        // =========================================================================

        private async Task<List<MovieSearchResultCard>> RunQuery(MovieSearchRequest req, int take)
        {
            IQueryable<Movie> q = _context.Movies.AsNoTracking();

            // GLOBAL RULES:
            // 1) Must have populated warnings.
            q = q.Where(m => m.MovieWarnings.Any(w => w.Answer != null));

            // 2) Must be streamable (not rent/buy).
            q = q.Where(m => m.MovieStreamings.Any(ms =>
                !EF.Functions.ILike(ms.OfferType, "rent") &&
                !EF.Functions.ILike(ms.OfferType, "buy")
            ));

            // Title (case-insensitive partial match via ILike)
            if (!string.IsNullOrWhiteSpace(req.TitleContains))
                q = q.Where(m => EF.Functions.ILike(m.Title!, $"%{req.TitleContains.Trim()}%"));

            // Providers (hard)
            if (req.StreamingProviderIds.Count > 0)
            {
                if (req.ProviderMatchMode == MatchMode.Any)
                {
                    q = q.Where(m =>
                        m.MovieStreamings.Any(ms =>
                            req.StreamingProviderIds.Contains(ms.TmdbProviderId) &&
                            !EF.Functions.ILike(ms.OfferType, "rent") &&
                            !EF.Functions.ILike(ms.OfferType, "buy")));
                }
                else
                {
                    foreach (var pid in req.StreamingProviderIds.Distinct())
                    {
                        var localPid = pid;
                        q = q.Where(m =>
                            m.MovieStreamings.Any(ms =>
                                ms.TmdbProviderId == localPid &&
                                !EF.Functions.ILike(ms.OfferType, "rent") &&
                                !EF.Functions.ILike(ms.OfferType, "buy")));
                    }
                }
            }

            // Genres (hard)
            if (req.GenreIds.Count > 0)
                q = q.Where(m => m.MovieGenres.Any(mg => req.GenreIds.Contains(mg.TmdbGenreId)));

            // Keywords (soft)
            if (req.KeywordIds.Count > 0)
                q = q.Where(m => m.MovieKeywords.Any(mk => req.KeywordIds.Contains(mk.TmdbKeywordId)));

            // People (hard) — role-aware filtering
            if (req.PersonIds.Count > 0)
            {
                var roleSet = (req.PersonRoles ?? new List<string>())
                    .Select(r => r.Trim().ToLowerInvariant())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToHashSet();

                var includeCast = roleSet.Count == 0 || roleSet.Contains("cast");
                var includeCrew = roleSet.Count == 0 || roleSet.Contains("crew")
                    || roleSet.Contains("director") || roleSet.Contains("writer")
                    || roleSet.Contains("producer");

                // Build the job name sets for specific roles
                var directorJobs = new List<string> { "Director" };
                var writerJobs   = new List<string> { "Writer", "Screenplay", "Story", "Characters", "Screenstory" };
                var producerJobs = new List<string> { "Producer", "Executive Producer", "Co-Producer" };

                q = q.Where(m =>
                    (includeCast && m.MovieCasts.Any(c => req.PersonIds.Contains(c.TmdbPersonId)))
                    ||
                    (includeCrew && m.MovieCrews.Any(c =>
                        req.PersonIds.Contains(c.TmdbPersonId) &&
                        (
                            // "crew" with no further restriction = any crew role
                            roleSet.Count == 0 || roleSet.Contains("crew")
                            || (roleSet.Contains("director") && c.Job != null && directorJobs.Contains(c.Job))
                            || (roleSet.Contains("writer")   && c.Job != null && writerJobs.Contains(c.Job))
                            || (roleSet.Contains("producer") && c.Job != null && producerJobs.Contains(c.Job))
                        )
                    ))
                );
            }

            // INCLUDE warnings (hard)
            if (req.IncludeWarningTopicIds.Count > 0)
            {
                if (req.IncludeWarningMatchMode == MatchMode.Any)
                {
                    q = q.Where(m =>
                        m.MovieWarnings.Any(w =>
                            req.IncludeWarningTopicIds.Contains(w.DtddTopicId) &&
                            w.Answer != null &&
                            EF.Functions.ILike(w.Answer, "yes%")));
                }
                else
                {
                    foreach (var tid in req.IncludeWarningTopicIds.Distinct())
                    {
                        var localTid = tid;
                        q = q.Where(m =>
                            m.MovieWarnings.Any(w =>
                                w.DtddTopicId == localTid &&
                                w.Answer != null &&
                                EF.Functions.ILike(w.Answer, "yes%")));
                    }
                }
            }

            // EXCLUDE warnings (hard)
            if (req.ExcludeWarningTopicIds.Count > 0)
            {
                q = q.Where(m =>
                    !m.MovieWarnings.Any(w =>
                        req.ExcludeWarningTopicIds.Contains(w.DtddTopicId) &&
                        w.Answer != null &&
                        EF.Functions.ILike(w.Answer, "yes%")));
            }

            return await q
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .Select(m => new MovieSearchResultCard
                {
                    ImdbId = m.ImdbId,
                    TmdbId = m.TmdbId,
                    Title = m.Title ?? "",
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .Take(take)
                .ToListAsync();
        }

        // =========================================================================
        // API FALLBACK (unchanged from original — only internal helper methods)
        // =========================================================================

        private static bool IsNonRentBuyOffer(string offerType)
        {
            if (string.IsNullOrWhiteSpace(offerType)) return false;
            var t = offerType.Trim().ToLowerInvariant();
            return t == "flatrate" || t == "free" || t == "ads";
        }

        private async Task<(int added, ApiFillStats stats)> TryApiFillAsync(MovieSearchRequest req)
        {
            var tmdbKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            var dtddKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");

            if (string.IsNullOrWhiteSpace(tmdbKey) || string.IsNullOrWhiteSpace(dtddKey))
                return (0, new ApiFillStats(0, 0, 0, 0, 0, 0, 0, 0, null, null));

            var candidateTmdbIds = await FetchTmdbCandidateIdsAsync(req, tmdbKey);
            if (candidateTmdbIds.Count == 0)
                return (0, new ApiFillStats(0, 0, 0, 0, 0, 0, 0, 0, null, null));

            int skippedMissingImdb = 0;
            int skippedAlreadyEligible = 0;
            int skippedStreamingEnrichFailed = 0;
            int skippedStillNotStreamable = 0;
            int skippedWarningsEnrichFailed = 0;
            int skippedStillNoWarnings = 0;
            string? exampleStillNoWarningsImdb = null;
            string? exampleWarningsEnrichFailedImdb = null;

            int added = 0;

            foreach (var tmdbId in candidateTmdbIds)
            {
                if (added >= req.MaxApiAdds)
                    break;

                var imdbId = await FetchTmdbImdbIdAsync(tmdbId, tmdbKey);
                if (string.IsNullOrWhiteSpace(imdbId))
                {
                    skippedMissingImdb++;
                    continue;
                }

                var hadWarningsBefore = await _context.MovieWarnings.AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);

                var hadStreamableBefore = await _context.MovieStreamings.AnyAsync(ms =>
                    ms.ImdbId == imdbId &&
                    ms.OfferType != null &&
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy"));

                var eligibleBefore = hadWarningsBefore && hadStreamableBefore;

                if (eligibleBefore)
                {
                    skippedAlreadyEligible++;
                    continue;
                }

                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);
                var wasNew = false;

                if (movie == null)
                {
                    var upserted = await UpsertMovieCoreFromTmdbAsync(tmdbId, imdbId, tmdbKey);
                    if (!upserted) continue;

                    movie = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);
                    if (movie == null) continue;

                    wasNew = true;
                }

                var hasStreamable = await _context.MovieStreamings.AnyAsync(ms =>
                    ms.ImdbId == imdbId &&
                    ms.OfferType != null &&
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy"));

                if (!hasStreamable)
                {
                    var ok = await TryEnrichStreamingFromTmdbAsync(movie, tmdbKey, req.WatchRegion);
                    if (!ok)
                    {
                        skippedStreamingEnrichFailed++;
                        continue;
                    }

                    hasStreamable = await _context.MovieStreamings.AnyAsync(ms =>
                        ms.ImdbId == imdbId &&
                        ms.OfferType != null &&
                        !EF.Functions.ILike(ms.OfferType, "rent") &&
                        !EF.Functions.ILike(ms.OfferType, "buy"));

                    if (!hasStreamable)
                    {
                        skippedStillNotStreamable++;
                        continue;
                    }
                }

                var hasWarnings = await _context.MovieWarnings.AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);
                if (!hasWarnings)
                {
                    var ok = await TryEnrichWarningsFromDtddAsync(movie, dtddKey);
                    if (!ok)
                    {
                        skippedWarningsEnrichFailed++;
                        if (exampleWarningsEnrichFailedImdb == null) exampleWarningsEnrichFailedImdb = imdbId;
                        continue;
                    }

                    hasWarnings = await _context.MovieWarnings.AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);
                    if (!hasWarnings)
                    {
                        skippedStillNoWarnings++;
                        if (exampleStillNoWarningsImdb == null) exampleStillNoWarningsImdb = imdbId;
                        continue;
                    }
                }

                if (req.GenreIds.Count > 0)
                {
                    var hasGenres = await _context.MovieGenres.AnyAsync(mg => mg.ImdbId == imdbId);
                    if (!hasGenres)
                        await TryEnrichGenresFromTmdbAsync(movie, tmdbKey);
                }

                if (req.KeywordIds.Count > 0)
                {
                    var hasKeywords = await _context.MovieKeywords.AnyAsync(mk => mk.ImdbId == imdbId);
                    if (!hasKeywords)
                        await TryEnrichKeywordsFromTmdbAsync(movie, tmdbKey);
                }

                if (req.PersonIds.Count > 0)
                {
                    var hasCastOrCrew =
                        await _context.MovieCasts.AnyAsync(mc => mc.ImdbId == imdbId) ||
                        await _context.MovieCrews.AnyAsync(mc => mc.ImdbId == imdbId);

                    if (!hasCastOrCrew)
                    {
                        await TryEnrichCastFromTmdbAsync(movie, tmdbKey);
                        await TryEnrichCrewFromTmdbAsync(movie, tmdbKey);
                    }
                }

                if (wasNew || !eligibleBefore)
                    added++;
            }

            var stats = new ApiFillStats(
                Candidates: candidateTmdbIds.Count,
                SkippedMissingImdb: skippedMissingImdb,
                SkippedAlreadyEligible: skippedAlreadyEligible,
                SkippedStreamingEnrichFailed: skippedStreamingEnrichFailed,
                SkippedStillNotStreamable: skippedStillNotStreamable,
                SkippedWarningsEnrichFailed: skippedWarningsEnrichFailed,
                SkippedStillNoWarnings: skippedStillNoWarnings,
                Added: added,
                ExampleStillNoWarningsImdb: exampleStillNoWarningsImdb,
                ExampleWarningsEnrichFailedImdb: exampleWarningsEnrichFailedImdb
            );

            return (added, stats);
        }

        private async Task<List<int>> FetchTmdbCandidateIdsAsync(MovieSearchRequest req, string apiKey)
        {
            // Title search takes priority
            if (!string.IsNullOrWhiteSpace(req.TitleContains))
                return await FetchTmdbSearchMovieIdsAsync(req.TitleContains!, apiKey);

            // When filtering by people, use the person credits endpoint instead of
            // discover — discover's with_people only matches crew, not cast members.
            if (req.PersonIds.Count > 0)
            {
                var personMovieIds = new List<int>();
                foreach (var personId in req.PersonIds.Distinct())
                {
                    var credits = await FetchTmdbPersonMovieCreditsAsync(personId, apiKey, req.PersonRoles);
                    personMovieIds.AddRange(credits);
                }

                // If we also have genre/provider filters, intersect with a discover
                // call so we don't flood the DB with every movie a person has ever
                // appeared in.
                if (req.GenreIds.Count > 0 || req.StreamingProviderIds.Count > 0)
                {
                    var discoverIds = await FetchTmdbDiscoverMovieIdsAsync(req, apiKey);
                    var discoverSet = new HashSet<int>(discoverIds);
                    return personMovieIds.Distinct().Where(id => discoverSet.Contains(id)).ToList();
                }

                return personMovieIds.Distinct().Take(150).ToList();
            }

            return await FetchTmdbDiscoverMovieIdsAsync(req, apiKey);
        }

        /// <summary>
        /// Fetches movie IDs for a given TMDB person, optionally filtered by role.
        /// roles: empty = all cast + crew; "cast" = acting only; "crew" = all crew;
        ///        "director"/"writer"/"producer" = specific crew jobs.
        /// </summary>
        private async Task<List<int>> FetchTmdbPersonMovieCreditsAsync(
            int tmdbPersonId, string apiKey, List<string>? roles = null)
        {
            try
            {
                using var http = new HttpClient();
                var url = $"https://api.themoviedb.org/3/person/{tmdbPersonId}/movie_credits?api_key={apiKey}";

                using var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return new List<int>();

                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var ids = new List<int>();

                // Normalise role filters to lowercase for comparison
                var roleSet = (roles ?? new List<string>())
                    .Select(r => r.Trim().ToLowerInvariant())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToHashSet();

                var includeCast = roleSet.Count == 0 || roleSet.Contains("cast");
                var includeCrew = roleSet.Count == 0 || roleSet.Contains("crew")
                    || roleSet.Contains("director") || roleSet.Contains("writer")
                    || roleSet.Contains("producer");

                // Specific job sets used when caller asks for a named role
                var directorJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { "Director" };
                var writerJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { "Writer", "Screenplay", "Story", "Characters", "Screenstory" };
                var producerJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { "Producer", "Executive Producer", "Co-Producer" };

                // Cast credits (acting roles)
                if (includeCast &&
                    root.TryGetProperty("cast", out var castEl) &&
                    castEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var m in castEl.EnumerateArray())
                    {
                        if (m.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                            ids.Add(idVal);
                    }
                }

                // Crew credits — apply job filter if specific roles were requested
                if (includeCrew &&
                    root.TryGetProperty("crew", out var crewEl) &&
                    crewEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var m in crewEl.EnumerateArray())
                    {
                        if (!m.TryGetProperty("id", out var idEl) || !idEl.TryGetInt32(out var idVal))
                            continue;

                        // If caller asked for "crew" broadly or no filter, include everything
                        if (roleSet.Count == 0 || roleSet.Contains("crew"))
                        {
                            ids.Add(idVal);
                            continue;
                        }

                        // Otherwise only include if the job matches a requested role
                        var job = m.TryGetProperty("job", out var jobEl) ? jobEl.GetString() ?? "" : "";

                        if (roleSet.Contains("director") && directorJobs.Contains(job))
                            ids.Add(idVal);
                        else if (roleSet.Contains("writer") && writerJobs.Contains(job))
                            ids.Add(idVal);
                        else if (roleSet.Contains("producer") && producerJobs.Contains(job))
                            ids.Add(idVal);
                    }
                }

                return ids.Distinct().ToList();
            }
            catch
            {
                return new List<int>();
            }
        }

        private async Task<List<int>> FetchTmdbSearchMovieIdsAsync(string query, string apiKey)
        {
            using var http = new HttpClient();

            var ids = new List<int>();
            for (int page = 1; page <= 2; page++)
            {
                var url =
                    $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}" +
                    $"&query={Uri.EscapeDataString(query)}&include_adult=false&page={page}";

                var json = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array)
                    break;

                foreach (var r in resultsEl.EnumerateArray())
                {
                    if (r.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        ids.Add(idVal);
                }

                if (ids.Count >= 40)
                    break;
            }

            return ids.Distinct().Take(40).ToList();
        }

        private async Task<List<int>> FetchTmdbDiscoverMovieIdsAsync(MovieSearchRequest req, string apiKey)
        {
            using var http = new HttpClient();

            var cutoff = DateTime.UtcNow.AddMonths(-5).ToString("yyyy-MM-dd");

            var ids = new List<int>();

            for (int page = 1; page <= 5; page++)
            {
                var baseUrl = "https://api.themoviedb.org/3/discover/movie";

                var query = new List<KeyValuePair<string, string>>
                {
                    new("api_key", apiKey),
                    new("include_adult", "false"),
                    new("include_video", "false"),
                    new("sort_by", "popularity.desc"),
                    new("page", page.ToString()),
                    new("primary_release_date.lte", cutoff)
                };

                if (req.GenreIds.Count > 0)
                    query.Add(new("with_genres", string.Join(",", req.GenreIds.Distinct())));

                if (req.KeywordIds.Count > 0)
                    query.Add(new("with_keywords", string.Join(",", req.KeywordIds.Distinct())));

                if (req.PersonIds.Count > 0)
                    query.Add(new("with_people", string.Join(",", req.PersonIds.Distinct())));

                if (req.StreamingProviderIds.Count > 0)
                {
                    query.Add(new("watch_region", req.WatchRegion));
                    query.Add(new("with_watch_providers", string.Join("|", req.StreamingProviderIds.Distinct())));
                    query.Add(new("with_watch_monetization_types", "flatrate|free|ads"));
                }

                var qs = string.Join("&", query.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

                var url = $"{baseUrl}?{qs}";

                try
                {
                    using var resp = await http.GetAsync(url);
                    if (!resp.IsSuccessStatusCode)
                        return ids.Distinct().Take(150).ToList();

                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);

                    if (!doc.RootElement.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array)
                        break;

                    foreach (var r in resultsEl.EnumerateArray())
                    {
                        if (r.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                            ids.Add(idVal);
                    }

                    if (ids.Count >= 150)
                        break;
                }
                catch
                {
                    return ids.Distinct().Take(150).ToList();
                }
            }

            return ids.Distinct().Take(150).ToList();
        }

        private async Task<string?> FetchTmdbImdbIdAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/external_ids?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return root.TryGetProperty("imdb_id", out var imdbEl) ? imdbEl.GetString() : null;
        }

        private sealed class TmdbDetailsBasic
        {
            public string? Title { get; set; }
            public int? ReleaseYear { get; set; }
            public int? RuntimeMinutes { get; set; }
            public string? PlotSummary { get; set; }
            public string? PosterUrl { get; set; }
            public string? OriginalLanguage { get; set; }
            public string? Tagline { get; set; }
            public string? Status { get; set; }
        }

        private async Task<TmdbDetailsBasic?> FetchTmdbDetailsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var title = root.TryGetProperty("title", out var titleEl) ? titleEl.GetString() : null;

            int? releaseYear = null;
            if (root.TryGetProperty("release_date", out var rdEl))
            {
                var rd = rdEl.GetString();
                if (!string.IsNullOrWhiteSpace(rd) && rd.Length >= 4 && int.TryParse(rd.Substring(0, 4), out var y))
                    releaseYear = y;
            }

            int? runtime = null;
            if (root.TryGetProperty("runtime", out var rtEl) && rtEl.TryGetInt32(out var rtVal))
                runtime = rtVal;

            var overview = root.TryGetProperty("overview", out var ovEl) ? ovEl.GetString() : null;

            string? posterUrl = null;
            if (root.TryGetProperty("poster_path", out var ppEl))
            {
                var p = ppEl.GetString();
                if (!string.IsNullOrWhiteSpace(p))
                    posterUrl = $"https://image.tmdb.org/t/p/w500{p}";
            }

            var lang = root.TryGetProperty("original_language", out var langEl) ? langEl.GetString() : null;
            var tagline = root.TryGetProperty("tagline", out var tgEl) ? tgEl.GetString() : null;
            var status = root.TryGetProperty("status", out var stEl) ? stEl.GetString() : null;

            return new TmdbDetailsBasic
            {
                Title = title,
                ReleaseYear = releaseYear,
                RuntimeMinutes = runtime,
                PlotSummary = overview,
                PosterUrl = posterUrl,
                OriginalLanguage = lang,
                Tagline = tagline,
                Status = status
            };
        }

        private async Task<bool> UpsertMovieCoreFromTmdbAsync(int tmdbId, string imdbId, string apiKey)
        {
            var details = await FetchTmdbDetailsAsync(tmdbId, apiKey);
            if (details == null)
                return false;

            int? runtimeSafe = null;
            if (details.RuntimeMinutes != null)
            {
                var rt = details.RuntimeMinutes.Value;
                if (rt > 0 && rt <= 600)
                    runtimeSafe = rt;
            }

            var now = DateTime.UtcNow;
            var existing = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);

            if (existing == null)
            {
                if (string.IsNullOrWhiteSpace(details.Title) || details.ReleaseYear == null)
                    return false;

                _context.Movies.Add(new Movie
                {
                    ImdbId = imdbId,
                    TmdbId = tmdbId,
                    Title = details.Title!,
                    ReleaseYear = details.ReleaseYear.Value,
                    RuntimeMinutes = runtimeSafe,
                    PlotSummary = details.PlotSummary,
                    PosterUrl = details.PosterUrl,
                    OriginalLanguage = details.OriginalLanguage,
                    MediaType = "movie",
                    Tagline = details.Tagline,
                    Status = details.Status,
                    MpaaRating = null,
                    CreatedAt = now,
                    UpdatedAt = now
                });

                await _context.SaveChangesAsync();
                return true;
            }

            existing.TmdbId = tmdbId;
            if (!string.IsNullOrWhiteSpace(details.Title)) existing.Title = details.Title;
            if (details.ReleaseYear != null) existing.ReleaseYear = details.ReleaseYear.Value;

            existing.RuntimeMinutes = runtimeSafe;
            existing.PlotSummary = details.PlotSummary;
            existing.PosterUrl = details.PosterUrl;
            existing.OriginalLanguage = details.OriginalLanguage;
            existing.Tagline = details.Tagline;
            existing.Status = details.Status;
            existing.UpdatedAt = now;

            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------------
        // Streaming enrichment (TMDB)
        // -------------------------

        private sealed class TmdbWatchProviderRow
        {
            public int ProviderId { get; set; }
            public string? ProviderName { get; set; }
            public string? OfferType { get; set; }
        }

        private async Task<bool> TryEnrichStreamingFromTmdbAsync(Movie movie, string apiKey, string watchRegion)
        {
            var tmdbId = movie.TmdbId;
            if (!tmdbId.HasValue || tmdbId.Value <= 0)
                return false;

            var providers = (await FetchTmdbWatchProvidersAsync(tmdbId.Value, apiKey, watchRegion))?.ToList();
            if (providers == null || providers.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existingLinks = await _context.MovieStreamings
                .Where(ms => ms.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieStreamings.RemoveRange(existingLinks);

            var providerIds = providers
                .Where(p => p.ProviderId > 0 && !string.IsNullOrWhiteSpace(p.ProviderName))
                .Select(p => p.ProviderId)
                .Distinct()
                .ToList();

            var existingProviders = await _context.StreamingProviders
                .Where(sp => providerIds.Contains(sp.TmdbProviderId))
                .ToDictionaryAsync(sp => sp.TmdbProviderId);

            foreach (var pid in providerIds)
            {
                var name = providers.First(p => p.ProviderId == pid).ProviderName!;

                if (!existingProviders.TryGetValue(pid, out var sp))
                {
                    sp = new StreamingProvider
                    {
                        TmdbProviderId = pid,
                        ProviderName = name,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    _context.StreamingProviders.Add(sp);
                    existingProviders[pid] = sp;
                }
                else
                {
                    if (sp.ProviderName != name)
                        sp.ProviderName = name;

                    sp.UpdatedAt = now;
                }
            }

            foreach (var p in providers)
            {
                if (p.ProviderId <= 0 || string.IsNullOrWhiteSpace(p.ProviderName) || string.IsNullOrWhiteSpace(p.OfferType))
                    continue;

                _context.MovieStreamings.Add(new MovieStreaming
                {
                    ImdbId = movie.ImdbId,
                    TmdbProviderId = p.ProviderId,
                    OfferType = p.OfferType!,
                    CreatedAt = now
                });
            }

            movie.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<List<TmdbWatchProviderRow>?> FetchTmdbWatchProvidersAsync(int tmdbId, string apiKey, string watchRegion)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/watch/providers?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Object)
                return new List<TmdbWatchProviderRow>();

            if (!resultsEl.TryGetProperty(watchRegion.ToUpperInvariant(), out var regEl) || regEl.ValueKind != JsonValueKind.Object)
                return new List<TmdbWatchProviderRow>();

            var map = new List<(string bucket, string offerType)>
            {
                ("flatrate", "subscription"),
                ("free", "free"),
                ("ads", "free_with_ads"),
                ("rent", "rent"),
                ("buy", "buy")
            };

            var list = new List<TmdbWatchProviderRow>();

            foreach (var (bucket, offerType) in map)
            {
                if (!regEl.TryGetProperty(bucket, out var arr) || arr.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (var p in arr.EnumerateArray())
                {
                    var id = p.TryGetProperty("provider_id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = p.TryGetProperty("provider_name", out var nameEl) ? nameEl.GetString() : null;

                    if (id > 0 && !string.IsNullOrWhiteSpace(name))
                    {
                        list.Add(new TmdbWatchProviderRow
                        {
                            ProviderId = id,
                            ProviderName = name,
                            OfferType = offerType
                        });
                    }
                }
            }

            return list
                .GroupBy(x => new { x.ProviderId, x.OfferType })
                .Select(g => g.First())
                .ToList();
        }

        // -------------------------
        // Warnings enrichment (DTDD)
        // -------------------------

        private sealed class DtddTopicStatRow
        {
            public int TopicId { get; set; }
            public string Answer { get; set; } = "unknown";
            public bool? IsSpoiler { get; set; }
            public string? Comment { get; set; }
        }

        private async Task<bool> TryEnrichWarningsFromDtddAsync(Movie movie, string apiKey)
        {
            var imdbId = movie.ImdbId;
            var tmdbId = movie.TmdbId;
            var title = movie.Title;
            var releaseYear = movie.ReleaseYear;

            int? dtddMediaId = null;

            var overrideRow = await _context.DtddOverrides.AsNoTracking()
                .FirstOrDefaultAsync(o => o.ImdbId == imdbId);

            if (overrideRow != null && overrideRow.DtddMediaId > 0)
                dtddMediaId = overrideRow.DtddMediaId;

            List<DtddTopicStatRow> stats;

            try
            {
                if (dtddMediaId == null && !string.IsNullOrWhiteSpace(imdbId))
                    dtddMediaId = await FetchDtddMediaIdByImdbAsync(imdbId, apiKey);

                if (dtddMediaId == null && tmdbId.HasValue && tmdbId.Value > 0)
                    dtddMediaId = await FetchDtddMediaIdByTmdbAsync(tmdbId.Value, apiKey);

                if (dtddMediaId == null && !string.IsNullOrWhiteSpace(title) && releaseYear > 0)
                    dtddMediaId = await FetchDtddMediaIdByTitleYearAsync(title, releaseYear, apiKey);

                if (dtddMediaId == null || string.IsNullOrWhiteSpace(imdbId))
                    return false;

                stats = await FetchDtddTopicStatsAsync(dtddMediaId.Value, apiKey);
            }
            catch
            {
                return false;
            }

            if (stats.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existing = await _context.MovieWarnings
                .Where(mw => mw.ImdbId == imdbId)
                .ToListAsync();

            if (existing.Count > 0)
                _context.MovieWarnings.RemoveRange(existing);

            var knownTopicIds = (await _context.Warnings
                    .AsNoTracking()
                    .Select(w => w.DtddTopicId)
                    .ToListAsync())
                .ToHashSet();

            foreach (var s in stats)
            {
                if (!knownTopicIds.Contains(s.TopicId))
                    continue;

                _context.MovieWarnings.Add(new MovieWarning
                {
                    ImdbId = imdbId,
                    DtddTopicId = s.TopicId,
                    Answer = s.Answer,
                    IsSpoiler = s.IsSpoiler,
                    WarningComment = s.Comment,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<int?> FetchDtddMediaIdByImdbAsync(string imdbId, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var url = $"https://www.doesthedogdie.com/dddsearch?imdb={Uri.EscapeDataString(imdbId)}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("items", out var itemsEl) || itemsEl.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var item in itemsEl.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                    return idVal;
            }

            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTmdbAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var url = $"https://www.doesthedogdie.com/dddsearch?tmdb={tmdbId}";

            HttpResponseMessage resp;
            try
            {
                resp = await http.GetAsync(url);
            }
            catch
            {
                return null;
            }

            if (!resp.IsSuccessStatusCode)
                return null;

            var contentType = resp.Content.Headers.ContentType?.MediaType ?? "";
            var text = await resp.Content.ReadAsStringAsync();

            if (!contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(text) && text.TrimStart().StartsWith("<", StringComparison.Ordinal))
                    return null;
                return null;
            }

            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;

            if (!root.TryGetProperty("items", out var itemsEl) || itemsEl.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var item in itemsEl.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                    return idVal;
            }

            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTitleYearAsync(string title, int releaseYear, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var q = Uri.EscapeDataString(title);
            var url = $"https://www.doesthedogdie.com/dddsearch?q={q}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("items", out var itemsEl) || itemsEl.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var item in itemsEl.EnumerateArray())
            {
                if (item.TryGetProperty("releaseYear", out var yEl) && yEl.ValueKind == JsonValueKind.Number)
                {
                    if (yEl.GetInt32() != releaseYear)
                        continue;
                }

                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                    return idVal;
            }

            foreach (var item in itemsEl.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                    return idVal;
            }

            return null;
        }

        private async Task<List<DtddTopicStatRow>> FetchDtddTopicStatsAsync(int dtddMediaId, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var url = $"https://www.doesthedogdie.com/media/{dtddMediaId}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var list = new List<DtddTopicStatRow>();

            if (!root.TryGetProperty("topicItemStats", out var statsEl) || statsEl.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var s in statsEl.EnumerateArray())
            {
                var topicId = s.TryGetProperty("TopicId", out var tidEl) && tidEl.TryGetInt32(out var tidVal) ? tidVal : 0;
                if (topicId <= 0)
                    continue;

                var comment = s.TryGetProperty("comment", out var cEl) ? cEl.GetString() : null;

                bool? isSpoiler = null;
                if (s.TryGetProperty("topic", out var topicEl) && topicEl.ValueKind == JsonValueKind.Object)
                {
                    if (topicEl.TryGetProperty("isSpoiler", out var spEl) &&
                        (spEl.ValueKind == JsonValueKind.True || spEl.ValueKind == JsonValueKind.False))
                        isSpoiler = spEl.GetBoolean();
                }

                int? yesSum = null;
                int? noSum = null;
                int? isYes = null;

                if (s.TryGetProperty("yesSum", out var yesSumEl) && yesSumEl.ValueKind == JsonValueKind.Number)
                    yesSum = yesSumEl.GetInt32();
                if (s.TryGetProperty("noSum", out var noSumEl) && noSumEl.ValueKind == JsonValueKind.Number)
                    noSum = noSumEl.GetInt32();
                if (s.TryGetProperty("isYes", out var isYesEl) && isYesEl.ValueKind == JsonValueKind.Number)
                    isYes = isYesEl.GetInt32();

                string answer;
                if (yesSum.HasValue || noSum.HasValue)
                {
                    var y = yesSum ?? 0;
                    var n = noSum ?? 0;
                    if (y == 0 && n == 0)
                        answer = "unknown";
                    else
                        answer = (y >= n && y > 0) ? "yes" : "no";
                }
                else if (isYes.HasValue)
                {
                    answer = isYes.Value == 1 ? "yes" : "no";
                }
                else
                {
                    answer = "unknown";
                }

                list.Add(new DtddTopicStatRow
                {
                    TopicId = topicId,
                    Answer = answer,
                    IsSpoiler = isSpoiler,
                    Comment = comment
                });
            }

            return list;
        }

        // -------------------------
        // Genres enrichment (TMDB)
        // -------------------------

        private async Task<bool> TryEnrichGenresFromTmdbAsync(Movie movie, string apiKey)
        {
            var tmdbId = movie.TmdbId;
            if (!tmdbId.HasValue || tmdbId.Value <= 0)
                return false;

            var genreIds = (await FetchTmdbGenreIdsAsync(tmdbId.Value, apiKey))?.ToList() ?? new List<int>();
            if (genreIds.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existingLinks = await _context.MovieGenres
                .Where(mg => mg.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieGenres.RemoveRange(existingLinks);

            foreach (var gid in genreIds.Distinct())
            {
                _context.MovieGenres.Add(new MovieGenre
                {
                    ImdbId = movie.ImdbId,
                    TmdbGenreId = gid,
                    CreatedAt = now
                });
            }

            movie.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<List<int>> FetchTmdbGenreIdsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var ids = new List<int>();

            if (root.TryGetProperty("genres", out var genresEl) && genresEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var g in genresEl.EnumerateArray())
                {
                    if (g.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        ids.Add(idVal);
                }
            }

            return ids;
        }

        // -------------------------
        // Keywords enrichment (TMDB)
        // -------------------------

        private async Task<bool> TryEnrichKeywordsFromTmdbAsync(Movie movie, string apiKey)
        {
            var tmdbId = movie.TmdbId;
            if (!tmdbId.HasValue || tmdbId.Value <= 0)
                return false;

            var keywords = (await FetchTmdbKeywordsAsync(tmdbId.Value, apiKey))?.ToList() ?? new List<TmdbKeywordRow>();
            if (keywords.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existingLinks = await _context.MovieKeywords
                .Where(mk => mk.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieKeywords.RemoveRange(existingLinks);

            foreach (var k in keywords)
            {
                if (k.KeywordId <= 0 || string.IsNullOrWhiteSpace(k.KeywordName))
                    continue;

                var existingKeyword = await _context.Keywords
                    .FirstOrDefaultAsync(x => x.TmdbKeywordId == k.KeywordId);

                if (existingKeyword == null)
                {
                    _context.Keywords.Add(new Keyword
                    {
                        TmdbKeywordId = k.KeywordId,
                        KeywordName = k.KeywordName!,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (existingKeyword.KeywordName != k.KeywordName)
                        existingKeyword.KeywordName = k.KeywordName!;
                    existingKeyword.UpdatedAt = now;
                }

                _context.MovieKeywords.Add(new MovieKeyword
                {
                    ImdbId = movie.ImdbId,
                    TmdbKeywordId = k.KeywordId,
                    CreatedAt = now
                });
            }

            movie.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private sealed class TmdbKeywordRow
        {
            public int KeywordId { get; set; }
            public string? KeywordName { get; set; }
        }

        private async Task<List<TmdbKeywordRow>> FetchTmdbKeywordsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/keywords?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var list = new List<TmdbKeywordRow>();

            if (!root.TryGetProperty("keywords", out var kwEl) || kwEl.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var k in kwEl.EnumerateArray())
            {
                var id = k.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                var name = k.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;

                if (id > 0 && !string.IsNullOrWhiteSpace(name))
                {
                    list.Add(new TmdbKeywordRow
                    {
                        KeywordId = id,
                        KeywordName = name
                    });
                }
            }

            return list;
        }

        // -------------------------
        // People enrichment (TMDB credits)
        // -------------------------

        private sealed class TmdbCredits
        {
            public List<TmdbCastRow> Cast { get; set; } = new();
            public List<TmdbCrewRow> Crew { get; set; } = new();
        }

        private sealed class TmdbCastRow
        {
            public int PersonId { get; set; }
            public string? Name { get; set; }
            public string? Character { get; set; }
            public int? Order { get; set; }
            public string? CreditId { get; set; }
        }

        private sealed class TmdbCrewRow
        {
            public int PersonId { get; set; }
            public string? Name { get; set; }
            public string? Department { get; set; }
            public string? Job { get; set; }
            public string? CreditId { get; set; }
        }

        private async Task<TmdbCredits?> FetchTmdbCreditsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/credits?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var credits = new TmdbCredits();

            if (root.TryGetProperty("cast", out var castEl) && castEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var c in castEl.EnumerateArray())
                {
                    var pid = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = c.TryGetProperty("name", out var nEl) ? nEl.GetString() : null;
                    var character = c.TryGetProperty("character", out var chEl) ? chEl.GetString() : null;
                    int? order = c.TryGetProperty("order", out var oEl) && oEl.TryGetInt32(out var oVal) ? oVal : (int?)null;
                    var creditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null;

                    if (pid > 0)
                    {
                        credits.Cast.Add(new TmdbCastRow
                        {
                            PersonId = pid,
                            Name = name,
                            Character = character,
                            Order = order,
                            CreditId = creditId
                        });
                    }
                }
            }

            if (root.TryGetProperty("crew", out var crewEl) && crewEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var c in crewEl.EnumerateArray())
                {
                    var pid = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = c.TryGetProperty("name", out var nEl) ? nEl.GetString() : null;
                    var dept = c.TryGetProperty("department", out var dEl) ? dEl.GetString() : null;
                    var job = c.TryGetProperty("job", out var jEl) ? jEl.GetString() : null;
                    var creditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null;

                    if (pid > 0)
                    {
                        credits.Crew.Add(new TmdbCrewRow
                        {
                            PersonId = pid,
                            Name = name,
                            Department = dept,
                            Job = job,
                            CreditId = creditId
                        });
                    }
                }
            }

            return credits;
        }

        private async Task UpsertPeopleFromCreditsAsync(TmdbCredits credits, DateTime now)
        {
            var ids = credits.Cast.Select(c => c.PersonId)
                .Concat(credits.Crew.Select(c => c.PersonId))
                .Distinct()
                .ToList();

            if (ids.Count == 0)
                return;

            var existing = await _context.People
                .Where(p => ids.Contains(p.TmdbPersonId))
                .ToListAsync();

            var existingMap = existing.ToDictionary(x => x.TmdbPersonId, x => x);

            foreach (var id in ids)
            {
                var name = credits.Cast.FirstOrDefault(x => x.PersonId == id)?.Name
                           ?? credits.Crew.FirstOrDefault(x => x.PersonId == id)?.Name;

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                if (!existingMap.TryGetValue(id, out var person))
                {
                    _context.People.Add(new Person
                    {
                        TmdbPersonId = id,
                        PersonName = name!,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (person.PersonName != name)
                        person.PersonName = name!;
                    person.UpdatedAt = now;
                }
            }
        }

        private async Task<bool> TryEnrichCastFromTmdbAsync(Movie movie, string apiKey)
        {
            var tmdbId = movie.TmdbId;
            if (!tmdbId.HasValue || tmdbId.Value <= 0)
                return false;

            var credits = await FetchTmdbCreditsAsync(tmdbId.Value, apiKey);
            if (credits == null || credits.Cast == null || credits.Cast.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            await UpsertPeopleFromCreditsAsync(credits, now);
            await _context.SaveChangesAsync();

            var existingCastLinks = await _context.MovieCasts
                .Where(mc => mc.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingCastLinks.Count > 0)
                _context.MovieCasts.RemoveRange(existingCastLinks);

            foreach (var c in credits.Cast)
            {
                if (c.PersonId <= 0 || string.IsNullOrWhiteSpace(c.CreditId))
                    continue;

                _context.MovieCasts.Add(new MovieCast
                {
                    TmdbCreditId = c.CreditId!,
                    ImdbId = movie.ImdbId,
                    TmdbPersonId = c.PersonId,
                    CharacterName = c.Character,
                    CastOrder = c.Order,
                    CreatedAt = now
                });
            }

            movie.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<bool> TryEnrichCrewFromTmdbAsync(Movie movie, string apiKey)
        {
            var tmdbId = movie.TmdbId;
            if (tmdbId == null || tmdbId <= 0)
                return false;

            var credits = await FetchTmdbCreditsAsync(tmdbId.Value, apiKey);
            if (credits == null || credits.Crew == null || credits.Crew.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            await UpsertPeopleFromCreditsAsync(credits, now);
            await _context.SaveChangesAsync();

            var existingCrewLinks = await _context.MovieCrews
                .Where(mc => mc.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingCrewLinks.Count > 0)
                _context.MovieCrews.RemoveRange(existingCrewLinks);

            var priorityJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Director",
                "Writer",
                "Screenplay",
                "Story",
                "Characters",
                "Producer",
                "Executive Producer",
                "Director of Photography",
                "Original Music Composer",
                "Editor"
            };

            var ordered = credits.Crew
                .Where(c => c.PersonId > 0 && !string.IsNullOrWhiteSpace(c.CreditId))
                .OrderByDescending(c => c.Job != null && priorityJobs.Contains(c.Job) ? 1 : 0)
                .ThenBy(c => c.Department ?? "")
                .ThenBy(c => c.Job ?? "")
                .Take(CrewLimit)
                .ToList();

            foreach (var c in ordered)
            {
                _context.MovieCrews.Add(new MovieCrew
                {
                    TmdbCreditId = c.CreditId!,
                    ImdbId = movie.ImdbId,
                    TmdbPersonId = c.PersonId,
                    Department = c.Department,
                    Job = c.Job,
                    CreatedAt = now
                });
            }

            movie.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        // =========================================================================
        // TMDB PERSON SEARCH + UPSERT
        // Called when a personName isn't found in our local people table.
        // Searches TMDB, upserts matching people rows, returns the TMDB person IDs
        // that were added so the caller can re-query our DB.
        // =========================================================================

        private async Task<List<int>> FetchAndUpsertTmdbPersonsByNameAsync(string name, string apiKey)
        {
            try
            {
                using var http = new HttpClient();
                var url = $"https://api.themoviedb.org/3/search/person?api_key={apiKey}" +
                          $"&query={Uri.EscapeDataString(name)}&include_adult=false&page=1";

                using var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return new List<int>();

                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array)
                    return new List<int>();

                var now = DateTime.UtcNow;
                var upsertedIds = new List<int>();

                foreach (var r in resultsEl.EnumerateArray())
                {
                    if (!r.TryGetProperty("id", out var idEl) || !idEl.TryGetInt32(out var tmdbPersonId))
                        continue;

                    var personName = r.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                    if (string.IsNullOrWhiteSpace(personName)) continue;

                    var knownFor = r.TryGetProperty("known_for_department", out var kfEl) ? kfEl.GetString() : null;
                    var profilePath = r.TryGetProperty("profile_path", out var ppEl) ? ppEl.GetString() : null;
                    string? profileUrl = !string.IsNullOrWhiteSpace(profilePath)
                        ? $"https://image.tmdb.org/t/p/w185{profilePath}"
                        : null;

                    var existing = await _context.People
                        .FirstOrDefaultAsync(p => p.TmdbPersonId == tmdbPersonId);

                    if (existing == null)
                    {
                        _context.People.Add(new Person
                        {
                            TmdbPersonId = tmdbPersonId,
                            PersonName = personName!,
                            KnownForDepartment = knownFor,
                            ProfileUrl = profileUrl,
                            CreatedAt = now,
                            UpdatedAt = now
                        });
                    }
                    else
                    {
                        existing.PersonName = personName!;
                        existing.KnownForDepartment = knownFor;
                        if (!string.IsNullOrWhiteSpace(profileUrl))
                            existing.ProfileUrl = profileUrl;
                        existing.UpdatedAt = now;
                    }

                    upsertedIds.Add(tmdbPersonId);
                }

                if (upsertedIds.Count > 0)
                    await _context.SaveChangesAsync();

                return upsertedIds;
            }
            catch
            {
                // Never crash the search endpoint over a person lookup
                return new List<int>();
            }
        }

                // =========================================================================
        // UMBRELLA TIER EXPANSION: category/subcategory → topic IDs
        // =========================================================================

        private async Task<List<int>> ResolveTopicIdsAsync(
            List<int> topicIds,
            List<int> categoryIds,
            List<int> subcategoryIds)
        {
            var set = new HashSet<int>(topicIds.Distinct());

            if (categoryIds.Count == 0 && subcategoryIds.Count == 0)
                return set.OrderBy(x => x).ToList();

            var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            if (categoryIds.Count > 0)
            {
                const string sqlCat = @"
SELECT DISTINCT wct.dtdd_topic_id
FROM public.warning_category_topics wct
WHERE wct.category_id = ANY(@categoryIds);";

                await using var cmd = new NpgsqlCommand(sqlCat, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@categoryIds", categoryIds.Distinct().ToArray());

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    set.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }

            if (subcategoryIds.Count > 0)
            {
                const string sqlSub = @"
SELECT DISTINCT wst.dtdd_topic_id
FROM public.warning_subcategory_topics wst
WHERE wst.subcategory_id = ANY(@subcategoryIds);";

                await using var cmd = new NpgsqlCommand(sqlSub, (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@subcategoryIds", subcategoryIds.Distinct().ToArray());

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    set.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }

            return set.OrderBy(x => x).ToList();
        }
    }

    // =========================================================================
    // EXTERNAL API TEST CONTROLLER (kept in the same file so Swagger sees it)
    // =========================================================================
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalApiTestController : ControllerBase
    {
        [HttpGet("tmdb/{tmdbId:int}")]
        public async Task<IActionResult> TestTmdb(int tmdbId)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest("TMDB_API_KEY is not set for this terminal/session.");

            using var http = new HttpClient();

            var detailsUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}";
            var detailsJson = await http.GetStringAsync(detailsUrl);

            using var detailsDoc = JsonDocument.Parse(detailsJson);
            var root = detailsDoc.RootElement;

            string? title = root.TryGetProperty("title", out var titleEl) ? titleEl.GetString() : null;
            string? releaseDate = root.TryGetProperty("release_date", out var rdEl) ? rdEl.GetString() : null;

            var extUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}/external_ids?api_key={apiKey}";
            var extJson = await http.GetStringAsync(extUrl);

            using var extDoc = JsonDocument.Parse(extJson);
            var extRoot = extDoc.RootElement;

            string? imdbId = extRoot.TryGetProperty("imdb_id", out var imdbEl) ? imdbEl.GetString() : null;

            return Ok(new { tmdbId, title, releaseDate, imdbId });
        }

        [HttpGet("dtdd/title/{dtddTitleId:int}")]
        public async Task<IActionResult> TestDtddTitle(int dtddTitleId)
        {
            var apiKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest("DTDD_API_KEY is not set for this terminal/session.");

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var url = $"https://www.doesthedogdie.com/media/{dtddTitleId}";
            var json = await http.GetStringAsync(url);

            return Content(json, "application/json");
        }
    }
}