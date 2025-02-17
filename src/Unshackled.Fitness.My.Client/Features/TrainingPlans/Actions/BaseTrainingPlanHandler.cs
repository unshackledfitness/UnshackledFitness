namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public abstract class BaseTrainingPlanHandler : BaseHandler
{
	public BaseTrainingPlanHandler(HttpClient httpClient) : base(httpClient, "training-plans") { }
}
