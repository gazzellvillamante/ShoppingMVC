using System.Collections.Generic;
using ShoppingMVC.Models; // Make sure this is present

namespace ShoppingMVC.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public ShippingDetails ShippingDetails { get; set; }
        public List<CartDetail> CartDetails { get; set; } 
        public decimal OrderTotal { get; set; }
    }
}