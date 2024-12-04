namespace Unshackled.Kitchen.My.Client.Features.Ingredients.Models;

public class IngredientListModel
{
	public string IngredientKey { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int RecipeCount { get; set; }
	public int SubstitutionsCount { get; set; }
}
