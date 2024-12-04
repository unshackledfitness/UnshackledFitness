namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class UpdateIngredientsModel
{
	public List<FormIngredientGroupModel> DeletedListGroups { get; set; } = new();
	public List<FormIngredientModel> DeletedIngredients { get; set; } = new();
	public List<FormIngredientGroupModel> ListGroups { get; set; } = new();
	public List<FormIngredientModel> Ingredients { get; set; } = new();
}
