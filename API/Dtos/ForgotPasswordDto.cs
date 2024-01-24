using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string? ClientURI { get; set; }

    }
}
