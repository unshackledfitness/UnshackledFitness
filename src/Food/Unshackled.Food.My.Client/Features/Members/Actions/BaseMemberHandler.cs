using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public abstract class BaseMemberHandler : BaseHandler
{
	public BaseMemberHandler(HttpClient httpClient) : base(httpClient, "members") { }
}
