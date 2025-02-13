using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class RecipeStepModel : BaseObject
{
	public string RecipeSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Instructions { get; set; } = string.Empty;
}
