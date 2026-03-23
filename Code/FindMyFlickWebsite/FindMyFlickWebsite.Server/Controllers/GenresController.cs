using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindMyFlickWebsite.Server.DataModels;

namespace FindMyFlickWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        public GenresController(FindmyflickContext context)
        {
            _context = context;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.GenreName)
                .Select(g => new
                {
                    tmdbGenreId = g.TmdbGenreId,
                    name = g.GenreName
                })
                .ToListAsync();

            return Ok(genres);
        }

        // GET: api/genres/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _context.Genres
                .AsNoTracking()
                .Where(g => g.TmdbGenreId == id)
                .Select(g => new
                {
                    tmdbGenreId = g.TmdbGenreId,
                    name = g.GenreName
                })
                .FirstOrDefaultAsync();

            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        // Optional: search by partial name -> api/genres/search?q=drama
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Query parameter 'q' is required.");

            var matches = await _context.Genres
                .AsNoTracking()
                .Where(g => EF.Functions.ILike(g.GenreName, $"%{q}%"))
                .OrderBy(g => g.GenreName)
                .Select(g => new
                {
                    tmdbGenreId = g.TmdbGenreId,
                    name = g.GenreName
                })
                .ToListAsync();

            return Ok(matches);
        }
    }
}