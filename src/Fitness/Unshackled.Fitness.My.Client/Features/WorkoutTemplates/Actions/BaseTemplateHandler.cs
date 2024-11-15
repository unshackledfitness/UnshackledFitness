using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Actions;

public abstract class BaseTemplateHandler : BaseHandler
{
	public BaseTemplateHandler(HttpClient httpClient) : base(httpClient, "templates") { }
}
