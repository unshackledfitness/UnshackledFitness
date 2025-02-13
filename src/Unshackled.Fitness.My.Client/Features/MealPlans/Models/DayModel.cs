namespace Unshackled.Fitness.My.Client.Features.MealPlans.Models;

public class DayModel
{
	public DateOnly Date { get; set; }
	public int DayOfWeek { get; set; }
	public bool IsChecked { get; set; }

	public List<MealPlanRecipeModel> Recipes { get; set; } = [];
}
