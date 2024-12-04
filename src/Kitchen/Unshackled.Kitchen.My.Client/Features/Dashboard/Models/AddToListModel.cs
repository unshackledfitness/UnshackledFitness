namespace Unshackled.Kitchen.My.Client.Features.Dashboard.Models;

public class AddToListModel
{
	public string ProductSid { get; set; } = string.Empty;
	public string ListSid { get; set; } = string.Empty;
	public int Quantity { get; set; } = 1;
}
