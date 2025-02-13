using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public abstract class BaseMealPlanHandler : BaseHandler
{
	public BaseMealPlanHandler(HttpClient httpClient) : base(httpClient, "meal-plans") { }
}
