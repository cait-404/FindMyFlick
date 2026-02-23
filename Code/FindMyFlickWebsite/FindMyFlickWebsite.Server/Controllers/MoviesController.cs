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
                var (dtoList, loadedEntities) = await LoadMovieDtosAsync();
                var results = AdvancedSearch(dtoList, name, streamingServices, matchAllStreaming,
                    ageRating, genres, matchAllGenres, year,
                    tagNamesInclude, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        // ============================================================
        // DEFAULT GET (GET /api/Movies)
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> GetMoviesView_ParseImdb()
        {
            try
            {
                var (dto, _) = await LoadMovieDtosAsync();
                return Ok(dto);
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
