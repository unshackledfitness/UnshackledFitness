using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unshackled.Food.Core.Models;

public class AddToShoppingListModel
{
	public string ListSid { get; set; } = string.Empty;
	public string IngredientKey { get; set; } = string.Empty;
	public string IngredientTitle { get; set; } = string.Empty;
	public string ProductSid { get; set; } = string.Empty;
	public string ProductTitle { get; set; } = string.Empty;
	public decimal RequiredAmount { get; set; }
	public string RequiredAmountLabel { get; set; } = string.Empty;
	public int Quantity { get; set; } = 1;
	public int QuantityInList { get; set; }
	public decimal PortionUsed { get; set; }
	public bool IsUnitMismatch { get; set; } = false;
	public string? RecipeAmountsJson { get; set; }

	[JsonIgnore]
	private List<RecipeAmountListModel>? recipeAmts;

	[JsonIgnore]
	public List<RecipeAmountListModel> RecipeAmounts
	{
		get
		{
			if (recipeAmts == null)
			{
				if (!string.IsNullOrEmpty(RecipeAmountsJson))
				{
					recipeAmts = JsonSerializer.Deserialize<List<RecipeAmountListModel>>(RecipeAmountsJson) ?? [];
				}
				else
				{
					recipeAmts = [];
				}
			}
			return recipeAmts;
		}
	}

	[JsonIgnore]
	public int TotalQuantity => Quantity + QuantityInList;
}
