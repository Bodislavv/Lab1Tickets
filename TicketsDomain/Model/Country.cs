using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Country: Entity
{

    public string? Name { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
}
