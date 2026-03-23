using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FindMyFlickWebsite.Server.Controllers
{
    public class PlotTagsController : Controller
    {
        private readonly IDbContextFactory<FindmyflickContext> _dbFactory;

        public PlotTagsController(IDbContextFactory<FindmyflickContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [HttpGet("api/movies/plot-tags/getall")]
        public async Task<IActionResult> GetAllPlotTags()
        {
            await using var ctx = _dbFactory.CreateDbContext();
            var plotTags = await ctx.PlotTags
                .Select(pt => new { pt.PlotTagId, pt.TagText })
                .ToListAsync();
            return Ok(plotTags);
        }

        [HttpGet("api/movies/plot-tags/getbyid/{id}")]
        public async Task<IActionResult> GetPlotTagById(int id)
        {
            await using var ctx = _dbFactory.CreateDbContext();
            var plotTag = await ctx.PlotTags
                .Where(pt => pt.PlotTagId == id)
                .Select(pt => new { pt.PlotTagId, pt.TagText })
                .FirstOrDefaultAsync();
            if (plotTag == null)
                return NotFound(new { message = $"Plot tag with id {id} not found." });
            return Ok(plotTag);
        }

        [HttpGet("api/movies/plot-tags/getbyname/{name}")]
        public async Task<IActionResult> GetPlotTagByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Name cannot be empty." });
            var normalized = name.Trim().ToLowerInvariant();
            await using var ctx = _dbFactory.CreateDbContext();
            var plotTag = await ctx.PlotTags
                .Where(pt => pt.TagTextNorm == normalized)
                .Select(pt => new { pt.PlotTagId, pt.TagText })
                .FirstOrDefaultAsync();
            if (plotTag == null)
                return NotFound(new { message = $"Plot tag with name '{name}' not found." });
            return Ok(plotTag);
        }
    }
}
