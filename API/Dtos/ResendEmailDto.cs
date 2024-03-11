using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResendEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ClientURI { get; set; } = string.Empty;
    }
}
