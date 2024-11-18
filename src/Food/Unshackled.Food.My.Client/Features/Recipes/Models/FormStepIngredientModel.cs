namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class FormStepIngredientModel
{
	public string RecipeStepSid { get; set; } = string.Empty;
	public string RecipeIngredientSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool Checked { get; set; }
}
