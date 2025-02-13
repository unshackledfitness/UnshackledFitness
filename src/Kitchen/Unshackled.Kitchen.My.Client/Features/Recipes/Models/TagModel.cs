using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class TagModel : BaseHouseholdObject
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
