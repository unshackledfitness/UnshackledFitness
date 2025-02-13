using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Services;

namespace Unshackled.Fitness.My.Services;

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
