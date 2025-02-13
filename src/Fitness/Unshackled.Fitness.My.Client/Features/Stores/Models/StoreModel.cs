using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Models;

public class StoreModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsArchived { get; set; }
}
