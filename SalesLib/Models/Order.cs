using SalesLib.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesLib.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Incorrect email format")]
        public string Email { get; set; }

        [Required(ErrorMessage ="From address is required")]
        public string FromAddress { get; set; }

        [Required(ErrorMessage ="To address is required")]
        public string ToAddress { get; set; }

        [Required(ErrorMessage ="Service type is required")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "Carried out date is required")]
        [FutureDate(ErrorMessage = "Date must be greater than the current date")]
        public DateTime CarriedOutDate { get; set; } = DateTime.Now;

        public string Note { get; set; }


    }
}
