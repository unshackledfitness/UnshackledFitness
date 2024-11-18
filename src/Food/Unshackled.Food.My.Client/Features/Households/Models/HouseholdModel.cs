using Unshackled.Food.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Households.Models;

public class HouseholdModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
