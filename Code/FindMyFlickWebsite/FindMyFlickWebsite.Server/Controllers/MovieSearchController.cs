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

    [IgnoreAntiforgeryToken]
    public class MovieSearchController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        private const int DefaultMaxApiAdds = 25;
        private const int CrewLimit = 25;
        private const int WarningStalenessDays = 30;
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

        public sealed class MovieSearchRequest
        {
            public int Take { get; set; } = 25;
            public int MinMatches { get; set; } = 5;

            public bool EnableApiFallback { get; set; } = true;
            public bool AlwaysAddFromApis { get; set; } = false;
            public int MaxApiAdds { get; set; } = DefaultMaxApiAdds;

            public string WatchRegion { get; set; } = "US";

            /// <summary>How many "you may also like" results to return. 0 disables recommendations.</summary>
            public int RecommendationTake { get; set; } = 10;

            // ------------------------------------------------------------------
            // TEXT-BASED filters
            // ------------------------------------------------------------------
            public List<string> StreamingProviderNames { get; set; } = new();
            public List<string> GenreNames { get; set; } = new();
            public List<string> KeywordNames { get; set; } = new();
            public List<string> PersonNames { get; set; } = new();
            public List<string> PersonRoles { get; set; } = new();
            public List<string> IncludeWarningNames { get; set; } = new();
            public List<string> IncludeWarningCategoryNames { get; set; } = new();
            public List<string> IncludeWarningSubcategoryNames { get; set; } = new();
            public List<string> ExcludeWarningNames { get; set; } = new();
            public List<string> ExcludeWarningCategoryNames { get; set; } = new();
            public List<string> ExcludeWarningSubcategoryNames { get; set; } = new();

            /// <summary>
            /// Filter by MPAA rating. Accepts "G", "PG", "PG-13", "R", "NC-17".
            /// Case-insensitive. Multiple values combined with OR.
            /// Leave empty to return all ratings.
            /// </summary>
            public List<string> MpaaRatings { get; set; } = new();

            // ------------------------------------------------------------------
            // ID-BASED filters
            // ------------------------------------------------------------------
            public List<int> StreamingProviderIds { get; set; } = new();
            public MatchMode ProviderMatchMode { get; set; } = MatchMode.Any;

            public List<int> GenreIds { get; set; } = new();
            public List<int> KeywordIds { get; set; } = new();
            public List<int> PersonIds { get; set; } = new();

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

            public List<string> UnresolvedNames { get; set; } = new();

            /// <summary>
            /// Tiered "you may also like" results.
            /// Tier 2: genre + person, rating dropped.
            /// Tier 3: 50/50 split of genre-only and person-only, rating dropped.
            /// Streaming providers, include warnings, and exclude warnings are
            /// always strict across all tiers.
            /// Combined into one list, capped at RecommendationTake, no duplicates.
            /// </summary>
            public List<MovieSearchResultCard> Recommendations { get; set; } = new();
        }

        // =========================================================================
        // MAIN ENDPOINT
        // =========================================================================

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<MovieSearchResponse>> Search([FromBody] MovieSearchRequest req)
        {
            if (req.Take <= 0) req.Take = 25;
            if (req.Take > 100) req.Take = 100;
            if (req.MinMatches <= 0) req.MinMatches = 5;
            if (req.MaxApiAdds <= 0) req.MaxApiAdds = DefaultMaxApiAdds;
            if (req.MaxApiAdds > 50) req.MaxApiAdds = 50;
            if (string.IsNullOrWhiteSpace(req.WatchRegion)) req.WatchRegion = "US";
            if (req.RecommendationTake < 0) req.RecommendationTake = 0;
            if (req.RecommendationTake > 50) req.RecommendationTake = 50;

            req.MpaaRatings = req.MpaaRatings
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim().ToUpperInvariant())
                .Distinct()
                .ToList();

            // STEP 1: Resolve names → IDs
            var unresolvedNames = await ResolveNamesToIdsAsync(req);

            // STEP 2: Expand warning tiers
            var expandedIncludeTopicIds = await ResolveTopicIdsAsync(
                req.IncludeWarningTopicIds,
                req.IncludeWarningCategoryIds,
                req.IncludeWarningSubcategoryIds);

            var expandedExcludeTopicIds = await ResolveTopicIdsAsync(
                req.ExcludeWarningTopicIds,
                req.ExcludeWarningCategoryIds,
                req.ExcludeWarningSubcategoryIds);

            var baseReq = Clone(req);
            baseReq.IncludeWarningTopicIds = expandedIncludeTopicIds;
            baseReq.ExcludeWarningTopicIds = expandedExcludeTopicIds;

            var relaxedSteps = new List<string>();

            // STEP 3: Refresh stale warnings
            int staleRefreshed = 0;
            var dtddKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (!string.IsNullOrWhiteSpace(dtddKey))
            {
                staleRefreshed = await RefreshStaleWarningsAsync(dtddKey);
                if (staleRefreshed > 0)
                    relaxedSteps.Add($"Stale warnings refreshed: {staleRefreshed} movie(s) updated");
            }

            // STEP 4: Primary query + progressive relaxation
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

            // STEP 5: API fallback
            int addedFromApis = 0;
            var shouldApiFill =
                req.EnableApiFallback &&
                (req.AlwaysAddFromApis ? results.Count < req.Take : results.Count < req.MinMatches);

            if (shouldApiFill)
            {
                var (addedCount, stats) = await TryApiFillAsync(effectiveReq);
                addedFromApis = addedCount;

                relaxedSteps.Add(
                    $"API fill stats: candidates={stats.Candidates}, added={stats.Added}, " +
                    $"skipMissingImdb={stats.SkippedMissingImdb}, skipAlreadyEligible={stats.SkippedAlreadyEligible}, " +
                    $"skipStreamingEnrichFailed={stats.SkippedStreamingEnrichFailed}, skipStillNotStreamable={stats.SkippedStillNotStreamable}, " +
                    $"skipWarningsEnrichFailed={stats.SkippedWarningsEnrichFailed}, skipStillNoWarnings={stats.SkippedStillNoWarnings}");

                relaxedSteps.Add(
                    $"API fill examples: stillNoWarningsImdb={stats.ExampleStillNoWarningsImdb ?? "(none)"}, " +
                    $"warningsEnrichFailedImdb={stats.ExampleWarningsEnrichFailedImdb ?? "(none)"}");

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

            // STEP 6: Tiered recommendations
            var recommendations = new List<MovieSearchResultCard>();
            if (req.RecommendationTake > 0)
            {
                var excludedIds = results.Select(r => r.ImdbId).ToHashSet();
                recommendations = await RunTieredRecommendationsAsync(effectiveReq, excludedIds, req.RecommendationTake);
            }

            return Ok(new MovieSearchResponse
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
                UnresolvedNames = unresolvedNames,
                Recommendations = recommendations
            });
        }

        // =========================================================================
        // TIERED RECOMMENDATIONS
        //
        // Tier 2: genre + person, rating dropped
        // Tier 3: 50/50 split — genre-only and person-only, rating dropped
        //
        // Always strict: streaming providers, include warnings, exclude warnings
        // =========================================================================

        private async Task<List<MovieSearchResultCard>> RunTieredRecommendationsAsync(
            MovieSearchRequest req,
            HashSet<string> excludeImdbIds,
            int totalTake)
        {
            var recommendations = new List<MovieSearchResultCard>();

            var hasGenre    = req.GenreIds.Count > 0;
            var hasPerson   = req.PersonIds.Count > 0;
            var hasRating   = req.MpaaRatings.Count > 0;
            var hasProvider = req.StreamingProviderIds.Count > 0;

            // If no genre and no person, nothing meaningful to relax —
            // return recent streamable movies that respect all strict filters.
            // If streaming providers are specified, return empty rather than
            // showing unrelated movies that just happen to be on that service.
            if (!hasGenre && !hasPerson)
            {
                if (hasProvider)
                    return new List<MovieSearchResultCard>();

                return await RunRecommendationSegment(
                    req, excludeImdbIds,
                    useGenre: false, usePerson: false, useRating: false,
                    take: totalTake);
            }

            // ------------------------------------------------------------------
            // TIER 2: genre + person together, rating dropped
            // Only runs when both genre AND person were searched and rating was set
            // (otherwise it would duplicate primary results).
            // ------------------------------------------------------------------
            if (hasRating && hasGenre && hasPerson)
            {
                var tier2 = await RunRecommendationSegment(
                    req, excludeImdbIds,
                    useGenre: true, usePerson: true, useRating: false,
                    take: totalTake - recommendations.Count);

                foreach (var r in tier2)
                {
                    excludeImdbIds.Add(r.ImdbId);
                    recommendations.Add(r);
                    if (recommendations.Count >= totalTake) return recommendations;
                }
            }

            // ------------------------------------------------------------------
            // TIER 3: 50/50 split of genre-only and person-only
            // Interleaved so the combined list feels balanced.
            // ------------------------------------------------------------------
            if (recommendations.Count < totalTake && (hasGenre || hasPerson))
            {
                var remaining = totalTake - recommendations.Count;

                // How many to fetch from each side
                var genreSlots  = hasGenre  ? (int)Math.Ceiling(remaining / 2.0) : 0;
                var personSlots = hasPerson ? (int)Math.Ceiling(remaining / 2.0) : 0;

                // If only one side exists give it all the slots
                if (!hasGenre)  { personSlots = remaining; genreSlots = 0; }
                if (!hasPerson) { genreSlots  = remaining; personSlots = 0; }

                var genreOnly = hasGenre ? await RunRecommendationSegment(
                    req, excludeImdbIds,
                    useGenre: true, usePerson: false, useRating: false,
                    take: genreSlots) : new List<MovieSearchResultCard>();

                // Add genre results to exclude set before fetching person results
                // so we never get the same movie from both sides.
                var genreOnlyIds = genreOnly.Select(r => r.ImdbId).ToHashSet();
                var personExclude = new HashSet<string>(excludeImdbIds.Concat(genreOnlyIds));

                var personOnly = hasPerson ? await RunRecommendationSegment(
                    req, personExclude,
                    useGenre: false, usePerson: true, useRating: false,
                    take: personSlots) : new List<MovieSearchResultCard>();

                // If one side came up short, top up from the other
                var genreShortfall  = genreSlots  - genreOnly.Count;
                var personShortfall = personSlots - personOnly.Count;

                if (genreShortfall > 0 && hasPerson)
                {
                    var topUp = await RunRecommendationSegment(
                        req,
                        new HashSet<string>(excludeImdbIds
                            .Concat(genreOnlyIds)
                            .Concat(personOnly.Select(r => r.ImdbId))),
                        useGenre: false, usePerson: true, useRating: false,
                        take: genreShortfall);
                    personOnly.AddRange(topUp);
                }
                else if (personShortfall > 0 && hasGenre)
                {
                    var topUp = await RunRecommendationSegment(
                        req,
                        new HashSet<string>(excludeImdbIds
                            .Concat(genreOnlyIds)
                            .Concat(personOnly.Select(r => r.ImdbId))),
                        useGenre: true, usePerson: false, useRating: false,
                        take: personShortfall);
                    genreOnly.AddRange(topUp);
                }

                // Interleave genre and person results
                var gi = 0; var pi = 0;
                while ((gi < genreOnly.Count || pi < personOnly.Count)
                       && recommendations.Count < totalTake)
                {
                    if (gi < genreOnly.Count)
                    {
                        var r = genreOnly[gi++];
                        if (!excludeImdbIds.Contains(r.ImdbId))
                        {
                            excludeImdbIds.Add(r.ImdbId);
                            recommendations.Add(r);
                        }
                        if (recommendations.Count >= totalTake) break;
                    }
                    if (pi < personOnly.Count)
                    {
                        var r = personOnly[pi++];
                        if (!excludeImdbIds.Contains(r.ImdbId))
                        {
                            excludeImdbIds.Add(r.ImdbId);
                            recommendations.Add(r);
                        }
                    }
                }
            }

            return recommendations;
        }

        // =========================================================================
        // RECOMMENDATION SEGMENT
        //
        // Builds a single query for one tier/side of the recommendation logic.
        // Streaming providers, include warnings, and exclude warnings are always
        // applied. Genre, person, and rating are applied only when flagged.
        // =========================================================================

        private async Task<List<MovieSearchResultCard>> RunRecommendationSegment(
            MovieSearchRequest req,
            HashSet<string> excludeImdbIds,
            bool useGenre,
            bool usePerson,
            bool useRating,
            int take)
        {
            if (take <= 0) return new List<MovieSearchResultCard>();

            IQueryable<Movie> q = _context.Movies.AsNoTracking();

            // Global rules — always applied
            q = q.Where(m => m.MovieWarnings.Any(w => w.Answer != null && EF.Functions.ILike(w.Answer, "yes")));
            q = q.Where(m => m.MovieStreamings.Any(ms =>
                !EF.Functions.ILike(ms.OfferType, "rent") &&
                !EF.Functions.ILike(ms.OfferType, "buy")));

            // Never return anything already shown
            q = q.Where(m => !excludeImdbIds.Contains(m.ImdbId));

            // Streaming providers — always strict if specified
            if (req.StreamingProviderIds.Count > 0)
            {
                q = q.Where(m => m.MovieStreamings.Any(ms =>
                    req.StreamingProviderIds.Contains(ms.TmdbProviderId) &&
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy")));
            }

            // Include warnings — always strict
            if (req.IncludeWarningTopicIds.Count > 0)
            {
                if (req.IncludeWarningMatchMode == MatchMode.Any)
                {
                    q = q.Where(m => m.MovieWarnings.Any(w =>
                        req.IncludeWarningTopicIds.Contains(w.DtddTopicId) &&
                        w.Answer != null &&
                        EF.Functions.ILike(w.Answer, "yes%")));
                }
                else
                {
                    foreach (var tid in req.IncludeWarningTopicIds.Distinct())
                    {
                        var localTid = tid;
                        q = q.Where(m => m.MovieWarnings.Any(w =>
                            w.DtddTopicId == localTid &&
                            w.Answer != null &&
                            EF.Functions.ILike(w.Answer, "yes%")));
                    }
                }
            }

            // Exclude warnings — always strict
            if (req.ExcludeWarningTopicIds.Count > 0)
            {
                q = q.Where(m => !m.MovieWarnings.Any(w =>
                    req.ExcludeWarningTopicIds.Contains(w.DtddTopicId) &&
                    w.Answer != null &&
                    EF.Functions.ILike(w.Answer, "yes%")));
            }

            // Rating — only applied when flagged
            if (useRating && req.MpaaRatings.Count > 0)
            {
                q = q.Where(m =>
                    m.MpaaRating != null &&
                    req.MpaaRatings.Contains(m.MpaaRating.ToUpper()));
            }

            // Genre — only applied when flagged
            if (useGenre && req.GenreIds.Count > 0)
            {
                q = q.Where(m => m.MovieGenres.Any(mg =>
                    req.GenreIds.Contains(mg.TmdbGenreId)));
            }

            // Person — only applied when flagged
            if (usePerson && req.PersonIds.Count > 0)
            {
                q = q.Where(m =>
                    m.MovieCasts.Any(c => req.PersonIds.Contains(c.TmdbPersonId)) ||
                    m.MovieCrews.Any(c => req.PersonIds.Contains(c.TmdbPersonId)));
            }

            return await q
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .Select(m => new MovieSearchResultCard
                {
                    ImdbId      = m.ImdbId,
                    TmdbId      = m.TmdbId,
                    Title       = m.Title ?? "",
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl   = m.PosterUrl
                })
                .Take(take)
                .ToListAsync();
        }

        // =========================================================================
        // NAME → ID RESOLUTION
        // =========================================================================

        private async Task<List<string>> ResolveNamesToIdsAsync(MovieSearchRequest req)
        {
            var unresolved = new List<string>();

            foreach (var name in req.StreamingProviderNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.StreamingProviders
                    .AsNoTracking()
                    .Where(sp => EF.Functions.ILike(sp.ProviderName, $"%{name.Trim()}%"))
                    .Select(sp => sp.TmdbProviderId)
                    .ToListAsync();
                if (ids.Count == 0) unresolved.Add($"streaming provider: '{name.Trim()}'");
                req.StreamingProviderIds.AddRange(ids);
            }
            req.StreamingProviderIds = req.StreamingProviderIds.Distinct().ToList();

            foreach (var name in req.GenreNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var ids = await _context.Genres
                    .AsNoTracking()
                    .Where(g => EF.Functions.ILike(g.GenreName, $"%{name.Trim()}%"))
                    .Select(g => g.TmdbGenreId)
                    .ToListAsync();
                if (ids.Count == 0) unresolved.Add($"genre: '{name.Trim()}'");
                req.GenreIds.AddRange(ids);
            }
            req.GenreIds = req.GenreIds.Distinct().ToList();

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

            var tmdbKeyForPeople = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            foreach (var name in req.PersonNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var trimmed = name.Trim();

                var ids = await _context.People
                    .AsNoTracking()
                    .Where(p => EF.Functions.ILike(p.PersonName, $"%{trimmed}%"))
                    .Select(p => p.TmdbPersonId)
                    .ToListAsync();

                if (ids.Count == 0 && !string.IsNullOrWhiteSpace(tmdbKeyForPeople))
                {
                    // Try original name first, then alternative formats for initials
                    // e.g. "J.K. Simmons" -> "J. K. Simmons" -> "JK Simmons"
                    // Added with Claude (April 2026)
                    var namesToTry = new List<string> { trimmed };

                    // Add version with spaces after periods: "J.K." -> "J. K."
                    var withSpaces = System.Text.RegularExpressions.Regex.Replace(trimmed, @"\.(?!\s)", ". ").Trim();
                    if (withSpaces != trimmed) namesToTry.Add(withSpaces);

                    // Add version without periods: "J.K." -> "JK"
                    var withoutPeriods = trimmed.Replace(".", "").Trim();
                    if (withoutPeriods != trimmed) namesToTry.Add(withoutPeriods);

                    foreach (var nameVariant in namesToTry)
                    {
                        var tmdbPersonIds = await FetchAndUpsertTmdbPersonsByNameAsync(nameVariant, tmdbKeyForPeople);
                         if (tmdbPersonIds.Count > 0)
                        {
                            // Search by TMDB person IDs directly since the stored name
                            // may differ from what the user typed (e.g. "jk simmons" vs "J.K. Simmons")
                            ids = tmdbPersonIds;
                            break;
                        }
                    }
                }

                if (ids.Count == 0) unresolved.Add($"person: '{trimmed}'");
                req.PersonIds.AddRange(ids);
            }
            req.PersonIds = req.PersonIds.Distinct().ToList();

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

            foreach (var name in req.IncludeWarningCategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand(
                    "SELECT category_id FROM public.warning_categories WHERE category_name ILIKE @name;",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) req.IncludeWarningCategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.IncludeWarningCategoryIds = req.IncludeWarningCategoryIds.Distinct().ToList();

            foreach (var name in req.IncludeWarningSubcategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand(
                    "SELECT subcategory_id FROM public.warning_subcategories WHERE subcategory_name ILIKE @name;",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) req.IncludeWarningSubcategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.IncludeWarningSubcategoryIds = req.IncludeWarningSubcategoryIds.Distinct().ToList();

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

            foreach (var name in req.ExcludeWarningCategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand(
                    "SELECT category_id FROM public.warning_categories WHERE category_name ILIKE @name;",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) req.ExcludeWarningCategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.ExcludeWarningCategoryIds = req.ExcludeWarningCategoryIds.Distinct().ToList();

            foreach (var name in req.ExcludeWarningSubcategoryNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var conn = _context.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand(
                    "SELECT subcategory_id FROM public.warning_subcategories WHERE subcategory_name ILIKE @name;",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@name", $"%{name.Trim()}%");
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) req.ExcludeWarningSubcategoryIds.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }
            req.ExcludeWarningSubcategoryIds = req.ExcludeWarningSubcategoryIds.Distinct().ToList();

            return unresolved;
        }

        // =========================================================================
        // STALE WARNING REFRESH
        // =========================================================================

        private const int StaleRefreshBatchSize = 5;

        private async Task<int> RefreshStaleWarningsAsync(string dtddKey)
        {
            var now = DateTime.UtcNow;
            var staleThreshold = now.AddDays(-WarningStalenessDays);
            var refreshed = 0;

            var noWarningMovies = await _context.Movies
                .AsNoTracking()
                .Where(m =>
                    m.ReleaseYear <= now.Year &&
                    (m.Status == null || EF.Functions.ILike(m.Status, "released")) &&
                    !m.MovieWarnings.Any())
                .OrderBy(m => m.UpdatedAt)
                .Take(StaleRefreshBatchSize)
                .ToListAsync();

            foreach (var movie in noWarningMovies)
            {
                try { if (await TryEnrichWarningsFromDtddAsync(movie, dtddKey)) refreshed++; }
                catch { }
            }

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
                    try { if (await TryEnrichWarningsFromDtddAsync(movie, dtddKey)) refreshed++; }
                    catch { }
                }
            }

            return refreshed;
        }

        // =========================================================================
        // CLONE
        // =========================================================================

        private MovieSearchRequest Clone(MovieSearchRequest req) => new MovieSearchRequest
        {
            Take                    = req.Take,
            MinMatches              = req.MinMatches,
            EnableApiFallback       = req.EnableApiFallback,
            MaxApiAdds              = req.MaxApiAdds,
            WatchRegion             = req.WatchRegion,
            RecommendationTake      = req.RecommendationTake,
            MpaaRatings             = req.MpaaRatings.ToList(),

            StreamingProviderIds    = req.StreamingProviderIds.ToList(),
            ProviderMatchMode       = req.ProviderMatchMode,

            GenreIds                = req.GenreIds.ToList(),
            KeywordIds              = req.KeywordIds.ToList(),
            PersonIds               = req.PersonIds.ToList(),
            PersonRoles             = req.PersonRoles.ToList(),
            TitleContains           = req.TitleContains,

            IncludeWarningTopicIds        = req.IncludeWarningTopicIds.ToList(),
            IncludeWarningCategoryIds     = req.IncludeWarningCategoryIds.ToList(),
            IncludeWarningSubcategoryIds  = req.IncludeWarningSubcategoryIds.ToList(),
            IncludeWarningMatchMode       = req.IncludeWarningMatchMode,

            ExcludeWarningTopicIds        = req.ExcludeWarningTopicIds.ToList(),
            ExcludeWarningCategoryIds     = req.ExcludeWarningCategoryIds.ToList(),
            ExcludeWarningSubcategoryIds  = req.ExcludeWarningSubcategoryIds.ToList(),

            StreamingProviderNames        = new(),
            GenreNames                    = new(),
            KeywordNames                  = new(),
            PersonNames                   = new(),
            IncludeWarningNames           = new(),
            IncludeWarningCategoryNames   = new(),
            IncludeWarningSubcategoryNames = new(),
            ExcludeWarningNames           = new(),
            ExcludeWarningCategoryNames   = new(),
            ExcludeWarningSubcategoryNames = new()
        };

        // =========================================================================
        // PRIMARY QUERY
        // =========================================================================

        private async Task<List<MovieSearchResultCard>> RunQuery(MovieSearchRequest req, int take)
        {
            IQueryable<Movie> q = _context.Movies.AsNoTracking();

            q = q.Where(m => m.MovieWarnings.Any(w => w.Answer != null && EF.Functions.ILike(w.Answer, "yes")));
            q = q.Where(m => m.MovieStreamings.Any(ms =>
                !EF.Functions.ILike(ms.OfferType, "rent") &&
                !EF.Functions.ILike(ms.OfferType, "buy")));

            if (!string.IsNullOrWhiteSpace(req.TitleContains))
                q = q.Where(m => EF.Functions.ILike(m.Title!, $"%{req.TitleContains.Trim()}%"));

            if (req.MpaaRatings.Count > 0)
                q = q.Where(m => m.MpaaRating != null && req.MpaaRatings.Contains(m.MpaaRating.ToUpper()));

            if (req.StreamingProviderIds.Count > 0)
            {
                if (req.ProviderMatchMode == MatchMode.Any)
                {
                    q = q.Where(m => m.MovieStreamings.Any(ms =>
                        req.StreamingProviderIds.Contains(ms.TmdbProviderId) &&
                        !EF.Functions.ILike(ms.OfferType, "rent") &&
                        !EF.Functions.ILike(ms.OfferType, "buy")));
                }
                else
                {
                    foreach (var pid in req.StreamingProviderIds.Distinct())
                    {
                        var localPid = pid;
                        q = q.Where(m => m.MovieStreamings.Any(ms =>
                            ms.TmdbProviderId == localPid &&
                            !EF.Functions.ILike(ms.OfferType, "rent") &&
                            !EF.Functions.ILike(ms.OfferType, "buy")));
                    }
                }
            }

            if (req.GenreIds.Count > 0)
                q = q.Where(m => m.MovieGenres.Any(mg => req.GenreIds.Contains(mg.TmdbGenreId)));

            if (req.KeywordIds.Count > 0)
                q = q.Where(m => m.MovieKeywords.Any(mk => req.KeywordIds.Contains(mk.TmdbKeywordId)));

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

                var directorJobs = new List<string> { "Director" };
                var writerJobs   = new List<string> { "Writer", "Screenplay", "Story", "Characters", "Screenstory" };
                var producerJobs = new List<string> { "Producer", "Executive Producer", "Co-Producer" };

                q = q.Where(m =>
                    (includeCast && m.MovieCasts.Any(c => req.PersonIds.Contains(c.TmdbPersonId))) ||
                    (includeCrew && m.MovieCrews.Any(c =>
                        req.PersonIds.Contains(c.TmdbPersonId) &&
                        (roleSet.Count == 0 || roleSet.Contains("crew")
                            || (roleSet.Contains("director") && c.Job != null && directorJobs.Contains(c.Job))
                            || (roleSet.Contains("writer")   && c.Job != null && writerJobs.Contains(c.Job))
                            || (roleSet.Contains("producer") && c.Job != null && producerJobs.Contains(c.Job))))));
            }

            if (req.IncludeWarningTopicIds.Count > 0)
            {
                if (req.IncludeWarningMatchMode == MatchMode.Any)
                {
                    q = q.Where(m => m.MovieWarnings.Any(w =>
                        req.IncludeWarningTopicIds.Contains(w.DtddTopicId) &&
                        w.Answer != null &&
                        EF.Functions.ILike(w.Answer, "yes%")));
                }
                else
                {
                    foreach (var tid in req.IncludeWarningTopicIds.Distinct())
                    {
                        var localTid = tid;
                        q = q.Where(m => m.MovieWarnings.Any(w =>
                            w.DtddTopicId == localTid &&
                            w.Answer != null &&
                            EF.Functions.ILike(w.Answer, "yes%")));
                    }
                }
            }

            if (req.ExcludeWarningTopicIds.Count > 0)
            {
                q = q.Where(m => !m.MovieWarnings.Any(w =>
                    req.ExcludeWarningTopicIds.Contains(w.DtddTopicId) &&
                    w.Answer != null &&
                    EF.Functions.ILike(w.Answer, "yes%")));
            }

            return await q
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .Select(m => new MovieSearchResultCard
                {
                    ImdbId      = m.ImdbId,
                    TmdbId      = m.TmdbId,
                    Title       = m.Title ?? "",
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl   = m.PosterUrl
                })
                .Take(take)
                .ToListAsync();
        }

        // =========================================================================
        // API FALLBACK
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

            int skippedMissingImdb = 0, skippedAlreadyEligible = 0;
            int skippedStreamingEnrichFailed = 0, skippedStillNotStreamable = 0;
            int skippedWarningsEnrichFailed = 0, skippedStillNoWarnings = 0;
            string? exampleStillNoWarningsImdb = null, exampleWarningsEnrichFailedImdb = null;
            int added = 0;

            foreach (var tmdbId in candidateTmdbIds)
            {
                if (added >= req.MaxApiAdds) break;

                var imdbId = await FetchTmdbImdbIdAsync(tmdbId, tmdbKey);
                if (string.IsNullOrWhiteSpace(imdbId)) { skippedMissingImdb++; continue; }

                var hadWarningsBefore = await _context.MovieWarnings
                    .AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);
                var hadStreamableBefore = await _context.MovieStreamings
                    .AnyAsync(ms => ms.ImdbId == imdbId && ms.OfferType != null &&
                        !EF.Functions.ILike(ms.OfferType, "rent") &&
                        !EF.Functions.ILike(ms.OfferType, "buy"));
                // Note: we no longer skip already-eligible movies — we want to always
                // process all TMDB candidates so sequels and series are fully added.
                // Added with Claude (April 2026)

                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);
                var wasNew = false;

                if (movie == null)
                {
                    if (!await UpsertMovieCoreFromTmdbAsync(tmdbId, imdbId, tmdbKey)) continue;
                    movie = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);
                    if (movie == null) continue;
                    wasNew = true;
                }

                var hasStreamable = await _context.MovieStreamings
                    .AnyAsync(ms => ms.ImdbId == imdbId && ms.OfferType != null &&
                        !EF.Functions.ILike(ms.OfferType, "rent") &&
                        !EF.Functions.ILike(ms.OfferType, "buy"));

                if (!hasStreamable)
                {
                    if (!await TryEnrichStreamingFromTmdbAsync(movie, tmdbKey, req.WatchRegion))
                    { skippedStreamingEnrichFailed++; continue; }

                    hasStreamable = await _context.MovieStreamings
                        .AnyAsync(ms => ms.ImdbId == imdbId && ms.OfferType != null &&
                            !EF.Functions.ILike(ms.OfferType, "rent") &&
                            !EF.Functions.ILike(ms.OfferType, "buy"));

                    if (!hasStreamable) { skippedStillNotStreamable++; continue; }
                }

                var hasWarnings = await _context.MovieWarnings
                    .AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);

                if (!hasWarnings)
                {
                    if (!await TryEnrichWarningsFromDtddAsync(movie, dtddKey))
                    {
                        skippedWarningsEnrichFailed++;
                        exampleWarningsEnrichFailedImdb ??= imdbId;
                        continue;
                    }

                    hasWarnings = await _context.MovieWarnings
                        .AnyAsync(mw => mw.ImdbId == imdbId && mw.Answer != null);

                    if (!hasWarnings)
                    {
                        skippedStillNoWarnings++;
                        exampleStillNoWarningsImdb ??= imdbId;
                        continue;
                    }
                }

                // Always enrich all data for every movie regardless of search criteria
                if (!await _context.MovieGenres.AnyAsync(mg => mg.ImdbId == imdbId))
                    await TryEnrichGenresFromTmdbAsync(movie, tmdbKey);

                if (!await _context.MovieKeywords.AnyAsync(mk => mk.ImdbId == imdbId))
                    await TryEnrichKeywordsFromTmdbAsync(movie, tmdbKey);

                var hasCastOrCrew =
                    await _context.MovieCasts.AnyAsync(mc => mc.ImdbId == imdbId) ||
                    await _context.MovieCrews.AnyAsync(mc => mc.ImdbId == imdbId);
                if (!hasCastOrCrew)
                {
                    await TryEnrichCastFromTmdbAsync(movie, tmdbKey);
                    await TryEnrichCrewFromTmdbAsync(movie, tmdbKey);
                }

                // Always attempt OMDB rating enrichment for newly added or unrated movies.
                // Check the database directly rather than the in-memory object since
                // multiple SaveChanges calls may cause EF tracking inconsistencies.
                var currentRating = await _context.Movies
                    .Where(m => m.ImdbId == imdbId)
                    .Select(m => m.MpaaRating)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(currentRating))
                {
                    var omdbKey = Environment.GetEnvironmentVariable("OMDB_API_KEY");
                    if (!string.IsNullOrWhiteSpace(omdbKey))
                        await TryEnrichMpaaRatingFromOmdbAsync(movie, omdbKey);
                }

                // Enrich collection data if not already present — added with Claude (April 2026)
                if (!await _context.MovieCollections.AnyAsync(mc => mc.ImdbId == imdbId))
                    await UpsertMovieCoreFromTmdbAsync(tmdbId, imdbId, tmdbKey);


                if (wasNew || !(hadWarningsBefore && hadStreamableBefore)) added++;
            }

            return (added, new ApiFillStats(
                candidateTmdbIds.Count, skippedMissingImdb, skippedAlreadyEligible,
                skippedStreamingEnrichFailed, skippedStillNotStreamable,
                skippedWarningsEnrichFailed, skippedStillNoWarnings, added,
                exampleStillNoWarningsImdb, exampleWarningsEnrichFailedImdb));
        }

        private async Task<List<int>> FetchTmdbCandidateIdsAsync(MovieSearchRequest req, string apiKey)
        {
            if (!string.IsNullOrWhiteSpace(req.TitleContains))
                return await FetchTmdbSearchMovieIdsAsync(req.TitleContains!, apiKey);

            if (req.PersonIds.Count > 0)
            {
                // Always fetch the full person credits first — this is much more
                // comprehensive than TMDB discover's with_people filter, which
                // misses many valid credits especially for cast members.
                var personMovieIds = new List<int>();
                foreach (var personId in req.PersonIds.Distinct())
                    personMovieIds.AddRange(await FetchTmdbPersonMovieCreditsAsync(personId, apiKey, req.PersonRoles));

                var personSet = personMovieIds.Distinct().ToHashSet();

                // If we also have genre or provider filters, fetch a broad discover
                // result set using ONLY those filters (not with_people) and intersect
                // locally. This avoids TMDB's overly strict AND behaviour when
                // combining with_people + with_genres + with_watch_providers.
                if (req.GenreIds.Count > 0 || req.StreamingProviderIds.Count > 0)
                {
                    // Build a discover request without the person filter
                    var discoverReq = Clone(req);
                    discoverReq.PersonIds = new List<int>();

                    var discoverIds = await FetchTmdbDiscoverMovieIdsAsync(discoverReq, apiKey);
                    var discoverSet = new HashSet<int>(discoverIds);

                    // Return movies that appear in BOTH the person credits AND
                    // the genre/provider discover results
                    var intersection = personSet.Where(id => discoverSet.Contains(id)).ToList();

                    // If the intersection is very small (TMDB discover may not have
                    // all movies for older or less popular titles), fall back to
                    // returning all person credits and let the DB query filter by
                    // genre/provider from our local data.
                    if (intersection.Count < 10)
                        return personSet.Take(150).ToList();

                    return intersection;
                }

                return personSet.Take(150).ToList();
            }

            return await FetchTmdbDiscoverMovieIdsAsync(req, apiKey);
        }

        private async Task<List<int>> FetchTmdbPersonMovieCreditsAsync(
            int tmdbPersonId, string apiKey, List<string>? roles = null)
        {
            try
            {
                using var http = new HttpClient();
                using var resp = await http.GetAsync(
                    $"https://api.themoviedb.org/3/person/{tmdbPersonId}/movie_credits?api_key={apiKey}");
                if (!resp.IsSuccessStatusCode) return new List<int>();

                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                var ids = new List<int>();

                var roleSet = (roles ?? new List<string>())
                    .Select(r => r.Trim().ToLowerInvariant())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToHashSet();

                var includeCast = roleSet.Count == 0 || roleSet.Contains("cast");
                var includeCrew = roleSet.Count == 0 || roleSet.Contains("crew")
                    || roleSet.Contains("director") || roleSet.Contains("writer")
                    || roleSet.Contains("producer");

                var directorJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Director" };
                var writerJobs   = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Writer", "Screenplay", "Story", "Characters", "Screenstory" };
                var producerJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Producer", "Executive Producer", "Co-Producer" };

                if (includeCast && root.TryGetProperty("cast", out var castEl) && castEl.ValueKind == JsonValueKind.Array)
                    foreach (var m in castEl.EnumerateArray())
                        if (m.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) ids.Add(idVal);

                if (includeCrew && root.TryGetProperty("crew", out var crewEl) && crewEl.ValueKind == JsonValueKind.Array)
                    foreach (var m in crewEl.EnumerateArray())
                    {
                        if (!m.TryGetProperty("id", out var idEl) || !idEl.TryGetInt32(out var idVal)) continue;
                        if (roleSet.Count == 0 || roleSet.Contains("crew")) { ids.Add(idVal); continue; }
                        var job = m.TryGetProperty("job", out var jobEl) ? jobEl.GetString() ?? "" : "";
                        if (roleSet.Contains("director") && directorJobs.Contains(job)) ids.Add(idVal);
                        else if (roleSet.Contains("writer") && writerJobs.Contains(job)) ids.Add(idVal);
                        else if (roleSet.Contains("producer") && producerJobs.Contains(job)) ids.Add(idVal);
                    }

                return ids.Distinct().ToList();
            }
            catch { return new List<int>(); }
        }

        private async Task<List<int>> FetchTmdbSearchMovieIdsAsync(string query, string apiKey)
        {
            using var http = new HttpClient();
            var ids = new List<int>();

            for (int page = 1; page <= 2; page++)
            {
                var json = await http.GetStringAsync(
                    $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}" +
                    $"&query={Uri.EscapeDataString(query)}&include_adult=false&page={page}");
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array) break;
                foreach (var r in resultsEl.EnumerateArray())
                    if (r.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) ids.Add(idVal);
                if (ids.Count >= 40) break;
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
                var query = new List<KeyValuePair<string, string>>
                {
                    new("api_key", apiKey), new("include_adult", "false"),
                    new("include_video", "false"), new("sort_by", "popularity.desc"),
                    new("page", page.ToString()), new("primary_release_date.lte", cutoff)
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

                try
                {
                    using var resp = await http.GetAsync($"https://api.themoviedb.org/3/discover/movie?{qs}");
                    if (!resp.IsSuccessStatusCode) return ids.Distinct().Take(150).ToList();

                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    if (!doc.RootElement.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array) break;
                    foreach (var r in resultsEl.EnumerateArray())
                        if (r.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) ids.Add(idVal);
                    if (ids.Count >= 150) break;
                }
                catch { return ids.Distinct().Take(150).ToList(); }
            }

            return ids.Distinct().Take(150).ToList();
        }

        private async Task<string?> FetchTmdbImdbIdAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync(
                $"https://api.themoviedb.org/3/movie/{tmdbId}/external_ids?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("imdb_id", out var el) ? el.GetString() : null;
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
            // Collection data added with Claude (April 2026)
            public int? CollectionId { get; set; }
            public string? CollectionName { get; set; }
        }

        private async Task<TmdbDetailsBasic?> FetchTmdbDetailsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            int? releaseYear = null;
            if (root.TryGetProperty("release_date", out var rdEl))
            {
                var rd = rdEl.GetString();
                if (!string.IsNullOrWhiteSpace(rd) && rd.Length >= 4 && int.TryParse(rd[..4], out var y))
                    releaseYear = y;
            }

            string? posterUrl = null;
            if (root.TryGetProperty("poster_path", out var ppEl))
            {
                var p = ppEl.GetString();
                if (!string.IsNullOrWhiteSpace(p)) posterUrl = $"https://image.tmdb.org/t/p/w500{p}";
            }

            int? runtime = null;
            if (root.TryGetProperty("runtime", out var rtEl) && rtEl.TryGetInt32(out var rtVal)) runtime = rtVal;

            // Extract collection data — added with Claude (April 2026)
            int? collectionId = null;
            string? collectionName = null;
            if (root.TryGetProperty("belongs_to_collection", out var colEl) && colEl.ValueKind == JsonValueKind.Object)
            {
                if (colEl.TryGetProperty("id", out var colIdEl) && colIdEl.TryGetInt32(out var colIdVal))
                    collectionId = colIdVal;
                if (colEl.TryGetProperty("name", out var colNameEl))
                    collectionName = colNameEl.GetString();
            }

            return new TmdbDetailsBasic
            {
                Title            = root.TryGetProperty("title",             out var tEl)   ? tEl.GetString()   : null,
                ReleaseYear      = releaseYear,
                RuntimeMinutes   = runtime,
                PlotSummary      = root.TryGetProperty("overview",          out var ovEl)  ? ovEl.GetString()  : null,
                PosterUrl        = posterUrl,
                OriginalLanguage = root.TryGetProperty("original_language", out var langEl)? langEl.GetString(): null,
                Tagline          = root.TryGetProperty("tagline",           out var tgEl)  ? tgEl.GetString()  : null,
                Status           = root.TryGetProperty("status",            out var stEl)  ? stEl.GetString()  : null,
                CollectionId     = collectionId,
                CollectionName   = collectionName
            };
        }

        private async Task<bool> UpsertMovieCoreFromTmdbAsync(int tmdbId, string imdbId, string apiKey)
        {
            var details = await FetchTmdbDetailsAsync(tmdbId, apiKey);
            if (details == null) return false;

            int? runtimeSafe = details.RuntimeMinutes is > 0 and <= 600 ? details.RuntimeMinutes : null;
            var now = DateTime.UtcNow;
            var existing = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);

            if (existing == null)
            {
                if (string.IsNullOrWhiteSpace(details.Title) || details.ReleaseYear == null) return false;
                _context.Movies.Add(new Movie
                {
                    ImdbId = imdbId, TmdbId = tmdbId, Title = details.Title!,
                    ReleaseYear = details.ReleaseYear.Value, RuntimeMinutes = runtimeSafe,
                    PlotSummary = details.PlotSummary, PosterUrl = details.PosterUrl,
                    OriginalLanguage = details.OriginalLanguage, MediaType = "movie",
                    Tagline = details.Tagline, Status = details.Status,
                    MpaaRating = null, CreatedAt = now, UpdatedAt = now
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

            // Enrich collection data if available — added with Claude (April 2026)
            if (details.CollectionId != null && !string.IsNullOrWhiteSpace(details.CollectionName))
                await TryEnrichCollectionAsync(imdbId, details.CollectionId.Value, details.CollectionName, now);

            return true;
        }

        // Saves collection membership for a movie using data already returned by the TMDB details call.
        // Added with Claude (April 2026)
        private async Task TryEnrichCollectionAsync(string imdbId, int tmdbCollectionId, string collectionName, DateTime now)
        {
            try
            {
                // Upsert the collection itself
                var collection = await _context.Collections
                    .FirstOrDefaultAsync(c => c.TmdbCollectionId == tmdbCollectionId);

                if (collection == null)
                {
                    _context.Collections.Add(new Collection
                    {
                        TmdbCollectionId = tmdbCollectionId,
                        CollectionName = collectionName
                    });
                    await _context.SaveChangesAsync();
                }

                // Upsert the movie-collection link
                var existing = await _context.MovieCollections
                    .FirstOrDefaultAsync(mc => mc.ImdbId == imdbId && mc.TmdbCollectionId == tmdbCollectionId);

                if (existing == null)
                {
                    _context.MovieCollections.Add(new MovieCollection
                    {
                        ImdbId = imdbId,
                        TmdbCollectionId = tmdbCollectionId,
                        CreatedAt = now
                    });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CollectionEnrich] Failed for {imdbId}: {ex.Message}");
            }
        }

        private sealed class TmdbWatchProviderRow
        {
            public int ProviderId { get; set; }
            public string? ProviderName { get; set; }
            public string? OfferType { get; set; }
        }

        private async Task<bool> TryEnrichStreamingFromTmdbAsync(Movie movie, string apiKey, string watchRegion)
        {
            if (!movie.TmdbId.HasValue || movie.TmdbId.Value <= 0) return false;

            var providers = (await FetchTmdbWatchProvidersAsync(movie.TmdbId.Value, apiKey, watchRegion))?.ToList();
            if (providers == null || providers.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();

            var existingLinks = await _context.MovieStreamings.Where(ms => ms.ImdbId == movie.ImdbId).ToListAsync();
            if (existingLinks.Count > 0) _context.MovieStreamings.RemoveRange(existingLinks);

            var providerIds = providers.Where(p => p.ProviderId > 0 && !string.IsNullOrWhiteSpace(p.ProviderName))
                .Select(p => p.ProviderId).Distinct().ToList();

            var existingProviders = await _context.StreamingProviders
                .Where(sp => providerIds.Contains(sp.TmdbProviderId))
                .ToDictionaryAsync(sp => sp.TmdbProviderId);

            foreach (var pid in providerIds)
            {
                var name = providers.First(p => p.ProviderId == pid).ProviderName!;
                if (!existingProviders.TryGetValue(pid, out var sp))
                {
                    sp = new StreamingProvider { TmdbProviderId = pid, ProviderName = name, CreatedAt = now, UpdatedAt = now };
                    _context.StreamingProviders.Add(sp);
                    existingProviders[pid] = sp;
                }
                else { if (sp.ProviderName != name) sp.ProviderName = name; sp.UpdatedAt = now; }
            }

            foreach (var p in providers)
            {
                if (p.ProviderId <= 0 || string.IsNullOrWhiteSpace(p.ProviderName) || string.IsNullOrWhiteSpace(p.OfferType)) continue;
                _context.MovieStreamings.Add(new MovieStreaming
                    { ImdbId = movie.ImdbId, TmdbProviderId = p.ProviderId, OfferType = p.OfferType!, CreatedAt = now });
            }

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<List<TmdbWatchProviderRow>?> FetchTmdbWatchProvidersAsync(int tmdbId, string apiKey, string watchRegion)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}/watch/providers?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Object) return new List<TmdbWatchProviderRow>();
            if (!resultsEl.TryGetProperty(watchRegion.ToUpperInvariant(), out var regEl) || regEl.ValueKind != JsonValueKind.Object) return new List<TmdbWatchProviderRow>();

            var map = new List<(string bucket, string offerType)>
            {
                ("flatrate", "subscription"), ("free", "free"), ("ads", "free_with_ads"), ("rent", "rent"), ("buy", "buy")
            };

            var list = new List<TmdbWatchProviderRow>();
            foreach (var (bucket, offerType) in map)
            {
                if (!regEl.TryGetProperty(bucket, out var arr) || arr.ValueKind != JsonValueKind.Array) continue;
                foreach (var p in arr.EnumerateArray())
                {
                    var id = p.TryGetProperty("provider_id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = p.TryGetProperty("provider_name", out var nameEl) ? nameEl.GetString() : null;
                    if (id > 0 && !string.IsNullOrWhiteSpace(name))
                        list.Add(new TmdbWatchProviderRow { ProviderId = id, ProviderName = name, OfferType = offerType });
                }
            }

            return list.GroupBy(x => new { x.ProviderId, x.OfferType }).Select(g => g.First()).ToList();
        }

        private sealed class DtddTopicStatRow
        {
            public int TopicId { get; set; }
            public string Answer { get; set; } = "unknown";
            public bool? IsSpoiler { get; set; }
            public string? Comment { get; set; }
        }

        private async Task<bool> TryEnrichWarningsFromDtddAsync(Movie movie, string apiKey)
        {
            int? dtddMediaId = null;
            var overrideRow = await _context.DtddOverrides.AsNoTracking().FirstOrDefaultAsync(o => o.ImdbId == movie.ImdbId);
            if (overrideRow != null && overrideRow.DtddMediaId > 0) dtddMediaId = overrideRow.DtddMediaId;

            List<DtddTopicStatRow> stats;
            try
            {
                if (dtddMediaId == null && !string.IsNullOrWhiteSpace(movie.ImdbId))
                    dtddMediaId = await FetchDtddMediaIdByImdbAsync(movie.ImdbId, apiKey);
                if (dtddMediaId == null && movie.TmdbId.HasValue && movie.TmdbId.Value > 0)
                    dtddMediaId = await FetchDtddMediaIdByTmdbAsync(movie.TmdbId.Value, apiKey);
                if (dtddMediaId == null && !string.IsNullOrWhiteSpace(movie.Title) && movie.ReleaseYear > 0)
                    dtddMediaId = await FetchDtddMediaIdByTitleYearAsync(movie.Title, movie.ReleaseYear, apiKey);
                if (dtddMediaId == null || string.IsNullOrWhiteSpace(movie.ImdbId)) return false;

                stats = await FetchDtddTopicStatsAsync(dtddMediaId.Value, apiKey);
            }
            catch { return false; }

            if (stats.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();

            var existing = await _context.MovieWarnings.Where(mw => mw.ImdbId == movie.ImdbId).ToListAsync();
            if (existing.Count > 0) _context.MovieWarnings.RemoveRange(existing);

            var knownTopicIds = (await _context.Warnings.AsNoTracking().Select(w => w.DtddTopicId).ToListAsync()).ToHashSet();
            foreach (var s in stats)
            {
                if (!knownTopicIds.Contains(s.TopicId)) continue;
                _context.MovieWarnings.Add(new MovieWarning
                {
                    ImdbId = movie.ImdbId, DtddTopicId = s.TopicId, Answer = s.Answer,
                    IsSpoiler = s.IsSpoiler, WarningComment = s.Comment, CreatedAt = now, UpdatedAt = now
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
            var json = await http.GetStringAsync($"https://www.doesthedogdie.com/dddsearch?imdb={Uri.EscapeDataString(imdbId)}");
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("items", out var el) || el.ValueKind != JsonValueKind.Array) return null;
            foreach (var item in el.EnumerateArray())
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) return idVal;
            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTmdbAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            HttpResponseMessage resp;
            try { resp = await http.GetAsync($"https://www.doesthedogdie.com/dddsearch?tmdb={tmdbId}"); }
            catch { return null; }
            if (!resp.IsSuccessStatusCode) return null;
            var contentType = resp.Content.Headers.ContentType?.MediaType ?? "";
            var text = await resp.Content.ReadAsStringAsync();
            if (!contentType.Contains("json", StringComparison.OrdinalIgnoreCase)) return null;
            using var doc = JsonDocument.Parse(text);
            if (!doc.RootElement.TryGetProperty("items", out var el) || el.ValueKind != JsonValueKind.Array) return null;
            foreach (var item in el.EnumerateArray())
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) return idVal;
            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTitleYearAsync(string title, int releaseYear, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            var json = await http.GetStringAsync($"https://www.doesthedogdie.com/dddsearch?q={Uri.EscapeDataString(title)}");
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("items", out var el) || el.ValueKind != JsonValueKind.Array) return null;
            foreach (var item in el.EnumerateArray())
            {
                if (item.TryGetProperty("releaseYear", out var yEl) && yEl.ValueKind == JsonValueKind.Number && yEl.GetInt32() != releaseYear) continue;
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) return idVal;
            }
            foreach (var item in el.EnumerateArray())
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) return idVal;
            return null;
        }

        private async Task<List<DtddTopicStatRow>> FetchDtddTopicStatsAsync(int dtddMediaId, string apiKey)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            var json = await http.GetStringAsync($"https://www.doesthedogdie.com/media/{dtddMediaId}");
            using var doc = JsonDocument.Parse(json);
            var list = new List<DtddTopicStatRow>();
            if (!doc.RootElement.TryGetProperty("topicItemStats", out var statsEl) || statsEl.ValueKind != JsonValueKind.Array) return list;

            foreach (var s in statsEl.EnumerateArray())
            {
                var topicId = s.TryGetProperty("TopicId", out var tidEl) && tidEl.TryGetInt32(out var tidVal) ? tidVal : 0;
                if (topicId <= 0) continue;

                bool? isSpoiler = null;
                if (s.TryGetProperty("topic", out var topicEl) && topicEl.ValueKind == JsonValueKind.Object)
                    if (topicEl.TryGetProperty("isSpoiler", out var spEl) &&
                        (spEl.ValueKind == JsonValueKind.True || spEl.ValueKind == JsonValueKind.False))
                        isSpoiler = spEl.GetBoolean();

                int? yesSum = null, noSum = null, isYes = null;
                if (s.TryGetProperty("yesSum",  out var ysEl) && ysEl.ValueKind == JsonValueKind.Number) yesSum = ysEl.GetInt32();
                if (s.TryGetProperty("noSum",   out var nsEl) && nsEl.ValueKind == JsonValueKind.Number) noSum  = nsEl.GetInt32();
                if (s.TryGetProperty("isYes",   out var iyEl) && iyEl.ValueKind == JsonValueKind.Number) isYes  = iyEl.GetInt32();

                string answer;
                if (yesSum.HasValue || noSum.HasValue)
                {
                    var y = yesSum ?? 0; var n = noSum ?? 0;
                    answer = (y == 0 && n == 0) ? "unknown" : (y >= n && y > 0) ? "yes" : "no";
                }
                else answer = isYes.HasValue ? (isYes.Value == 1 ? "yes" : "no") : "unknown";

                list.Add(new DtddTopicStatRow
                {
                    TopicId = topicId, Answer = answer, IsSpoiler = isSpoiler,
                    Comment = s.TryGetProperty("comment", out var cEl) ? cEl.GetString() : null
                });
            }

            return list;
        }

        private async Task<bool> TryEnrichGenresFromTmdbAsync(Movie movie, string apiKey)
        {
            if (!movie.TmdbId.HasValue || movie.TmdbId.Value <= 0) return false;
            var genreIds = await FetchTmdbGenreIdsAsync(movie.TmdbId.Value, apiKey);
            if (genreIds.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();
            var existing = await _context.MovieGenres.Where(mg => mg.ImdbId == movie.ImdbId).ToListAsync();
            if (existing.Count > 0) _context.MovieGenres.RemoveRange(existing);
            foreach (var gid in genreIds.Distinct())
                _context.MovieGenres.Add(new MovieGenre { ImdbId = movie.ImdbId, TmdbGenreId = gid, CreatedAt = now });
            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<List<int>> FetchTmdbGenreIdsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            var ids = new List<int>();
            if (doc.RootElement.TryGetProperty("genres", out var genresEl) && genresEl.ValueKind == JsonValueKind.Array)
                foreach (var g in genresEl.EnumerateArray())
                    if (g.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal)) ids.Add(idVal);
            return ids;
        }

        private async Task<bool> TryEnrichKeywordsFromTmdbAsync(Movie movie, string apiKey)
        {
            if (!movie.TmdbId.HasValue || movie.TmdbId.Value <= 0) return false;
            var keywords = await FetchTmdbKeywordsAsync(movie.TmdbId.Value, apiKey);
            if (keywords.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();
            var existing = await _context.MovieKeywords.Where(mk => mk.ImdbId == movie.ImdbId).ToListAsync();
            if (existing.Count > 0) _context.MovieKeywords.RemoveRange(existing);

            foreach (var k in keywords)
            {
                if (k.KeywordId <= 0 || string.IsNullOrWhiteSpace(k.KeywordName)) continue;
                var ek = await _context.Keywords.FirstOrDefaultAsync(x => x.TmdbKeywordId == k.KeywordId);
                if (ek == null) _context.Keywords.Add(new Keyword { TmdbKeywordId = k.KeywordId, KeywordName = k.KeywordName!, CreatedAt = now, UpdatedAt = now });
                else { if (ek.KeywordName != k.KeywordName) ek.KeywordName = k.KeywordName!; ek.UpdatedAt = now; }
                _context.MovieKeywords.Add(new MovieKeyword { ImdbId = movie.ImdbId, TmdbKeywordId = k.KeywordId, CreatedAt = now });
            }

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private sealed class TmdbKeywordRow { public int KeywordId { get; set; } public string? KeywordName { get; set; } }

        private async Task<List<TmdbKeywordRow>> FetchTmdbKeywordsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}/keywords?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            var list = new List<TmdbKeywordRow>();
            if (!doc.RootElement.TryGetProperty("keywords", out var kwEl) || kwEl.ValueKind != JsonValueKind.Array) return list;
            foreach (var k in kwEl.EnumerateArray())
            {
                var id = k.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                var name = k.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                if (id > 0 && !string.IsNullOrWhiteSpace(name)) list.Add(new TmdbKeywordRow { KeywordId = id, KeywordName = name });
            }
            return list;
        }

        private sealed class TmdbCredits { public List<TmdbCastRow> Cast { get; set; } = new(); public List<TmdbCrewRow> Crew { get; set; } = new(); }
        private sealed class TmdbCastRow { public int PersonId { get; set; } public string? Name { get; set; } public string? Character { get; set; } public int? Order { get; set; } public string? CreditId { get; set; } }
        private sealed class TmdbCrewRow { public int PersonId { get; set; } public string? Name { get; set; } public string? Department { get; set; } public string? Job { get; set; } public string? CreditId { get; set; } }

        private async Task<TmdbCredits?> FetchTmdbCreditsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}/credits?api_key={apiKey}");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var credits = new TmdbCredits();

            if (root.TryGetProperty("cast", out var castEl) && castEl.ValueKind == JsonValueKind.Array)
                foreach (var c in castEl.EnumerateArray())
                {
                    var pid = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    if (pid > 0) credits.Cast.Add(new TmdbCastRow
                    {
                        PersonId = pid,
                        Name = c.TryGetProperty("name", out var nEl) ? nEl.GetString() : null,
                        Character = c.TryGetProperty("character", out var chEl) ? chEl.GetString() : null,
                        Order = c.TryGetProperty("order", out var oEl) && oEl.TryGetInt32(out var oVal) ? oVal : (int?)null,
                        CreditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null
                    });
                }

            if (root.TryGetProperty("crew", out var crewEl) && crewEl.ValueKind == JsonValueKind.Array)
                foreach (var c in crewEl.EnumerateArray())
                {
                    var pid = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    if (pid > 0) credits.Crew.Add(new TmdbCrewRow
                    {
                        PersonId = pid,
                        Name = c.TryGetProperty("name", out var nEl) ? nEl.GetString() : null,
                        Department = c.TryGetProperty("department", out var dEl) ? dEl.GetString() : null,
                        Job = c.TryGetProperty("job", out var jEl) ? jEl.GetString() : null,
                        CreditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null
                    });
                }

            return credits;
        }

        private async Task UpsertPeopleFromCreditsAsync(TmdbCredits credits, DateTime now)
        {
            var ids = credits.Cast.Select(c => c.PersonId).Concat(credits.Crew.Select(c => c.PersonId)).Distinct().ToList();
            if (ids.Count == 0) return;
            var existingMap = (await _context.People.Where(p => ids.Contains(p.TmdbPersonId)).ToListAsync()).ToDictionary(x => x.TmdbPersonId);
            foreach (var id in ids)
            {
                var name = credits.Cast.FirstOrDefault(x => x.PersonId == id)?.Name ?? credits.Crew.FirstOrDefault(x => x.PersonId == id)?.Name;
                if (string.IsNullOrWhiteSpace(name)) continue;
                if (!existingMap.TryGetValue(id, out var person))
                    _context.People.Add(new Person { TmdbPersonId = id, PersonName = name!, CreatedAt = now, UpdatedAt = now });
                else { if (person.PersonName != name) person.PersonName = name!; person.UpdatedAt = now; }
            }
        }

        private async Task<bool> TryEnrichCastFromTmdbAsync(Movie movie, string apiKey)
        {
            if (!movie.TmdbId.HasValue || movie.TmdbId.Value <= 0) return false;
            var credits = await FetchTmdbCreditsAsync(movie.TmdbId.Value, apiKey);
            if (credits == null || credits.Cast.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();
            await UpsertPeopleFromCreditsAsync(credits, now);
            await _context.SaveChangesAsync();

            var existing = await _context.MovieCasts.Where(mc => mc.ImdbId == movie.ImdbId).ToListAsync();
            if (existing.Count > 0) _context.MovieCasts.RemoveRange(existing);

            foreach (var c in credits.Cast)
            {
                if (c.PersonId <= 0 || string.IsNullOrWhiteSpace(c.CreditId)) continue;
                _context.MovieCasts.Add(new MovieCast
                {
                    TmdbCreditId = c.CreditId!, ImdbId = movie.ImdbId, TmdbPersonId = c.PersonId,
                    CharacterName = c.Character, CastOrder = c.Order, CreatedAt = now
                });
            }

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<bool> TryEnrichCrewFromTmdbAsync(Movie movie, string apiKey)
        {
            if (movie.TmdbId == null || movie.TmdbId <= 0) return false;
            var credits = await FetchTmdbCreditsAsync(movie.TmdbId.Value, apiKey);
            if (credits == null || credits.Crew.Count == 0) return false;

            var now = DateTime.UtcNow;
            await using var tx = await _context.Database.BeginTransactionAsync();
            await UpsertPeopleFromCreditsAsync(credits, now);
            await _context.SaveChangesAsync();

            var existing = await _context.MovieCrews.Where(mc => mc.ImdbId == movie.ImdbId).ToListAsync();
            if (existing.Count > 0) _context.MovieCrews.RemoveRange(existing);

            var priorityJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "Director", "Writer", "Screenplay", "Story", "Characters", "Producer", "Executive Producer", "Director of Photography", "Original Music Composer", "Editor" };

            foreach (var c in credits.Crew
                .Where(c => c.PersonId > 0 && !string.IsNullOrWhiteSpace(c.CreditId))
                .OrderByDescending(c => c.Job != null && priorityJobs.Contains(c.Job) ? 1 : 0)
                .ThenBy(c => c.Department ?? "").ThenBy(c => c.Job ?? "")
                .Take(CrewLimit))
            {
                _context.MovieCrews.Add(new MovieCrew
                {
                    TmdbCreditId = c.CreditId!, ImdbId = movie.ImdbId, TmdbPersonId = c.PersonId,
                    Department = c.Department, Job = c.Job, CreatedAt = now
                });
            }

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private async Task<List<int>> FetchAndUpsertTmdbPersonsByNameAsync(string name, string apiKey)
        {
            try
            {
                using var http = new HttpClient();
                using var resp = await http.GetAsync(
                    $"https://api.themoviedb.org/3/search/person?api_key={apiKey}&query={Uri.EscapeDataString(name)}&include_adult=false&page=1");
                if (!resp.IsSuccessStatusCode) return new List<int>();

                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array) return new List<int>();

                var now = DateTime.UtcNow;
                var upsertedIds = new List<int>();

                foreach (var r in resultsEl.EnumerateArray())
                {
                    if (!r.TryGetProperty("id", out var idEl) || !idEl.TryGetInt32(out var tmdbPersonId)) continue;
                    var personName = r.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                    if (string.IsNullOrWhiteSpace(personName)) continue;

                    var knownFor = r.TryGetProperty("known_for_department", out var kfEl) ? kfEl.GetString() : null;
                    var profilePath = r.TryGetProperty("profile_path", out var ppEl) ? ppEl.GetString() : null;
                    string? profileUrl = !string.IsNullOrWhiteSpace(profilePath) ? $"https://image.tmdb.org/t/p/w185{profilePath}" : null;

                    var existing = await _context.People.FirstOrDefaultAsync(p => p.TmdbPersonId == tmdbPersonId);
                    if (existing == null)
                        _context.People.Add(new Person { TmdbPersonId = tmdbPersonId, PersonName = personName!, KnownForDepartment = knownFor, ProfileUrl = profileUrl, CreatedAt = now, UpdatedAt = now });
                    else { existing.PersonName = personName!; existing.KnownForDepartment = knownFor; if (!string.IsNullOrWhiteSpace(profileUrl)) existing.ProfileUrl = profileUrl; existing.UpdatedAt = now; }

                    upsertedIds.Add(tmdbPersonId);
                }

                if (upsertedIds.Count > 0) await _context.SaveChangesAsync();
                return upsertedIds;
            }
            catch { return new List<int>(); }
        }

// =========================================================================
        // MPAA RATING ENRICHMENT (OMDB)
        // =========================================================================

        private async Task<bool> TryEnrichMpaaRatingFromOmdbAsync(Movie movie, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(movie.ImdbId)) return false;

            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "FindMyFlick/search (mpaa enrichment)");

                var url = $"https://www.omdbapi.com/?apikey={apiKey}&i={Uri.EscapeDataString(movie.ImdbId)}";
                var json = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("Response", out var respEl) || respEl.GetString() != "True")
                {
                    movie.MpaaRating = "Not Rated";
                    movie.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }

                if (!root.TryGetProperty("Rated", out var ratedEl)) return false;

                var rated = ratedEl.GetString();
                if (string.IsNullOrWhiteSpace(rated) || rated == "N/A")
                {
                    movie.MpaaRating = "Not Rated";
                    movie.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }

                rated = rated.Trim();

                // The database trigger will normalize the value, but we also
                // normalize here so the in-memory object stays consistent.
                rated = rated.ToUpperInvariant() switch
                {
                    "NOT RATED" or "UNRATED" or "NR" or "N/A" => "Not Rated",
                    "G"      => "G",
                    "PG"     => "PG",
                    "GP" or "M/PG" or "M" => "PG",
                    "PG-13"  => "PG-13",
                    "R"      => "R",
                    "NC-17" or "X" => "NC-17",
                    "PASSED" or "APPROVED" or "AO" => "Not Rated",
                    _        => rated.StartsWith("TV-", StringComparison.OrdinalIgnoreCase) ? rated : "Not Rated"
                };

                movie.MpaaRating = rated;
                movie.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private async Task<List<int>> ResolveTopicIdsAsync(List<int> topicIds, List<int> categoryIds, List<int> subcategoryIds)
        {
            var set = new HashSet<int>(topicIds.Distinct());
            if (categoryIds.Count == 0 && subcategoryIds.Count == 0) return set.OrderBy(x => x).ToList();

            var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            if (categoryIds.Count > 0)
            {
                await using var cmd = new NpgsqlCommand(
                    "SELECT DISTINCT wct.dtdd_topic_id FROM public.warning_category_topics wct WHERE wct.category_id = ANY(@ids);",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@ids", categoryIds.Distinct().ToArray());
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) set.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }

            if (subcategoryIds.Count > 0)
            {
                await using var cmd = new NpgsqlCommand(
                    "SELECT DISTINCT wst.dtdd_topic_id FROM public.warning_subcategory_topics wst WHERE wst.subcategory_id = ANY(@ids);",
                    (NpgsqlConnection)conn);
                cmd.Parameters.AddWithValue("@ids", subcategoryIds.Distinct().ToArray());
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) set.Add(reader.GetInt32(0));
                await reader.CloseAsync();
            }

            return set.OrderBy(x => x).ToList();
        }
    }

    // =========================================================================
    // EXTERNAL API TEST CONTROLLER
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
            var detailsJson = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}");
            using var detailsDoc = JsonDocument.Parse(detailsJson);
            var root = detailsDoc.RootElement;
            string? title = root.TryGetProperty("title", out var tEl) ? tEl.GetString() : null;
            string? releaseDate = root.TryGetProperty("release_date", out var rdEl) ? rdEl.GetString() : null;

            var extJson = await http.GetStringAsync($"https://api.themoviedb.org/3/movie/{tmdbId}/external_ids?api_key={apiKey}");
            using var extDoc = JsonDocument.Parse(extJson);
            string? imdbId = extDoc.RootElement.TryGetProperty("imdb_id", out var imdbEl) ? imdbEl.GetString() : null;

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
            var json = await http.GetStringAsync($"https://www.doesthedogdie.com/media/{dtddTitleId}");
            return Content(json, "application/json");
        }
    }
}