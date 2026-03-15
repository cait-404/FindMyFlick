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

        public MoviesController(FindmyflickContext context)
        {
            _context = context;
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
        // Refactored to delegate identification to MovieSearchController.Search,
        // then load and map matching Movies into MoviesView and apply the
        // existing AdvancedSearch post-filters (matchAll flags, year, age rating).
        // refactored with copilot to avoid making external API calls during a simple GET search by disabling API fallback in the MovieSearchRequest, and to handle the various possible shapes of the ActionResult returned by MovieSearchController.Search.
        // ============================================================

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MoviesView>), 200)]
        public async Task<ActionResult<IEnumerable<MoviesView>>> Search(
            [FromQuery] string? name = null,
            [FromQuery] List<string>? streamingServices = null,
            [FromQuery] bool matchAllStreaming = false,
            [FromQuery] string? ageRating = null,
            [FromQuery] List<string>? genres = null,
            [FromQuery] bool matchAllGenres = false,
            [FromQuery] int? year = null,
            [FromQuery] List<string>? tagNamesInclude = null,
            [FromQuery] List<string>? tagNamesExclude = null,
            [FromQuery] bool matchAllTagsIn = false,
            [FromQuery] bool matchAllTagsEx = true)
        {
            try
            {
                // Build a MovieSearchRequest from the incoming query parameters.
                // Use name -> TitleContains, streamingServices -> StreamingProviderNames,
                // genres -> GenreNames, tag includes/excludes -> Include/ExcludeWarningNames.
                // Disable API fallback here to avoid external calls during a simple GET.
                var movieSearchReq = new MovieSearchController.MovieSearchRequest
                {
                    TitleContains = name,
                    StreamingProviderNames = streamingServices ?? new List<string>(),
                    GenreNames = genres ?? new List<string>(),
                    IncludeWarningNames = tagNamesInclude ?? new List<string>(),
                    ExcludeWarningNames = tagNamesExclude ?? new List<string>(),
                    Take = 200, // fetch a reasonably large candidate set
                    MinMatches = 1,
                    EnableApiFallback = false,
                    MaxApiAdds = 0,
                    WatchRegion = "US"
                };

                // Delegate the core matching to MovieSearchController.
                var searchController = new MovieSearchController(_context);
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

                // Apply any remaining advanced in-memory filtering (matchAll flags, year, age rating, exact streaming/genre match logic)
                var final = AdvancedSearch(dtoList, name, streamingServices, matchAllStreaming,
                    ageRating, genres, matchAllGenres, year,
                    tagNamesInclude, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

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
