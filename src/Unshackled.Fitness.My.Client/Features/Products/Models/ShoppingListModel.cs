using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Models;

public class ShoppingListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
