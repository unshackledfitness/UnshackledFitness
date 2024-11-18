using System.Text.Json.Serialization;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.My.Client.Extensions;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class RecipeListModel : BaseHouseholdObject, IRecipeDietTags
{
	public string Title { get; set; } = string.Empty;
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }

	public bool IsGlutenFree { get; set; }
	public bool IsLowCarb { get; set; }
	public bool IsLowFat { get; set; }
	public bool IsLowSodium { get; set; }
	public bool IsNutFree { get; set; }
	public bool IsVegetarian { get; set; }
	public bool IsVegan { get; set; }

	[JsonIgnore]
	public TimeSpan PrepTime => new TimeSpan(0, PrepTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan CookTime => new TimeSpan(0, CookTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan TotalTime => new TimeSpan(0, PrepTimeMinutes + CookTimeMinutes, 0);

	[JsonIgnore]
	public List<string> DietTags => this.GetSelectedDiets().Select(x => x.Title()).ToList();
}
