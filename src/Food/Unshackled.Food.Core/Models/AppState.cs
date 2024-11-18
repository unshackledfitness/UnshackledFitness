using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.Core.Models;

public class AppState : BaseAppState, IAppState
{
	public override required IMember ActiveMember { get; set; } = new Member();
}