using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class RecipeIngredientGroupModel : BaseHouseholdObject, ISortableGroup
{
	public string RecipeSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Title { get; set; } = string.Empty;
}
