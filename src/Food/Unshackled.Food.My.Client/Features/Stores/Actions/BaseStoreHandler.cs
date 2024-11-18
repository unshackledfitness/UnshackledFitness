using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public abstract class BaseStoreHandler : BaseHandler
{
	public BaseStoreHandler(HttpClient httpClient) : base(httpClient, "stores") { }
}
