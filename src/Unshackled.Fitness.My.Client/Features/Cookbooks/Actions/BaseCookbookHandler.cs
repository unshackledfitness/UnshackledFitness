namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public abstract class BaseCookbookHandler : BaseHandler
{
	public BaseCookbookHandler(HttpClient httpClient) : base(httpClient, "cookbooks") { }
}
