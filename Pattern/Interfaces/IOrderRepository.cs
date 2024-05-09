using Backend.Models;

namespace Backend.Pattern.Interfaces
{
	public interface IOrderRepository
	{
		Task<IEnumerable<Order>> GetAllOrders(int userId);
		Task<Order> GetOrderById(int userId, int id);
		Task AddOrder(Order order);
		Task DeleteOrder(int userId,int id);
	}
}
