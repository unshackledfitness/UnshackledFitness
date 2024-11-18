using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Client.Features.Households.Models;

public class FormMemberModel
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string MemberSid { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
