using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Data.DataOrEntity
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ShippingFirstName { get; set; }
        public string? ShippingLastName { get; set; }
        public string? ShippingStreetAddress { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingPhone { get; set; }
    }
}