using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMovieCollection
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public int? TmdbCollectionId { get; set; }

    public string? CollectionName { get; set; }

    public string? CollectionPosterUrl { get; set; }

    public string? CollectionBackdropUrl { get; set; }
}
