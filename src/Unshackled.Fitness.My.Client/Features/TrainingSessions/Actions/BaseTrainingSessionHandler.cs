namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public abstract class BaseTrainingSessionHandler : BaseHandler
{
	public BaseTrainingSessionHandler(HttpClient httpClient) : base(httpClient, "training-sessions") { }
}
