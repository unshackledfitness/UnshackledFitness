using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class RecipeAmountListModel
{
	public string ProductSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public string IngredientKey { get; set; } = string.Empty;
	public decimal IngredientAmount { get; set; }
	public string IngredientAmountUnitLabel { get; set; } = string.Empty;
	public UnitTypes IngredientAmountUnitType { get; set; }
	public decimal PortionUsed { get; set; }
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public UnitTypes ServingSizeUnitType { get; set; }
	public bool IsUnitMismatch { get; set; }
}
