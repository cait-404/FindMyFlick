using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieCollection
{
    public string ImdbId { get; set; } = null!;

    public int TmdbCollectionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual Collection TmdbCollection { get; set; } = null!;
}
