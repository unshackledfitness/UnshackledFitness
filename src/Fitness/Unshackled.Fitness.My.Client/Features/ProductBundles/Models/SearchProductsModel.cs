using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

public class SearchProductsModel : SearchModel
{
	public string ProductBundleSid { get; set; } = string.Empty;
	public string? Title { get; set; }
}
