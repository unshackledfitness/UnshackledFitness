using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.ActivityTypes.Actions;

public abstract class BaseActivityTypeHandler : BaseHandler
{
	public BaseActivityTypeHandler(HttpClient httpClient) : base(httpClient, "activity-types") { }
}
