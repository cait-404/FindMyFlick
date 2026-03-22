using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels
{
    public partial class PlotTag
    {
        public PlotTag()
        {
            MoviePlotTags = new HashSet<MoviePlotTag>();
        }

        public int PlotTagId { get; set; }
        public string TagText { get; set; } = null!;
        public string TagTextNorm { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MoviePlotTag> MoviePlotTags { get; set; }
    }

    public partial class MoviePlotTag
    {
        public MoviePlotTag()
        {
            MoviePlotTagVotes = new HashSet<MoviePlotTagVote>();
        }

        // Composite key: ImdbId + PlotTagId
        public string ImdbId { get; set; } = null!;
        public int PlotTagId { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedByUserId { get; set; }    // user id represented as string per your project
        public string Status { get; set; } = "approved";

        public virtual PlotTag PlotTag { get; set; } = null!;
        public virtual Movie? Movie { get; set; }
        public virtual ICollection<MoviePlotTagVote> MoviePlotTagVotes { get; set; }
    }

    public partial class MoviePlotTagVote
    {
        // Composite key: ImdbId + PlotTagId + UserId
        public string ImdbId { get; set; } = null!;
        public int PlotTagId { get; set; }
        public string UserId { get; set; } = null!;     // string user id
        public short Vote { get; set; }                 // -1 or 1
        public DateTime CreatedAt { get; set; }

        public virtual MoviePlotTag MoviePlotTag { get; set; } = null!;
    }
}