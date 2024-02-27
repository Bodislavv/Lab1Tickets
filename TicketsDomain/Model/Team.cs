using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Team: Entity
{

    public string? Name { get; set; }

    public int? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Game> GameTeamANavigations { get; set; } = new List<Game>();

    public virtual ICollection<Game> GameTeamBNavigations { get; set; } = new List<Game>();
}
