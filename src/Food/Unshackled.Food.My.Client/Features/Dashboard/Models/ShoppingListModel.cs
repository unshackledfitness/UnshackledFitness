using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Dashboard.Models;

public class ShoppingListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
