using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback<RecipeModel> RecipeChanged { get; set; }
	[Parameter] public List<ImageModel> Images { get; set; } = [];
	[Parameter] public decimal Scale { get; set; }

	protected const string FormId = "formRecipeProperties";
	protected bool IsSaving { get; set; }
	protected FormRecipeModel Model { get; set; } = new();
	protected List<RecipeTagSelectItem> RecipeTags { get; set; } = [];
	protected bool DisableControls => IsSaving;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		RecipeTags = await Mediator.Send(new ListRecipeTags.Query());
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			CookTimeMinutes = Recipe.CookTimeMinutes,
			Description = Recipe.Description,
			HouseholdSid = Recipe.HouseholdSid,
			PrepTimeMinutes = Recipe.PrepTimeMinutes,
			Sid = Recipe.Sid,
			TagSids = Recipe.Tags.Select(x => x.Sid).ToList(),
			Title = Recipe.Title,
			TotalServings = Recipe.TotalServings
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this recipe?",
				yesText: "Yes", cancelText: "No");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteRecipe.Command(Recipe.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/recipes");
			}
			await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormRecipeModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateRecipeProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await RecipeChanged.InvokeAsync(result.Payload);
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}