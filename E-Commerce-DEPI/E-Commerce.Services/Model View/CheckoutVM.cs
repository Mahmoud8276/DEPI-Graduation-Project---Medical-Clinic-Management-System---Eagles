using System.ComponentModel.DataAnnotations;
using E_Commerce.Data.DataOrEntity;
// Make sure E-Commerce.Services has a project reference to E-Commerce MVC.PL for CartItem
// using E_Commerce_MVC.PL.Models;

namespace E_Commerce.Services.Model_View
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select a payment method")]
        public string PaymentMethod { get; set; }

        public bool SaveInfo { get; set; }
        
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}