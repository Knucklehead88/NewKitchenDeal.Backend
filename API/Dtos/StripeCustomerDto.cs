using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StripeCustomerDto
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}
