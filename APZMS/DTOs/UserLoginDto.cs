using System.ComponentModel.DataAnnotations;

namespace APZMS.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
