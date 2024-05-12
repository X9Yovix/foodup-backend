using Backend.Data;
using Backend.Models;
using Backend.Pattern.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pattern.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext _dataContext;

		public ProductRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IEnumerable<Product>> GetAllProducts()
		{
			return await _dataContext.Products.ToListAsync();
		}

		public async Task<Product> GetProductById(int id)
		{
			return await _dataContext.Products.FindAsync(id);
		}

		public async Task AddProduct(Product product)
		{
			await _dataContext.Products.AddAsync(product);
		}

		public async Task UpdateProduct(Product product)
		{
			_dataContext.Entry(product).State = EntityState.Modified;
		}

		public async Task DeleteProduct(int id)
		{
			var product = await _dataContext.Products.FindAsync(id);
			_dataContext.Products.Remove(product);
		}
		public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
		{
			return await _dataContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByIds(List<int> productIds)
		{
			string productIdsString = string.Join(",", productIds);
			var query = $"SELECT * FROM Products WHERE Id IN ({productIdsString})";

			return await _dataContext.Products.FromSqlRaw(query).ToListAsync();
			/*
			return await _dataContext.Products
				.Where(p => productIds.Contains(p.Id))
				.ToListAsync();
			*/
		}

		public async Task<int> GetNumberOfProducts()
		{
			return await _dataContext.Products.CountAsync();
		}

	}
}
