using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.Core.Models;

public class AppState : BaseAppState, IAppState
{
	public override required IMember ActiveMember { get; set; } = new Member();
	public override string StoragePrefix => "ufd_";
}