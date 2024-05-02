using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Column(TypeName = "nvarchar(50)")]
		public string? Name { get; set; }
	}
}
