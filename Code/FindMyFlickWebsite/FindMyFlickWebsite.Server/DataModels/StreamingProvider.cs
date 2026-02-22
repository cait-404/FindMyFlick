using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class StreamingProvider
{
    public int TmdbProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public string? LogoPath { get; set; }

    public int? DisplayPriority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MovieStreaming> MovieStreamings { get; set; } = new List<MovieStreaming>();
}
