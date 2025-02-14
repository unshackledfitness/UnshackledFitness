using Unshackled.Fitness.Core.Interfaces;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class MealPrepPlanRecipeModel : BaseHouseholdObject, IGroupedSortable
{
	public string ListGroupSid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public decimal Scale { get; set; }
	public int SortOrder { get; set; }
}
