namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class ToggleListItemModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public string ProductSid { get; set; } = string.Empty;
	public bool ToggleValue { get; set; }
}
