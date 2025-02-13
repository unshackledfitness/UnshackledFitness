using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public class MemberCookbook
{
	public string CookbookSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

}
