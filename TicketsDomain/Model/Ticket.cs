using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Ticket: Entity
{

    public string? SerialNumber { get; set; }

    public string? Seat { get; set; }

    public decimal? Price { get; set; }

    public int? CustomnerId { get; set; }

    public int? GameId { get; set; }

    public virtual Customer? Customner { get; set; }

    public virtual Game? Game { get; set; }
}
