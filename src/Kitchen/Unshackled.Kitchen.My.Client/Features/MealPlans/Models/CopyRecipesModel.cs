namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Models;

public class CopyRecipesModel
{
	public DateOnly DateSelected { get; set; }
	public List<MealPlanRecipeModel> Recipes { get; set; } = [];
}
