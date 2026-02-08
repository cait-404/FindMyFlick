using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class Warning
{
    public int DtddTopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public string? TopicType { get; set; }

    public int? ParentDtddTopicId { get; set; }

    public short? Tier { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Warning> InverseParentDtddTopic { get; set; } = new List<Warning>();

    public virtual ICollection<MovieWarning> MovieWarnings { get; set; } = new List<MovieWarning>();

    public virtual Warning? ParentDtddTopic { get; set; }
}
