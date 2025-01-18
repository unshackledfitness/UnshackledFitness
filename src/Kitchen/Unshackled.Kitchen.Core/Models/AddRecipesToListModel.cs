namespace Unshackled.Kitchen.Core.Models;

public class AddRecipesToListModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public Dictionary<string, string> Recipes { get; set; } = [];
	public List<AddToShoppingListModel> List { get; set; } = [];
}
