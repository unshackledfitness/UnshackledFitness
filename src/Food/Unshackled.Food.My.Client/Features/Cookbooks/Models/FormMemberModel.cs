using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Models;

public class FormMemberModel
{
	public string CookbookSid { get; set; } = string.Empty;
	public string MemberSid { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
