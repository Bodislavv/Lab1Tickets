using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketsDomain.Model
{
    public partial class Venue : Entity
    {
        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(50, ErrorMessage = "Venue name must not exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Venue name must contain only letters and numbers.")]
        [Display(Name = "Venue")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int? Capacity { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location must not exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Location must contain only letters.")]
        public string? Location { get; set; }

        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
