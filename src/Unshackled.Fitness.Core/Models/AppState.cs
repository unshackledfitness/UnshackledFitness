using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.Core.Models;

public class AppState : BaseAppState, IAppState
{
	public override required IMember ActiveMember { get; set; } = new Member();
}