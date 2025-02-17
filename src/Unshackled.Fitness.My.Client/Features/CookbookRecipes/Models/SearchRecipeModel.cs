using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

public class SearchRecipeModel : SearchModel
{
	public string? Title { get; set; }
	public List<string> Keys { get; set; } = [];
}
