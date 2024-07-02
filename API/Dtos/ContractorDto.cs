using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ContractorDto
    {

        [Required]
        public string ContractorId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string DisplayName { get; set; }

        [Required]
        public string CustomerId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public SubscriptionDto Subscription { get; set; }
        public ResponseBusinessInfoDto BusinessInfo { get; set; }
        public ResponsePersonalInfoDto PersonalInfo { get; set; }

    }
}