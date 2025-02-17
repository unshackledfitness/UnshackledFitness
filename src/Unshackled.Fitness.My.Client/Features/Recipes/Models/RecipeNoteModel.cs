using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class RecipeNoteModel : BaseObject
{
	public string RecipeSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Note { get; set; } = string.Empty;
}
