using System.Text.Json.Serialization;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Models;

public class RecipeListModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;

	[JsonIgnore]
	public decimal Scale { get; set; } = 1M;
}
