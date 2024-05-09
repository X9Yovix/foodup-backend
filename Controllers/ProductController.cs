using Backend.Data;
using Backend.DTOS;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/products")]
	public class ProductController : ControllerBase
	{
		private readonly IUnitOfWork _uow;

		public ProductController(IUnitOfWork uow)
		{
			_uow = uow;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var products = await _uow.ProductRepository.GetAllProducts();
			return Ok(new { products = products });
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(int id)
		{
			var product = await _uow.ProductRepository.GetProductById(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> AddProduct(ProductCreateRequest productRequest)
		{
			var product = new Product
			{
				Name = productRequest.Name,
				Description = productRequest.Description,
				Price = productRequest.Price,
				CategoryId = productRequest.CategoryId
			};

			if (productRequest.Image != null && productRequest.Image.Length > 0)
			{
				var imagePath = $"Uploads/{Guid.NewGuid().ToString()}_{productRequest.Image.FileName}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await productRequest.Image.CopyToAsync(stream);
				}

				product.Image = imagePath;
			}

			await _uow.ProductRepository.AddProduct(product);
			await _uow.SaveChangesAsync();

			var response = new CategoryCreateResponse
			{
				Message = "Product created",
			};
			return Ok(response);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> UpdateProduct(int id, ProductUpdateRequest productRequest)
		{
			var existingProduct = await _uow.ProductRepository.GetProductById(id);
			if (existingProduct == null)
			{
				return NotFound();
			}

			existingProduct.Name = productRequest.Name;
			existingProduct.Description = productRequest.Description;
			existingProduct.Price = productRequest.Price;
			existingProduct.CategoryId = productRequest.CategoryId;

			if (productRequest.Image != null && productRequest.Image.Length > 0)
			{
				var imagePath = $"uploads/{Guid.NewGuid().ToString()}_{productRequest.Image.FileName}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await productRequest.Image.CopyToAsync(stream);
				}

				if (!string.IsNullOrEmpty(existingProduct.Image))
				{
					var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), existingProduct.Image);
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				existingProduct.Image = imagePath;
			}

			await _uow.ProductRepository.UpdateProduct(existingProduct);
			await _uow.SaveChangesAsync();

			var response = new CategoryUpdateResponse
			{
				Message = "Product updated",
			};
			return Ok(response);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _uow.ProductRepository.GetProductById(id);
			if (product == null)
			{
				return BadRequest(new { Status = "Error", Message = "Product not found" });
			}
			await _uow.ProductRepository.DeleteProduct(id);
			await _uow.SaveChangesAsync();
			var response = new ProductDeleteResponse
			{
				Message = "Product deleted",
			};
			return Ok(response);
		}
	}
}