using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;

public abstract class BaseActivityTemplateHandler : BaseHandler
{
	public BaseActivityTemplateHandler(HttpClient httpClient) : base(httpClient, "activity-templates") { }
}
