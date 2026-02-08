using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class Person
{
    public int TmdbPersonId { get; set; }

    public string PersonName { get; set; } = null!;

    public string? KnownForDepartment { get; set; }

    public string? ProfileUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    public virtual ICollection<MovieCrew> MovieCrews { get; set; } = new List<MovieCrew>();
}
