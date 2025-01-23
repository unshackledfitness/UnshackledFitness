using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients.Models;

public class SearchRecipeModel : SearchModel
{
	public string IngredientKey { get; set; } = string.Empty;
}
