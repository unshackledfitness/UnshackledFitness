namespace Unshackled.Food.Core.Models.ShoppingLists;

public class AddRecipeToListModel
{
	public string RecipeSid { get; set; } = string.Empty;
	public string RecipeTitle { get; set; } = string.Empty;
	public string ShoppingListSid { get; set; } = string.Empty;
	public List<AddToListModel> List { get; set; } = new();
}
