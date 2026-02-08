using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieCollectionsAgg
{
    public int? TmdbCollectionId { get; set; }

    public string? CollectionName { get; set; }

    public string? CollectionPosterUrl { get; set; }

    public string? CollectionBackdropUrl { get; set; }

    public long? MovieCount { get; set; }

    public string? ImdbIds { get; set; }

    public string? Titles { get; set; }
}
