using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public SubscriptionDto Subscription { get; set; }
    }
}