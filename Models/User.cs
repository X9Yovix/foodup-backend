using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName ="nvarchar(50)")]
        public string? FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? Role { get; set; }
    }
}
