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

            // Hard constraints
            public List<int> StreamingProviderIds { get; set; } = new();
            public MatchMode ProviderMatchMode { get; set; } = MatchMode.Any;

            // Soft constraints (may relax later)
            public List<int> GenreIds { get; set; } = new();
            public List<int> KeywordIds { get; set; } = new();
            public List<int> PersonIds { get; set; } = new();
            public string? TitleContains { get; set; }

            // INCLUDE warnings (hard)
            public List<int> IncludeWarningTopicIds { get; set; } = new();
            public List<int> IncludeWarningCategoryIds { get; set; } = new();
            public List<int> IncludeWarningSubcategoryIds { get; set; } = new();
            public MatchMode IncludeWarningMatchMode { get; set; } = MatchMode.Any;

            // EXCLUDE warnings (hard)
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
        }

        [HttpPost]
        public async Task<ActionResult<MovieSearchResponse>> Search([FromBody] MovieSearchRequest req)
        {
            if (req.Take <= 0) req.Take = 25;
            if (req.MinMatches <= 0) req.MinMatches = 5;

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
            var results = await RunQuery(baseReq, take: req.Take);

            // Relaxing only touches soft filters.
            if (results.Count < req.MinMatches)
            {
                if (baseReq.KeywordIds.Count > 0)
                {
                    var clone = Clone(baseReq);
                    clone.KeywordIds.Clear();
                    results = await RunQuery(clone, req.Take);
                    relaxedSteps.Add("Relax: removed KeywordIds");

                    if (results.Count < req.MinMatches && clone.PersonIds.Count > 0)
                    {
                        clone = Clone(clone);
                        clone.PersonIds.Clear();
                        results = await RunQuery(clone, req.Take);
                        relaxedSteps.Add("Relax: removed PersonIds");
                    }

                    if (results.Count < req.MinMatches && clone.GenreIds.Count > 0)
                    {
                        clone = Clone(clone);
                        clone.GenreIds.Clear();
                        results = await RunQuery(clone, req.Take);
                        relaxedSteps.Add("Relax: removed GenreIds");
                    }
                }
                else
                {
                    var clone = Clone(baseReq);

                    if (results.Count < req.MinMatches && clone.PersonIds.Count > 0)
                    {
                        clone.PersonIds.Clear();
                        results = await RunQuery(clone, req.Take);
                        relaxedSteps.Add("Relax: removed PersonIds");
                    }

                    if (results.Count < req.MinMatches && clone.GenreIds.Count > 0)
                    {
                        clone.GenreIds.Clear();
                        results = await RunQuery(clone, req.Take);
                        relaxedSteps.Add("Relax: removed GenreIds");
                    }
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
                ExcludedWarningTopicsExpandedCount = expandedExcludeTopicIds.Count
            };

            return Ok(response);
        }

        private MovieSearchRequest Clone(MovieSearchRequest req) => new MovieSearchRequest
        {
            Take = req.Take,
            MinMatches = req.MinMatches,

            StreamingProviderIds = req.StreamingProviderIds.ToList(),
            ProviderMatchMode = req.ProviderMatchMode,

            GenreIds = req.GenreIds.ToList(),
            KeywordIds = req.KeywordIds.ToList(),
            PersonIds = req.PersonIds.ToList(),
            TitleContains = req.TitleContains,

            IncludeWarningTopicIds = req.IncludeWarningTopicIds.ToList(),
            IncludeWarningCategoryIds = req.IncludeWarningCategoryIds.ToList(),
            IncludeWarningSubcategoryIds = req.IncludeWarningSubcategoryIds.ToList(),
            IncludeWarningMatchMode = req.IncludeWarningMatchMode,

            ExcludeWarningTopicIds = req.ExcludeWarningTopicIds.ToList(),
            ExcludeWarningCategoryIds = req.ExcludeWarningCategoryIds.ToList(),
            ExcludeWarningSubcategoryIds = req.ExcludeWarningSubcategoryIds.ToList()
        };

        private async Task<List<MovieSearchResultCard>> RunQuery(MovieSearchRequest req, int take)
        {
            IQueryable<Movie> q = _context.Movies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(req.TitleContains))
                q = q.Where(m => EF.Functions.ILike(m.Title!, $"%{req.TitleContains}%"));

            // Providers (hard)
            if (req.StreamingProviderIds.Count > 0)
            {
                if (req.ProviderMatchMode == MatchMode.Any)
                {
                    q = q.Where(m => m.MovieStreamings.Any(ms => req.StreamingProviderIds.Contains(ms.TmdbProviderId)));
                }
                else
                {
                    foreach (var pid in req.StreamingProviderIds.Distinct())
                    {
                        var localPid = pid;
                        q = q.Where(m => m.MovieStreamings.Any(ms => ms.TmdbProviderId == localPid));
                    }
                }
            }

            // Genres (soft)
            if (req.GenreIds.Count > 0)
                q = q.Where(m => m.MovieGenres.Any(mg => req.GenreIds.Contains(mg.TmdbGenreId)));

            // Keywords (soft)
            if (req.KeywordIds.Count > 0)
                q = q.Where(m => m.MovieKeywords.Any(mk => req.KeywordIds.Contains(mk.TmdbKeywordId)));

            // People (soft)
            if (req.PersonIds.Count > 0)
            {
                q = q.Where(m =>
                    m.MovieCasts.Any(c => req.PersonIds.Contains(c.TmdbPersonId)) ||
                    m.MovieCrews.Any(c => req.PersonIds.Contains(c.TmdbPersonId)));
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

    // These GET endpoints live in the same compiled file as MovieSearchController
    // so Swagger will definitely pick them up.
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalApiTestController : ControllerBase
    {
        // GET /api/ExternalApiTest/tmdb/238
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

        // GET /api/ExternalApiTest/dtdd/imdb/tt0068646
        [HttpGet("dtdd/imdb/{imdbId}")]
        public async Task<IActionResult> TestDtddByImdb(string imdbId)
        {
            var apiKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest("DTDD_API_KEY is not set for this terminal/session.");

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var url = $"https://www.doesthedogdie.com/dddsearch?imdb={Uri.EscapeDataString(imdbId)}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("items", out var itemsEl) || itemsEl.ValueKind != JsonValueKind.Array)
                return Ok(new { imdbId, foundItems = 0, items = Array.Empty<object>() });

            // Return only safe string-ish fields so we can't crash on type mismatches.
            var items = itemsEl.EnumerateArray().Take(5).Select(it =>
            {
                string? idRaw = it.TryGetProperty("id", out var idEl) ? idEl.ToString() : null;
                string? name = it.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                string? tmdbIdRaw = it.TryGetProperty("tmdbId", out var tmdbEl) ? tmdbEl.ToString() : null;
                string? yearRaw = it.TryGetProperty("releaseYear", out var yEl) ? yEl.ToString() : null;

                return new { id = idRaw, name, tmdbId = tmdbIdRaw, releaseYear = yearRaw };
            }).ToList();

            return Ok(new { imdbId, foundItems = itemsEl.GetArrayLength(), items });
        }

        // GET /api/ExternalApiTest/dtdd/title/11800
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

            // Return the raw JSON so we don't run into JsonDocument disposal issues.
            return Content(json, "application/json");
        }

    }
}