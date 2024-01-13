using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
        public Address Address { get; set; }
    }
}