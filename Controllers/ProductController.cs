using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
