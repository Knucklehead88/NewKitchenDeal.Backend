using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SendToEmailDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Message { get; set; }

    }
}
