using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unshackled.Kitchen.My.Middleware;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Kitchen.My.Features;

[Authorize]
public abstract class BaseController : ControllerBase
{
	private IMediator? mediator;
	private SiteConfiguration? siteConfiguration;

	protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
	protected SiteConfiguration SiteConfig => siteConfiguration ??= HttpContext.RequestServices.GetRequiredService<SiteConfiguration>();

	public ServerMember Member => HttpContext.Items.ContainsKey(Globals.MiddlewareItemKeys.Member)
		? (ServerMember)HttpContext.Items[Globals.MiddlewareItemKeys.Member]!
		: new();
}
