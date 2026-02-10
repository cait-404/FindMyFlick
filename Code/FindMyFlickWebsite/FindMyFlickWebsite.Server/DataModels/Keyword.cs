using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class Keyword
{
    public int TmdbKeywordId { get; set; }

    public string KeywordName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MovieKeyword> MovieKeywords { get; set; } = new List<MovieKeyword>();
}
