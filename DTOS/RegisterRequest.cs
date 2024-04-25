using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class RegisterRequest
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? PasswordConfirmation { get; set; }

        [Required]
        public string? Gender { get; set; }
    }
}
