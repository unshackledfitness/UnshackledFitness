namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class IngredientTitleList
{
	public DateTime DateRetrieved { get; set; }
	public List<string> Titles { get; set; } = new();
}
