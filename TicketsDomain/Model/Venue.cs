using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Venue: Entity
{

    public string? VenueName { get; set; }

    public int? Capacity { get; set; }

    public string? Location { get; set; }

    public int? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
