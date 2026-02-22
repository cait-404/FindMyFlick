using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieWarningsAgg
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public List<int>? YesTopicIds { get; set; }

    public List<int>? NoTopicIds { get; set; }

    public List<int>? UnknownTopicIds { get; set; }

    public string? YesTopics { get; set; }

    public string? NoTopics { get; set; }

    public string? UnknownTopics { get; set; }
}
