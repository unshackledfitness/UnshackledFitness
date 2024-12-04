namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class RecipeStepIngredientModel
{
	public string RecipeStepSid { get; set; } = string.Empty;
	public string RecipeIngredientSid { get; set; } = string.Empty;
	public decimal Amount { get; set; }
	public string AmountLabel { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? PrepNote { get; set; }
	public int SortOrder { get; set; }
}
