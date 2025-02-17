namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public abstract class BaseHouseholdHandler : BaseHandler
{
	public BaseHouseholdHandler(HttpClient httpClient) : base(httpClient, "households") { }
}
