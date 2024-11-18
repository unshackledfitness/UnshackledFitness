using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Models;

public class StoreListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
