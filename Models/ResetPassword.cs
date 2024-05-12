using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class ResetPassword
	{
		[Key]
		public int Id { get; set; }
		public string OTP { get; set; }
		public DateTime ExpirationTime { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }
	}
}
