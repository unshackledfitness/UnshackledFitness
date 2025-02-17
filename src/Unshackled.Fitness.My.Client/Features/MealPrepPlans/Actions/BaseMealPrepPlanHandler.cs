namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public abstract class BaseMealPrepPlanHandler : BaseHandler
{
	public BaseMealPrepPlanHandler(HttpClient httpClient) : base(httpClient, "meal-prep-plans") { }
}
