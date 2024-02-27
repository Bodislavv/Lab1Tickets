using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Game: Entity
{

    public byte[]? Date { get; set; }

    public TimeOnly? Duration { get; set; }

    public int? TeamA { get; set; }

    public int? VenueId { get; set; }

    public int? TournamentId { get; set; }

    public int? TeamB { get; set; }

    public virtual Team? TeamANavigation { get; set; }

    public virtual Team? TeamBNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Tournament? Tournament { get; set; }

    public virtual Venue? Venue { get; set; }
}
