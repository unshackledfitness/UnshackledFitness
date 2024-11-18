using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public abstract class BaseRecipeHandler : BaseHandler
{
	public BaseRecipeHandler(HttpClient httpClient) : base(httpClient, "recipes") { }
}
