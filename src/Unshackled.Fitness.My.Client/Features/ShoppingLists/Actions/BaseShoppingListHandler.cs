using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public abstract class BaseShoppingListHandler : BaseHandler
{
	public BaseShoppingListHandler(HttpClient httpClient) : base(httpClient, "shopping-lists") { }
}
