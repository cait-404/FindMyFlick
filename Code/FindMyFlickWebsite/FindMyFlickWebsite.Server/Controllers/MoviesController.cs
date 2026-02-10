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
    public class MoviesController : ControllerBase
    {
        private readonly List<MoviesView> _movies;
        private ApplicationDbContext _context;


        public MoviesController(ApplicationDbContext context)
        {

                _context = context;       

        }

            


        ///// 
        ///// Get all movies.
        ///// 
        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<MoviesView>), 200)]
        //public async Task<ActionResult<IEnumerable<MoviesView>>> GetAllAsync()
        //{
        //    List<MoviesView> movies = await _context.Set<MoviesView>().ToListAsync();
        //    return Ok(movies);

        //}

        


        /// <summary>
        /// Advanced search helper that filters a sequence of movies by various optional criteria.
        /// Any null or empty filter parameter is ignored.
        /// - name: substring match (case-insensitive)
        /// - streamingServices: match any (or all if matchAllStreaming true)
        /// - ageRating: exact case-insensitive match
        /// - genres: match any (or all if matchAllGenres true)
        /// - year: exact match
        /// - tagNamesInclude: tag name match across Plot/Trigger/Person tags (any or all via matchAllTagsIn) to include in search
        /// - tagNamesExclude: tag name match across Plot/Trigger/Person tag (any or all via matchAllTagsEx (on by default) to exclude from search
        /// </summary>
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

            var svcList = streamingServices?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var genreList = genres?.Where(g => !string.IsNullOrWhiteSpace(g)).ToList();
            var tagNameListInclude = tagNamesInclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();
            var tagNameListExclude = tagNamesExclude?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();

            foreach (var m in source)
            {
                if (m == null) continue;

                // name (substring, case-insensitive)
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (string.IsNullOrWhiteSpace(m.Name) ||
                        !m.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                //// streaming services
                //if (svcList != null && svcList.Any())
                //{
                //    var hasMatches = svcList.All(s => m.StreamingServices.Any(ms => string.Equals(ms, s, System.StringComparison.OrdinalIgnoreCase)))
                //                     && matchAllStreaming
                //        || (!matchAllStreaming && svcList.Any(s => m.StreamingServices.Any(ms => string.Equals(ms, s, System.StringComparison.OrdinalIgnoreCase))));

                //    if (!hasMatches) continue;
                //}

                // age rating (exact, case-insensitive)
                if (!string.IsNullOrWhiteSpace(ageRating))
                {
                    if (string.IsNullOrWhiteSpace(m.AgeRating) ||
                        !string.Equals(m.AgeRating, ageRating, System.StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // genres
                if (genreList != null && genreList.Any())
                {
                    var hasGenreMatches = matchAllGenres
                        ? genreList.All(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, System.StringComparison.OrdinalIgnoreCase)))
                        : genreList.Any(g => m.Genre.Any(mg => string.Equals(mg.ToString(), g, System.StringComparison.OrdinalIgnoreCase)));

                    if (!hasGenreMatches) continue;
                }

                // year
                if (year.HasValue && m.Year != year.Value) continue;

                // tags include (check tag names across PlotTags, TriggerTags, PersonTags)
                if (tagNameListInclude != null && tagNameListInclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<TagsView.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

                    var queryTagNamesLower = tagNameListInclude.Select(t => t.ToLowerInvariant()).ToList();

                    var tagMatch = matchAllTagsIn
                        ? queryTagNamesLower.All(q => movieTagNames.Contains(q))
                        : queryTagNamesLower.Any(q => movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                // tags exclude (check tag names across PlotTags, TriggerTags, PersonTags)
                if (tagNameListExclude != null && tagNameListExclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<TagsView.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<TagsView.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<TagsView.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

                    var queryTagNamesLower = tagNameListExclude.Select(t => t.ToLowerInvariant()).ToList();

                    var tagMatch = matchAllTagsEx
                        ? queryTagNamesLower.All(q => !movieTagNames.Contains(q))
                        : queryTagNamesLower.Any(q => !movieTagNames.Contains(q));

                    if (!tagMatch) continue;
                }

                yield return m;
            }
        }

        /// 
        /// Get a single movie by id.
        /// 
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MoviesView), 200)]
        [ProducesResponseType(404)]
        public ActionResult<MoviesView> GetById(int id)
        {
            //var movie = _movies.FirstOrDefault(m => m.ID == id);
            //if (movie is null) return NotFound();
            //return Ok(movie);
            return Ok("wip");
        }

        /// <summary>
        /// Search movies by multiple optional criteria.
        /// Query example:
        /// GET api/movies/search?name=cool&streamingServices=netflix&streamingServices=hulu&genres=action&year=2012&tagNames=Violence&matchAllTags=true
        /// updated wth copilot query "using GetMoviesView_ParseImdb as a reference make the search endpoint use Movie.cs as a datamodel for the MoviesView view model
        /// 
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MoviesView>), 200)]
        public async Task<ActionResult<IEnumerable<MoviesView>>> Search(
            [FromQuery] string? name = null,
            [FromQuery] List<string>? streamingServices = null,
            [FromQuery] bool matchAllStreaming = false,
            [FromQuery] string? ageRating = null,
            [FromQuery] List<string>? genres = null,
            [FromQuery] bool matchAllGenres = false,
            [FromQuery] int? year = null,
            [FromQuery] List<string>? tagNamesInclude = null,
            [FromQuery] List<string>? tagNamesExclude = null,
            [FromQuery] bool matchAllTagsIn = false,
            [FromQuery] bool matchAllTagsEx = true)
        {
            // Load Movie data models, include MovieGenres -> Genre so we can read genre_name
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = loaded.Select(m => new Models.MoviesView
            {
                ID = ParseImdbToInt(m.ImdbId),
                Name = m.Title,
                Year = m.ReleaseYear,
                AgeRating = m.MpaaRating,
                Summary = m.PlotSummary,
                Poster = m.PosterUrl,
                GenreEntries = m.MovieGenres
                    .Select(g => new Models.MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    })
                    .ToList(),
                StreamingProviders = m.MovieStreamings
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new Models.MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider.ProviderName
                    })
                    .ToList(),
                Tags = new Models.TagsView(),
                TagVotes = new List<Models.MoviesView.TagVote>()
            }).ToList();

            // Run the existing AdvancedSearch helper against the projected DTOs
            var results = AdvancedSearch(
                dtoList,
                name,
                streamingServices,
                matchAllStreaming,
                ageRating,
                genres,
                matchAllGenres,
                year,
                tagNamesInclude,
                tagNamesExclude,
                matchAllTagsIn,
                matchAllTagsEx);

            return Ok(results);
        }

        // Also update GetMoviesView_ParseImdb to include genre_name in the projection
        [HttpGet()]
        public async Task<IActionResult> GetMoviesView_ParseImdb()
        {   
            var loaded = await _context.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.TmdbGenre)
                .Include(m => m.MovieStreamings).ThenInclude(ms => ms.TmdbProvider)
                .AsNoTracking()
                .ToListAsync(); // client-side projection follows

            var dto = loaded.Select(m => new Models.MoviesView
            {
                ID = ParseImdbToInt(m.ImdbId),             // parsed on client
                Name = m.Title,
                Year = m.ReleaseYear,
                AgeRating = m.MpaaRating,
                Summary = m.PlotSummary,
                Poster = m.PosterUrl,
                GenreEntries = m.MovieGenres
                    .Select(g => new Models.MoviesView.GenreEntry
                    {
                        TmdbGenreId = g.TmdbGenreId,
                        GenreName = g.TmdbGenre?.GenreName ?? string.Empty
                    })
                    .ToList(),
                StreamingProviders = m.MovieStreamings
                    .GroupBy(ms => ms.TmdbProviderId)
                    .Select(g => new Models.MoviesView.StreamingProviderView
                    {
                        Id = g.Key,
                        ProviderName = g.First().TmdbProvider.ProviderName
                    })
                    .ToList(),
                Tags = new Models.TagsView(),
                TagVotes = new List<Models.MoviesView.TagVote>()
            }).ToList();

            return Ok(dto);
        }

        private static int ParseImdbToInt(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
                return 0;
            // Assumes IMDb IDs are in the form "tt1234567"
            var digits = new string(imdbId.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var result) ? result : 0;
        //tag voting endpoints---------------------------------

        // //upvote tag
        // //generated using intellisense, no alterations
        // [HttpPatch("upvote/{movieId:int}/tag/{tagId:int}")]
        // [ProducesResponseType(typeof(Movies.TagVote), 200)]
        // [ProducesResponseType(404)]
        // public ActionResult<Movies.TagVote> UpvoteTag(int movieId, int tagId)
        // {
        //     var movie = _movies.FirstOrDefault(m => m.ID == movieId);
        //     if (movie is null) return NotFound();
        //     var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
        //     if (tagVote is null) return NotFound();
        //     tagVote.Upvotes += 1;
        //     return Ok(tagVote);
        // }

        // //downvote tag
        // //copied from upvote code with alterations
        // [HttpPatch("downvote/{movieId:int}/tag/{tagId:int}")]
        // [ProducesResponseType(typeof(Movies.TagVote), 200)]
        // [ProducesResponseType(404)]
        // public ActionResult<Movies.TagVote> DownvoteTag(int movieId, int tagId)
        // {
        //     var movie = _movies.FirstOrDefault(m => m.ID == movieId);
        //     if (movie is null) return NotFound();
        //     var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
        //     if (tagVote is null) return NotFound();
        //     tagVote.Downvotes += 1;
        //     //remove tag from movie if downvotes are equal to upvotes
        //     if (tagVote.Downvotes == tagVote.Upvotes) RemoveTagFromMovie(movieId, tagId); 
        //     return Ok(tagVote);
        // }

        // //add new tag to movie
        // //mostly generated with intellisense with minor alterations
        // [HttpPost("{movieId:int}/tag/{tagId:int}")]
        // [ProducesResponseType(typeof(Movies.TagVote), 201)]
        // [ProducesResponseType(404)]
        // public ActionResult<Movies.TagVote> AddTagToMovie(int movieId, int tagId)
        // {
        //     var movie = _movies.FirstOrDefault(m => m.ID == movieId);
        //     if (movie is null) return NotFound();
        //     // Check if tag already exists
        //     var existingTagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
        //     if (existingTagVote != null)
        //     {
        //         return Conflict("Tag already exists for this movie. Go vote for it intstead!"); //do I want this to instead just add a vote?
        //     }
        //     //tag creation counts as an upvote if it becomes equal with the downvotes it is removed
        //     var newTagVote = new Movies.TagVote
        //     {
        //         TagID = tagId,
        //         Upvotes = 1,
        //         Downvotes = 0
        //     };
        //     movie.TagVotes.Add(newTagVote);
        //     return CreatedAtAction(nameof(GetById), new { id = movieId }, newTagVote);
        // }

        // //remove tag from movie
        // //intellisense generated
        // [HttpDelete("{movieId:int}/tag/{tagId:int}")]
        // [ProducesResponseType(204)]
        // [ProducesResponseType(404)]
        // public IActionResult RemoveTagFromMovie(int movieId, int tagId)
        // {
        //     var movie = _movies.FirstOrDefault(m => m.ID == movieId);
        //     if (movie is null) return NotFound();
        //     var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
        //     if (tagVote is null) return NotFound();
        //     movie.TagVotes.Remove(tagVote);
        //     return NoContent();
        // }

    }
}