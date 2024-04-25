using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class ResendOTPRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}
