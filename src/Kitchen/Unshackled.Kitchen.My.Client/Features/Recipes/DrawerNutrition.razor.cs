using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class DrawerNutritionBase : BaseComponent<Member>
{
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public decimal Scale { get; set; }
	protected NutritionLabelModel LabelModel { get; set; } = new();
	protected List<RecipeIngredientModel> Mismatches { get; set; } = [];
	protected List<RecipeIngredientModel> MissingNutritionInfo { get; set; } = [];
	protected List<RecipeIngredientModel> MissingProductSubstitutions { get; set; } = [];
	protected bool HasMismatches => Mismatches.Count > 0;
	protected bool HasMissingNutritionInfo => MissingNutritionInfo.Count > 0;
	protected bool HasMissingProductSubstitutions => MissingProductSubstitutions.Count > 0;
	protected bool IsMmCollapsed { get; set; } = true;
	protected bool IsNiCollapsed { get; set; } = true;
	protected bool IsPsCollapsed { get; set; } = true;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		LabelModel = new();
		LabelModel.LoadNutritionLabel(Recipe.TotalServings, Scale, Ingredients.ToList<ILabelIngredient>());

		Mismatches = Ingredients.Where(x => LabelModel.MismatchSids.Contains(x.Sid)).ToList();
		MissingNutritionInfo = Ingredients.Where(x => x.HasNutritionInfo == false && x.HasSubstitution == true).ToList();
		MissingProductSubstitutions = Ingredients.Where(x => x.HasSubstitution == false).ToList();
	}
}