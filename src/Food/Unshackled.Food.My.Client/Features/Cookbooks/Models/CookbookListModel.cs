using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Models;

public class CookbookListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public bool IsInvite { get; set; } = false;
}
