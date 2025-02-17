using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Models;

public class StoreListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
