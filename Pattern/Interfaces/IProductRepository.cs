﻿using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pattern.Interfaces
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetAllProducts();
		Task<Product> GetProductById(int id);
		Task AddProduct(Product product);
		Task UpdateProduct(Product product);
		Task DeleteProduct(int id);
		Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);
		Task<IEnumerable<Product>> GetProductsByIds(List<int> productIds);
		Task<int> GetNumberOfProducts();
	}
}
