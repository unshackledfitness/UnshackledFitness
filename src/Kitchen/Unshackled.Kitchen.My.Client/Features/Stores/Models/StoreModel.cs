using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class StoreModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsArchived { get; set; }
}
