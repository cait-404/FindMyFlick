using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieCrew
{
    public string TmdbCreditId { get; set; } = null!;

    public string ImdbId { get; set; } = null!;

    public int TmdbPersonId { get; set; }

    public string? Department { get; set; }

    public string? Job { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;

    public virtual Person TmdbPerson { get; set; } = null!;
}
