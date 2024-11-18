using System.Text.Json.Serialization;

namespace Unshackled.Food.Core.Models.ShoppingLists;

public class AddToListModel
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
	public bool IsUnitMismatch { get; set; } = false;

	[JsonIgnore]
	public int TotalQuantity => Quantity + QuantityInList;
}
