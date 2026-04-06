using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindMyFlickWebsite.Server.DataModels;

//create me a controller that can get movies by the first letter in their title (so all movies that start with an
//A) and another endpoint in the controller that can get all movies associated with a genre name
namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/movies/getby")]
    public class GetMoviesBy : ControllerBase
    {
        private readonly IDbContextFactory<FindmyflickContext> _dbFactory;

        public GetMoviesBy(IDbContextFactory<FindmyflickContext> dbFactory)
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

        // GET api/movies/starts-with/{letter}?limit=100
        // Returns movies whose Title starts with the specified letter (case-insensitive).
        [HttpGet("starts-with/{letter}")]
        public async Task<IActionResult> GetByFirstLetter(string letter, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(letter))
                return BadRequest("letter path parameter is required.");

            var first = letter.Trim()[0].ToString();

            await using var ctx = _dbFactory.CreateDbContext();

            // Use ILike for case-insensitive match on PostgreSQL; falls back to ToLower comparison otherwise.
            var query = ctx.Movies
                .AsNoTracking()
                .Where(m =>
                    EF.Functions.ILike(m.Title, $"{first}%")
                )
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .Distinct()
                .OrderBy(m => m.Title)
                .Take(Math.Max(1, Math.Min(limit, 1000))); // guard limit

            var results = await query.ToListAsync();
            return Ok(results);
        }

        // GET api/movies/genre/{genreName}?limit=200
        // Returns movies associated with the given genre name (case-insensitive)
        [HttpGet("genre/{genreName}")]
        public async Task<IActionResult> GetByGenre(string genreName, int limit = 200)
        {
            if (string.IsNullOrWhiteSpace(genreName))
                return BadRequest("genreName path parameter is required.");

            var normalized = genreName.Trim();

            await using var ctx = _dbFactory.CreateDbContext();

            // Join via MovieGenres -> Genre -> Movie
            // Select the Movie navigation, filter nulls, then group by ImdbId to remove duplicates.
            var query = ctx.MovieGenres
                .AsNoTracking()
                .Where(mg => EF.Functions.ILike(mg.TmdbGenre.GenreName, normalized))
                .Select(mg => mg.Imdb)
                .Where(m => m != null)
                .OrderBy(m => m.Title)
                .Select(m => new MovieSummary
                {
                    ImdbId = EF.Property<string>(m, "ImdbId"),
                    TmdbId = m.TmdbId,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    PosterUrl = m.PosterUrl
                })
                .Distinct()
                .Take(Math.Max(1, Math.Min(limit, 2000)));

            var results = await query.ToListAsync();
            return Ok(results);
        }
    }
}