using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieStreaming
{
    public string ImdbId { get; set; } = null!;

    public int TmdbProviderId { get; set; }

    public string OfferType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual StreamingProvider TmdbProvider { get; set; } = null!;
}
