using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
	public class ApplyResetPasswordRequest
	{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
        [Required]
        public string Password { get; set; }
		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string PasswordConfirmation { get; set; }
	}
}
