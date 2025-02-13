namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

public class AddProductsModel
{
	public string ProductBundleSid { get; set; } = string.Empty;
	public Dictionary<string, int> Products { get; set; } = new();
}
