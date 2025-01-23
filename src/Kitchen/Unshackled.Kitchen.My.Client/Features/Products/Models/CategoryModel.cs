using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Models;

public class CategoryModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
