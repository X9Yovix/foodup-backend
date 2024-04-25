using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class LoginRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
