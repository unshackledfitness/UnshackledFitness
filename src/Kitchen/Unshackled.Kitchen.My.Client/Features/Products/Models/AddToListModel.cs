namespace Unshackled.Kitchen.My.Client.Features.Products.Models;

public class AddToListModel
{
	public List<string> ProductSids { get; set; } = [];
	public string ListSid { get; set; } = string.Empty;
	public int Quantity { get; set; } = 1;
}
