using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketsDomain.Model
{
    public partial class Tournament : Entity
    {
        [Required(ErrorMessage = "Tournament name is required.")]
        [StringLength(50, ErrorMessage = "Tournament name must not exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Tournament name must contain only letters and numbers.")]
        [Display(Name = "Tournament")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string? Description { get; set; }

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
