using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Ingredients.Models;

public class ProductListModel : BaseHouseholdObject
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
