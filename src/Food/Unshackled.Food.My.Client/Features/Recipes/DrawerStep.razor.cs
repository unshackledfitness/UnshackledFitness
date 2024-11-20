using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class DrawerStepBase : BaseFormComponent<FormStepModel, FormStepModel.Validator>
{
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}

	protected void HandleIngredientCheckChange(bool isChecked, RecipeIngredientModel item)
	{
		var match = Model.Ingredients.Where(x => x.RecipeIngredientSid == item.Sid).SingleOrDefault();
		if (isChecked && match == null)
		{
			Model.Ingredients.Add(new FormStepIngredientModel
			{
				RecipeIngredientSid = item.Sid,
				RecipeStepSid = Model.Sid,
				Title = item.Title,
			});
		}
		else if (!isChecked && match != null)
		{
			Model.Ingredients.Remove(match);
		}
	}

	protected bool IsChecked(RecipeIngredientModel recipeIngredient)
	{
		return Model.Ingredients.Where(x => x.RecipeIngredientSid == recipeIngredient.Sid).Any();
	}
}