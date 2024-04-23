using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketsDomain.Model
{
    public partial class Customer : Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only letters.")]
        [Display(Name = "Customers name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "BirthDate is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth date")]
        public DateOnly? BirthDate { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
