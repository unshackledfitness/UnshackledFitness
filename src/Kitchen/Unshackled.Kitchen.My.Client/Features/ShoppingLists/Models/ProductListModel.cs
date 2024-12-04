namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class ProductListModel
{
	public string Sid { get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int Quantity { get; set; } = 1;
}
