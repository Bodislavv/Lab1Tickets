using System;
using System.Collections.Generic;

namespace TicketsDomain.Model;

public partial class Customer : Entity
{

    public string? Name { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Sex { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
