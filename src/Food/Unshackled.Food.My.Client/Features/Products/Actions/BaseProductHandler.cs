using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Products.Actions;

public abstract class BaseProductHandler : BaseHandler
{
	public BaseProductHandler(HttpClient httpClient) : base(httpClient, "products") { }
}
