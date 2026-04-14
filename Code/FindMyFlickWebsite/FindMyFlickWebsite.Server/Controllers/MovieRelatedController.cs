using FindMyFlickWebsite.Server.Models;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//this is used to handle related data for movies, such as cast, crew, genres, plot tags, warnings, streaming providers, and collections. Each endpoint is designed to return relevant information in a lightweight shape, often projecting to anonymous types for efficiency.
//The controller uses a DbContextFactory to create contexts for each request, ensuring thread safety and efficient resource management. It also includes error handling for invalid or non-existent movie IDs, returning appropriate HTTP status codes and messages.
namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/movies/{id}")]
    public class MovieRelatedController : ControllerBase
    {
        private readonly IDbContextFactory<FindmyflickContext> _dbFactory;

        public MovieRelatedController(IDbContextFactory<FindmyflickContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private static string NormalizeImdb(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return id ?? string.Empty;
            return id.StartsWith("tt", StringComparison.OrdinalIgnoreCase) ? id : "tt" + id;
        }

        private async Task<bool> MovieExistsAsync(string imdbId)
        {
            await using var ctx = _dbFactory.CreateDbContext();
            return await ctx.Movies.AsNoTracking().AnyAsync(m => m.ImdbId == imdbId);
        }

        // GET /api/movies/{id}/plot-tags
        // Returns Plot tags associated with the movie (from movie_plot_tags -> plot_tags).
        [HttpGet("plot-tags")]
        public async Task<ActionResult<IEnumerable<Models.TagsView.PlotTag>>> GetPlotTags(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });

            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });

            await using var ctx = _dbFactory.CreateDbContext();

            var conn = ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            const string sql = @"
SELECT pt.plot_tag_id, pt.tag_text, mpt.created_at, mpt.created_by_user_id, mpt.status
FROM public.movie_plot_tags mpt
JOIN public.plot_tags pt ON pt.plot_tag_id = mpt.plot_tag_id
WHERE mpt.imdb_id = @imdbId
  AND mpt.status = 'approved'
