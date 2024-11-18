using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ProductBundles.Models;

public class SearchProductsModel : SearchModel
{
	public string ProductBundleSid { get; set; } = string.Empty;
	public string? Title { get; set; }
}
