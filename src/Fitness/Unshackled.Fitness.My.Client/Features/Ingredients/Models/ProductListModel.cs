using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Models;

public class ProductListModel : BaseHouseholdObject
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
