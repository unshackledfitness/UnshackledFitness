using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class MemberHousehold
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

}
