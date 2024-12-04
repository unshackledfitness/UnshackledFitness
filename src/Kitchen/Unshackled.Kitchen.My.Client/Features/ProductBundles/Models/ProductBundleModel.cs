using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;

public class ProductBundleModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;

	public List<FormProductModel> Products { get; set; } = new();
}
