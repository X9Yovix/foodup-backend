using Backend.Data;
using Backend.Models;
using Backend.Pattern.Interfaces;
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

		public async Task AddCategoriesToProduct(int productId, List<int> categoryIds)
		{
			var product = await _dataContext.Products.FirstAsync(p => p.Id == productId);
			if (categoryIds != null)
			{
				var categories = new List<Category>();
				foreach (var categoryId in categoryIds)
				{
					var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
					if (category != null)
					{
						categories.Add(category);
					}
				}
				product.Categories = categories;
			}
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



	}
}
