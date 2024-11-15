using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Programs.Actions;

public abstract class BaseProgramHandler : BaseHandler
{
	public BaseProgramHandler(HttpClient httpClient) : base(httpClient, "programs") { }
}
