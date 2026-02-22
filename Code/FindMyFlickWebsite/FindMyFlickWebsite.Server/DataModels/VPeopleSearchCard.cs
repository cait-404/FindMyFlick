using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VPeopleSearchCard
{
    public int? TmdbPersonId { get; set; }

    public string? PersonName { get; set; }

    public string? KnownForDepartment { get; set; }

    public string? ProfileUrl { get; set; }

    public bool? IsActor { get; set; }

    public bool? IsDirector { get; set; }

    public bool? IsWriter { get; set; }

    public bool? IsProducer { get; set; }

    public long? ActorCredits { get; set; }

    public long? DirectorCredits { get; set; }

    public long? WriterCredits { get; set; }

    public long? ProducerCredits { get; set; }

    public string? RoleLabels { get; set; }
}
