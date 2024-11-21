using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.ProductCategories.Actions;

public abstract class BaseCategoryHandler : BaseHandler
{
	public BaseCategoryHandler(HttpClient httpClient) : base(httpClient, "product-categories") { }
}
