using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindMyFlickWebsite.Server.DataModels;

//create me a controller that can get movies by the first letter in their title (so all movies that start with an
//A) and another endpoint in the controller that can get all movies associated with a genre name

// Streaming availability and Does the Dog Die warning filters added with Claude (April 2026)
// Article-stripping logic (A, An, The) for letter filtering added with Claude (April 2026)
// 0-9 (numbers/symbols) filter endpoint added with Claude (April 2026)
// Random movie endpoint added with Claude (April 2026)

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/movies/getby")]
    public class GetMoviesByControllerr : ControllerBase
    {
        private readonly IDbContextFactory<FindmyflickContext> _dbFactory;

        public GetMoviesByControllerr(IDbContextFactory<FindmyflickContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Simple DTO returned by these endpoints
        public sealed class MovieSummary
        {
            public string ImdbId { get; set; } = string.Empty;
            public int? TmdbId { get; set; }
            public string Title { get; set; } = string.Empty;
            public int? ReleaseYear { get; set; }
            public string? PosterUrl { get; set; }
        }

        // Strips leading articles (A, An, The) from a title for sorting purposes.
        // "The Dark Knight" -> "Dark Knight", "A Bug's Life" -> "Bug's Life"
        private static string StripArticle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return title;
            var t = title.Trim();
            if (t.StartsWith("the ", StringComparison.OrdinalIgnoreCase)) return t.Substring(4).TrimStart();
            if (t.StartsWith("an ", StringComparison.OrdinalIgnoreCase))  return t.Substring(3).TrimStart();
            if (t.StartsWith("a ", StringComparison.OrdinalIgnoreCase))   return t.Substring(2).TrimStart();
            return t;
        }

        // GET api/movies/getby/random?count=12
        // Returns a random selection of eligible movies for the home page.
        // Only returns movies that have US subscription/free streaming AND Does the Dog Die warning data.
        // Uses PostgreSQL's random ordering to ensure a different set each time.
        [HttpGet("random")]
        public async Task<IActionResult> GetRandom(int count = 12)
        {
            count = Math.Max(1, Math.Min(count, 50)); // guard: between 1 and 50

            await using var ctx = _dbFactory.CreateDbContext();

            var results = await ctx.Movies
                .AsNoTracking()
                // Only movies with at least one subscription or free streaming option (not rent/buy)
                .Where(m => m.MovieStreamings.Any(ms =>
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy")))
                // Only movies with Does the Dog Die warning data
                .Where(m => m.MovieWarnings.Any(w => w.Answer != null))
                // Only movies with a poster
                .Where(m => m.PosterUrl != null)
                .OrderBy(m => EF.Functions.Random())
                .Take(count)
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET api/movies/getby/starts-with/{letter}?limit=500
        // Returns movies whose Title (after stripping leading articles) starts with the specified letter.
        // "A Bug's Life" appears under B, "The Dark Knight" under D, "An American Werewolf" under A.
        // Only returns movies that have US subscription/free streaming AND Does the Dog Die warning data.
        [HttpGet("starts-with/{letter}")]
        public async Task<IActionResult> GetByFirstLetter(string letter, int limit = 500)
        {
            if (string.IsNullOrWhiteSpace(letter))
                return BadRequest("letter path parameter is required.");

            var first = letter.Trim()[0].ToString().ToUpperInvariant();

            await using var ctx = _dbFactory.CreateDbContext();

            // Fetch all eligible movies first, then filter by article-stripped letter in memory.
            // This is necessary because EF Core cannot translate StripArticle() to SQL.
            var allMovies = await ctx.Movies
                .AsNoTracking()
                // Only movies with at least one subscription or free streaming option (not rent/buy)
                .Where(m => m.MovieStreamings.Any(ms =>
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy")))
                // Only movies with Does the Dog Die warning data
                .Where(m => m.MovieWarnings.Any(w => w.Answer != null))
                // Pre-filter in SQL: titles starting with the letter directly,
                // OR starting with "A ", "An ", or "The " (articles that may be stripped)
                .Where(m =>
                    EF.Functions.ILike(m.Title, $"{first}%") ||
                    EF.Functions.ILike(m.Title, $"A {first}%") ||
                    EF.Functions.ILike(m.Title, $"A {first.ToLower()}%") ||
                    EF.Functions.ILike(m.Title, $"An {first}%") ||
                    EF.Functions.ILike(m.Title, $"An {first.ToLower()}%") ||
                    EF.Functions.ILike(m.Title, $"The {first}%") ||
                    EF.Functions.ILike(m.Title, $"The {first.ToLower()}%"))
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .ToListAsync();

            // Apply article stripping in memory and filter to only the correct letter
            var results = allMovies
                .Where(m => StripArticle(m.Title).StartsWith(first, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => StripArticle(m.Title))
                .Take(Math.Max(1, Math.Min(limit, 1000)))
                .ToList();

            return Ok(results);
        }

        // GET api/movies/getby/non-alpha?limit=500
        // Returns movies whose Title (after stripping leading articles) starts with a number or symbol.
        // Covers titles like "1408", "13 Hours", "(500) Days of Summer", "¡Three Amigos!"
        // Only returns movies that have US subscription/free streaming AND Does the Dog Die warning data.
        [HttpGet("non-alpha")]
        public async Task<IActionResult> GetByNonAlpha(int limit = 500)
        {
            await using var ctx = _dbFactory.CreateDbContext();

            var allMovies = await ctx.Movies
                .AsNoTracking()
                // Only movies with at least one subscription or free streaming option (not rent/buy)
                .Where(m => m.MovieStreamings.Any(ms =>
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy")))
                // Only movies with Does the Dog Die warning data
                .Where(m => m.MovieWarnings.Any(w => w.Answer != null))
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .ToListAsync();

            // Filter in memory: after stripping articles, title must start with a non-letter character
            var results = allMovies
                .Where(m => {
                    var stripped = StripArticle(m.Title ?? "");
                    return stripped.Length > 0 && !char.IsLetter(stripped[0]);
                })
                .OrderBy(m => StripArticle(m.Title))
                .Take(Math.Max(1, Math.Min(limit, 1000)))
                .ToList();

            return Ok(results);
        }

        // GET api/movies/getby/genre/{genreName}?limit=200
        // Returns movies associated with the given genre name (case-insensitive).
        // Sorted by title with leading articles (A, An, The) stripped for ordering.
        // Only returns movies that have US subscription/free streaming AND Does the Dog Die warning data.
        [HttpGet("genre/{genreName}")]
        public async Task<IActionResult> GetByGenre(string genreName, int limit = 200)
        {
            if (string.IsNullOrWhiteSpace(genreName))
                return BadRequest("genreName path parameter is required.");

            var normalized = genreName.Trim();

            await using var ctx = _dbFactory.CreateDbContext();

            var results = await ctx.MovieGenres
                .AsNoTracking()
                .Where(mg => EF.Functions.ILike(mg.TmdbGenre.GenreName, normalized))
                .Select(mg => mg.Imdb)
                .Where(m => m != null)
                // Only movies with at least one subscription or free streaming option (not rent/buy)
                .Where(m => m.MovieStreamings.Any(ms =>
                    !EF.Functions.ILike(ms.OfferType, "rent") &&
                    !EF.Functions.ILike(ms.OfferType, "buy")))
                // Only movies with Does the Dog Die warning data
                .Where(m => m.MovieWarnings.Any(w => w.Answer != null))
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .Distinct()
                .Take(Math.Max(1, Math.Min(limit, 2000)))
                .ToListAsync();

            // Sort by article-stripped title in memory
            results = results
                .OrderBy(m => StripArticle(m.Title))
                .ToList();

            return Ok(results);
        }
    }
}