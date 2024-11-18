using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Models;

public class MemberCookbook
{
	public string CookbookSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }

}
