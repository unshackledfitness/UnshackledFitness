namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

public class UpdateQuantityModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public string ProductSid {  get; set; } = string.Empty;
	public int Quantity { get; set; }
}
