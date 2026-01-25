using FindMyFlickWebsite.Server.Models;
using Microsoft.AspNetCore.Mvc;

using FindMyFlickWebsite.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly List<Movies> _movies;

        public MoviesController()
        {
            //will need to get movie list from api or db here. 

            //generated with copilot using the json schema and DTO 
            // Seed in-memory sample data matching the DTO shape
            _movies = new List<Movies>
            {
                new Movies
                {
                    ID = 123,
                    Name = "cool movie 1",
                    Summary = "long string",
                    UserRatings = 9.8,
                    UserWatchStatus = true,
                    Poster = "link",
                    StreamingServices = new List<string> {"hulu" },
                    Year = 2012,
                    AgeRating = "pg-13",
                    Genre = new List<string> { "action", "comedy" },
                    Tags = new Tags
                    {
                        PlotTags = new List<Tags.PlotTag>
                        {
                            new Tags.PlotTag { TagID = 8, TagType = "plot", TagName = "Major Death" },
                            new Tags.PlotTag { TagID = 9, TagType = "plot", TagName = "Hero Sacrifice" }
                        },
                        TriggerTags = new List<Tags.TriggerTag>
                        {
                            new Tags.TriggerTag { TagID = 1, TagType = "trigger", TagName = "Violence" },
                            new Tags.TriggerTag { TagID = 10, TagType = "trigger", TagName = "Blood" },
                            new Tags.TriggerTag { TagID = 11, TagType = "trigger", TagName = "Loud Noises" }
                        },
                        PersonTags = new List<Tags.PersonTag>
                        {
                            new Tags.PersonTag { TagID = 6, TagType = "person", TagName = "Famous Actor" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote>
                    {
                        new Movies.TagVote { TagID = 8, Upvotes = 12, Downvotes = 2 },
                        new Movies.TagVote { TagID = 1, Upvotes = 12, Downvotes = 2 },
                        new Movies.TagVote { TagID = 6, Upvotes = 3, Downvotes = 2 },
                        new Movies.TagVote { TagID = 9, Upvotes = 5, Downvotes = 0 },
                        new Movies.TagVote { TagID = 10, Upvotes = 3, Downvotes = 1 },
                        new Movies.TagVote { TagID = 11, Upvotes = 0, Downvotes = 0 }
                    }
                },
                new Movies{
                    ID = 3,
                    Name = "cool movie",
                    Summary = "long string",
                    UserRatings = 9.5,
                    UserWatchStatus = true,
                    Poster = "link",
                    StreamingServices = new List<string> { "netflix", "hulu" },
                    Year = 2011,
                    AgeRating = "pg-13",
                    Genre = new List<string> { "action", "comedy", "romance" },
                    Tags = new Tags
                    {
                        PlotTags = new List<Tags.PlotTag>
                        {
                            new Tags.PlotTag { TagID = 2, TagType = "plot", TagName = "Twist" },
                            new Tags.PlotTag { TagID = 12, TagType = "plot", TagName = "Secret Identity" }
                        },
                        TriggerTags = new List<Tags.TriggerTag>
                        {
                            new Tags.TriggerTag { TagID = 1, TagType = "trigger", TagName = "Violence" },
                            new Tags.TriggerTag { TagID = 13, TagType = "trigger", TagName = "Flashing Lights" },
                            new Tags.TriggerTag { TagID = 14, TagType = "trigger", TagName = "Smoking" }
                        },
                        PersonTags = new List<Tags.PersonTag>
                        {
                            new Tags.PersonTag { TagID = 4, TagType = "person", TagName = "Supporting Actor" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote>
                    {
                        new Movies.TagVote { TagID = 2, Upvotes = 12, Downvotes = 2 },
                        new Movies.TagVote { TagID = 1, Upvotes = 12, Downvotes = 2 },
                        new Movies.TagVote { TagID = 4, Upvotes = 12, Downvotes = 2 },
                        new Movies.TagVote { TagID = 12, Upvotes = 4, Downvotes = 1 },
                        new Movies.TagVote { TagID = 13, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 14, Upvotes = 0, Downvotes = 0 }
                    }
                },

                // Additional 8 movies (each with 2 plot tags, 3 trigger tags, 1 person tag)
                new Movies {
                    ID = 200,
                    Name = "Midnight Chase",
                    Summary = "A tense night pursuit through city streets.",
                    UserRatings = 7.4,
                    UserWatchStatus = false,
                    Poster = "link-midnight",
                    StreamingServices = new List<string> { "netflix" },
                    Year = 2018,
                    AgeRating = "r",
                    Genre = new List<string> { "thriller", "action" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 201, TagType = "plot", TagName = "Car Chase" },
                            new Tags.PlotTag { TagID = 202, TagType = "plot", TagName = "Wrongly Accused" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 203, TagType = "trigger", TagName = "Gunshots" },
                            new Tags.TriggerTag { TagID = 204, TagType = "trigger", TagName = "Dark Scenes" },
                            new Tags.TriggerTag { TagID = 205, TagType = "trigger", TagName = "Police Action" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 206, TagType = "person", TagName = "Lead Detective" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 201, Upvotes = 8, Downvotes = 1 },
                        new Movies.TagVote { TagID = 202, Upvotes = 6, Downvotes = 0 },
                        new Movies.TagVote { TagID = 203, Upvotes = 10, Downvotes = 2 },
                        new Movies.TagVote { TagID = 204, Upvotes = 2, Downvotes = 0 },
                        new Movies.TagVote { TagID = 205, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 206, Upvotes = 3, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 201,
                    Name = "Garden of Echoes",
                    Summary = "An introspective drama about memory and family.",
                    UserRatings = 8.1,
                    UserWatchStatus = true,
                    Poster = "link-garden",
                    StreamingServices = new List<string> { "hulu" },
                    Year = 2016,
                    AgeRating = "pg",
                    Genre = new List<string> { "drama" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 207, TagType = "plot", TagName = "Family Reunion" },
                            new Tags.PlotTag { TagID = 208, TagType = "plot", TagName = "Lost Memory" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 209, TagType = "trigger", TagName = "Death Mention" },
                            new Tags.TriggerTag { TagID = 210, TagType = "trigger", TagName = "Emotional Scenes" },
                            new Tags.TriggerTag { TagID = 211, TagType = "trigger", TagName = "Phone Calls" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 212, TagType = "person", TagName = "Matriarch" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 207, Upvotes = 7, Downvotes = 0 },
                        new Movies.TagVote { TagID = 208, Upvotes = 9, Downvotes = 1 },
                        new Movies.TagVote { TagID = 209, Upvotes = 2, Downvotes = 0 },
                        new Movies.TagVote { TagID = 210, Upvotes = 4, Downvotes = 0 },
                        new Movies.TagVote { TagID = 211, Upvotes = 0, Downvotes = 0 },
                        new Movies.TagVote { TagID = 212, Upvotes = 5, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 202,
                    Name = "Sunset Heist",
                    Summary = "A heist crew plans one last job at dusk.",
                    UserRatings = 6.9,
                    UserWatchStatus = false,
                    Poster = "link-sunset",
                    StreamingServices = new List<string> { "prime" },
                    Year = 2020,
                    AgeRating = "pg-13",
                    Genre = new List<string> { "crime", "action" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 213, TagType = "plot", TagName = "Inside Job" },
                            new Tags.PlotTag { TagID = 214, TagType = "plot", TagName = "Double Cross" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 215, TagType = "trigger", TagName = "Guns" },
                            new Tags.TriggerTag { TagID = 216, TagType = "trigger", TagName = "Tension" },
                            new Tags.TriggerTag { TagID = 217, TagType = "trigger", TagName = "Chase" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 218, TagType = "person", TagName = "Crew Leader" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 213, Upvotes = 4, Downvotes = 1 },
                        new Movies.TagVote { TagID = 214, Upvotes = 5, Downvotes = 2 },
                        new Movies.TagVote { TagID = 215, Upvotes = 6, Downvotes = 3 },
                        new Movies.TagVote { TagID = 216, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 217, Upvotes = 2, Downvotes = 0 },
                        new Movies.TagVote { TagID = 218, Upvotes = 2, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 203,
                    Name = "Neon Dreams",
                    Summary = "A visually stylized sci-fi about augmented reality.",
                    UserRatings = 8.7,
                    UserWatchStatus = true,
                    Poster = "link-neon",
                    StreamingServices = new List<string> { "netflix", "prime" },
                    Year = 2022,
                    AgeRating = "r",
                    Genre = new List<string> { "sci-fi", "thriller" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 219, TagType = "plot", TagName = "Reality Blur" },
                            new Tags.PlotTag { TagID = 220, TagType = "plot", TagName = "Corporate Conspiracy" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 221, TagType = "trigger", TagName = "Strobe Effects" },
                            new Tags.TriggerTag { TagID = 222, TagType = "trigger", TagName = "Violence" },
                            new Tags.TriggerTag { TagID = 223, TagType = "trigger", TagName = "Drug Use" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 224, TagType = "person", TagName = "Lead Hacker" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 219, Upvotes = 11, Downvotes = 1 },
                        new Movies.TagVote { TagID = 220, Upvotes = 7, Downvotes = 0 },
                        new Movies.TagVote { TagID = 221, Upvotes = 0, Downvotes = 1 },
                        new Movies.TagVote { TagID = 222, Upvotes = 8, Downvotes = 2 },
                        new Movies.TagVote { TagID = 223, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 224, Upvotes = 4, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 204,
                    Name = "Quiet Harbor",
                    Summary = "A small-town mystery unravels slowly.",
                    UserRatings = 7.9,
                    UserWatchStatus = false,
                    Poster = "link-harbor",
                    StreamingServices = new List<string> { "hulu" },
                    Year = 2015,
                    AgeRating = "pg-13",
                    Genre = new List<string> { "mystery", "drama" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 225, TagType = "plot", TagName = "Missing Person" },
                            new Tags.PlotTag { TagID = 226, TagType = "plot", TagName = "Small Town Secrets" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 227, TagType = "trigger", TagName = "Police Procedure" },
                            new Tags.TriggerTag { TagID = 228, TagType = "trigger", TagName = "Disturbing Imagery" },
                            new Tags.TriggerTag { TagID = 229, TagType = "trigger", TagName = "Creepy Atmosphere" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 230, TagType = "person", TagName = "Local Sheriff" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 225, Upvotes = 3, Downvotes = 0 },
                        new Movies.TagVote { TagID = 226, Upvotes = 6, Downvotes = 1 },
                        new Movies.TagVote { TagID = 227, Upvotes = 0, Downvotes = 0 },
                        new Movies.TagVote { TagID = 228, Upvotes = 2, Downvotes = 1 },
                        new Movies.TagVote { TagID = 229, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 230, Upvotes = 2, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 205,
                    Name = "Highland Song",
                    Summary = "A period piece about love and rebellion.",
                    UserRatings = 7.2,
                    UserWatchStatus = true,
                    Poster = "link-highland",
                    StreamingServices = new List<string> { "prime" },
                    Year = 2013,
                    AgeRating = "pg",
                    Genre = new List<string> { "romance", "drama" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 231, TagType = "plot", TagName = "Forbidden Romance" },
                            new Tags.PlotTag { TagID = 232, TagType = "plot", TagName = "Rebellion" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 233, TagType = "trigger", TagName = "Battle Scenes" },
                            new Tags.TriggerTag { TagID = 234, TagType = "trigger", TagName = "Injury" },
                            new Tags.TriggerTag { TagID = 235, TagType = "trigger", TagName = "Fire" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 236, TagType = "person", TagName = "Rebel Leader" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 231, Upvotes = 5, Downvotes = 0 },
                        new Movies.TagVote { TagID = 232, Upvotes = 4, Downvotes = 1 },
                        new Movies.TagVote { TagID = 233, Upvotes = 3, Downvotes = 2 },
                        new Movies.TagVote { TagID = 234, Upvotes = 1, Downvotes = 0 },
                        new Movies.TagVote { TagID = 235, Upvotes = 0, Downvotes = 0 },
                        new Movies.TagVote { TagID = 236, Upvotes = 1, Downvotes = 0 }
                    }
                },

                new Movies {
                    ID = 206,
                    Name = "Echoes of Tomorrow",
                    Summary = "A time-travel tale with personal stakes.",
                    UserRatings = 8.4,
                    UserWatchStatus = false,
                    Poster = "link-echoes",
                    StreamingServices = new List<string> { "netflix", "prime" },
                    Year = 2019,
                    AgeRating = "pg-13",
                    Genre = new List<string> { "sci-fi", "drama" },
                    Tags = new Tags {
                        PlotTags = new List<Tags.PlotTag> {
                            new Tags.PlotTag { TagID = 237, TagType = "plot", TagName = "Time Loop" },
                            new Tags.PlotTag { TagID = 238, TagType = "plot", TagName = "Alternate Timeline" }
                        },
                        TriggerTags = new List<Tags.TriggerTag> {
                            new Tags.TriggerTag { TagID = 239, TagType = "trigger", TagName = "Temporal Paradox" },
                            new Tags.TriggerTag { TagID = 240, TagType = "trigger", TagName = "Loss" },
                            new Tags.TriggerTag { TagID = 241, TagType = "trigger", TagName = "Emotional Conflict" }
                        },
                        PersonTags = new List<Tags.PersonTag> {
                            new Tags.PersonTag { TagID = 242, TagType = "person", TagName = "Scientist" }
                        }
                    },
                    TagVotes = new List<Movies.TagVote> {
                        new Movies.TagVote { TagID = 237, Upvotes = 9, Downvotes = 1 },
                        new Movies.TagVote { TagID = 238, Upvotes = 7, Downvotes = 0 },
                        new Movies.TagVote { TagID = 239, Upvotes = 0, Downvotes = 0 },
                        new Movies.TagVote { TagID = 240, Upvotes = 3, Downvotes = 0 },
                        new Movies.TagVote { TagID = 241, Upvotes = 2, Downvotes = 0 },
                        new Movies.TagVote { TagID = 242, Upvotes = 4, Downvotes = 0 }
                    }
                }
            };


        }

        /// 
        /// Get all movies.
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Movies>), 200)]
        public ActionResult<IEnumerable<Movies>> GetAll() =>
            Ok(_movies);

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
public static IEnumerable<Movies> AdvancedSearch(
            IEnumerable<Movies> source,
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

                // streaming services
                if (svcList != null && svcList.Any())
                {
                    var hasMatches = svcList.All(s => m.StreamingServices.Any(ms => string.Equals(ms, s, System.StringComparison.OrdinalIgnoreCase)))
                                     && matchAllStreaming
                        || (!matchAllStreaming && svcList.Any(s => m.StreamingServices.Any(ms => string.Equals(ms, s, System.StringComparison.OrdinalIgnoreCase))));

                    if (!hasMatches) continue;
                }

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
                        ? genreList.All(g => m.Genre.Any(mg => string.Equals(mg, g, System.StringComparison.OrdinalIgnoreCase)))
                        : genreList.Any(g => m.Genre.Any(mg => string.Equals(mg, g, System.StringComparison.OrdinalIgnoreCase)));

                    if (!hasGenreMatches) continue;
                }

                // year
                if (year.HasValue && m.Year != year.Value) continue;

                // tags include (check tag names across PlotTags, TriggerTags, PersonTags)
                if (tagNameListInclude != null && tagNameListInclude.Any())
                {
                    var movieTagNames = new HashSet<string>(
                        (m.Tags?.PlotTags ?? Enumerable.Empty<Tags.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<Tags.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<Tags.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

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
                        (m.Tags?.PlotTags ?? Enumerable.Empty<Tags.PlotTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())
                        .Concat((m.Tags?.TriggerTags ?? Enumerable.Empty<Tags.TriggerTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant()))
                        .Concat((m.Tags?.PersonTags ?? Enumerable.Empty<Tags.PersonTag>()).Select(t => (t.TagName ?? string.Empty).Trim().ToLowerInvariant())));

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
        [ProducesResponseType(typeof(Movies), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Movies> GetById(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.ID == id);
            if (movie is null) return NotFound();
            return Ok(movie);
        }

        /// <summary>
        /// Search movies by multiple optional criteria.
        /// Query example:
        /// GET api/movies/search?name=cool&streamingServices=netflix&streamingServices=hulu&genres=action&year=2012&tagNames=Violence&matchAllTags=true
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Movies>), 200)]
        public ActionResult<IEnumerable<Movies>> Search(
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
            var results = AdvancedSearch(
                _movies,
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

        //tag voting endpoints---------------------------------

        //upvote tag
        //generated using intellisense, no alterations
        [HttpPatch("upvote/{movieId:int}/tag/{tagId:int}")]
        [ProducesResponseType(typeof(Movies.TagVote), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Movies.TagVote> UpvoteTag(int movieId, int tagId)
        {
            var movie = _movies.FirstOrDefault(m => m.ID == movieId);
            if (movie is null) return NotFound();
            var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
            if (tagVote is null) return NotFound();
            tagVote.Upvotes += 1;
            return Ok(tagVote);
        }

        //downvote tag
        //copied from upvote code with alterations
        [HttpPatch("downvote/{movieId:int}/tag/{tagId:int}")]
        [ProducesResponseType(typeof(Movies.TagVote), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Movies.TagVote> DownvoteTag(int movieId, int tagId)
        {
            var movie = _movies.FirstOrDefault(m => m.ID == movieId);
            if (movie is null) return NotFound();
            var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
            if (tagVote is null) return NotFound();
            tagVote.Downvotes += 1;
            //remove tag from movie if downvotes are equal to upvotes
            if (tagVote.Downvotes == tagVote.Upvotes) RemoveTagFromMovie(movieId, tagId); 
            return Ok(tagVote);
        }

        //add new tag to movie
        //mostly generated with intellisense with minor alterations
        [HttpPost("{movieId:int}/tag/{tagId:int}")]
        [ProducesResponseType(typeof(Movies.TagVote), 201)]
        [ProducesResponseType(404)]
        public ActionResult<Movies.TagVote> AddTagToMovie(int movieId, int tagId)
        {
            var movie = _movies.FirstOrDefault(m => m.ID == movieId);
            if (movie is null) return NotFound();
            // Check if tag already exists
            var existingTagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
            if (existingTagVote != null)
            {
                return Conflict("Tag already exists for this movie. Go vote for it intstead!"); //do I want this to instead just add a vote?
            }
            //tag creation counts as an upvote if it becomes equal with the downvotes it is removed
            var newTagVote = new Movies.TagVote
            {
                TagID = tagId,
                Upvotes = 1,
                Downvotes = 0
            };
            movie.TagVotes.Add(newTagVote);
            return CreatedAtAction(nameof(GetById), new { id = movieId }, newTagVote);
        }

        //remove tag from movie
        //intellisense generated
        [HttpDelete("{movieId:int}/tag/{tagId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTagFromMovie(int movieId, int tagId)
        {
            var movie = _movies.FirstOrDefault(m => m.ID == movieId);
            if (movie is null) return NotFound();
            var tagVote = movie.TagVotes.FirstOrDefault(tv => tv.TagID == tagId);
            if (tagVote is null) return NotFound();
            movie.TagVotes.Remove(tagVote);
            return NoContent();
        }
    }
}