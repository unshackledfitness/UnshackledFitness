using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;

public class SearchRecipeModel : SearchModel
{
	public string? Title { get; set; }
	public List<string> Keys { get; set; } = [];
}
