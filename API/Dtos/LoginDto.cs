using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}