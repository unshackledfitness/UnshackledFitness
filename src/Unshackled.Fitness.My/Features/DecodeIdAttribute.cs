using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Studio.Core.Server.Features;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DecodeIdAttribute : TypeFilterAttribute
{
	public DecodeIdAttribute() : base(typeof(DecodeIdFilter)) { }

	private class DecodeIdFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var sid = context.RouteData.Values["sid"] as string;
			if (string.IsNullOrEmpty(sid))
			{
				context.Result = new NotFoundResult();
				return;
			}

			long id = sid.DecodeLong();
			context.ActionArguments.Add("id", id);
			await next();
		}
	}
}