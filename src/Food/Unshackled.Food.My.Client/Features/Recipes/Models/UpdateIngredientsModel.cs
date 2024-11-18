namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class UpdateIngredientsModel
{
	public List<FormIngredientGroupModel> DeletedListGroups { get; set; } = new();
	public List<FormEditIngredientModel> DeletedIngredients { get; set; } = new();
	public List<FormIngredientGroupModel> ListGroups { get; set; } = new();
	public List<FormEditIngredientModel> Ingredients { get; set; } = new();
}
