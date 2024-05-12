using Backend.Models;

namespace Backend.Pattern.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllCategories();
		Task<Category> GetCategoryById(int id);
		Task AddCategory(Category category);
		Task UpdateCategory(Category category);
		Task DeleteCategory(int id);
		Task<IEnumerable<Category>> GetPaginatedCategories(int pageNumber, int pageSize);
		Task<int> GetNumberOfCategories();
	}
}
