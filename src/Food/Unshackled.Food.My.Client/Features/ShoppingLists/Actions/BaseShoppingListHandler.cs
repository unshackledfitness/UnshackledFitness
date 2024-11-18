using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public abstract class BaseShoppingListHandler : BaseHandler
{
	public BaseShoppingListHandler(HttpClient httpClient) : base(httpClient, "shopping-lists") { }
}
