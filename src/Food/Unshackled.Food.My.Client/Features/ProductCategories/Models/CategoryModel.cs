using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.ProductCategories.Models;

public class CategoryModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
