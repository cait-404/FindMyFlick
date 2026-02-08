using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieCreditsAgg
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public string? TopBilledCast { get; set; }

    public string? Directors { get; set; }

    public string? Writers { get; set; }
}
