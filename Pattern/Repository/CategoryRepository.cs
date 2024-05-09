using Backend.Data;
using Backend.Models;
using Backend.Pattern.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pattern.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext _dataContext;

		public CategoryRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return await _dataContext.Categories.ToListAsync();
		}

		public async Task<Category> GetCategoryById(int id)
		{
			return await _dataContext.Categories.FindAsync(id);
		}

		public async Task AddCategory(Category category)
		{
			await _dataContext.Categories.AddAsync(category);
		}

		public async Task UpdateCategory(Category category)
		{
			var existingCategory = await _dataContext.Categories.FindAsync(category.Id);
			if (existingCategory == null)
			{
				throw new ArgumentException("Category not found");
			}

			_dataContext.Entry(existingCategory).CurrentValues.SetValues(category);
			_dataContext.Entry(existingCategory).Property(x => x.Name).IsModified = true;
		}

		public async Task DeleteCategory(int id)
		{
			var category = await _dataContext.Categories.FindAsync(id);
			_dataContext.Categories.Remove(category);
		}
		public async Task<IEnumerable<Category>> GetPaginatedCategories(int pageNumber, int pageSize)
		{
			return await _dataContext.Categories
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}
	}
}
