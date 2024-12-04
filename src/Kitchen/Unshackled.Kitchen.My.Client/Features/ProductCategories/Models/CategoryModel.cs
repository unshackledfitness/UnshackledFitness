using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;

public class CategoryModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
