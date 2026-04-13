using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

//copilot query: create a new controller with plot tag voting logic. it needs to only allow logged in users to vote once per tag,
//and allow them to either vote to agree (1) or to disagree (0) in the movie_plot_tags_vote table and if the count of disagrees
//outwiegh the count of agrees change the status of the tag in the Movie_plot_tags table to unapproved. if a user votes agree for a plot tag
//not currently associated with this movie, add it to the movie_plot_tags table as approved

namespace FindMyFlickWebsite.Server.Controllers
{


    [ApiController]
    [Route("api/movies/{id}/plot-tags")]
    public class MoviePlotTagVotingController : ControllerBase
    {
        private readonly IDbContextFactory<FindmyflickContext> _dbFactory;

        public MoviePlotTagVotingController(IDbContextFactory<FindmyflickContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private static string NormalizeImdb(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return id ?? string.Empty;
            return id.StartsWith("tt", System.StringComparison.OrdinalIgnoreCase) ? id : "tt" + id;
        }

        private string? GetCurrentUserId()
        {
            // Return raw string claim (GUID or other) — your DB stores user ids as string.
            var claim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? User.FindFirst("id")?.Value;

            return string.IsNullOrWhiteSpace(claim) ? null : claim;
        }

        public sealed class VoteRequest
        {
            // client sends 1 = agree, 0 = disagree
            public int Vote { get; set; }
        }

        public sealed class VoteResult
        {
            public int Agrees { get; set; }
            public int Disagrees { get; set; }
            public string NewStatus { get; set; } = "";
        }

        // GET /api/movies/{id}/plot-tags/my-votes
        // Returns the current user's votes for all tags on this movie.
        [HttpGet("my-votes")]
        [Authorize]
        public async Task<IActionResult> GetMyVotes(string id)
        {
            id = NormalizeImdb(id);
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            await using var ctx = _dbFactory.CreateDbContext();
            var votes = await ctx.MoviePlotTagVotes
                .AsNoTracking()
                .Where(v => v.ImdbId == id && v.UserId == userId)
                .Select(v => new { v.PlotTagId, v.Vote })
                .ToListAsync();

            return Ok(votes);
        }

        // POST /api/movies/{id}/plot-tags/{tagId}/vote
        // Body: { "vote": 1 } or { "vote": 0 }
        // Only authenticated users. Each user may vote once per (movie, tag) pair.
        [HttpPost("{tagId:int}/vote")]
        [Authorize]
        public async Task<IActionResult> VotePlotTag(string id, int tagId, [FromBody] VoteRequest req)
        {
            id = NormalizeImdb(id);
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = "id cannot be empty." });

            if (req == null || (req.Vote != 0 && req.Vote != 1))
                return BadRequest(new { message = "Vote must be 1 (agree) or 0 (disagree)." });

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "Unable to determine authenticated user id." });

            var dbVote = req.Vote == 1 ? (short)1 : (short)-1;

            await using var ctx = _dbFactory.CreateDbContext();

            // Ensure the referenced plot tag exists
            var plotTag = await ctx.PlotTags.FindAsync(tagId);
            if (plotTag == null)
                return NotFound(new { message = $"Plot tag id {tagId} not found." });

            // Find (movie,plotTag) link
            var moviePlotTag = await ctx.MoviePlotTags
                .FindAsync(new object[] { id, tagId });

            // If link doesn't exist and user agrees -> create approved link
            if (moviePlotTag == null)
            {
                if (dbVote == 1)
                {
                    moviePlotTag = new MoviePlotTag
                    {
                        ImdbId = id,
                        PlotTagId = tagId,
                        CreatedByUserId = userId,
                        Status = "approved",
                        CreatedAt = DateTime.UtcNow
                    };
                    ctx.MoviePlotTags.Add(moviePlotTag);
                    await ctx.SaveChangesAsync();
                }
                else
                {
                    // disagreeing on a tag that's not associated
                    return NotFound(new { message = "Plot tag not associated with this movie." });
                }
            }

            // Upsert vote (one row per imdb+plotTag+user)
            var existingVote = await ctx.MoviePlotTagVotes
                .FindAsync(new object[] { id, tagId, userId });

            if (existingVote == null)
            {
                var voteRow = new MoviePlotTagVote
                {
                    ImdbId = id,
                    PlotTagId = tagId,
                    UserId = userId,
                    Vote = dbVote,
                    CreatedAt = DateTime.UtcNow
                };
                ctx.MoviePlotTagVotes.Add(voteRow);
            }
            else
            {
                existingVote.Vote = dbVote;
                existingVote.CreatedAt = DateTime.UtcNow;
                ctx.MoviePlotTagVotes.Update(existingVote);
            }

            await ctx.SaveChangesAsync();

            // Recompute counts
            var agrees = await ctx.MoviePlotTagVotes
                .Where(v => v.ImdbId == id && v.PlotTagId == tagId && v.Vote == 1)
                .CountAsync();

            var disagrees = await ctx.MoviePlotTagVotes
                .Where(v => v.ImdbId == id && v.PlotTagId == tagId && v.Vote == -1)
                .CountAsync();

            // Update status: rejected when disagrees >= agrees, otherwise approved
            var newStatus = disagrees >= agrees ? "rejected" : "approved"; 
            if (moviePlotTag.Status != newStatus)
            {
                moviePlotTag.Status = newStatus;
                ctx.MoviePlotTags.Update(moviePlotTag);
                await ctx.SaveChangesAsync();
            }

            var result = new VoteResult
            {
                Agrees = agrees,
                Disagrees = disagrees,
                NewStatus = newStatus
            };

            return Ok(result);
        }
    }
}
