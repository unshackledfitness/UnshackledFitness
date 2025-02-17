using System.Text.Json.Serialization;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

public class RecipeListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;

	[JsonIgnore]
	public decimal Scale { get; set; } = 1M;
}
