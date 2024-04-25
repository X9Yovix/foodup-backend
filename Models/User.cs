using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string? FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Gender { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Role { get; set; }

        public string? Otp { get; set; }
        public DateTime? OtpExpirationTime { get; set; }
        public bool IsVerified { get; set; }
    }
}
