namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public abstract class BaseMealPlanHandler : BaseHandler
{
	public BaseMealPlanHandler(HttpClient httpClient) : base(httpClient, "meal-plans") { }
}
