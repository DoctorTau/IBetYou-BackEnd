using System.ComponentModel.DataAnnotations;

namespace IBUAPI.Models.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}