using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieDtddTitle
{
    public string ImdbId { get; set; } = null!;

    public int? DtddTitleId { get; set; }

    public string? MatchMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? DtddMediaId { get; set; }

    public string? DtddImdbId { get; set; }

    public int? DtddTmdbId { get; set; }

    public string? DtddTitle { get; set; }

    public int? DtddReleaseYear { get; set; }

    public decimal? MatchScore { get; set; }

    public virtual Movie Imdb { get; set; } = null!;
}
