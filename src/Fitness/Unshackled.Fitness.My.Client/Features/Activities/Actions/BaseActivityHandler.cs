using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public abstract class BaseActivityHandler : BaseHandler
{
	public BaseActivityHandler(HttpClient httpClient) : base(httpClient, "activities") { }
}
