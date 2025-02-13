using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Models;

public class HouseholdListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public bool IsInvite { get; set; } = false;
}
