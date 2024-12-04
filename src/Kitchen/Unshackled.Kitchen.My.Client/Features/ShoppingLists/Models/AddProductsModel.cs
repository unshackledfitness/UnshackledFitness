namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class AddProductsModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public Dictionary<string, int> Products { get; set; } = new();
}
