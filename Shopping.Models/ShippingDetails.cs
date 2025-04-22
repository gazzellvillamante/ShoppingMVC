using System.ComponentModel.DataAnnotations;


namespace ShoppingMVC.Models
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter your full name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your shipping address")]
        public string Address { get; set; }
    }
}