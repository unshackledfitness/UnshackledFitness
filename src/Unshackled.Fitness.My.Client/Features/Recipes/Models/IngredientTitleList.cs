namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class IngredientTitleList
{
	public DateTimeOffset DateRetrieved { get; set; }
	public List<string> Titles { get; set; } = new();
}
