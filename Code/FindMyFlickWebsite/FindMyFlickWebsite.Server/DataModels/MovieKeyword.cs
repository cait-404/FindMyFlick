using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieKeyword
{
    public string ImdbId { get; set; } = null!;

    public int TmdbKeywordId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual Keyword TmdbKeyword { get; set; } = null!;
}
