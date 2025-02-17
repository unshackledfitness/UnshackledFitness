using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

public class ProductBundleModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;

	public List<FormProductModel> Products { get; set; } = new();
}
