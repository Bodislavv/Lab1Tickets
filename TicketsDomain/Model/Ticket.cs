using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketsDomain.Model
{
    public partial class Ticket : Entity
    {
        [Required(ErrorMessage = "Serial Number is required.")]
        [Display(Name = "Serial number")]
        public string? SerialNumber { get; set; }

        [Required(ErrorMessage = "Seat is required.")]
        public string? Seat { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
        [DisplayFormat(DataFormatString = "{0:$0.00}")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Customer is required.")]
        public int? CustomnerId { get; set; }

        public int? GameId { get; set; }

        public virtual Customer? Customner { get; set; }

        public virtual Game? Game { get; set; }
    }
}
