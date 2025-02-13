using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Models;

public class CategoryModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
