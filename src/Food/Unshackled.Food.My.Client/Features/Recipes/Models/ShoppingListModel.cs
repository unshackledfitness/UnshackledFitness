using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class ShoppingListModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
}
