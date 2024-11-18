using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Households.Models;

public class HouseholdListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public bool IsInvite { get; set; } = false;
}
