using Backend.Data;
using Backend.Models;
using Backend.Pattern.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pattern.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DataContext _dataContext;

		public OrderRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IEnumerable<Order>> GetAllOrders(int userId)
		{
			return await _dataContext.Orders
				.Where(o => o.UserId == userId)
				.Include(o => o.OrderItems)
				.ToListAsync();
		}

		public async Task<Order> GetOrderById(int userId, int id)
		{
			return await _dataContext.Orders
				.Include(o => o.OrderItems) 
				.FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);
		}

		public async Task AddOrder(Order order)
		{
			await _dataContext.Orders.AddAsync(order);
			await _dataContext.SaveChangesAsync();
		}

		public async Task DeleteOrder(int userId, int id)
		{
			var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);
			if (order != null)
			{
				_dataContext.Orders.Remove(order);
				await _dataContext.SaveChangesAsync();
			}
		}
	}
}
