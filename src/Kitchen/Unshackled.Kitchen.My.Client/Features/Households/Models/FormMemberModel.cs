using Unshackled.Kitchen.Core.Enums;

namespace Unshackled.Kitchen.My.Client.Features.Households.Models;

public class FormMemberModel
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string MemberSid { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
