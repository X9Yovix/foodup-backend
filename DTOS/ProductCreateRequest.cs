using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
	public class ProductCreateRequest
	{
		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		public string Image { get; set; }

		[Required]
		public List<int> CategoryIds { get; set; }
	}
}
