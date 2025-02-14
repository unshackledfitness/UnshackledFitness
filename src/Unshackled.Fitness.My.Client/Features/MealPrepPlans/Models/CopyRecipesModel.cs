namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

public class CopyRecipesModel
{
	public DateOnly DateSelected { get; set; }
	public List<MealPrepPlanRecipeModel> Recipes { get; set; } = [];
}
