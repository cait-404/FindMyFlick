using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        // How many movies to attempt per batch run.
        // Keep this reasonable — each movie makes 1-3 HTTP calls to DTDD.
        private const int DefaultBatchSize = 50;
        private const int MaxBatchSize = 200;

        public AdminController(FindmyflickContext context)
        {
            _context = context;
        }

        // =====================================================================
        // RESPONSE TYPES
        // =====================================================================

        public sealed class RefreshMissingWarningsResponse
        {
            /// <summary>Total released movies with no warning data found in DB.</summary>
            public int TotalMissingInDb { get; set; }

            /// <summary>How many we attempted this run (capped by BatchSize).</summary>
            public int Attempted { get; set; }

            /// <summary>Successfully enriched with at least one warning row.</summary>
            public int Succeeded { get; set; }

            /// <summary>DTDD had no data for this movie (legitimate gap).</summary>
            public int NoDataOnDtdd { get; set; }

            /// <summary>DTDD API call failed (network error, HTML response, etc).</summary>
            public int DtddCallFailed { get; set; }

            /// <summary>Movie has no TMDB ID — can't look up on DTDD.</summary>
            public int SkippedNoTmdbId { get; set; }

            /// <summary>Remaining movies still needing warnings after this run.</summary>
            public int StillMissingAfterRun { get; set; }

            /// <summary>Sample of IMDb IDs that failed DTDD lookup (up to 10).</summary>
            public List<string> SampleFailed { get; set; } = new();

            /// <summary>Sample of IMDb IDs with no DTDD data (up to 10).</summary>
            public List<string> SampleNoData { get; set; } = new();
        }

        // =====================================================================
        // ENDPOINTS
        // =====================================================================

        /// <summary>
        /// Returns a count of released movies in the DB that have no warning data,
        /// broken down by likely reason. Use this to assess the gap before running
        /// the bulk refresh.
        /// </summary>
        [HttpGet("missing-warnings-summary")]
        public async Task<IActionResult> GetMissingWarningsSummary()
        {
            var now = DateTime.UtcNow;

            var total = await _context.Movies
                .Where(m => !m.MovieWarnings.Any(w => w.Answer != null))
                .CountAsync();

            var unreleased = await _context.Movies
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    (m.ReleaseYear > now.Year ||
                     (m.Status != null && (
                         EF.Functions.ILike(m.Status, "planned") ||
                         EF.Functions.ILike(m.Status, "in production") ||
                         EF.Functions.ILike(m.Status, "post production")
                     ))))
                .CountAsync();

            var releasedMissingWarnings = await _context.Movies
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    m.ReleaseYear <= now.Year &&
                    (m.Status == null ||
                     EF.Functions.ILike(m.Status, "released") ||
                     EF.Functions.ILike(m.Status, "rumored")))
                .CountAsync();

            var noTmdbId = await _context.Movies
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    m.TmdbId == null &&
                    m.ReleaseYear <= now.Year)
                .CountAsync();

            return Ok(new
            {
                TotalWithNoWarnings = total,
                Unreleased = unreleased,
                ReleasedMissingWarnings = releasedMissingWarnings,
                ReleasedWithNoTmdbId = noTmdbId,
                ActionableCount = releasedMissingWarnings
            });
        }

        /// <summary>
        /// Attempts to fetch and populate warnings from DTDD for all released movies
        /// that currently have no warning data. Processes in batches to avoid timeouts.
        /// 
        /// Query params:
        ///   batchSize  — how many movies to attempt (default 50, max 200)
        ///   olderThanYear — only process movies released on or before this year
        ///                   (useful to skip brand-new releases DTDD won't have yet)
        /// </summary>
        [HttpPost("refresh-missing-warnings")]
        public async Task<ActionResult<RefreshMissingWarningsResponse>> RefreshMissingWarnings(
            [FromQuery] int batchSize = DefaultBatchSize,
            [FromQuery] int? olderThanYear = null)
        {
            if (batchSize <= 0) batchSize = DefaultBatchSize;
            if (batchSize > MaxBatchSize) batchSize = MaxBatchSize;

            var dtddKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (string.IsNullOrWhiteSpace(dtddKey))
                return BadRequest("DTDD_API_KEY environment variable is not set.");

            var now = DateTime.UtcNow;
            var yearCutoff = olderThanYear ?? now.Year;

            // Count total scope so the caller knows how much work remains.
            var totalMissing = await _context.Movies
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    m.ReleaseYear <= yearCutoff &&
                    (m.Status == null ||
                     EF.Functions.ILike(m.Status, "released") ||
                     EF.Functions.ILike(m.Status, "rumored")))
                .CountAsync();

            // Fetch the batch — oldest updated_at first so we make steady progress
            // rather than retrying the same movies every run.
            var candidates = await _context.Movies
                .AsNoTracking()
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    m.ReleaseYear <= yearCutoff &&
                    (m.Status == null ||
                     EF.Functions.ILike(m.Status, "released") ||
                     EF.Functions.ILike(m.Status, "rumored")))
                .OrderBy(m => m.UpdatedAt)
                .Take(batchSize)
                .ToListAsync();

            var response = new RefreshMissingWarningsResponse
            {
                TotalMissingInDb = totalMissing,
                Attempted = candidates.Count
            };

            foreach (var movie in candidates)
            {
                if (movie.TmdbId == null || movie.TmdbId <= 0)
                {
                    response.SkippedNoTmdbId++;
                    continue;
                }

                try
                {
                    var result = await TryEnrichWarningsFromDtddAsync(movie, dtddKey);

                    switch (result)
                    {
                        case DtddEnrichResult.Success:
                            response.Succeeded++;
                            break;
                        case DtddEnrichResult.NoData:
                            response.NoDataOnDtdd++;
                            if (response.SampleNoData.Count < 10)
                                response.SampleNoData.Add(movie.ImdbId);
                            break;
                        case DtddEnrichResult.Failed:
                            response.DtddCallFailed++;
                            if (response.SampleFailed.Count < 10)
                                response.SampleFailed.Add(movie.ImdbId);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    response.DtddCallFailed++;
                    if (response.SampleFailed.Count < 10)
                        response.SampleFailed.Add($"{movie.ImdbId} ({ex.GetType().Name})");
                }
            }

            // Recount so the caller knows how much is left.
            response.StillMissingAfterRun = await _context.Movies
                .Where(m =>
                    !m.MovieWarnings.Any(w => w.Answer != null) &&
                    m.ReleaseYear <= yearCutoff &&
                    (m.Status == null ||
                     EF.Functions.ILike(m.Status, "released") ||
                     EF.Functions.ILike(m.Status, "rumored")))
                .CountAsync();

            return Ok(response);
        }

        // =====================================================================
        // DTDD ENRICHMENT (self-contained copy — keeps AdminController
        // independent of MovieSearchController internals)
        // =====================================================================

        private enum DtddEnrichResult { Success, NoData, Failed }

        private async Task<DtddEnrichResult> TryEnrichWarningsFromDtddAsync(Movie movie, string apiKey)
        {
            int? dtddMediaId = null;

            // Check for a manual override first
            var overrideRow = await _context.DtddOverrides.AsNoTracking()
                .FirstOrDefaultAsync(o => o.ImdbId == movie.ImdbId);

            if (overrideRow != null && overrideRow.DtddMediaId > 0)
                dtddMediaId = overrideRow.DtddMediaId;

            // Try each lookup strategy in order of reliability
            if (dtddMediaId == null && !string.IsNullOrWhiteSpace(movie.ImdbId))
                dtddMediaId = await FetchDtddMediaIdByImdbAsync(movie.ImdbId, apiKey);

            if (dtddMediaId == null && movie.TmdbId.HasValue && movie.TmdbId.Value > 0)
                dtddMediaId = await FetchDtddMediaIdByTmdbAsync(movie.TmdbId.Value, apiKey);

            if (dtddMediaId == null && !string.IsNullOrWhiteSpace(movie.Title) && movie.ReleaseYear > 0)
                dtddMediaId = await FetchDtddMediaIdByTitleYearAsync(movie.Title, movie.ReleaseYear, apiKey);

            if (dtddMediaId == null)
                return DtddEnrichResult.NoData;

            List<DtddTopicStatRow> stats;
            try
            {
                stats = await FetchDtddTopicStatsAsync(dtddMediaId.Value, apiKey);
            }
            catch
            {
                return DtddEnrichResult.Failed;
            }

            if (stats.Count == 0)
                return DtddEnrichResult.NoData;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existing = await _context.MovieWarnings
                .Where(mw => mw.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existing.Count > 0)
                _context.MovieWarnings.RemoveRange(existing);

            var knownTopicIds = (await _context.Warnings
                    .AsNoTracking()
                    .Select(w => w.DtddTopicId)
                    .ToListAsync())
                .ToHashSet();

            int added = 0;
            foreach (var s in stats)
            {
                if (!knownTopicIds.Contains(s.TopicId))
                    continue;

                _context.MovieWarnings.Add(new MovieWarning
                {
                    ImdbId = movie.ImdbId,
                    DtddTopicId = s.TopicId,
                    Answer = s.Answer,
                    IsSpoiler = s.IsSpoiler,
                    WarningComment = s.Comment,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                added++;
            }

            // Update the movie's updated_at so the staleness tracker knows
            // we checked this movie, even if DTDD returned all "unknown" answers.
            var tracked = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == movie.ImdbId);
            if (tracked != null)
                tracked.UpdatedAt = now;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return added > 0 ? DtddEnrichResult.Success : DtddEnrichResult.NoData;
        }

        // -------------------------------------------------------------------------
        // DTDD lookup helpers
        // -------------------------------------------------------------------------

        private sealed class DtddTopicStatRow
        {
            public int TopicId { get; set; }
            public string Answer { get; set; } = "unknown";
            public bool? IsSpoiler { get; set; }
            public string? Comment { get; set; }
        }

        private async Task<int?> FetchDtddMediaIdByImdbAsync(string imdbId, string apiKey)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Accept", "application/json");
                http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                var url = $"https://www.doesthedogdie.com/dddsearch?imdb={Uri.EscapeDataString(imdbId)}";
                var json = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("items", out var items) ||
                    items.ValueKind != JsonValueKind.Array)
                    return null;

                foreach (var item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        return idVal;
                }
            }
            catch { /* fall through to next strategy */ }

            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTmdbAsync(int tmdbId, string apiKey)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Accept", "application/json");
                http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                var url = $"https://www.doesthedogdie.com/dddsearch?tmdb={tmdbId}";
                var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return null;

                var contentType = resp.Content.Headers.ContentType?.MediaType ?? "";
                if (!contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
                    return null;

                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("items", out var items) ||
                    items.ValueKind != JsonValueKind.Array)
                    return null;

                foreach (var item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        return idVal;
                }
            }
            catch { /* fall through to next strategy */ }

            return null;
        }

        private async Task<int?> FetchDtddMediaIdByTitleYearAsync(string title, int releaseYear, string apiKey)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Accept", "application/json");
                http.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                var url = $"https://www.doesthedogdie.com/dddsearch?q={Uri.EscapeDataString(title)}";
                var json = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("items", out var items) ||
                    items.ValueKind != JsonValueKind.Array)
                    return null;

                // Prefer year-exact match first
                foreach (var item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("releaseYear", out var yEl) &&
                        yEl.ValueKind == JsonValueKind.Number &&
                        yEl.GetInt32() != releaseYear)
                        continue;

                    if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        return idVal;
                }

                // Fall back to first result
                foreach (var item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                        return idVal;
                }
            }
            catch { /* fall through */ }

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
            var list = new List<DtddTopicStatRow>();

            if (!doc.RootElement.TryGetProperty("topicItemStats", out var statsEl) ||
                statsEl.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var s in statsEl.EnumerateArray())
            {
                var topicId = s.TryGetProperty("TopicId", out var tidEl) &&
                              tidEl.TryGetInt32(out var tidVal) ? tidVal : 0;
                if (topicId <= 0) continue;

                var comment = s.TryGetProperty("comment", out var cEl) ? cEl.GetString() : null;

                bool? isSpoiler = null;
                if (s.TryGetProperty("topic", out var topicEl) &&
                    topicEl.ValueKind == JsonValueKind.Object &&
                    topicEl.TryGetProperty("isSpoiler", out var spEl) &&
                    (spEl.ValueKind == JsonValueKind.True || spEl.ValueKind == JsonValueKind.False))
                    isSpoiler = spEl.GetBoolean();

                int? yesSum = null, noSum = null, isYes = null;
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
                    answer = (y == 0 && n == 0) ? "unknown" : (y >= n && y > 0 ? "yes" : "no");
                }
                else if (isYes.HasValue)
                    answer = isYes.Value == 1 ? "yes" : "no";
                else
                    answer = "unknown";

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
    }
}