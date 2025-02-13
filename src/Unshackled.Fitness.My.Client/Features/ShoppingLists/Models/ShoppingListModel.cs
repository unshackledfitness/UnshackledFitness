using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

public class ShoppingListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? StoreSid { get; set; }
	public string? StoreName { get; set; }

	[JsonIgnore]
	public bool HasStore => !string.IsNullOrEmpty(StoreSid);
}
