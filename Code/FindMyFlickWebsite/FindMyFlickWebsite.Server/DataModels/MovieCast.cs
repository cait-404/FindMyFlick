using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieCast
{
    public string TmdbCreditId { get; set; } = null!;

    public string ImdbId { get; set; } = null!;

    public int TmdbPersonId { get; set; }

    public string? CharacterName { get; set; }

    public int? CastOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual Person TmdbPerson { get; set; } = null!;
}
