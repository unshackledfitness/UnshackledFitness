namespace Unshackled.Fitness.My.Client.Models;

public class MakeItRecipeModel
{
	public string Sid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public decimal Scale { get; set; } = 1M;
	public int SortOrder { get; set; }

	public List<MakeItRecipeIngredientGroupModel> Groups { get; set; } = [];
	public List<MakeItRecipeIngredientModel> Ingredients { get; set; } = [];
	public List<MakeItRecipeStepModel> Steps { get; set; } = [];
}
