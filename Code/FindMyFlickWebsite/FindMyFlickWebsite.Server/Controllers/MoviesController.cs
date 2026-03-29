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

        // Normalizes a user-supplied tag name into the DB's tag_text_norm form:
        // - lower-case
        // - non-alphanumerics removed
        // - spaces -> underscores
        // - collapse multiple underscores / trim leading/trailing underscores
        private static string NormalizeToTagTextNorm(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var sb = new StringBuilder();
            var prevWasUnderscore = false;
            foreach (var ch in input.Trim().ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                    prevWasUnderscore = false;
                    continue;
                }

                if (char.IsWhiteSpace(ch) || ch == '_' || ch == '-')
                {
                    if (!prevWasUnderscore)
                    {
                        sb.Append('_');
                        prevWasUnderscore = true;
                    }
                    continue;
                }

                // drop other punctuation/symbols
            }

            var s = sb.ToString().Trim('_');
            // collapse repeat underscores (in case)
            while (s.Contains("__")) s = s.Replace("__", "_");
            return s;
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

            // tagNamesInclude will be provided as raw user strings; normalize them to tag_text_norm form
            var tagNameListIncludeNorm = (tagNamesInclude ?? Enumerable.Empty<string>())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => NormalizeToTagTextNorm(t))
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            // tagNamesExclude remains legacy: we use it across all tag types (plot/trigger/person),
            // but we normalize for case-insensitive comparison (not to tag_text_norm).
            var tagNameListExclude = (tagNamesExclude ?? Enumerable.Empty<string>())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim().ToLowerInvariant())
                .Distinct()
                .ToList();

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

                // Plot tag include (NEW behavior) � operates on normalized values stored in DTO PlotTags.TagName
                if (tagNameListIncludeNorm.Any())
                {
                    var moviePlotTagNorms = (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>())
                        .Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToHashSet();

                    var tagMatch = matchAllTagsIn
                        ? tagNameListIncludeNorm.All(q => moviePlotTagNorms.Contains(q))
                        : tagNameListIncludeNorm.Any(q => moviePlotTagNorms.Contains(q));

                    if (!tagMatch) continue;
                }

                // Tag exclude (legacy: checks across all tag lists)
                if (tagNameListExclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>())
                            .Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>())
                            .Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<TagsView.PersonTag>())
                            .Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                    );

                    var tagMatch = matchAllTagsEx
                        ? tagNameListExclude.All(q => !movieTagNames.Contains(q))
                        : tagNameListExclude.Any(q => !movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                yield return m;
            }
        }

        // ============================================================
        // DATABASE LOADER
        // - populate PlotTags using plot_tags.tag_text_norm for searching
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
                    PlotTags = new List<TagsView.PlotTag>(),    // populated below
                    PersonTags = new List<TagsView.PersonTag>()
                },
                TagVotes = new List<MoviesView.TagVote>()
            }).ToList();

            // Populate PlotTags for DTOs using plot_tags.tag_text_norm (normalized form)
            if (dtoList.Count > 0)
            {
                var imdbIds = loaded.Select(m => m.ImdbId).Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList();

                var plotRows = await _context.MoviePlotTags
                    .AsNoTracking()
                    .Include(mpt => mpt.PlotTag)
                    .Where(mpt => imdbIds.Contains(mpt.ImdbId) && mpt.Status == "approved" && EF.Functions.ILike(mpt.Status, "approved"))
                    .ToListAsync();

                var byImdb = plotRows.GroupBy(r => r.ImdbId).ToDictionary(g => g.Key, g => g.ToList());

                foreach (var dto in dtoList)
                {
                    if (dto == null) continue;
                    // Find original imdb string from loaded entities
                    var movieEntity = loaded.FirstOrDefault(x => ParseImdbToInt(x.ImdbId) == dto.ID);
                    if (movieEntity == null) continue;
                    var mid = movieEntity.ImdbId;

                    if (byImdb.TryGetValue(mid, out var rows))
                    {
                        dto.Tags.PlotTags = rows
                            .Where(r => r.PlotTag != null)
                            .Select(r => new TagsView.PlotTag
                            {
                                TagID = r.PlotTagId,
                                // Use tag_text_norm for searching � store the normalized text here
                                TagName = r.PlotTag?.TagTextNorm ?? (r.PlotTag?.TagText ?? string.Empty),
                                TagType = "plot"
                            })
                            .GroupBy(t => (t.TagName ?? string.Empty).ToLowerInvariant())
                            .Select(g => g.First())
                            .ToList();
                    }
                }
            }

            return (dtoList, loaded);
        }

        // ============================================================
        // SEARCH (GET /api/Movies/search)
        // Uses helper ResolveImdbIdsByPlotTagNormsAsync for plot-tag-only DB searches
        // ============================================================
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MoviesView>), 200)]
        public async Task<ActionResult<IEnumerable<MoviesView>>> Search(
            [FromQuery(Name = "titleContains")] string? titleContains = null,
            [FromQuery(Name = "name")] string? name = null,
            [FromQuery(Name = "genreNames")] List<string>? genreNames = null,
            [FromQuery(Name = "personNames")] List<string>? personNames = null,
            [FromQuery(Name = "mpaaRatings")] List<string>? mpaaRatings = null,
            [FromQuery(Name = "streamingProviderNames")] List<string>? streamingProviderNames = null,
            [FromQuery(Name = "includeWarningNames")] List<string>? includeWarningNames = null,
            [FromQuery(Name = "excludeWarningNames")] List<string>? excludeWarningNames = null,
            [FromQuery(Name = "take")] int take = 25,
            [FromQuery(Name = "recommendationTake")] int recommendationTake = 0,
            [FromQuery(Name = "enableApiFallback")] bool enableApiFallback = false,
            [FromQuery(Name = "maxApiAdds")] int maxApiAdds = 25,
            [FromQuery(Name = "watchRegion")] string? watchRegion = "US",
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
                var titleFilter = string.IsNullOrWhiteSpace(titleContains) ? name : titleContains;

                genreNames = genreNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                personNames = personNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                mpaaRatings = mpaaRatings?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                streamingProviderNames = streamingProviderNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                includeWarningNames = includeWarningNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();
                excludeWarningNames = excludeWarningNames?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() ?? new List<string>();

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

                // NOTE: do not copy plot-tag params into the warning lists.
                // Previously we did:
                //   if ((includeWarningNames == null || includeWarningNames.Count == 0) && tagNamesInclude != null && tagNamesInclude.Count > 0)
                //       includeWarningNames = tagNamesInclude;
                //   if ((excludeWarningNames == null || excludeWarningNames.Count == 0) && tagNamesExclude != null && tagNamesExclude.Count > 0)
                //       excludeWarningNames = tagNamesExclude;
                //
                // Removing those assignments ensures that when only `tagNamesInclude`/`tagNamesExclude`
                // are provided the controller treats the MovieSearchRequest as empty and runs the DB plot-tag search.

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

                bool MovieSearchRequestIsEmpty =
                    string.IsNullOrWhiteSpace(movieSearchReq.TitleContains) &&
                    (movieSearchReq.GenreNames == null || movieSearchReq.GenreNames.Count == 0) &&
                    (movieSearchReq.PersonNames == null || movieSearchReq.PersonNames.Count == 0) &&
                    (movieSearchReq.MpaaRatings == null || movieSearchReq.MpaaRatings.Count == 0) &&
                    (movieSearchReq.StreamingProviderNames == null || movieSearchReq.StreamingProviderNames.Count == 0) &&
                    (movieSearchReq.IncludeWarningNames == null || movieSearchReq.IncludeWarningNames.Count == 0) &&
                    (movieSearchReq.ExcludeWarningNames == null || movieSearchReq.ExcludeWarningNames.Count == 0);

                var includeNorms = (tagNamesInclude ?? Enumerable.Empty<string>())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => NormalizeToTagTextNorm(t))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                var excludeNorms = (tagNamesExclude ?? Enumerable.Empty<string>())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => NormalizeToTagTextNorm(t))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                List<MoviesView> dtoList;

                // If MovieSearchRequest is empty but caller supplied plot-tag filters, do DB search by plot tags directly.
                if (MovieSearchRequestIsEmpty && (includeNorms.Any() || excludeNorms.Any()))
                {
                    // Use helper that resolves TagTextNorm -> PlotTagId -> MoviePlotTags reliably
                    var imdbIds = await ResolveImdbIdsByPlotTagNormsAsync(includeNorms, excludeNorms, matchAllTagsIn);
                    if (!imdbIds.Any())
                        return Ok(Array.Empty<MoviesView>());

                    var loaded = await _context.Movies
                        .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                        .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                        .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                        .AsNoTracking()
                        .Where(m => imdbIds.Contains(m.ImdbId))
                        .ToListAsync();

                    dtoList = loaded.Select(m => new MoviesView
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

                    var plotRows = await _context.MoviePlotTags
                        .AsNoTracking()
                        .Include(mpt => mpt.PlotTag)
                        .Where(mpt => imdbIds.Contains(mpt.ImdbId) && mpt.Status == "approved")
                        .ToListAsync();

                    var byImdb = plotRows.GroupBy(r => r.ImdbId).ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var dto in dtoList)
                    {
                        var movieEntity = loaded.FirstOrDefault(x => ParseImdbToInt(x.ImdbId) == dto.ID);
                        if (movieEntity == null) continue;

                        if (byImdb.TryGetValue(movieEntity.ImdbId, out var rows))
                        {
                            dto.Tags.PlotTags = rows
                                .Where(r => r.PlotTag != null)
                                .Select(r => new TagsView.PlotTag
                                {
                                    TagID = r.PlotTagId,
                                    TagName = r.PlotTag?.TagTextNorm ?? (r.PlotTag?.TagText ?? string.Empty),
                                    TagType = "plot"
                                })
                                .GroupBy(t => (t.TagName ?? string.Empty).ToLowerInvariant())
                                .Select(g => g.First())
                                .ToList();
                        }
                    }

                    var finalFromDb = AdvancedSearch(dtoList, titleFilter ?? name, streamingProviderNames, matchAllStreaming,
                        null, genreNames, matchAllGenres, year,
                        null, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

                    finalFromDb = finalFromDb.OrderByDescending(m => m.Year).ThenBy(m => m.Name).ToList();
                    return Ok(finalFromDb);
                }

                // Otherwise fall back to the existing MovieSearchController path
                await using var ctxForSearch = _dbContextFactory.CreateDbContext();
                var searchController = new MovieSearchController(ctxForSearch);
                var searchActionResult = await searchController.Search(movieSearchReq);

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
                    return StatusCode(500, new { message = "MovieSearchController.Search returned unexpected result shape." });
                }

                // If MovieSearchController returned no results but caller supplied plot tags, try the DB-plot-tag search as fallback
                if (searchResp == null || searchResp.Results == null || searchResp.Results.Count == 0)
                {
                    if (includeNorms.Any() || excludeNorms.Any())
                    {
                        var imdbIds = await ResolveImdbIdsByPlotTagNormsAsync(includeNorms, excludeNorms, matchAllTagsIn);
                        if (!imdbIds.Any()) return Ok(Array.Empty<MoviesView>());

                        var loaded2 = await _context.Movies
                            .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                            .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                            .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                            .AsNoTracking()
                            .Where(m => imdbIds.Contains(m.ImdbId))
                            .ToListAsync();

                        dtoList = loaded2.Select(m => new MoviesView
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

                        var plotRows2 = await _context.MoviePlotTags
                            .AsNoTracking()
                            .Include(mpt => mpt.PlotTag)
                            .Where(mpt => imdbIds.Contains(mpt.ImdbId) && mpt.Status == "approved")
                            .ToListAsync();

                        var byImdb2 = plotRows2.GroupBy(r => r.ImdbId).ToDictionary(g => g.Key, g => g.ToList());

                        foreach (var dto in dtoList)
                        {
                            var movieEntity = loaded2.FirstOrDefault(x => ParseImdbToInt(x.ImdbId) == dto.ID);
                            if (movieEntity == null) continue;

                            if (byImdb2.TryGetValue(movieEntity.ImdbId, out var rows))
                            {
                                dto.Tags.PlotTags = rows
                                    .Where(r => r.PlotTag != null)
                                    .Select(r => new TagsView.PlotTag
                                    {
                                        TagID = r.PlotTagId,
                                        TagName = r.PlotTag?.TagTextNorm ?? (r.PlotTag?.TagText ?? string.Empty),
                                        TagType = "plot"
                                    })
                                    .GroupBy(t => (t.TagName ?? string.Empty).ToLowerInvariant())
                                    .Select(g => g.First())
                                    .ToList();
                            }
                        }

                        var finalFromDb2 = AdvancedSearch(dtoList, titleFilter ?? name, streamingProviderNames, matchAllStreaming,
                            null, genreNames, matchAllGenres, year,
                            null, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

                        finalFromDb2 = finalFromDb2.OrderByDescending(m => m.Year).ThenBy(m => m.Name).ToList();
                        return Ok(finalFromDb2);
                    }

                    return Ok(Array.Empty<MoviesView>());
                }

                // Existing flow when MovieSearchController returned results
                var imdbIdsResp = searchResp.Results
                    .Select(r => r.ImdbId)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Distinct()
                    .ToList();

                if (!imdbIdsResp.Any())
                    return Ok(Array.Empty<MoviesView>());

                var loadedResp = await _context.Movies
                    .Include(m => m.MovieGenres).ThenInclude(g => g.TmdbGenre)
                    .Include(m => m.MovieStreamings).ThenInclude(s => s.TmdbProvider)
                    .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                    .AsNoTracking()
                    .Where(m => imdbIdsResp.Contains(m.ImdbId))
                    .ToListAsync();

                dtoList = loadedResp.Select(m => new MoviesView
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

                // Populate plot tags for results (use tag_text_norm)
                if (dtoList.Count > 0)
                {
                    var plotRows = await _context.MoviePlotTags
                        .AsNoTracking()
                        .Include(mpt => mpt.PlotTag)
                        .Where(mpt => imdbIdsResp.Contains(mpt.ImdbId) && mpt.Status != null && EF.Functions.ILike(mpt.Status, "approved"))
                        .ToListAsync();

                    var byImdb = plotRows.GroupBy(r => r.ImdbId).ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var dto in dtoList)
                    {
                        var movieEntity = loadedResp.FirstOrDefault(x => ParseImdbToInt(x.ImdbId) == dto.ID);
                        if (movieEntity == null) continue;

                        if (byImdb.TryGetValue(movieEntity.ImdbId, out var rows))
                        {
                            dto.Tags.PlotTags = rows
                                .Where(r => r.PlotTag != null)
                                .Select(r => new TagsView.PlotTag
                                {
                                    TagID = r.PlotTagId,
                                    // store normalized token for matching
                                    TagName = r.PlotTag?.TagTextNorm ?? (r.PlotTag?.TagText ?? string.Empty),
                                    TagType = "plot"
                                })
                                .GroupBy(t => (t.TagName ?? string.Empty).ToLowerInvariant())
                                .Select(g => g.First())
                                .ToList();
                        }
                    }
                }

                var filteredByPlot = dtoList;
                if (includeNorms.Any())
                {
                    if (matchAllTagsIn)
                    {
                        filteredByPlot = filteredByPlot
                            .Where(d => includeNorms.All(q => (d.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>())
    .Select(pt => (pt.TagName ?? string.Empty).Trim().ToLowerInvariant())
    .Contains(q)))
                            .ToList();
                    }
                    else
                    {
                        filteredByPlot = filteredByPlot
                           .Where(d => includeNorms.Any(q => (d.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>())
    .Select(pt => (pt.TagName ?? string.Empty).Trim().ToLowerInvariant())
    .Contains(q)))
                            .ToList();
                    }
                }

                var final = AdvancedSearch(filteredByPlot, titleFilter ?? name, streamingProviderNames, matchAllStreaming,
                    null, genreNames, matchAllGenres, year,
                    null, tagNamesExclude, matchAllTagsIn, matchAllTagsEx).ToList();

                var position = imdbIdsResp.Select((id, idx) => (id, idx)).ToDictionary(x => x.id, x => x.idx);
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

                // Apply ordering based on query param:
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

        // NEW helper: resolve include/exclude normalized tag tokens to imdb ids using plot_tag ids.
        private async Task<List<string>> ResolveImdbIdsByPlotTagNormsAsync(
            IReadOnlyCollection<string> includeNorms,
            IReadOnlyCollection<string> excludeNorms,
            bool matchAllTagsIn)
        {
            // If no include nor exclude norms, return all imdb ids
            if ((includeNorms == null || includeNorms.Count == 0) && (excludeNorms == null || excludeNorms.Count == 0))
                return await _context.Movies.AsNoTracking().Select(m => m.ImdbId).ToListAsync();

            // Base join of approved MoviePlotTags -> PlotTags
            var baseJoin = _context.MoviePlotTags
                .AsNoTracking()
                .Where(mpt => mpt.Status != null && EF.Functions.ILike(mpt.Status, "approved"))
                .Join(_context.PlotTags.AsNoTracking(),
                      mpt => mpt.PlotTagId,
                      pt => pt.PlotTagId,
                      (mpt, pt) => new { mpt.ImdbId, pt.TagTextNorm, mpt.PlotTagId });

            List<string> imdbIds;

            // INCLUDE processing
            if (includeNorms != null && includeNorms.Count > 0)
            {
                // Any-match
                if (!matchAllTagsIn)
                {
                    imdbIds = await baseJoin
                        .Where(x => includeNorms.Contains(x.TagTextNorm))
                        .Select(x => x.ImdbId)
                        .Distinct()
                        .ToListAsync();
                }
                else
                {
                    // All-match: group by imdb and ensure distinct tag_text_norm count >= includeNorms.Count
                    imdbIds = await baseJoin
                        .Where(x => includeNorms.Contains(x.TagTextNorm))
                        .GroupBy(x => x.ImdbId)
                        .Where(g => g.Select(x => x.TagTextNorm).Distinct().Count() >= includeNorms.Count)
                        .Select(g => g.Key)
                        .ToListAsync();
                }

                if (imdbIds.Count == 0)
                    return new List<string>();
            }
            else
            {
                // No include constraint -> start with all movie ids referenced by approved MoviePlotTags
                imdbIds = await baseJoin.Select(x => x.ImdbId).Distinct().ToListAsync();
            }

            // EXCLUDE processing: remove movies that reference any excluded normalized tag
            if (excludeNorms != null && excludeNorms.Count > 0 && imdbIds.Count > 0)
            {
                var excludedImdbs = await baseJoin
                    .Where(x => excludeNorms.Contains(x.TagTextNorm))
                    .Select(x => x.ImdbId)
                    .Distinct()
                    .ToListAsync();

                if (excludedImdbs.Count > 0)
                    imdbIds = imdbIds.Except(excludedImdbs).ToList();
            }

            return imdbIds;
        }
    }
}
