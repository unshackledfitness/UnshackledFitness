using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients.Actions;

public abstract class BaseIngredientHandler : BaseHandler
{
	public BaseIngredientHandler(HttpClient httpClient) : base(httpClient, "ingredients") { }
}
