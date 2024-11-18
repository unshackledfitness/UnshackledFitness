namespace Unshackled.Food.My.Client.Features.Ingredients.Models;

public class IngredientModel
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;

	public List<ProductSubstitutionModel> Substitutions { get; set; } = new();
}
