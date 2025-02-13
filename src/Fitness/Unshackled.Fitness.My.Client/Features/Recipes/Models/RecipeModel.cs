using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class RecipeModel : BaseHouseholdObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }
	public List<TagModel> Tags { get; set; } = [];

	[JsonIgnore]
	public TimeSpan PrepTime => new(0, PrepTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan CookTime => new(0, CookTimeMinutes, 0);
}
