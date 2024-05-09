using Backend.Data;
using Backend.DTOS;
using Backend.Middleware;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/categories")]
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _uow;

		public CategoryController(IUnitOfWork uow)
		{
			_uow = uow;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories(int pageNumber = 1, int pageSize = 12)
		{
			var categories = await _uow.CategoryRepository.GetPaginatedCategories(pageNumber, pageSize);
			return Ok(new { categories = categories });
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

		[Authorize(Roles = "admin")]
        [HttpPost]
		public async Task<IActionResult> AddCategory(CategoryCreateRequest request)
		{
			var category = new Category()
			{
				Name = request.Name,
			};

			if (request.Image != null && request.Image.Length > 0)
			{
				var imagePath = $"Uploads/{Guid.NewGuid().ToString()}_{request.Image.FileName}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await request.Image.CopyToAsync(stream);
				}

				category.Image = imagePath;
			}

			await _uow.CategoryRepository.AddCategory(category);
			await _uow.SaveChangesAsync();
			var response = new CategoryCreateResponse
			{
				Message = "Category created",
			};
			return Ok(response);
		}

		[Authorize(Roles = "admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateRequest request)
		{
			var category = await _uow.CategoryRepository.GetCategoryById(id);
			if (category == null)
			{
				return BadRequest(new { Status = "Error", Message = "Category not found" });
			}

			category.Name = request.Name;

			if (request.Image != null && request.Image.Length > 0)
			{
				var imagePath = $"Uploads/{Guid.NewGuid().ToString()}_{request.Image.FileName}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await request.Image.CopyToAsync(stream);
				}

				if (!string.IsNullOrEmpty(category.Image))
				{
					var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), category.Image);
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				category.Image = imagePath;
			}

			await _uow.CategoryRepository.UpdateCategory(category);
			await _uow.SaveChangesAsync();
			var response = new CategoryUpdateResponse
			{
				Message = "Category updated",
			};
			return Ok(response);
		}

		[Authorize(Roles = "admin")]
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
