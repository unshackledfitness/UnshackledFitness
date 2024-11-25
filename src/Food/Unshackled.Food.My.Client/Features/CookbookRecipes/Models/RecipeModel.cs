using System.Text.Json.Serialization;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.CookbookRecipes.Models;

public class RecipeModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }
	public bool IsOwner { get; set; }

	public List<RecipeIngredientGroupModel> Groups { get; set; } = [];
	public List<RecipeIngredientModel> Ingredients { get; set; } = [];
	public List<RecipeStepModel> Steps { get; set; } = [];
	public List<RecipeNoteModel> Notes { get; set; } = [];
	public List<TagModel> Tags { get; set; } = [];

	[JsonIgnore]
	public TimeSpan PrepTime => new(0, PrepTimeMinutes, 0);

	[JsonIgnore]
	public TimeSpan CookTime => new(0, CookTimeMinutes, 0);
}
