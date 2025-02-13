using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public abstract class BaseStoreHandler : BaseHandler
{
	public BaseStoreHandler(HttpClient httpClient) : base(httpClient, "stores") { }
}
