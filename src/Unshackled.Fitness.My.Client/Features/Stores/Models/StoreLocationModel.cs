using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Models;

public class StoreLocationModel : BaseHouseholdObject
{
	public string StoreSid { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public List<FormProductLocationModel> ProductLocations { get; set; } = new();
}
