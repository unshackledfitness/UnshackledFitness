using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Actions;
using Unshackled.Kitchen.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Ingredients;

public class SectionSubstitutionsBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public IngredientModel Ingredient { get; set; } = new();

	protected SearchProductModel StartingSearch { get; set; } = new();
	protected bool DrawerOpen { get; set; }
	protected bool IsWorking { get; set; }

	protected bool DisableControls => DisableSectionControls || IsWorking;

	protected async Task HandleAddClicked()
	{
		StartingSearch.Title = Ingredient.Title;
		DrawerOpen = await UpdateIsEditingSection(true);
	}

	protected async Task HandleCancelAddClicked()
	{
		DrawerOpen = await UpdateIsEditingSection(false);
	}

	protected async Task HandleDeleteClicked(ProductSubstitutionModel model)
	{
		bool? confirm = await DialogService.ShowMessageBox(
					"Confirm Delete",
					"Are you sure you want to remove this product?",
					yesText: "Remove", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = await UpdateIsEditingSection(true);
			FormSubstitutionModel delete = new()
			{
				IngredientKey = Ingredient.Key,
				ProductSid = model.ProductSid
			};
			var result = await Mediator.Send(new DeleteSubstitution.Command(delete));
			if (result.Success)
			{
				Ingredient.Substitutions.Remove(model);

				var newPrimary = Ingredient.Substitutions
					.Where(x => x.ProductSid == result.Payload)
					.SingleOrDefault();

				if (newPrimary != null)
					newPrimary.IsPrimary = true;
			}
			ShowNotification(result);
			IsWorking = await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleSetPrimaryClicked(ProductSubstitutionModel model)
	{
		IsWorking = await UpdateIsEditingSection(true);
		FormSubstitutionModel primary = new()
		{
			IngredientKey = Ingredient.Key,
			ProductSid = model.ProductSid
		};
		var result = await Mediator.Send(new MakePrimary.Command(primary));
		if (result.Success)
		{
			var oldPrimary = Ingredient.Substitutions
				.Where(x => x.IsPrimary == true)
				.SingleOrDefault();

			if (oldPrimary != null)
				oldPrimary.IsPrimary = false;

			model.IsPrimary = true;
		}
		ShowNotification(result);
		IsWorking = await UpdateIsEditingSection(false);
	}

	protected async Task HandleSubstitutionAdded(string productSid)
	{
		IsWorking = await UpdateIsEditingSection(true);
		FormSubstitutionModel model = new()
		{
			IngredientKey = Ingredient.Key,
			ProductSid = productSid
		};
		var result = await Mediator.Send(new AddSubstitution.Command(model));
		if (result.Success && result.Payload != null)
		{
			Ingredient.Substitutions.Add(result.Payload);
			Ingredient.Substitutions = Ingredient.Substitutions
				.OrderBy(x => x.Brand).ThenBy(x => x.Title)
				.ToList();
		}
		ShowNotification(result);
		DrawerOpen = false;
		IsWorking = await UpdateIsEditingSection(false);
	}
}