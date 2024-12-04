namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class UpdateStepsModel
{
	public List<FormStepModel> Steps { get; set; } = new();
	public List<FormStepModel> DeletedSteps { get; set; } = new();
}
