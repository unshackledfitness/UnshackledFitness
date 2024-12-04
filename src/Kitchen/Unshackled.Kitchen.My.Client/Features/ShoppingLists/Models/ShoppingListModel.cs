using System.Text.Json.Serialization;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class ShoppingListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? StoreSid { get; set; }
	public string? StoreName { get; set; }

	[JsonIgnore]
	public bool HasStore => !string.IsNullOrEmpty(StoreSid);
}
