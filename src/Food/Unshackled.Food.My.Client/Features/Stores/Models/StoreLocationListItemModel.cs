using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Models;

public class StoreLocationListItemModel : BaseHouseholdObject
{
	public string StoreSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }
}
