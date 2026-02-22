using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace FindMyFlickWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDataController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        // Adjust this if you want 20 instead of 25
        private const int CrewLimit = 25;

        public MovieDataController(FindmyflickContext context)
        {
            _context = context;
        }

        [HttpGet("{imdbId}")]
        public async Task<IActionResult> GetMovieByImdb(string imdbId)
        {
            // 1) DB FIRST
            var movie = await LoadMovieGraphAsync(imdbId);

            // 2) API FALLBACK (only if missing)
            if (movie == null)
            {
                var upserted = await TryUpsertMovieFromTmdbByImdbAsync(imdbId);
                if (upserted)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            if (movie == null)
                return NotFound();

            // 3) ENRICHMENT (MPAA rating) if missing
            if (string.IsNullOrWhiteSpace(movie.MpaaRating))
            {
                var enrichedRating = await TryEnrichMpaaRatingFromTmdbAsync(movie);
                if (enrichedRating)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 4) ENRICHMENT (genres) if missing
            if (!movie.MovieGenres.Any())
            {
                var enrichedGenres = await TryEnrichGenresFromTmdbAsync(movie);
                if (enrichedGenres)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 5) ENRICHMENT (keywords) if missing
            if (!movie.MovieKeywords.Any())
            {
                var enrichedKeywords = await TryEnrichKeywordsFromTmdbAsync(movie);
                if (enrichedKeywords)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 6) ENRICHMENT (cast) if missing
            if (!movie.MovieCasts.Any())
            {
                var enrichedCast = await TryEnrichCastFromTmdbAsync(movie);
                if (enrichedCast)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 7) ENRICHMENT (crew) if missing
            if (!movie.MovieCrews.Any())
            {
                var enrichedCrew = await TryEnrichCrewFromTmdbAsync(movie);
                if (enrichedCrew)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 8) ENRICHMENT (streaming providers) if missing
            if (!movie.MovieStreamings.Any())
            {
                var enrichedStreaming = await TryEnrichStreamingFromTmdbAsync(movie);
                if (enrichedStreaming)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 9) ENRICHMENT (warnings) if missing
            var hasWarnings = await _context.MovieWarnings.AnyAsync(mw => mw.ImdbId == imdbId);
            if (!hasWarnings)
            {
                var enrichedWarnings = await TryEnrichWarningsFromDtddAsync(imdbId);
                if (enrichedWarnings)
                    movie = await LoadMovieGraphAsync(imdbId);
            }

            // 10) WARNINGS (DB ONLY, tiered)
            var tierRows = await LoadWarningTierRowsAsync(imdbId);

            var result = new
            {
                imdbId = movie.ImdbId,
                tmdbId = movie.TmdbId,
                title = movie.Title,
                releaseYear = movie.ReleaseYear,
                mpaaRating = movie.MpaaRating,
                runtimeMinutes = movie.RuntimeMinutes,
                plotSummary = movie.PlotSummary,
                posterUrl = movie.PosterUrl,
                originalLanguage = movie.OriginalLanguage,
                mediaType = movie.MediaType,
                tagline = movie.Tagline,
                status = movie.Status,
                createdAt = movie.CreatedAt,
                updatedAt = movie.UpdatedAt,

                movieGenres = movie.MovieGenres.Select(g => new
                {
                    tmdbGenreId = g.TmdbGenreId,
                    genreName = g.TmdbGenre?.GenreName
                }),

                movieStreamings = movie.MovieStreamings.Select(s => new
                {
                    tmdbProviderId = s.TmdbProviderId,
                    providerName = s.TmdbProvider?.ProviderName,
                    offerType = s.OfferType
                }),

                movieCasts = movie.MovieCasts
                    .OrderBy(c => c.CastOrder)
                    .Select(c => new
                    {
                        tmdbPersonId = c.TmdbPersonId,
                        name = c.TmdbPerson?.PersonName,
                        character = c.CharacterName,
                        order = c.CastOrder ?? 0
                    }),

                movieCrews = movie.MovieCrews
                    .Select(c => new
                    {
                        tmdbPersonId = c.TmdbPersonId,
                        name = c.TmdbPerson?.PersonName,
                        department = c.Department,
                        job = c.Job
                    }),

                movieKeywords = movie.MovieKeywords.Select(k => new
                {
                    tmdbKeywordId = k.TmdbKeywordId,
                    keyword = k.TmdbKeyword?.KeywordName
                }),

                movieWarnings = tierRows
            };

            return Ok(result);
        }

        private Task<Movie?> LoadMovieGraphAsync(string imdbId)
        {
            return _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings)
                    .ThenInclude(ms => ms.TmdbProvider)
                .Include(m => m.MovieCasts)
                    .ThenInclude(mc => mc.TmdbPerson)
                .Include(m => m.MovieCrews)
                    .ThenInclude(mcr => mcr.TmdbPerson)
                .Include(m => m.MovieKeywords)
                    .ThenInclude(mk => mk.TmdbKeyword)
                .FirstOrDefaultAsync(m => m.ImdbId == imdbId);
        }

        /// <summary>
        /// Minimal API fallback:
        /// - IMDb -> TMDB id
        /// - TMDB details (basic fields only)
        /// - Upsert ONLY public.movies row
        /// </summary>
        private async Task<bool> TryUpsertMovieFromTmdbByImdbAsync(string imdbId)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            int? tmdbId = await FetchTmdbIdFromImdbAsync(imdbId, apiKey);
            if (tmdbId == null)
                return false;

            var details = await FetchTmdbDetailsAsync(tmdbId.Value, apiKey);
            if (details == null)
                return false;

            var existing = await _context.Movies.FirstOrDefaultAsync(m => m.ImdbId == imdbId);
            var now = DateTime.UtcNow;

            if (existing == null)
            {
                if (string.IsNullOrWhiteSpace(details.Title) || details.ReleaseYear == null)
                    return false;

                _context.Movies.Add(new Movie
                {
                    ImdbId = imdbId,
                    TmdbId = tmdbId.Value,
                    Title = details.Title!,
                    ReleaseYear = details.ReleaseYear.Value,
                    RuntimeMinutes = details.RuntimeMinutes,
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
            }
            else
            {
                existing.TmdbId = tmdbId.Value;

                if (!string.IsNullOrWhiteSpace(details.Title))
                    existing.Title = details.Title!;

                if (details.ReleaseYear != null)
                    existing.ReleaseYear = details.ReleaseYear.Value;

                existing.RuntimeMinutes = details.RuntimeMinutes;
                existing.PlotSummary = details.PlotSummary;
                existing.PosterUrl = details.PosterUrl;
                existing.OriginalLanguage = details.OriginalLanguage;
                existing.Tagline = details.Tagline;
                existing.Status = details.Status;
                existing.MediaType = "movie";
                existing.UpdatedAt = now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------------
        // MPAA RATING ENRICHMENT (TMDB release_dates)
        // -------------------------

        private async Task<bool> TryEnrichMpaaRatingFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var rating = await FetchUsCertificationAsync(tmdbId.Value, apiKey);
            if (string.IsNullOrWhiteSpace(rating))
                return false;

            movie.MpaaRating = rating;
            movie.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string?> FetchUsCertificationAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/release_dates?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var country in resultsEl.EnumerateArray())
            {
                var iso = country.TryGetProperty("iso_3166_1", out var isoEl) ? isoEl.GetString() : null;
                if (!string.Equals(iso, "US", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!country.TryGetProperty("release_dates", out var rdEl) || rdEl.ValueKind != JsonValueKind.Array)
                    continue;

                string? fallback = null;

                foreach (var rd in rdEl.EnumerateArray())
                {
                    var cert = rd.TryGetProperty("certification", out var certEl) ? certEl.GetString() : null;
                    if (string.IsNullOrWhiteSpace(cert))
                        continue;

                    fallback ??= cert;

                    var type = rd.TryGetProperty("type", out var typeEl) && typeEl.TryGetInt32(out var t) ? t : (int?)null;
                    if (type == 3 || type == 2)
                        return cert;
                }

                return fallback;
            }

            return null;
        }

        // -------------------------
        // GENRES ENRICHMENT
        // -------------------------

        private async Task<bool> TryEnrichGenresFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var details = await FetchTmdbDetailsWithGenresAsync(tmdbId.Value, apiKey);
            if (details == null || details.Genres.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();

            await UpsertGenresForMovieAsync(movie.ImdbId, details.Genres, now);
            await _context.SaveChangesAsync();

            await tx.CommitAsync();
            return true;
        }

        private sealed class TmdbGenreRow
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private sealed class TmdbDetailsWithGenres
        {
            public List<TmdbGenreRow> Genres { get; set; } = new();
        }

        private async Task UpsertGenresForMovieAsync(string imdbId, List<TmdbGenreRow> genres, DateTime now)
        {
            foreach (var g in genres)
            {
                if (g == null || g.Id <= 0 || string.IsNullOrWhiteSpace(g.Name))
                    continue;

                var existingGenre = await _context.Genres.FirstOrDefaultAsync(x => x.TmdbGenreId == g.Id);

                if (existingGenre == null)
                {
                    _context.Genres.Add(new Genre
                    {
                        TmdbGenreId = g.Id,
                        GenreName = g.Name!,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (existingGenre.GenreName != g.Name)
                        existingGenre.GenreName = g.Name!;

                    existingGenre.UpdatedAt = now;
                }
            }

            var existingLinks = await _context.MovieGenres
                .Where(mg => mg.ImdbId == imdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieGenres.RemoveRange(existingLinks);

            foreach (var g in genres)
            {
                if (g == null || g.Id <= 0 || string.IsNullOrWhiteSpace(g.Name))
                    continue;

                _context.MovieGenres.Add(new MovieGenre
                {
                    ImdbId = imdbId,
                    TmdbGenreId = g.Id,
                    CreatedAt = now
                });
            }
        }

        private async Task<TmdbDetailsWithGenres?> FetchTmdbDetailsWithGenresAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var genres = new List<TmdbGenreRow>();
            if (root.TryGetProperty("genres", out var genresEl) && genresEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var g in genresEl.EnumerateArray())
                {
                    var id = g.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = g.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;

                    if (id > 0 && !string.IsNullOrWhiteSpace(name))
                        genres.Add(new TmdbGenreRow { Id = id, Name = name });
                }
            }

            return new TmdbDetailsWithGenres { Genres = genres };
        }

        // -------------------------
        // KEYWORDS ENRICHMENT
        // -------------------------

        private async Task<bool> TryEnrichKeywordsFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var keywords = await FetchTmdbKeywordsAsync(tmdbId.Value, apiKey);
            if (keywords == null || keywords.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            movie.UpdatedAt = now;
            await _context.SaveChangesAsync();

            await UpsertKeywordsForMovieAsync(movie.ImdbId, keywords, now);
            await _context.SaveChangesAsync();

            await tx.CommitAsync();
            return true;
        }

        private sealed class TmdbKeywordRow
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private async Task<List<TmdbKeywordRow>?> FetchTmdbKeywordsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/keywords?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("keywords", out var kwEl) || kwEl.ValueKind != JsonValueKind.Array)
                return new List<TmdbKeywordRow>();

            var list = new List<TmdbKeywordRow>();
            foreach (var k in kwEl.EnumerateArray())
            {
                var id = k.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                var name = k.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;

                if (id > 0 && !string.IsNullOrWhiteSpace(name))
                    list.Add(new TmdbKeywordRow { Id = id, Name = name });
            }

            return list;
        }

        private async Task UpsertKeywordsForMovieAsync(string imdbId, List<TmdbKeywordRow> keywords, DateTime now)
        {
            foreach (var k in keywords)
            {
                if (k == null || k.Id <= 0 || string.IsNullOrWhiteSpace(k.Name))
                    continue;

                var existingKeyword = await _context.Keywords.FirstOrDefaultAsync(x => x.TmdbKeywordId == k.Id);

                if (existingKeyword == null)
                {
                    _context.Keywords.Add(new Keyword
                    {
                        TmdbKeywordId = k.Id,
                        KeywordName = k.Name!,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (existingKeyword.KeywordName != k.Name)
                        existingKeyword.KeywordName = k.Name!;

                    existingKeyword.UpdatedAt = now;
                }
            }

            var existingLinks = await _context.MovieKeywords
                .Where(mk => mk.ImdbId == imdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieKeywords.RemoveRange(existingLinks);

            foreach (var k in keywords)
            {
                if (k == null || k.Id <= 0 || string.IsNullOrWhiteSpace(k.Name))
                    continue;

                _context.MovieKeywords.Add(new MovieKeyword
                {
                    ImdbId = imdbId,
                    TmdbKeywordId = k.Id,
                    CreatedAt = now
                });
            }
        }

        // -------------------------
        // CAST + CREW ENRICHMENT (TMDB credits endpoint)
        // -------------------------

        private async Task<bool> TryEnrichCastFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var credits = await FetchTmdbCreditsAsync(tmdbId.Value, apiKey);
            if (credits == null || credits.Cast.Count == 0)
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
                if (c == null)
                    continue;
                if (string.IsNullOrWhiteSpace(c.CreditId))
                    continue;
                if (c.PersonId <= 0)
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

        private async Task<bool> TryEnrichCrewFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var credits = await FetchTmdbCreditsAsync(tmdbId.Value, apiKey);
            if (credits == null || credits.Crew.Count == 0)
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

            // Crew gets massive; limit to top N with a simple priority order.
            // (No guessing about job buckets, just a consistent cutoff.)
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
                .Where(c => c != null
                            && c.PersonId > 0
                            && !string.IsNullOrWhiteSpace(c.CreditId)
                            && !string.IsNullOrWhiteSpace(c.Name))
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
            public string? KnownForDepartment { get; set; }
            public string? ProfilePath { get; set; }
        }

        private sealed class TmdbCrewRow
        {
            public int PersonId { get; set; }
            public string? Name { get; set; }
            public string? Department { get; set; }
            public string? Job { get; set; }
            public string? CreditId { get; set; }
            public string? KnownForDepartment { get; set; }
            public string? ProfilePath { get; set; }
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
                    var personId = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = c.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                    var character = c.TryGetProperty("character", out var chEl) ? chEl.GetString() : null;
                    var order = c.TryGetProperty("order", out var orEl) && orEl.TryGetInt32(out var orVal) ? orVal : (int?)null;
                    var creditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null;

                    var knownFor = c.TryGetProperty("known_for_department", out var kfdEl) ? kfdEl.GetString() : null;
                    var profilePath = c.TryGetProperty("profile_path", out var ppEl) ? ppEl.GetString() : null;

                    if (personId > 0 && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(creditId))
                    {
                        credits.Cast.Add(new TmdbCastRow
                        {
                            PersonId = personId,
                            Name = name,
                            Character = character,
                            Order = order,
                            CreditId = creditId,
                            KnownForDepartment = knownFor,
                            ProfilePath = profilePath
                        });
                    }
                }
            }

            if (root.TryGetProperty("crew", out var crewEl) && crewEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var c in crewEl.EnumerateArray())
                {
                    var personId = c.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal) ? idVal : 0;
                    var name = c.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;
                    var department = c.TryGetProperty("department", out var depEl) ? depEl.GetString() : null;
                    var job = c.TryGetProperty("job", out var jobEl) ? jobEl.GetString() : null;
                    var creditId = c.TryGetProperty("credit_id", out var crEl) ? crEl.GetString() : null;

                    var knownFor = c.TryGetProperty("known_for_department", out var kfdEl) ? kfdEl.GetString() : null;
                    var profilePath = c.TryGetProperty("profile_path", out var ppEl) ? ppEl.GetString() : null;

                    if (personId > 0 && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(creditId))
                    {
                        credits.Crew.Add(new TmdbCrewRow
                        {
                            PersonId = personId,
                            Name = name,
                            Department = department,
                            Job = job,
                            CreditId = creditId,
                            KnownForDepartment = knownFor,
                            ProfilePath = profilePath
                        });
                    }
                }
            }

            return credits;
        }

        private async Task UpsertPeopleFromCreditsAsync(TmdbCredits credits, DateTime now)
        {
            var map = new Dictionary<int, (string name, string? knownFor, string? profilePath)>();

            foreach (var c in credits.Cast)
            {
                if (c.PersonId > 0 && !string.IsNullOrWhiteSpace(c.Name))
                    map[c.PersonId] = (c.Name!, c.KnownForDepartment, c.ProfilePath);
            }

            foreach (var c in credits.Crew)
            {
                if (c.PersonId > 0 && !string.IsNullOrWhiteSpace(c.Name))
                    map[c.PersonId] = (c.Name!, c.KnownForDepartment, c.ProfilePath);
            }

            foreach (var kvp in map)
            {
                var tmdbPersonId = kvp.Key;
                var name = kvp.Value.name;
                var knownFor = kvp.Value.knownFor;

                var profileUrl = string.IsNullOrWhiteSpace(kvp.Value.profilePath)
                    ? null
                    : $"https://image.tmdb.org/t/p/w500{kvp.Value.profilePath}";

                var existingPerson = await _context.People.FirstOrDefaultAsync(p => p.TmdbPersonId == tmdbPersonId);

                if (existingPerson == null)
                {
                    _context.People.Add(new Person
                    {
                        TmdbPersonId = tmdbPersonId,
                        PersonName = name,
                        KnownForDepartment = knownFor,
                        ProfileUrl = profileUrl,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (existingPerson.PersonName != name)
                        existingPerson.PersonName = name;

                    existingPerson.KnownForDepartment = knownFor;
                    existingPerson.ProfileUrl = profileUrl;
                    existingPerson.UpdatedAt = now;
                }
            }
        }

        // -------------------------
        // STREAMING ENRICHMENT (TMDB watch/providers)
        // -------------------------

        private async Task<bool> TryEnrichStreamingFromTmdbAsync(Movie movie)
        {
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var tmdbId = await EnsureTmdbIdAsync(movie, apiKey);
            if (tmdbId == null)
                return false;

            var providers = await FetchTmdbWatchProvidersUsAsync(tmdbId.Value, apiKey);
            if (providers == null || providers.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            var existingLinks = await _context.MovieStreamings
                .Where(ms => ms.ImdbId == movie.ImdbId)
                .ToListAsync();

            if (existingLinks.Count > 0)
                _context.MovieStreamings.RemoveRange(existingLinks);

            foreach (var p in providers)
            {
                if (p.ProviderId <= 0 || string.IsNullOrWhiteSpace(p.ProviderName) || string.IsNullOrWhiteSpace(p.OfferType))
                    continue;

                var existingProvider = await _context.StreamingProviders
                    .FirstOrDefaultAsync(sp => sp.TmdbProviderId == p.ProviderId);

                if (existingProvider == null)
                {
                    _context.StreamingProviders.Add(new StreamingProvider
                    {
                        TmdbProviderId = p.ProviderId,
                        ProviderName = p.ProviderName!,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }
                else
                {
                    if (existingProvider.ProviderName != p.ProviderName)
                        existingProvider.ProviderName = p.ProviderName!;

                    existingProvider.UpdatedAt = now;
                }

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

        private sealed class TmdbWatchProviderRow
        {
            public int ProviderId { get; set; }
            public string? ProviderName { get; set; }

            // Values we store in DB:
            // subscription, free, free_with_ads, rent, buy
            public string? OfferType { get; set; }
        }

        private async Task<List<TmdbWatchProviderRow>?> FetchTmdbWatchProvidersUsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}/watch/providers?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("results", out var resultsEl) || resultsEl.ValueKind != JsonValueKind.Object)
                return new List<TmdbWatchProviderRow>();

            if (!resultsEl.TryGetProperty("US", out var usEl) || usEl.ValueKind != JsonValueKind.Object)
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
                if (!usEl.TryGetProperty(bucket, out var arr) || arr.ValueKind != JsonValueKind.Array)
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
        // WARNINGS ENRICHMENT (DTDD API v1.1) - YES ONLY
        // -------------------------

        private async Task<bool> TryEnrichWarningsFromDtddAsync(string imdbId)
        {
            var apiKey = Environment.GetEnvironmentVariable("DTDD_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var dtddMediaId = await FetchDtddMediaIdByImdbAsync(imdbId, apiKey);
            if (dtddMediaId == null)
                return false;

            // DTDD returns all topics; we will keep YES only
            var stats = await FetchDtddTopicStatsAsync(dtddMediaId.Value, apiKey);
            if (stats.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            await using var tx = await _context.Database.BeginTransactionAsync();

            // This function is only called when the movie has no warnings yet,
            // but keeping the "remove existing" makes it safe to rerun for the same imdbId.
            var existing = await _context.MovieWarnings
                .Where(mw => mw.ImdbId == imdbId)
                .ToListAsync();

            if (existing.Count > 0)
                _context.MovieWarnings.RemoveRange(existing);

            // Load known topic ids once (fast), then check membership in-memory.
            // NOTE: ToHashSetAsync may not exist in your EF Core version, so we do ToListAsync + ToHashSet().
            var knownTopicIds = (await _context.Warnings
                    .AsNoTracking()
                    .Select(w => w.DtddTopicId)
                    .ToListAsync())
                .ToHashSet();

            foreach (var s in stats)
            {
                // Store YES only
                if (!string.Equals(s.Answer, "yes", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Only store topics that exist in your warnings table (so tier joins work)
                if (!knownTopicIds.Contains(s.TopicId))
                    continue;

                _context.MovieWarnings.Add(new MovieWarning
                {
                    ImdbId = imdbId,
                    DtddTopicId = s.TopicId,
                    Answer = "yes",
                    IsSpoiler = s.IsSpoiler,
                    WarningComment = s.Comment,
                    CreatedAt = now
                });
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        private sealed class DtddTopicStatRow
        {
            public int TopicId { get; set; }
            public string Answer { get; set; } = "unknown"; // yes/no/unknown
            public bool? IsSpoiler { get; set; }
            public string? Comment { get; set; }
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

                // DTDD: isYes is typically 1 (yes), 0 (no), null/absent (unknown)
                var isYes = s.TryGetProperty("isYes", out var yesEl) && yesEl.TryGetInt32(out var yesVal) ? yesVal : (int?)null;

                var comment = s.TryGetProperty("comment", out var cEl) ? cEl.GetString() : null;

                bool? isSpoiler = null;
                if (s.TryGetProperty("topic", out var topicEl) && topicEl.ValueKind == JsonValueKind.Object)
                {
                    if (topicEl.TryGetProperty("isSpoiler", out var spEl) && (spEl.ValueKind == JsonValueKind.True || spEl.ValueKind == JsonValueKind.False))
                        isSpoiler = spEl.GetBoolean();
                }

                var answer = "unknown";
                if (isYes == 1) answer = "yes";
                if (isYes == 0) answer = "no";

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
        // SHARED HELPERS
        // -------------------------

        private async Task<int?> EnsureTmdbIdAsync(Movie movie, string apiKey)
        {
            var tmdbId = movie.TmdbId;
            if (tmdbId != null && tmdbId > 0)
                return tmdbId;

            var found = await FetchTmdbIdFromImdbAsync(movie.ImdbId, apiKey);
            if (found == null)
                return null;

            movie.TmdbId = found.Value;
            return found.Value;
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

        private async Task<int?> FetchTmdbIdFromImdbAsync(string imdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url =
                $"https://api.themoviedb.org/3/find/{Uri.EscapeDataString(imdbId)}" +
                $"?api_key={apiKey}&external_source=imdb_id";

            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("movie_results", out var moviesEl) || moviesEl.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var item in moviesEl.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idEl) && idEl.TryGetInt32(out var idVal))
                    return idVal;
            }

            return null;
        }

        private async Task<TmdbDetailsBasic?> FetchTmdbDetailsAsync(int tmdbId, string apiKey)
        {
            using var http = new HttpClient();

            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            string? title = root.TryGetProperty("title", out var titleEl) ? titleEl.GetString() : null;
            string? releaseDate = root.TryGetProperty("release_date", out var rdEl) ? rdEl.GetString() : null;
            int? runtime = root.TryGetProperty("runtime", out var rtEl) && rtEl.TryGetInt32(out var rtVal) ? rtVal : null;
            string? overview = root.TryGetProperty("overview", out var ovEl) ? ovEl.GetString() : null;
            string? posterPath = root.TryGetProperty("poster_path", out var ppEl) ? ppEl.GetString() : null;
            string? originalLanguage = root.TryGetProperty("original_language", out var olEl) ? olEl.GetString() : null;
            string? tagline = root.TryGetProperty("tagline", out var tgEl) ? tgEl.GetString() : null;
            string? status = root.TryGetProperty("status", out var stEl) ? stEl.GetString() : null;

            int? year = null;
            if (!string.IsNullOrWhiteSpace(releaseDate) && releaseDate.Length >= 4)
            {
                if (int.TryParse(releaseDate.Substring(0, 4), out var y))
                    year = y;
            }

            string? posterUrl = null;
            if (!string.IsNullOrWhiteSpace(posterPath))
                posterUrl = $"https://image.tmdb.org/t/p/w500{posterPath}";

            return new TmdbDetailsBasic
            {
                Title = title,
                ReleaseYear = year,
                RuntimeMinutes = runtime,
                PlotSummary = overview,
                PosterUrl = posterUrl,
                OriginalLanguage = originalLanguage,
                Tagline = tagline,
                Status = status
            };
        }

        // -------------------------
        // WARNINGS (DB ONLY, tiered output)
        // -------------------------

        private async Task<List<object>> LoadWarningTierRowsAsync(string imdbId)
        {
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();

            var openedHere = false;
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
                openedHere = true;
            }

            try
            {
                const string sql = @"
                    SELECT
                        wc.category_name AS ""umbrellaCategory"",
                        ws.subcategory_name AS ""subcategory"",
                        w.dtdd_topic_id AS ""dtddTopicId"",
                        w.topic_name AS ""warningTopic"",
                        mw.answer AS ""answer"",
                        mw.is_spoiler AS ""isSpoiler"",
                        mw.warning_comment AS ""warningComment""
                    FROM public.movie_warnings mw
                    JOIN public.warnings w
                      ON w.dtdd_topic_id = mw.dtdd_topic_id
                    LEFT JOIN public.warning_category_topics wct
                      ON wct.dtdd_topic_id = w.dtdd_topic_id
                    LEFT JOIN public.warning_categories wc
                      ON wc.category_id = wct.category_id
                    LEFT JOIN public.warning_subcategory_topics wst
                      ON wst.dtdd_topic_id = w.dtdd_topic_id
                    LEFT JOIN public.warning_subcategories ws
                      ON ws.subcategory_id = wst.subcategory_id
                    WHERE mw.imdb_id = @imdbId
                     AND mw.answer = 'yes'
                    ORDER BY wc.category_name NULLS LAST,
                             ws.subcategory_name NULLS LAST,
                             w.topic_name;
                ";

                await using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@imdbId", imdbId);

                await using var reader = await cmd.ExecuteReaderAsync();

                var rows = new List<object>();
                while (await reader.ReadAsync())
                {
                    rows.Add(new
                    {
                        umbrellaCategory = reader["umbrellaCategory"] as string,
                        subcategory = reader["subcategory"] as string,
                        dtddTopicId = (int)reader["dtddTopicId"],
                        warningTopic = reader["warningTopic"] as string,
                        answer = reader["answer"] as string,
                        isSpoiler = reader["isSpoiler"] as bool?,
                        warningComment = reader["warningComment"] as string
                    });
                }

                return rows;
            }
            finally
            {
                if (openedHere)
                    await conn.CloseAsync();
            }
        }
    }
}