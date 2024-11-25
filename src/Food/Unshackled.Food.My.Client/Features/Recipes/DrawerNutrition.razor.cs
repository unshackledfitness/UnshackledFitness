using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class DrawerNutritionBase : BaseComponent<Member>
{
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public decimal Scale { get; set; }
	protected NutritionLabelModel LabelModel { get; set; } = new();
	protected bool HasMissingNutritionInfo => Ingredients.Where(x => x.HasNutritionInfo == false).Any();
	protected bool HasMissingProductSubstitutions => Ingredients.Where(x => x.HasSubstitution == false).Any();

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		LabelModel = new();
		LabelModel.LoadNutritionLabel(Recipe.TotalServings, Scale, Ingredients.ToList<ILabelIngredient>());
	}
}