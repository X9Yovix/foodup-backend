using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
    public class CategoryCreateRequest
	{
        [Required]
        public string? Name { get; set; }
    }
}
