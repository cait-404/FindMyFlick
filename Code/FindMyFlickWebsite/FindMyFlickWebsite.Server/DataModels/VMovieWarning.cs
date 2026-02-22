using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieWarning
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public int? DtddTopicId { get; set; }

    public string? TopicName { get; set; }

    public string? Answer { get; set; }

    public bool? IsSpoiler { get; set; }

    public string? WarningComment { get; set; }

    public string? Source { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
