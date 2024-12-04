using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Kitchen.My.Client.Features.ProductBundles.Actions;

public abstract class BaseProductBundleHandler : BaseHandler
{
	public BaseProductBundleHandler(HttpClient httpClient) : base(httpClient, "product-bundles") { }
}
