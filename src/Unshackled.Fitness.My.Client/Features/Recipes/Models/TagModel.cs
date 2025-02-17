using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class TagModel : BaseHouseholdObject
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int ItemCount { get; set; }
}
