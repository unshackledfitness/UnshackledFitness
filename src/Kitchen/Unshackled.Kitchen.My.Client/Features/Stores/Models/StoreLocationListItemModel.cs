using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class StoreLocationListItemModel : BaseHouseholdObject
{
	public string StoreSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }
}
