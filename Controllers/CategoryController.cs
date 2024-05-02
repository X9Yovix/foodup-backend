using Backend.Data;
using Backend.DTOS;
using Backend.Middleware;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Authorize(Roles = "admin")]
	[Route("api/categories")]
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _uow;

		public CategoryController(IUnitOfWork uow)
		{
			_uow = uow;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var categories = await _uow.CategoryRepository.GetAllCategories();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var category = await _uow.CategoryRepository.GetCategoryById(id);
			if (category == null)
			{
				return BadRequest(new { Status = "Error", Message = "Category not found" });
			}
			return Ok(category);
		}

		[HttpPost]
		public async Task<IActionResult> AddCategory(CategoryCreateRequest request)
		{
			var category = new Category()
			{
				Name = request.Name,
			};
			await _uow.CategoryRepository.AddCategory(category);
			await _uow.SaveChangesAsync();
			var response = new CategoryCreateResponse
			{
				Message = "Category created",
			};
			return Ok(response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateRequest request)
		{
			var category = new Category()
			{
				Id = id,
				Name = request.Name,
			};
			await _uow.CategoryRepository.UpdateCategory(category);
			await _uow.SaveChangesAsync();
			var response = new CategoryUpdateResponse
			{
				Message = "Category updated",
			};
			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await _uow.CategoryRepository.GetCategoryById(id);
			if (category == null)
			{
				return BadRequest(new { Status = "Error", Message = "Category not found" });
			}
			await _uow.CategoryRepository.DeleteCategory(id);
			await _uow.SaveChangesAsync();
			var response = new CategoryDeleteResponse
			{
				Message = "Category deleted",
			};
			return Ok(response);
		}
	}
}
