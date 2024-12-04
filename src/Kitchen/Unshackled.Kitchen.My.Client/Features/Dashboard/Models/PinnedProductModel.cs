namespace Unshackled.Kitchen.My.Client.Features.Dashboard.Models;

public class PinnedProductModel
{
	public string Sid { get; set; } = string.Empty;
	public string? ProductCategorySid { get; set; }
	public string? Category { get; set; }
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}

