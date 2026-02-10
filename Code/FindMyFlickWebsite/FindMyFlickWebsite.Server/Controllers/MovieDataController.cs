using FindMyFlickWebsite.Server.Models;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovieDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/MovieData
        /// Returns all movies (lightweight). Use query params for simple filtering.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll(
            [FromQuery] string? title = null,
            [FromQuery] int? year = null,
            [FromQuery] int? tmdbId = null,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 100)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 100;

            IQueryable<Movie> q = _context.Movies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(title))
                q = q.Where(m => EF.Functions.ILike(m.Title, $"%{title}%"));

            if (year.HasValue)
                q = q.Where(m => m.ReleaseYear == year.Value);

            if (tmdbId.HasValue)
                q = q.Where(m => m.TmdbId == tmdbId.Value);

            var results = await q
                .OrderBy(m => m.Title)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();

            return Ok(results);
        }

        /// <summary>
        /// GET: api/MovieData/{imdbId}
        /// Returns a single movie with related collections eagerly loaded.
        /// imdbId is the PK (tt...).
        /// </summary>
        [HttpGet("{imdbId}")]
        public async Task<ActionResult<Movie>> GetByImdbId(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
                return BadRequest("imdbId is required.");

            var movie = await _context.Movies
                .AsNoTracking()
                .Include(m => m.MovieGenres)
                    .ThenInclude(g => g.TmdbGenre)
                .Include(m => m.MovieStreamings)
                    .ThenInclude(s => s.TmdbProvider)
                .Include(m => m.MovieCasts)
                    .ThenInclude(c => c.TmdbPerson)
                .Include(m => m.MovieCrews)
                    .ThenInclude(c => c.TmdbPerson)
                .Include(m => m.MovieKeywords)
                    .ThenInclude(k => k.TmdbKeyword)
                .FirstOrDefaultAsync(m => m.ImdbId == imdbId);

            if (movie == null) return NotFound();

            return Ok(movie);
        }

        /// <summary>
        /// GET api/MovieData/by-tmdb/{tmdbId}
        /// Find a movie by TMDB id.
        /// </summary>
        [HttpGet("by-tmdb/{tmdbId:int}")]
        public async Task<ActionResult<Movie>> GetByTmdbId(int tmdbId)
        {
            var movie = await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TmdbId == tmdbId);

            if (movie == null) return NotFound();

            return Ok(movie);
        }

        /// <summary>
        /// POST: api/MovieData
        /// Create a new movie. Body must include ImdbId (PK).
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Movie>> Create([FromBody] Movie movie)
        {
            if (movie == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(movie.ImdbId)) return BadRequest("ImdbId (primary key) is required.");

            var exists = await _context.Movies.AnyAsync(m => m.ImdbId == movie.ImdbId);
            if (exists) return Conflict($"Movie with ImdbId '{movie.ImdbId}' already exists.");

            // Let DB defaults handle created_at/updated_at where applicable
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByImdbId), new { imdbId = movie.ImdbId }, movie);
        }

        /// <summary>
        /// PUT: api/MovieData/{imdbId}
        /// Update an existing movie. Path imdbId must match body.ImdbId.
        /// </summary>
        [HttpPut("{imdbId}")]
        public async Task<IActionResult> Update(string imdbId, [FromBody] Movie movie)
        {
            if (movie == null) return BadRequest();
            if (!string.Equals(imdbId, movie.ImdbId, StringComparison.Ordinal))
                return BadRequest("Path imdbId must match body.ImdbId.");

            var exists = await _context.Movies.AnyAsync(m => m.ImdbId == imdbId);
            if (!exists) return NotFound();

            // Attach and mark modified. Alternatively load current entity and patch fields.
            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(e => e.ImdbId == imdbId))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// DELETE: api/MovieData/{imdbId}
        /// Remove a movie by ImdbId.
        /// </summary>
        [HttpDelete("{imdbId}")]
        public async Task<IActionResult> Delete(string imdbId)
        {
            var movie = await _context.Movies.FindAsync(imdbId);
            if (movie == null) return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}