using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class MovieWarning
{
    public string ImdbId { get; set; } = null!;

    public int DtddTopicId { get; set; }

    public string Source { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? Answer { get; set; }

    public bool? IsSpoiler { get; set; }

    public string? WarningComment { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Warning DtddTopic { get; set; } = null!;

    public virtual Movie Imdb { get; set; } = null!;
}
