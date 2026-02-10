using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieGenre
{
    public string ImdbId { get; set; } = null!;

    public int TmdbGenreId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual Genre TmdbGenre { get; set; } = null!;
}
