using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class Movie
{
    public string ImdbId { get; set; } = null!;

    public int? TmdbId { get; set; }

    public string Title { get; set; } = null!;

    public int ReleaseYear { get; set; }

    public string? MpaaRating { get; set; }

    public int? RuntimeMinutes { get; set; }

    public string? PlotSummary { get; set; }

    public string? PosterUrl { get; set; }

    public string? OriginalLanguage { get; set; }

    public string MediaType { get; set; } = null!;

    public string? Tagline { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual DtddOverride? DtddOverride { get; set; }

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    public virtual MovieCollection? MovieCollection { get; set; }

    public virtual ICollection<MovieCrew> MovieCrews { get; set; } = new List<MovieCrew>();

    public virtual MovieDtddTitle? MovieDtddTitle { get; set; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<MovieKeyword> MovieKeywords { get; set; } = new List<MovieKeyword>();

    public virtual ICollection<MovieStreaming> MovieStreamings { get; set; } = new List<MovieStreaming>();

    public virtual ICollection<MovieWarning> MovieWarnings { get; set; } = new List<MovieWarning>();
}
