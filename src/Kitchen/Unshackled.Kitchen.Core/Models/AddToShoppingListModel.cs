using System.Text.Json;
using System.Text.Json.Serialization;
using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Models;

public class AddToShoppingListModel
{
	public string ListSid { get; set; } = string.Empty;
	public string IngredientKey { get; set; } = string.Empty;
	public string IngredientTitle { get; set; } = string.Empty;
	public decimal IngredientAmount { get; set; }
	public string IngredientAmountUnitLabel { get; set; } = string.Empty;
	public string ProductSid { get; set; } = string.Empty;
	public string ProductTitle { get; set; } = string.Empty;
	public decimal ContainerSizeAmount { get; set; }
	public string ContainerSizeUnitLabel { get; set; } = string.Empty;
	public decimal RequiredAmount { get; set; }
	public string RequiredAmountLabel { get; set; } = string.Empty;
	public int Quantity { get; set; } = 1;
	public int QuantityInList { get; set; }
	public decimal PortionUsed { get; set; }
	public bool IsUnitMismatch { get; set; } = false;

	public List<RecipeAmountListModel> RecipeAmounts { get; set; } = [];

	[JsonIgnore]
	public int TotalQuantity => Quantity + QuantityInList;
}
