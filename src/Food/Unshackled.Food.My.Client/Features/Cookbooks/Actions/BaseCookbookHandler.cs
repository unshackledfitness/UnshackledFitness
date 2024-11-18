using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Actions;

public abstract class BaseCookbookHandler : BaseHandler
{
	public BaseCookbookHandler(HttpClient httpClient) : base(httpClient, "cookbooks") { }
}
