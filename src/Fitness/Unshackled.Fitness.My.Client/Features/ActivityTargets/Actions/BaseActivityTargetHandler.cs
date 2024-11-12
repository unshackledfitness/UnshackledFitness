using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets.Actions;

public abstract class BaseActivityTargetHandler : BaseHandler
{
	public BaseActivityTargetHandler(HttpClient httpClient) : base(httpClient, "activity-targets") { }
}
