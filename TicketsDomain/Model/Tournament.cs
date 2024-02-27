using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Tournament: Entity
{

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
