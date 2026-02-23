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
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController(ApplicationDbContext context) : ControllerBase
    {
        private ApplicationDbContext _context = context;

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

                // name (substring, case-insensitive)
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (string.IsNullOrWhiteSpace(m.Name) ||
                        !m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // age rating (exact, case-insensitive)
                if (!string.IsNullOrWhiteSpace(ageRating))
                {
                    if (string.IsNullOrWhiteSpace(m.AgeRating) ||
                        !string.Equals(m.AgeRating, ageRating, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // genres
                if (genreList != null && genreList.Any())
                {
                    var hasGenreMatches = matchAllGenres
                        ? genreList.All(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, StringComparison.OrdinalIgnoreCase)))
                        : genreList.Any(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, StringComparison.OrdinalIgnoreCase)));

                    if (!hasGenreMatches) continue;
                }

                // streaming services
                if (svcList != null && svcList.Any())
                {
                    // Normalize provider names from the DTO (use ProviderName)
                    var movieProviderNames = (m.StreamingProviders ?? Enumerable.Empty<MoviesView.StreamingProviderView>())
                        .Select(sp => (sp.ProviderName ?? string.Empty).Trim())
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .ToList();

                    var hasStreamingMatches = matchAllStreaming
                        ? svcList.All(query => movieProviderNames.Any(p => string.Equals(p, query, StringComparison.OrdinalIgnoreCase)))
                        : svcList.Any(query => movieProviderNames.Any(p => string.Equals(p, query, StringComparison.OrdinalIgnoreCase)));

                    if (!hasStreamingMatches) continue;
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

        /// <summary>
        /// Loads movies from the database and projects to the MoviesView DTO.
        /// Returns both DTOs and the original entity list so callers can inspect entity fields (like UpdatedAt)
        /// </summary>
        private async Task<(List<MoviesView> Dtos, List<Movie> Entities)> LoadMovieDtosAsync()
        {
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                .Include(m => m.MovieWarnings).ThenInclude(mw => mw.DtddTopic)
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
                // populate string list of genre names for AdvancedSearch convenience
                Genre = m.MovieGenres.Select(g => g.TmdbGenre?.GenreName ?? string.Empty).Where(s => !string.IsNullOrWhiteSpace(s)).ToList(),
                StreamingProviders = m.MovieStreamings
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new Models.MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider?.ProviderName ?? string.Empty
                    })
                    .ToList(),
                Tags = new Models.TagsView
                {
                    // Trigger tags pulled from movie_warnings -> warnings (DtddTopic)
                    // Only include movie_warnings where Answer == "yes" (case-insensitive)
                    TriggerTags = (m.MovieWarnings ?? Enumerable.Empty<MovieWarning>())
                        .Where(mw => string.Equals(mw.Answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                        .Select(mw => new Models.TagsView.TriggerTag
                        {
                            TagID = mw.DtddTopicId,
                            TagName = mw.DtddTopic?.TopicName ?? string.Empty
                        })
                        .Where(t => !string.IsNullOrWhiteSpace(t.TagName))
                        .GroupBy(t => t.TagName!.Trim().ToLowerInvariant())
                        .Select(g => g.First())
                        .ToList(),
                    // Keep other tag lists empty for now
                    PlotTags = new List<Models.TagsView.PlotTag>(),
                    PersonTags = new List<Models.TagsView.PersonTag>()
                },
                TagVotes = new List<Models.MoviesView.TagVote>()
            }).ToList();

            return (dtoList, loaded);
        }

        /// <summary>
        /// Search movies by multiple optional criteria.
        /// If no DB results are found the internal api/MovieSearch endpoint is called to
        /// attempt to fetch & upsert matches from external APIs; if that adds movies we reload DB and search again.
        /// The internal MovieSearch call is only issued when the relevant DB movie(s) are older than one week.
        /// This revision calls the MovieSearch POST endpoint (not GET) so the server will run TryApiFillAsync
        /// and therefore force DTDD warnings enrichment when applicable.
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
            // Load DTOs + entities from DB (first pass)
            var (dtoList, loadedEntities) = await LoadMovieDtosAsync();

            // Run AdvancedSearch against the projected DTOs
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

            // If DB search returned nothing, decide whether to call internal MovieSearch.
            if (!results.Any())
            {
                bool allowFallback = true;

                // If a name filter is provided, only fallback when the matching DB entities are older than a week
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var matchingEntities = loadedEntities
                        .Where(e => !string.IsNullOrWhiteSpace(e.Title) && e.Title.Contains(name, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    // If there are matching entities and all of them were updated within the past week, skip fallback.
                    if (matchingEntities.Any() && matchingEntities.All(e => (DateTime.UtcNow - e.UpdatedAt).TotalDays < 7))
                    {
                        allowFallback = false;
                    }
                }
                else
                {
                    // No name filter: if all DB movies are updated within a week, skip fallback.
                    if (loadedEntities.Any() && loadedEntities.All(e => (DateTime.UtcNow - e.UpdatedAt).TotalDays < 7))
                    {
                        allowFallback = false;
                    }
                }

                if (allowFallback)
                {
                    try
                    {
                        // Call MovieSearch POST (not GET) to ensure TryApiFillAsync runs and warnings are enriched.
                        var baseUrl = $"{Request.Scheme}://{Request.Host}";
                        var movieSearchUrl = $"{baseUrl}/api/MovieSearch";

                        var reqBody = new
                        {
                            Take = 25,
                            MinMatches = 0,
                            EnableApiFallback = true,
                            AlwaysAddFromApis = false,
                            MaxApiAdds = 25,
                            WatchRegion = "US",
                            TitleContains = string.IsNullOrWhiteSpace(name) ? null : name
                        };

                        var jsonReq = JsonSerializer.Serialize(reqBody);
                        using var http = new HttpClient();
                        using var content = new StringContent(jsonReq, Encoding.UTF8, "application/json");

                        using var resp = await http.PostAsync(movieSearchUrl, content);
                        //capture for debugging
                        Console.WriteLine("MovieSearch POST status: " + resp.StatusCode);

                        var respContent = await resp.Content.ReadAsStringAsync();

                        // Always capture response for diagnostics
                        Console.WriteLine("MovieSearch response: " + respContent);

                        if (resp.IsSuccessStatusCode)
                        {
                            // Parse addedFromApis + results.imdbId (if present)
                            using var doc = JsonDocument.Parse(respContent);
                            var root = doc.RootElement;

                            int addedFromApis = 0;
                            if (root.TryGetProperty("addedFromApis", out var addedEl) && addedEl.ValueKind == JsonValueKind.Number && addedEl.TryGetInt32(out var addedVal))
                                addedFromApis = addedVal;
                            else if (root.TryGetProperty("AddedFromApis", out var addedEl2) && addedEl2.ValueKind == JsonValueKind.Number && addedEl2.TryGetInt32(out var addedVal2))
                                addedFromApis = addedVal2;

                            // If MovieSearch added rows (including warnings), reload DB projection
                            (dtoList, loadedEntities) = await LoadMovieDtosAsync();

                            
                        }

                        // else non-fatal: if internal call fails or returns non-success, continue and return empty results.
                    }
                    catch
                    {
                        // Non-fatal: if internal call fails, continue and return empty results.
                    }
                }
            }
            // Final search pass (in case MovieSearch added results)
            var finalResults = AdvancedSearch(
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

            results = finalResults;
            return Ok(results);
            
        }

        // Also update GetMoviesView_ParseImdb to include genre_name in the projection
        [HttpGet()]
        public async Task<IActionResult> GetMoviesView_ParseImdb()
        {
            var (dto, _) = await LoadMovieDtosAsync();
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