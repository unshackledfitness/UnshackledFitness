namespace Unshackled.Food.Core.Models;

public class RecipeAmountListModel
{
	public decimal Amount { get; set; }
	public string UnitLabel { get; set; } = string.Empty;
	public decimal PortionUsed { get; set; }
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
}
