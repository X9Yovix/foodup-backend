using Backend.Data;
using Backend.DTOS;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/orders")]
	public class OrderController : ControllerBase
	{
		private readonly IUnitOfWork _uow;
		private readonly IHttpContextAccessor _httpContextAccessor;
		//private readonly JsonSerializerOptions _jsonOptions;

		public OrderController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
		{
			_uow = uow;
			_httpContextAccessor = httpContextAccessor;
			/*
			_jsonOptions = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.Preserve,
				IgnoreNullValues = true,
				PropertyNamingPolicy = null
			};
			*/
		}

		private int GetUserIdFromToken()
		{
			return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOrders()
		{
			int userId = GetUserIdFromToken();
			var orders = await _uow.OrderRepository.GetAllOrders(userId);

			var jsonOrders = orders.Select(order => new
			{
				order.Id,
				order.OrderDate,
				order.UserId,
				OrderItems = order.OrderItems.Select(item => new
				{
					item.Id,
					item.ProductId,
					item.Quantity
				}).ToList()
			}).ToList();
			return Ok(new { orders = jsonOrders });
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrder(int id)
		{
			int userId = GetUserIdFromToken();
			var order = await _uow.OrderRepository.GetOrderById(userId, id);
			if (order == null)
			{
				return BadRequest(new { Status = "Error", Message = "Order not found" });
			}

			var jsonOrder = new
			{
				order.Id,
				order.OrderDate,
				order.UserId,
				OrderItems = order.OrderItems.Select(item => new
				{
					item.Id,
					item.ProductId,
					item.Quantity
				}).ToList()
			};

			return Ok(jsonOrder);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateOrder(OrderCreateRequest request)
		{
			int userId = GetUserIdFromToken();
			var order = new Order
			{
				OrderDate = DateTime.Now,
				UserId = userId
			};
			foreach (var item in request.OrderItems)
			{
				var orderItem = new OrderItem
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
				};
				order.OrderItems.Add(orderItem);
			}
			await _uow.OrderRepository.AddOrder(order);
			await _uow.SaveChangesAsync();
			var response = new OrderCreateResponse
			{
				Message = "Order created",
			};
			return Ok(response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			int userId = GetUserIdFromToken();
			var order = await _uow.OrderRepository.GetOrderById(userId, id);
			if (order == null)
			{
				return BadRequest(new { Status = "Error", Message = "Order not found" });
			}
			if (order.UserId != userId)
			{
				return Forbid();
			}
			await _uow.OrderRepository.DeleteOrder(userId, id);
			await _uow.SaveChangesAsync();
			var response = new OrderDeleteResponse
			{
				Message = "Order deleted",
			};
			return Ok(response);
		}

		[Authorize(Roles = "admin")]
		[HttpGet("dashboard/count")]
		public async Task<IActionResult> GetNumberOfOrders()
		{
			var count = await _uow.OrderRepository.GetNumberOfOrders();
			return Ok(new { count = count });
		}
	}
}
