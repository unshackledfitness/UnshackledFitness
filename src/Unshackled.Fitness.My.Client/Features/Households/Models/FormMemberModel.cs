using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Households.Models;

public class FormMemberModel
{
	public string HouseholdSid { get; set; } = string.Empty;
	public string MemberSid { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
