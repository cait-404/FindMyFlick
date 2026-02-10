using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindMyFlickWebsite.Server.Models
{
    public class MoviesView
    {
        // core identifiers
        [JsonPropertyName("id")]
        public int ID { get; set; }                       // maps to movies.imdb_id

        // primary metadata
        [JsonPropertyName("title")]
        public string? Name { get; set; }                 // maps to movies.title

        [JsonPropertyName("release_year")]
        public int Year { get; set; }                     // maps to movies.release_year

        [JsonPropertyName("mpaa_rating")]
        public string? AgeRating { get; set; }            // maps to movies.mpaa_rating

        [JsonPropertyName("plot_summary")]
        public string? Summary { get; set; }              // maps to movies.plot_summary

        [JsonPropertyName("poster_url")]
        public string? Poster { get; set; }               // maps to movies.poster_url

        // convenience CLR-only values (backed by owned collections)
        // "genre" JSON now contains genre names (strings) instead of TMDB ids.
        [NotMapped]
        [JsonPropertyName("genre")]
        public List<string> Genre
        {
            get => GenreEntries?.Select(g => g.GenreName ?? string.Empty).ToList() ?? new List<string>();
            set => GenreEntries = (value ?? Enumerable.Empty<string>())
                        .Select(n => new GenreEntry { GenreName = n }).ToList();
        }

        // persisted owned collections (mapped in ApplicationDbContext)
        // Keep GenreEntries so clients that inspect details can get both id+name when available.
        public List<GenreEntry> GenreEntries { get; set; } = new List<GenreEntry>();

        // If you still want a lightweight streaming provider DTO, use a different name:
        public List<StreamingProviderView> StreamingProviders { get; set; } = new List<StreamingProviderView>();

        // Tags + TagVotes mappings remain (owned types)
        [JsonPropertyName("Tags")]
        public TagsView Tags { get; set; } = new TagsView();

        [JsonPropertyName("tagVotes")]
        public List<TagVote> TagVotes { get; set; } = new List<TagVote>();

        public class TagVote
        {
            [JsonPropertyName("tagID")]
            public int TagID { get; set; }

            [JsonPropertyName("upvotes")]
            public int Upvotes { get; set; }

            [JsonPropertyName("downvotes")]
            public int Downvotes { get; set; }
        }

        // Owned/linked helper types
        public class GenreEntry
        {
            public int Id { get; set; }

            // keep id in the DTO in case consumers still want it
            [JsonPropertyName("tmdb_genre_id")]
            public int TmdbGenreId { get; set; }

            // new: genre display name pulled from genres.genre_name
            [JsonPropertyName("genre_name")]
            public string? GenreName { get; set; }
        }

        // Renamed DTO to avoid naming collision with DataModels.StreamingProvider
        public class StreamingProviderView
        {
            public int Id { get; set; }

            [JsonPropertyName("provider_name")]
            public string? ProviderName { get; set; }
        }
    }
}