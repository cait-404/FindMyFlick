using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class Collection
{
    public int TmdbCollectionId { get; set; }

    public string CollectionName { get; set; } = null!;

    public string? PosterUrl { get; set; }

    public string? BackdropUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MovieCollection> MovieCollections { get; set; } = new List<MovieCollection>();
}
