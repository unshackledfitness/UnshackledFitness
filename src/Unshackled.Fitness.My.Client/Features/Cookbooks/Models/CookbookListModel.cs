using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

public class CookbookListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public bool IsInvite { get; set; } = false;
}
