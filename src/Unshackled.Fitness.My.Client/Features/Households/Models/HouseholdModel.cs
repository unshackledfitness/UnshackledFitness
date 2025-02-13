using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Models;

public class HouseholdModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
