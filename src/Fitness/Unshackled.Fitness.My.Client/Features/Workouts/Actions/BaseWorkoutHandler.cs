using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public abstract class BaseWorkoutHandler : BaseHandler
{
	public BaseWorkoutHandler(HttpClient httpClient) : base(httpClient, "workouts") { }
}
