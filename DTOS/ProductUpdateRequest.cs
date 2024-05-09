using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
	public class ProductUpdateRequest
	{
		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public IFormFile Image { get; set; }

		[Required]
		public int CategoryId { get; set; }
	}
}
