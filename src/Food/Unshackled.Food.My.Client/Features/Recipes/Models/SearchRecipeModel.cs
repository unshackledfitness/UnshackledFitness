using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class SearchRecipeModel : SearchModel
{
	public string? Title { get; set; }
	public List<string> TagSids { get; set; } = [];
}
