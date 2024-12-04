using System.Text.Json.Serialization;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;

public class RecipeListModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }
	public List<TagModel> Tags { get; set; } = [];

	[JsonIgnore]
	public TimeSpan PrepTime => new TimeSpan(0, PrepTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan CookTime => new TimeSpan(0, CookTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan TotalTime => new TimeSpan(0, PrepTimeMinutes + CookTimeMinutes, 0);

	[JsonIgnore]
	public string TagTitles => string.Join(", ", Tags.Select(x => x.Title).ToArray());
}
