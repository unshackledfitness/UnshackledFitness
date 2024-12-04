using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;

public class SearchProductsModel : SearchModel
{
	public string ShoppingListSid { get; set; } = string.Empty;
	public string? Title { get; set; }
	public string? CategorySid { get; set; }
}
