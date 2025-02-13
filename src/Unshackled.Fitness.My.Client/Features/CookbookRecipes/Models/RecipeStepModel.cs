using System.Text.Json.Serialization;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

public class RecipeStepModel : BaseObject
{
	public string RecipeSid { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public string Instructions { get; set; } = string.Empty;

	public List<RecipeStepIngredientModel> Ingredients { get; set; } = new(); 
	
	[JsonIgnore]
	public string IngredientList => string.Join(", ", Ingredients.Select(x => x.Title).ToArray());
}
