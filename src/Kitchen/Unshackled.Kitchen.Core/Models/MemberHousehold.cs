using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.Core.Models;

public class MemberHousehold
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

}
