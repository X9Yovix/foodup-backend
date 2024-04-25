using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class VerifyOTPRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Otp { get; set; }
    }
}
