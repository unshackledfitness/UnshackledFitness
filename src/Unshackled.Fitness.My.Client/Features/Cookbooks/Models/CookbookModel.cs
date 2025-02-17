using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

public class CookbookModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public PermissionLevels PermissionLevel { get; set; }
}
