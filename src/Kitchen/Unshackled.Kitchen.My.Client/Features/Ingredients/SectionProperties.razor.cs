using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Actions;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Parameter] public IngredientModel Ingredient { get; set; } = new();
	[Parameter] public EventCallback<IngredientModel> IngredientChanged { get; set; }

	protected const string FormId = "formIngredientProperties";
	protected bool IsSaving { get; set; }
	protected FormIngredientModel Model { get; set; } = new();

	protected bool DisableControls => IsSaving;

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Key = Ingredient.Key,
			Title = Ingredient.Title
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormIngredientModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateIngredient.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Ingredient.Title = model.Title;
			string newKey = model.Title.NormalizeKey();
			if (model.Key != newKey)
			{
				NavigateOnSuccess($"/ingredients/{newKey}");
			}
			await IngredientChanged.InvokeAsync(Ingredient);
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}