using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.Dashboard.Actions;

public abstract class BaseDashboardHandler : BaseHandler
{
	public BaseDashboardHandler(HttpClient httpClient) : base(httpClient, "dashboard") { }
}
