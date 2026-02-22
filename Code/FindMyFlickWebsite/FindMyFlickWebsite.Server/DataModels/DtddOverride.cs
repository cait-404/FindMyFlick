using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class DtddOverride
{
    public string ImdbId { get; set; } = null!;

    public int DtddMediaId { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Movie Imdb { get; set; } = null!;
}
