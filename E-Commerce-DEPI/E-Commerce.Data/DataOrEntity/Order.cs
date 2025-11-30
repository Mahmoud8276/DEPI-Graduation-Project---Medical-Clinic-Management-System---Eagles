using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Data.DataOrEntity
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }

        public string PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Accepted, Declined

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}