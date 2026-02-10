using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMoviesStreamableU
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public string? PosterUrl { get; set; }

    public int? RuntimeMinutes { get; set; }

    public string? PlotSummary { get; set; }

    public string? OriginalLanguage { get; set; }

    public string? ProviderName { get; set; }

    public string? OfferType { get; set; }

    public int? OfferRank { get; set; }
}
