using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class CategoryUpdateRequest
	{
		[Required]
		public string? Name { get; set; }

		public IFormFile Image { get; set; }
	}
}
