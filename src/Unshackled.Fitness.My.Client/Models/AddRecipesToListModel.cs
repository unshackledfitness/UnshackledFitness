namespace Unshackled.Fitness.My.Client.Models;

public class AddRecipesToListModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public List<AddToShoppingListModel> List { get; set; } = [];
}
