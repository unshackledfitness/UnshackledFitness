using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;

public class RecipeNoteModel : BaseObject
{
	public string RecipeSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Note { get; set; } = string.Empty;
}