ORDER BY pt.tag_text;";

            await using var cmd = new Npgsql.NpgsqlCommand(sql, (Npgsql.NpgsqlConnection)conn);
            cmd.Parameters.AddWithValue("@imdbId", id);

            var list = new List<Models.TagsView.PlotTag>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var plotTag = new Models.TagsView.PlotTag
                {
                    TagType = "plot",
                    TagID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    TagName = reader.IsDBNull(1) ? null : reader.GetString(1)
                };

                list.Add(plotTag);
            }

            await reader.CloseAsync();
            return Ok(list);
        }

        [HttpGet("collections")]
        public async Task<ActionResult<IEnumerable<object>>> GetCollections(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });
            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });
            await using var ctx = _dbFactory.CreateDbContext();
            var rows = await ctx.MovieCollections
                .AsNoTracking()
                .Where(mc => mc.ImdbId == id)
                .Select(mc => new
                {
                    mc.TmdbCollectionId,
                    CollectionName = mc.TmdbCollection != null ? mc.TmdbCollection.CollectionName : null,
                    mc.CreatedAt
                })
                .ToListAsync();
            return Ok(rows);
        }

        //returns cast for the movie
        [HttpGet("cast")]
        public async Task<ActionResult<IEnumerable<object>>> GetCast(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });

            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });
            await using var ctx = _dbFactory.CreateDbContext();
            var cast = await ctx.MovieCasts
                .AsNoTracking()
                .Where(mc => mc.ImdbId == id)
                .Where(mc => !mc.CharacterName.Contains("uncredited")) //if they aint credited idc tbh
                .Select(mc => new
                {
                    mc.TmdbCreditId,
                    mc.TmdbPersonId,
                    PersonName = mc.TmdbPerson != null ? mc.TmdbPerson.PersonName : null,
                    mc.CharacterName,
                    mc.CastOrder,
                    mc.CreatedAt
                })
                .OrderBy(c => c.CastOrder ?? int.MaxValue)
                .ToListAsync();
            var distinctCast = cast
                .GroupBy(c => new { c.TmdbPersonId, c.PersonName })
                .Select(g => new
                {
                    g.Key.TmdbPersonId,
                    g.Key.PersonName,
                    CharacterNames = g.Select(x => x.CharacterName).Where(cn => !string.IsNullOrWhiteSpace(cn)).Distinct().ToList(),
                    CastOrders = g.Select(x => x.CastOrder).Where(co => co.HasValue).Select(co => co.Value).Distinct().ToList()
                })
                .ToList();
            return Ok(distinctCast);
        }

        //returns crew for the movie
        [HttpGet("crew")]
        public async Task<ActionResult<IEnumerable<object>>> GetCrew(string id) 
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });
            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });
            await using var ctx = _dbFactory.CreateDbContext();
            var crew = await ctx.MovieCrews
                .AsNoTracking()
                .Where(mc => mc.ImdbId == id)
                .Select(mc => new
                {
                    mc.TmdbCreditId,
                    mc.TmdbPersonId,
                    PersonName = mc.TmdbPerson != null ? mc.TmdbPerson.PersonName : null,
                    mc.Department,
                    mc.Job,
                    mc.CreatedAt
                })
                .ToListAsync();
            
            var distinctCrew = crew
                .GroupBy(c => new { c.TmdbPersonId, c.PersonName })
                .Select(g => new
                {
                    g.Key.TmdbPersonId,
                    g.Key.PersonName,
                    Departments = g.Select(x => x.Department).Where(d => !string.IsNullOrWhiteSpace(d)).Distinct().ToList(),
                    Jobs = g.Select(x => x.Job).Where(j => !string.IsNullOrWhiteSpace(j)).Distinct().ToList()
                })
                .ToList();
            return Ok(distinctCrew);
        }

        // GET /api/movies/{id}/warnings
        // Returns warnings projected to a lightweight shape
        [HttpGet("warnings")]
        public async Task<ActionResult<IEnumerable<object>>> GetWarnings(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });

            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });

            await using var ctx = _dbFactory.CreateDbContext();

            var rows = await ctx.MovieWarnings
                .AsNoTracking()
                .Where(mw => mw.ImdbId == id)
                .Where(mw =>mw.Answer == "yes")
                .Select(mw => new
                {
                    mw.DtddTopicId,
                    TopicName = mw.DtddTopic != null ? mw.DtddTopic.TopicName : null,
                    mw.Answer,
                    mw.IsSpoiler,
                    Comment = mw.WarningComment,
                    mw.Source,
                    mw.UpdatedAt
                })
                .ToListAsync();

            return Ok(rows);
        }

        // GET /api/movies/{id}/streaming-providers
        // Returns providers grouped with offer types (lightweight shape)
        [HttpGet("streaming-providers")]
        public async Task<ActionResult<IEnumerable<object>>> GetStreamingProviders(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });

            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });

            await using var ctx = _dbFactory.CreateDbContext();

            var rows = await ctx.MovieStreamings
                .AsNoTracking()
                .Where(ms => ms.ImdbId == id)
                .Select(ms => new
                {
                    ms.TmdbProviderId,
                    ProviderName = ms.TmdbProvider != null ? ms.TmdbProvider.ProviderName : null,
                    ms.OfferType
                })
                .ToListAsync();

            var grouped = rows
                .Where(r => r.TmdbProviderId > 0)
                .GroupBy(r => new { r.TmdbProviderId, r.ProviderName })
                .Select(g => new
                {
                    ProviderId = g.Key.TmdbProviderId,
                    ProviderName = g.Key.ProviderName,
                    OfferTypes = g.Select(x => x.OfferType).Where(o => !string.IsNullOrWhiteSpace(o)).Distinct().ToList()
                })
                .ToList();

            return Ok(grouped);
        }

        // GET /api/movies/{id}/genres
        // Returns genres projected to a lightweight shape
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<object>>> GetGenres(string id)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id)) return BadRequest(new { message = "id cannot be empty." });

            if (!await MovieExistsAsync(id))
                return NotFound(new { message = $"Movie with ID '{id}' not found." });

            await using var ctx = _dbFactory.CreateDbContext();

            var rows = await ctx.MovieGenres
                .AsNoTracking()
                .Where(mg => mg.ImdbId == id)
                .Select(mg => new
                {
                    mg.TmdbGenreId,
                    GenreName = mg.TmdbGenre != null ? mg.TmdbGenre.GenreName : null
                })
                .Distinct()
                .ToListAsync();

            return Ok(rows);
        }
    }
}