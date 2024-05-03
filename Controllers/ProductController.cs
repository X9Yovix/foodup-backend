using Backend.Data;
using Backend.DTOS;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
			return Ok(products);
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
		public async Task<IActionResult> AddProduct(ProductCreateRequest productRequest)
		{
			var product = new Product
			{
				Name = productRequest.Name,
				Description = productRequest.Description,
				Price = productRequest.Price,
				Image = productRequest.Image,
			};

			await _uow.ProductRepository.AddProduct(product);
			await _uow.SaveChangesAsync();
			/*
			foreach (var categoryId in productRequest.CategoryIds)
			{
				System.Diagnostics.Debug.WriteLine($"Category ID: {categoryId}");
			}
			System.Diagnostics.Debug.WriteLine($"product.Id: {product.Id}");
			*/
			await _uow.ProductRepository.AddCategoriesToProduct(product.Id, productRequest.CategoryIds);
			await _uow.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
		}

		[HttpPut("{id}")]
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

			await _uow.ProductRepository.UpdateProduct(existingProduct);
			await _uow.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _uow.ProductRepository.GetProductById(id);
			if (product == null)
			{
				return NotFound();
			}

			try
			{
				await _uow.ProductRepository.DeleteProduct(id);
				await _uow.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}