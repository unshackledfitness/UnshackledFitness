namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

public class DayModel
{
	public DateOnly Date { get; set; }
	public int DayOfWeek { get; set; }
	public bool IsChecked { get; set; }

	public List<MealPrepPlanRecipeModel> Recipes { get; set; } = [];
}
