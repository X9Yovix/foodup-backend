using System.ComponentModel.DataAnnotations;

namespace Backend.DTOS
{
	public class OrderCreateRequest
	{
		[Required]
		public List<OrderItemDTO> OrderItems { get; set; }
	}
	public class OrderItemDTO
	{
		[Required]
		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}
