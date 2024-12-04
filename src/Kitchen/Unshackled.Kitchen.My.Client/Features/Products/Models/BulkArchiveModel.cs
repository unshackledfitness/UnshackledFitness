namespace Unshackled.Kitchen.My.Client.Features.Products.Models;

public class BulkArchiveModel
{
	public bool IsArchiving { get; set; }
	public List<string> ProductSids { get; set; } = new();
}
