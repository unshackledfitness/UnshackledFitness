namespace Unshackled.Kitchen.My.Client.Features.Products.Models;

public class BulkCategoryModel
{
	public string CategorySid { get; set; } = string.Empty;
	public List<string> ProductSids { get; set; } = new();
}
