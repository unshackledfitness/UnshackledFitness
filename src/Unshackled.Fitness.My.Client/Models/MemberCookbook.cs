using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Models;

public class MemberCookbook
{
	public string CookbookSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

}
