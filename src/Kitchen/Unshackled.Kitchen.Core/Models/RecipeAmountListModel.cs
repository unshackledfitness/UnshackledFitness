namespace Unshackled.Kitchen.Core.Models;

public class RecipeAmountListModel
{
	public string ProductSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public decimal Amount { get; set; }
	public string UnitLabel { get; set; } = string.Empty;
	public decimal PortionUsed { get; set; }
}
