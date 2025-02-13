namespace Unshackled.Fitness.My.Client.Features.MealPlans.Models;

public class AddPlanRecipeModel
{
	public DateOnly DatePlanned { get; set; }
	public string MealDefinitionSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public decimal Scale { get; set; }
}
