using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketsDomain.Model
{
    public partial class Team : Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Name must contain only letters and numbers.")]
        [Display(Name = "Team")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        public virtual ICollection<Game> GameTeamANavigations { get; set; } = new List<Game>();

        public virtual ICollection<Game> GameTeamBNavigations { get; set; } = new List<Game>();
    }
}
