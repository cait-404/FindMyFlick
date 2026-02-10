using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MoviesWithWarning
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public string? MpaaRating { get; set; }

    public int? RuntimeMinutes { get; set; }

    public string? PlotSummary { get; set; }

    public string? PosterUrl { get; set; }

    public string? OriginalLanguage { get; set; }

    public string? MediaType { get; set; }

    public string? Tagline { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
