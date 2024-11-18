namespace Unshackled.Food.My.Client.Features.ProductBundles.Models;

public class ProductListModel
{
	public string Sid { get; set; } = string.Empty;
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Category { get; set; }
	public int Quantity { get; set; } = 1;
}
