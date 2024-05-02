using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Middleware
{
	public class AdminAccessMiddleware
	{
		private readonly RequestDelegate _next;

		public AdminAccessMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
				if (roleClaim != null && roleClaim.Value == "admin")
				{
					await _next(context);
					return;
				}
			}
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
		}
	}
}
