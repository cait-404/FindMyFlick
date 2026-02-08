using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieKeywordsAgg
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public List<string>? Keywords { get; set; }
}
