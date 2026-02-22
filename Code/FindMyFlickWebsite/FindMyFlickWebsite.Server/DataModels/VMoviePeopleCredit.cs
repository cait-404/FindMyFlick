using System;
using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class VMoviePeopleCredit
{
    public string? ImdbId { get; set; }

    public int? TmdbId { get; set; }

    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public int? TmdbPersonId { get; set; }

    public string? PersonName { get; set; }

    public string? CreditType { get; set; }

    public string? Department { get; set; }

    public string? Job { get; set; }

    public string? CharacterName { get; set; }

    public int? CastOrder { get; set; }

    public string? TmdbCreditId { get; set; }
}
