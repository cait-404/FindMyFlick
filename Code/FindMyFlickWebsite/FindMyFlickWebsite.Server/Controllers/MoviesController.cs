using FindMyFlickWebsite.Server.Models;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly List<MoviesView> _movies;
        private ApplicationDbContext _context;


        public MoviesController(ApplicationDbContext context)
        {

            _context = context;

        }

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

            var svcList = streamingServices?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var genreList = genres?.Where(g => !string.IsNullOrWhiteSpace(g)).ToList();
            var tagNameListInclude = tagNamesInclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();
            var tagNameListExclude = tagNamesExclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();

            foreach (var m in source)
            {
                if (m == null) continue;

                // name (substring, case-insensitive)
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (string.IsNullOrWhiteSpace(m.Name) ||
                        !m.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // age rating (exact, case-insensitive)
                if (!string.IsNullOrWhiteSpace(ageRating))
                {
                    if (string.IsNullOrWhiteSpace(m.AgeRating) ||
                        !string.Equals(m.AgeRating, ageRating, System.StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // genres
                if (genreList != null && genreList.Any())
                {
                    var hasGenreMatches = matchAllGenres
                        ? genreList.All(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, System.StringComparison.OrdinalIgnoreCase)))
                        : genreList.Any(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, System.StringComparison.OrdinalIgnoreCase)));

                    if (!hasGenreMatches) continue;
                }

                // year
                if (year.HasValue && m.Year != year.Value) continue;

                // tags include (check tag names across PlotTags, TriggerTags, PersonTags)
                if (tagNameListInclude != null && tagNameListInclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<TagsView.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

                    var queryTagNamesLower = tagNameListInclude.Select(t => t.ToLowerInvariant()).ToList();

                    var tagMatch = matchAllTagsIn
                        ? queryTagNamesLower.All(q => movieTagNames.Contains(q))
                        : queryTagNamesLower.Any(q => movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                // tags exclude (check tag names across PlotTags, TriggerTags, PersonTags)
                if (tagNameListExclude != null && tagNameListExclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<TagsView.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

                    var queryTagNamesLower = tagNameListExclude.Select(t => t.ToLowerInvariant()).ToList();

                    var tagMatch = matchAllTagsEx
                        ? queryTagNamesLower.All(q => !movieTagNames.Contains(q))
                        : queryTagNamesLower.Any(q => !movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                yield return m;
            }
        }

        /// 
        /// Get a single movie by id.
        /// 
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MoviesView), 200)]
        [ProducesResponseType(404)]
        public ActionResult<MoviesView> GetById(int id)
        {
            return Ok("wip");
        }

        /// <summary>
        /// Search movies by multiple optional criteria.
        /// Query example:
        /// GET api/movies/search?name=cool&streamingServices=netflix&streamingServices=hulu&genres=action&year=2012&tagNames=Violence&matchAllTags=true
        /// </summary>
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
            // Load Movie data models, include MovieGenres -> Genre so we can read genre_name
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = loaded.Select(m => new Models.MoviesView
            {
                ID = ParseImdbToInt(m.ImdbId),
                Name = m.Title,
                Year = m.ReleaseYear,
                AgeRating = m.MpaaRating,
                Summary = m.PlotSummary,
                Poster = m.PosterUrl,
                GenreEntries = m.MovieGenres
                    .Select(g => new Models.MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    })
                    .ToList(),
                StreamingProviders = m.MovieStreamings
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new Models.MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider.ProviderName
                    })
                    .ToList(),
                Tags = new Models.TagsView(),
                TagVotes = new List<Models.MoviesView.TagVote>()
            }).ToList();

            // Run the existing AdvancedSearch helper against the projected DTOs
            var results = AdvancedSearch(
                dtoList,
                name,
                streamingServices,
                matchAllStreaming,
                ageRating,
                genres,
                matchAllGenres,
                year,
                tagNamesInclude,
                tagNamesExclude,
                matchAllTagsIn,
                matchAllTagsEx).ToList();

            // If DB search returned nothing, attempt to call internal MovieSearch API to fetch from external APIs,
            // upsert into DB, then re-run the database search.
            if (!results.Any())
            {
                try
                {
                    // Build query for the MovieSearch endpoint. We'll forward TitleContains (name) and request an API fill.
                    var queryParams = new List<KeyValuePair<string, string?>>();

                    // Ensure API fallback will run and attempt to add matches for the title.
                    queryParams.Add(new KeyValuePair<string, string?>("EnableApiFallback", "true"));
                    queryParams.Add(new KeyValuePair<string, string?>("AlwaysAddFromApis", "true"));

                    if (!string.IsNullOrWhiteSpace(name))
                        queryParams.Add(new KeyValuePair<string, string?>("TitleContains", name));

                    // Keep reasonable defaults; allow the caller to override via query if desired.
                    queryParams.Add(new KeyValuePair<string, string?>("Take", "25"));
                    queryParams.Add(new KeyValuePair<string, string?>("MinMatches", "1"));
                    queryParams.Add(new KeyValuePair<string, string?>("MaxApiAdds", "10"));
                    queryParams.Add(new KeyValuePair<string, string?>("WatchRegion", "US"));

                    var baseUrl = $"{Request.Scheme}://{Request.Host}";
                    var movieSearchUrl = QueryHelpers.AddQueryString($"{baseUrl}/api/MovieSearch", queryParams);

                    using var http = new HttpClient();
                    // Call internal MovieSearch GET endpoint
                    var json = await http.GetStringAsync(movieSearchUrl);

                    // Parse addedFromApis from the response (controller returns camelCase JSON)
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    int addedFromApis = 0;
                    if (root.TryGetProperty("addedFromApis", out var addedEl) && addedEl.ValueKind == JsonValueKind.Number && addedEl.TryGetInt32(out var addedVal))
                        addedFromApis = addedVal;
                    else if (root.TryGetProperty("AddedFromApis", out var addedEl2) && addedEl2.ValueKind == JsonValueKind.Number && addedEl2.TryGetInt32(out var addedVal2))
                        addedFromApis = addedVal2;

                    // If MovieSearch added any movies, re-load DB and re-run AdvancedSearch so we return DB-backed results.
                    if (addedFromApis > 0)
                    {
                        loaded = await _context.Movies
                            .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                            .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                            .AsNoTracking()
                            .ToListAsync();

                        dtoList = loaded.Select(m => new Models.MoviesView
                        {
                            ID = ParseImdbToInt(m.ImdbId),
                            Name = m.Title,
                            Year = m.ReleaseYear,
                            AgeRating = m.MpaaRating,
                            Summary = m.PlotSummary,
                            Poster = m.PosterUrl,
                            GenreEntries = m.MovieGenres
                                .Select(g => new Models.MoviesView.GenreEntry
                                {
                                    TmdbGenreId = g.TmdbGenreId,
                                    GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                                })
                                .ToList(),
                            StreamingProviders = m.MovieStreamings
                                .GroupBy(ms => ms.TmdbProviderId)
                                .Select(g => new Models.MoviesView.StreamingProviderView
                                {
                                    Id = g.Key,
                                    ProviderName = g.First().TmdbProvider.ProviderName
                                })
                                .ToList(),
                            Tags = new Models.TagsView(),
                            TagVotes = new List<Models.MoviesView.TagVote>()
                        }).ToList();

                        results = AdvancedSearch(
                            dtoList,
                            name,
                            streamingServices,
                            matchAllStreaming,
                            ageRating,
                            genres,
                            matchAllGenres,
                            year,
                            tagNamesInclude,
                            tagNamesExclude,
                            matchAllTagsIn,
                            matchAllTagsEx).ToList();
                    }
                    else
                    {
                        // No items added by API; results remain empty.
                    }
                }
                catch
                {
                    // Non-fatal: if internal call fails, continue and return empty results.
                }
            }

            return Ok(results);
        }

        // Also update GetMoviesView_ParseImdb to include genre_name in the projection
        [HttpGet()]
        public async Task<IActionResult> GetMoviesView_ParseImdb()
        {
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                .AsNoTracking()
                .ToListAsync(); // client-side projection follows

            var dto = loaded.Select(m => new Models.MoviesView
            {
                ID = ParseImdbToInt(m.ImdbId),             // parsed on client
                Name = m.Title,
                Year = m.ReleaseYear,
                AgeRating = m.MpaaRating,
                Summary = m.PlotSummary,
                Poster = m.PosterUrl,
                GenreEntries = m.MovieGenres
                    .Select(g => new Models.MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    })
                    .ToList(),
                StreamingProviders = m.MovieStreamings
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new Models.MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider.ProviderName
                    })
                    .ToList(),
                Tags = new Models.TagsView(),
                TagVotes = new List<Models.MoviesView.TagVote>()
            }).ToList();

            return Ok(dto);
        }

        private static int ParseImdbToInt(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
                return 0;
            // Assumes IMDb IDs are in the form "tt1234567"
            var digits = new string(imdbId.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var result) ? result : 0;
        }
    }
}