using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		public DateTime OrderDate { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}
}
