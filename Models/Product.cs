using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Column(TypeName = "nvarchar(50)")]
		public string Name { get; set; }

		[Column(TypeName = "nvarchar(255)")]
		public string Description { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Column(TypeName = "nvarchar(255)")]
		public string Image { get; set; }

		public int CategoryId { get; set; }

		public Category Category { get; set; }
	}
}
