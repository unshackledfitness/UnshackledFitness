using Unshackled.Food.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Models;

public class CookbookModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
