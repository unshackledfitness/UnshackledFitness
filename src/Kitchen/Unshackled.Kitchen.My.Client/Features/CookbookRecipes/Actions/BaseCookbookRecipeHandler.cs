using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Actions;

public abstract class BaseCookbookRecipeHandler : BaseHandler
{
	public BaseCookbookRecipeHandler(HttpClient httpClient) : base(httpClient, "cookbook-recipes") { }
}
