using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
        public string Email { get; set; }
    }
}
