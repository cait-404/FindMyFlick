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

        //[JsonPropertyName("tmdb_id")]
        //public int? TmdbId { get; set; }                  // maps to movies.tmdb_id

        // primary metadata
        [JsonPropertyName("title")]
        public string? Name { get; set; }                 // maps to movies.title

        [JsonPropertyName("release_year")]
        public int Year { get; set; }                     // maps to movies.release_year

        [JsonPropertyName("mpaa_rating")]
        public string? AgeRating { get; set; }            // maps to movies.mpaa_rating

        //[JsonPropertyName("runtime_minutes")]
        //public int? RuntimeMinutes { get; set; }          // maps to movies.runtime_minutes

        [JsonPropertyName("plot_summary")]
        public string? Summary { get; set; }              // maps to movies.plot_summary

        [JsonPropertyName("poster_url")]
        public string? Poster { get; set; }               // maps to movies.poster_url

        //[JsonPropertyName("original_language")]
        //public string? OriginalLanguage { get; set; }     // maps to movies.original_language

        //[JsonPropertyName("media_type")]
        //public string? MediaType { get; set; }            // maps to movies.media_type

        //[JsonPropertyName("tagline")]
        //public string? Tagline { get; set; }              // maps to movies.tagline

        //[JsonPropertyName("status")]
        //public string? Status { get; set; }               // maps to movies.status

        //[JsonPropertyName("created_at")]
        //public DateTime? CreatedAt { get; set; }          // maps to movies.created_at (timestamptz)

        //[JsonPropertyName("updated_at")]
        //public DateTime? UpdatedAt { get; set; }          // maps to movies.updated_at (timestamptz)

        // convenience CLR-only values (backed by owned collections)
        [NotMapped]
        [JsonPropertyName("genre")]
        public List<int> Genre
        {
            get => GenreEntries?.Select(g => g.TmdbGenreId).ToList() ?? new List<int>();
            set => GenreEntries = (value ?? Enumerable.Empty<int>()).Select(v => new GenreEntry { TmdbGenreId = v }).ToList();
        }

        // persisted owned collections (mapped in ApplicationDbContext)
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

            [JsonPropertyName("tmdb_genre_id")]
            public int TmdbGenreId { get; set; }
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