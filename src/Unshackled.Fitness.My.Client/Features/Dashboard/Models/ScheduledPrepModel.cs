namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class ScheduledPrepModel
{
	public List<MealPrepPlanRecipeModel> Recipes { get; set; } = [];
	public List<SlotModel> Slots { get; set; } = [];
}
