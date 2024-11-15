using Microsoft.AspNetCore.Http;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Server.Services;

public class RenderStateService : IRenderStateService
{
	private readonly IHttpContextAccessor httpContextAccessor;

	public bool IsInteractive => !(httpContextAccessor.HttpContext is null || httpContextAccessor.HttpContext.Request.Path.StartsWithSegments(Globals.AccountUrlPrefix));

	public bool IsPreRender => !(httpContextAccessor.HttpContext is not null && httpContextAccessor.HttpContext.Response.HasStarted);

	public RenderStateService(IHttpContextAccessor httpContextAccessor)
	{
		this.httpContextAccessor = httpContextAccessor;
	}
}
