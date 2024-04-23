using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketsDomain.Model;

namespace TicketsDomain.Model
{
    public partial class Country : Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only letters.")]
        [Display(Name = "Country")]
        public string? Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
    }

}
