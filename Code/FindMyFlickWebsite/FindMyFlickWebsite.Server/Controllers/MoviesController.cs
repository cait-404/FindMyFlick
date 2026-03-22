using FindMyFlickWebsite.Server.Models;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly FindmyflickContext _context;
        private readonly IDbContextFactory<FindmyflickContext> _dbContextFactory;

        public MoviesController(FindmyflickContext context, IDbContextFactory<FindmyflickContext> dbContextFactory)
        {
            _context = context;
            _dbContextFactory = dbContextFactory;
        }

        // ============================================================
        // ADVANCED FILTERING LOGIC
        // ============================================================

        public static IEnumerable<MoviesView> AdvancedSearch(
            IEnumerable<MoviesView> source,
            string? name = null,
            IEnumerable<string>? streamingServices = null,
            bool matchAllStreaming = false,
            string? ageRating = null,
            IEnumerable<string>? genres = null,
            bool matchAllGenres = false,
            int? year = null,
            IEnumerable<string>? tagNamesInclude = null,
            IEnumerable<string>? tagNamesExclude = null,
            bool matchAllTagsIn = false,
            bool matchAllTagsEx = true)
        {
            if (source == null) yield break;

            var svcList = streamingServices?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
            var genreList = genres?.Where(g => !string.IsNullOrWhiteSpace(g)).ToList();
            var tagNameListInclude = tagNamesInclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();
            var tagNameListExclude = tagNamesExclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();

            foreach (var m in source)
            {
                if (m == null) continue;

                // Name (substring)
                if (!string.IsNullOrWhiteSpace(name) && 
                    (string.IsNullOrWhiteSpace(m.Name) || !m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)))
                    continue;

                // Age rating (exact)
                if (!string.IsNullOrWhiteSpace(ageRating) && 
                    (string.IsNullOrWhiteSpace(m.AgeRating) || !string.Equals(m.AgeRating, ageRating, StringComparison.OrdinalIgnoreCase)))
                    continue;

                // Genres
                if (genreList?.Any() == true)
                {
                    var hasGenreMatches = matchAllGenres
                        ? genreList.All(g => m.Genre.Any(mg => string.Equals(mg, g, StringComparison.OrdinalIgnoreCase)))
                        : genreList.Any(g => m.Genre.Any(mg => string.Equals(mg, g, StringComparison.OrdinalIgnoreCase)));

                    if (!hasGenreMatches) continue;
                }

                // Streaming services
                if (svcList?.Any() == true)
                {
                    var movieProviderNames = (m.StreamingProviders ?? Enumerable.Empty<MoviesView.StreamingProviderView>())
                        .Select(sp => sp.ProviderName?.Trim() ?? string.Empty)
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .ToList();

                    var hasStreamingMatches = matchAllStreaming
                        ? svcList.All(query => movieProviderNames.Any(p => string.Equals(p, query, StringComparison.OrdinalIgnoreCase)))
                        : svcList.Any(query => movieProviderNames.Any(p => string.Equals(p, query, StringComparison.OrdinalIgnoreCase)));

                    if (!hasStreamingMatches) continue;
                }

                // Year
                if (year.HasValue && m.Year != year.Value)
                    continue;

                // Tags include
                if (tagNameListInclude?.Any() == true)
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>())
                        .Select(t => t.TagName?.Trim().ToLowerInvariant() ?? string.Empty)
                    );

                    var queryTags = tagNameListInclude.Select(t => t.ToLowerInvariant()).ToList();
                    var tagMatch = matchAllTagsIn
                        ? queryTags.All(q => movieTagNames.Contains(q))
                        : queryTags.Any(q => movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                // Tags exclude
                if (tagNameListExclude?.Any() == true)
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>())
                        .Select(t => t.TagName?.Trim().ToLowerInvariant() ?? string.Empty)
                    );

                    var queryTags = tagNameListExclude.Select(t => t.ToLowerInvariant()).ToList();
                    var tagMatch = matchAllTagsEx
                        ? queryTags.All(q => !movieTagNames.Contains(q))
                        : queryTags.Any(q => !movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                yield return m;
            }
        }

        // ============================================================
        // DATABASE LOADER
        // ============================================================

        private async Task<(List<MoviesView> Dtos, List<Movie> Entities)> LoadMovieDtosAsync()
        {
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = loaded.Select(m => new MoviesView
            {
                ID = ParseImdbToInt(m.ImdbId),
                Name = m.Title ?? "(Untitled)",
                Year = m.ReleaseYear,
                AgeRating = m.MpaaRating,
                Summary = m.PlotSummary ?? "",
                Poster = m.PosterUrl,
                GenreEntries = m.MovieGenres?.Select(g => new MoviesView.GenreEntry
                {
                    TmdbGenreId = g.TmdbGenreId,
                    GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                }).ToList() ?? [],
                Genre = m.MovieGenres?
                    .Select(g => g.TmdbGenre?.GenreName ?? string.Empty)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? [],
                StreamingProviders = m.MovieStreamings?
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider?.ProviderName ?? string.Empty
                    })
                    .ToList() ?? [],
                Tags = new TagsView
                {
                    TriggerTags = (m.MovieWarnings ?? [])
                        .Where(w => string.Equals(w.Answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                        .Select(w => new TagsView.TriggerTag
                        {
                            TagID = w.DtddTopicId,
                            TagName = w.DtddTopic?.TopicName ?? string.Empty
                        })
                      .GroupBy(t => t.TagName?.ToLowerInvariant() ?? string.Empty)

                        .Select(g => g.First())
                        .ToList(),
                    PlotTags = new List<TagsView.PlotTag>(),
                    PersonTags = new List<TagsView.PersonTag>()
                },
                TagVotes = new List<MoviesView.TagVote>()
            }).ToList();

            return (dtoList, loaded);
        }

        // ============================================================
        // SEARCH (GET /api/Movies/search)
        // Refactored: accept the new MovieSearchRequest query fields,
        // build a MovieSearchRequest and delegate entirely to MovieSearchController.Search.
        // ============================================================

        //My search endpoint in MoviesController uses the MovieSearchController's logic to return search results. The logic in MovieSearchController
        //recently changed to use the queries Like this:
        //{ "genreNames": ["thriller", "comedy"], "personNames": ["Tom Hanks"], "mpaaRatings": ["PG-13"], "streamingProviderNames": ["netflix", "hulu"], "excludeWarningNames": ["graphic violence"], "take": 10, "recommendationTake": 10, "enableApiFallback": true }
        //    and
        //{ "genreNames": ["comedy"], "mpaaRatings": ["PG", "PG-13"], "streamingProviderNames": ["disney", "amazon"], "includeWarningNames": ["animal death"], "take": 10, "recommendationTake": 10, "enableApiFallback": true }
        //refactor my search endpoint in Movies Controller to utalize the new logic

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MoviesView>), 200)]
        public async Task<ActionResult<IEnumerable<MoviesView>>> Search(
            // Primary free-text / legacy name
            [FromQuery(Name = "titleContains")] string? titleContains = null,
            [FromQuery(Name = "name")] string? name = null,

            // New structured search fields
            [FromQuery(Name = "genreNames")] List<string>? genreNames = null,
            [FromQuery(Name = "personNames")] List<string>? personNames = null,
            [FromQuery(Name = "mpaaRatings")] List<string>? mpaaRatings = null,
            [FromQuery(Name = "streamingProviderNames")] List<string>? streamingProviderNames = null,
            [FromQuery(Name = "includeWarningNames")] List<string>? includeWarningNames = null,
            [FromQuery(Name = "excludeWarningNames")] List<string>? excludeWarningNames = null,

            // Control flags
            [FromQuery(Name = "take")] int take = 25,
            [FromQuery(Name = "recommendationTake")] int recommendationTake = 0,
            [FromQuery(Name = "enableApiFallback")] bool enableApiFallback = false,
            [FromQuery(Name = "maxApiAdds")] int maxApiAdds = 25,
            [FromQuery(Name = "watchRegion")] string? watchRegion = "US",

            // Backwards-compatible advanced in-memory filters (optional)
            [FromQuery] bool matchAllStreaming = false,
            [FromQuery] bool matchAllGenres = false,
            [FromQuery] int? year = null,
            [FromQuery] List<string>? tagNamesInclude = null,
            [FromQuery] List<string>? tagNamesExclude = null,
            [FromQuery] bool matchAllTagsIn = false,
            [FromQuery] bool matchAllTagsEx = true)
        {
            try
            {
                // Prefer explicit titleContains param, fall back to legacy 'name'
                var titleFilter = string.IsNullOrWhiteSpace(titleContains) ? name : titleContains;

                // Normalize inputs (trim, remove empties)
                genreNames = genreNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                personNames = personNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                mpaaRatings = mpaaRatings?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                streamingProviderNames = streamingProviderNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                includeWarningNames = includeWarningNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                excludeWarningNames = excludeWarningNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();

                // Backwards compatibility: if callers used the old params (genres/streamingServices/tagNamesInclude/tagNamesExclude),
                // prefer those when provided. This keeps existing clients working.
                if ((genreNames == null || genreNames.Count == 0) && Request.Query.ContainsKey("genres"))
                {
                    var legacyGenres = Request.Query["genres"].ToList().Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
                    if (legacyGenres.Any()) genreNames = legacyGenres.ToList();
                }

                if ((streamingProviderNames == null || streamingProviderNames.Count == 0) && Request.Query.ContainsKey("streamingServices"))
                {
                    var legacyS = Request.Query["streamingServices"].ToList().Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
                    if (legacyS.Any()) streamingProviderNames = legacyS.ToList();
                }

                if ((includeWarningNames == null || includeWarningNames.Count == 0) && tagNamesInclude != null && tagNamesInclude.Count > 0)
                    includeWarningNames = tagNamesInclude;

                if ((excludeWarningNames == null || excludeWarningNames.Count == 0) && tagNamesExclude != null && tagNamesExclude.Count > 0)
                    excludeWarningNames = tagNamesExclude;

                // Build the MovieSearchRequest using the MovieSearchController contract.
                var movieSearchReq = new MovieSearchController.MovieSearchRequest
                {
                    TitleContains = titleFilter,
                    GenreNames = genreNames ?? new List<string>(),
                    PersonNames = personNames ?? new List<string>(),
                    MpaaRatings = (mpaaRatings ?? new List<string>()),
                    StreamingProviderNames = streamingProviderNames ?? new List<string>(),
                    IncludeWarningNames = includeWarningNames ?? new List<string>(),
                    ExcludeWarningNames = excludeWarningNames ?? new List<string>(),
                    Take = Math.Clamp(take, 1, 200),
                    RecommendationTake = Math.Clamp(recommendationTake, 0, 50),
                    EnableApiFallback = enableApiFallback,
                    MaxApiAdds = Math.Clamp(maxApiAdds, 0, 50),
                    WatchRegion = string.IsNullOrWhiteSpace(watchRegion) ? "US" : watchRegion!
                };

                // Use a NEW DbContext instance for the delegated controller call to avoid optimistic concurrency
                // between this controller's _context and the mutating work that MovieSearchController may perform.
                await using var ctxForSearch = _dbContextFactory.CreateDbContext();
                var searchController = new MovieSearchController(ctxForSearch);
                var searchActionResult = await searchController.Search(movieSearchReq);

                // Extract the MovieSearchResponse from ActionResult.
                MovieSearchController.MovieSearchResponse? searchResp = null;
                if (searchActionResult.Value != null)
                {
                    searchResp = searchActionResult.Value;
                }
                else if (searchActionResult.Result is ObjectResult obj && obj.Value is MovieSearchController.MovieSearchResponse msr)
                {
                    searchResp = msr;
                }
                else if (searchActionResult.Result is StatusCodeResult sc && sc.StatusCode != 200)
                {
                    return StatusCode(sc.StatusCode, new { message = "MovieSearchController.Search returned non-success status." });
                }
                else
                {
                    // Unexpected shape
                    return StatusCode(500, new { message = "MovieSearchController.Search returned unexpected result shape." });
                }

                if (searchResp == null || searchResp.Results == null || searchResp.Results.Count == 0)
                    return Ok(Array.Empty<MoviesView>());

                var imdbIds = searchResp.Results
                    .Select(r => r.ImdbId)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct()
                    .ToList();

                if (!imdbIds.Any())
                    return Ok(Array.Empty<MoviesView>());

                // Load full movie rows for the matched imdb ids and map to MoviesView
                var loaded = await _context.Movies
                    .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                    .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                    .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                    .AsNoTracking()
                    .Where(m => imdbIds.Contains(m.ImdbId))
                    .ToListAsync();

                var dtoList = loaded.Select(m => new MoviesView
                {
                    ID = ParseImdbToInt(m.ImdbId),
                    Name = m.Title ?? "(Untitled)",
                    Year = m.ReleaseYear,
                    AgeRating = m.MpaaRating,
                    Summary = m.PlotSummary ?? "",
                    Poster = m.PosterUrl,
                    GenreEntries = m.MovieGenres?.Select(g => new MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    }).ToList() ?? [],
                    Genre = m.MovieGenres?
                        .Select(g => g.TmdbGenre?.GenreName ?? string.Empty)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList() ?? [],
                    StreamingProviders = m.MovieStreamings?
                        .GroupBy(ms => ms.TmdbProviderId)
                        .Select(g => new MoviesView.StreamingProviderView
                        {
                            Id = g.Key,
                            ProviderName = g.First().TmdbProvider?.ProviderName ?? string.Empty
                        })
                        .ToList() ?? [],
                    Tags = new TagsView
                    {
                        TriggerTags = (m.MovieWarnings ?? [])
                            .Where(w => string.Equals(w.Answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                            .Select(w => new TagsView.TriggerTag
                            {
                                TagID = w.DtddTopicId,
                                TagName = w.DtddTopic?.TopicName ?? string.Empty
                            })
                          .GroupBy(t => t.TagName?.ToLowerInvariant() ?? string.Empty)
                            .Select(g => g.First())
                            .ToList(),
                        PlotTags = new List<TagsView.PlotTag>(),
                        PersonTags = new List<TagsView.PersonTag>()
                    },
                    TagVotes = new List<MoviesView.TagVote>()
                }).ToList();

                // Apply any remaining advanced in-memory filtering if the caller supplied legacy advanced params.
                var final = AdvancedSearch(dtoList, titleFilter ?? name, streamingProviderNames, matchAllStreaming,
                    // ageRating is not in this signature any more, but mpaaRatings was passed to MovieSearchRequest.
                    // We leave ageRating null here so AdvancedSearch doesn't double-filter incorrectly.
                    null, genreNames, matchAllGenres, year,
                    tagNamesInclude, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

                // Preserve order from searchResp.Results where possible by ordering final list by the position in imdbIds
                var position = imdbIds.Select((id, idx) => (id, idx)).ToDictionary(x => x.id, x => x.idx);
                final = final.OrderBy(m => position.TryGetValue("tt" + m.ID.ToString().TrimStart('0'), out var p) ? p : (position.TryGetValue(m.ID.ToString(), out var p2) ? p2 : int.MaxValue))
                             .ThenByDescending(m => m.Year)
                             .ToList();

                return Ok(final);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        // ============================================================
        // DEFAULT GET (GET /api/Movies)
        // Added optional paging via ?page=1 (page size is fixed to PageSize)
        // Added optional ordering via ?order={release_year|none|title_asc|title_desc}
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> GetMoviesView_ParseImdb(
            [FromQuery] int page = 1,
            [FromQuery] string? order = "release_year")
        {
            try
            {
                if (page <= 0) page = 1;
                const int PageSize = 50;

                // Build base query (no ordering yet)
                IQueryable<Movie> query = _context.Movies
                    .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                    .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                    .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                    .AsNoTracking();

                // Normalise order param
                var ord = (order ?? "release_year").Trim().ToLowerInvariant();

                // Apply ordering based on query parameter:
                // - "release_year" (default): release year desc, then title asc
                // - "none": no ordering (database default)
                // - "title_asc": order by title ascending
                // - "title_desc": order by title descending
                switch (ord)
                {
                    case "none":
                        // leave unordered
                        break;
                    case "title_asc":
                        query = query.OrderBy(m => m.Title);
                        break;
                    case "title_desc":
                        query = query.OrderByDescending(m => m.Title);
                        break;
                    case "release_year":
                    default:
                        query = query.OrderByDescending(m => m.ReleaseYear).ThenBy(m => m.Title);
                        break;
                }

                var total = await query.CountAsync();

                var loaded = await query
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                var dtoList = loaded.Select(m => new MoviesView
                {
                    ID = ParseImdbToInt(m.ImdbId),
                    Name = m.Title ?? "(Untitled)",
                    Year = m.ReleaseYear,
                    AgeRating = m.MpaaRating,
                    Summary = m.PlotSummary ?? "",
                    Poster = m.PosterUrl,
                    GenreEntries = m.MovieGenres?.Select(g => new MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    }).ToList() ?? [],
                    Genre = m.MovieGenres?
                        .Select(g => g.TmdbGenre?.GenreName ?? string.Empty)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList() ?? [],
                    StreamingProviders = m.MovieStreamings?
                        .GroupBy(ms => ms.TmdbProviderId)
                        .Select(g => new MoviesView.StreamingProviderView
                        {
                            Id = g.Key,
                            ProviderName = g.First().TmdbProvider?.ProviderName ?? string.Empty
                        })
                        .ToList() ?? [],
                    Tags = new TagsView
                    {
                        TriggerTags = (m.MovieWarnings ?? [])
                            .Where(w => string.Equals(w.Answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                            .Select(w => new TagsView.TriggerTag
                            {
                                TagID = w.DtddTopicId,
                                TagName = w.DtddTopic?.TopicName ?? string.Empty
                            })
                          .GroupBy(t => t.TagName?.ToLowerInvariant() ?? string.Empty)
                            .Select(g => g.First())
                            .ToList(),
                        PlotTags = new List<TagsView.PlotTag>(),
                        PersonTags = new List<TagsView.PersonTag>()
                    },
                    TagVotes = new List<MoviesView.TagVote>()
                }).ToList();

                // Expose simple pagination + ordering headers (optional for clients)
                Response.Headers["X-Total-Count"] = total.ToString();
                Response.Headers["X-Page"] = page.ToString();
                Response.Headers["X-Page-Size"] = PageSize.ToString();
                Response.Headers["X-Order"] = ord;

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        // ============================================================
        // GET BY ID (GET /api/Movies/{id})
        // Returns core movie information only (title, poster, year, summary) to avoid timeouts.
        // ============================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(new { message = "id cannot be empty." });

                if (!id.StartsWith("tt", StringComparison.OrdinalIgnoreCase))
                    id = "tt" + id; // ensure it starts with 'tt' for consistent searching

                //half copilot gnerated via asking it to create a get by id method using imdb ids ex. tt31227572, but then refactored for preformance by me
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(new { message = "id cannot be empty." });

                var movie = await _context.Movies
                    .AsNoTracking()
                    .Select(m => m)
                    .Where(m => m.ImdbId == id)
                    .Take(1)
                    .ToListAsync();
                if (movie == null) return NotFound(new { message = $"Movie with ID '{id}' not found." });
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        // ============================================================
        // HELPERS
        // ============================================================

        private static int ParseImdbToInt(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId)) return 0;
            var digits = new string(imdbId.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var result) ? result : 0;
        }
    }
}
