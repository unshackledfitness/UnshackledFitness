using Microsoft.AspNetCore.Http;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Studio.Core.Server.Middleware;

public class AuthorizedUserMiddleware
{
	private readonly RequestDelegate next;

	public AuthorizedUserMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var path = context.Request.Path;
		// Skip paths not starting with /api
		if (!path.StartsWithSegments("/api"))
		{
			await next(context);
			return;
		}

		if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return;
		}

		string email = context.User.GetEmailClaim();
		if (string.IsNullOrEmpty(email))
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return;
		}

		await next(context);
	}
}
