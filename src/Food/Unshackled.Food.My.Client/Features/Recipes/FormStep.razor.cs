using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class FormStepBase : BaseFormComponent<FormStepModel, FormStepModel.Validator>
{
	[Parameter] public List<FormStepIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public string SubmitButtonLabel { get; set; } = "Save";

	protected List<FormStepIngredientModel> UnusedIngredients => Ingredients
		.Where(x => x.RecipeStepSid == Model.Sid || x.Checked == false).ToList();

	protected void HandleIngredientCheckChange(bool isChecked, FormStepIngredientModel item)
	{
		item.Checked = isChecked;
		if(isChecked)
		{
			item.RecipeStepSid = Model.Sid;
			Model.Ingredients.Add(item);
		}
		else
		{
			item.RecipeStepSid = string.Empty;

			var match = Model.Ingredients.Where(x => x.RecipeIngredientSid == item.RecipeIngredientSid).SingleOrDefault();
			if(match != null)
				Model.Ingredients.Remove(match);
		}
	}
}