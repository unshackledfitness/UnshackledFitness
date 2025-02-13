using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

public class TagModel : BaseHouseholdObject
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
}
