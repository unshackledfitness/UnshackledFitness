using Unshackled.Food.Core.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class TagModel : BaseHouseholdObject
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
}
