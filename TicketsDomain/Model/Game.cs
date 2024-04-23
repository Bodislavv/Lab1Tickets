using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketsDomain.Model;

namespace TicketsDomain.Model
{
    public class Game : Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "TeamA is required.")]
        public int? TeamA { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        public int? VenueId { get; set; }

        [Required(ErrorMessage = "Tournament is required.")]
        public int? TournamentId { get; set; }

        [Required(ErrorMessage = "TeamB is required.")]
        public int? TeamB { get; set; }


        public virtual Team? TeamANavigation { get; set; }

        public virtual Team? TeamBNavigation { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public virtual Tournament? Tournament { get; set; }
        public virtual Venue? Venue { get; set; }

    }
}
