using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Server.Features;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ActiveMemberRequiredAttribute : TypeFilterAttribute
{
	public ActiveMemberRequiredAttribute() : base(typeof(ActiveMemberRequiredFilter)) { }

	private class ActiveMemberRequiredFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var member = context.HttpContext.Items[Globals.MiddlewareItemKeys.Member] as IMember;

			if (member == null || !member.IsActive)
			{
				context.Result = new NotFoundResult();
				return;
			}
			await next();
		}
	}
}