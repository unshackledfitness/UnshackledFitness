using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class SectionStepsBase : BaseSectionComponent
{
	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();
	[Parameter] public EventCallback UpdateComplete { get; set; }

	protected List<FormStepIngredientModel> Ingredients { get; set; } = new();
	protected List<FormStepModel> FormSteps { get; set; } = new();
	protected List<FormStepModel> DeletedSteps { get; set; } = new();
	protected FormStepModel AddFormModel { get; set; } = new();
	protected FormBulkAddStepModel BulkAddFormModel { get; set; } = new();

	protected bool IsWorking { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsAdding { get; set; } = false;
	protected bool IsBulkAdding { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool IsEditingItem => FormSteps.Where(x => x.IsEditing == true).Any();
	protected bool DisableControls => IsWorking || IsSorting || IsAdding || IsBulkAdding;

	protected void HandleAddClicked()
	{
		IsAdding = true;
	}

	protected void HandleAddFormSubmitted(FormStepModel model)
	{
		IsWorking = true;
		FormSteps.Add(new()
		{
			Sid = model.Sid,
			Ingredients = model.Ingredients,
			Instructions = model.Instructions,
			IsNew = true,
			RecipeSid = RecipeSid,
			SortOrder = FormSteps.Count()
		});
		AddFormModel = new();
		IsWorking = false;
	}

	protected void HandleBulkAddClicked()
	{
		IsBulkAdding = true;
	}

	protected void HandleBulkAddFormSubmitted(FormBulkAddStepModel model)
	{
		if (!string.IsNullOrEmpty(model.BulkText))
		{
			IsWorking = true;
			var reader = new StringReader(model.BulkText);
			string? line;
			while (null != (line = reader.ReadLine()))
			{
				string instructions = line.Trim();
				
				// Remove existing step #'s.
				var m = Regex.Match(instructions, @"^\d+[.)\]]");
				if (m.Success && !string.IsNullOrEmpty(m.Value))
				{
					instructions = instructions.Remove(0, m.Value.Length).Trim();
				}
				
				if (!string.IsNullOrEmpty(instructions))
				{
					FormSteps.Add(new()
					{
						Sid = Guid.NewGuid().ToString(),
						Instructions = instructions,
						IsNew = true,
						RecipeSid = RecipeSid,
						SortOrder = FormSteps.Count()
					});
				}
			}
			IsWorking = false;
		}
		IsBulkAdding = false;
	}

	protected void HandleCancelAddClicked()
	{
		IsAdding = false;
	}

	protected void HandleCancelBulkAddClicked()
	{
		IsBulkAdding = false;
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected void HandleCancelEditItemClicked(FormStepModel item)
	{
		item.IsEditing = false;
	}

	protected void HandleDeleteClicked(FormStepModel item)
	{
		FormSteps.Remove(item);
		DeletedSteps.Add(item);

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormSteps.Count; i++)
		{
			FormSteps[i].SortOrder = i;
		}
	}

	protected async Task HandleEditClicked()
	{
		await LoadFormSteps();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleEditFormSubmitted(FormStepModel model)
	{
		IsWorking = true;
		var item = FormSteps.Where(x => x.Sid == model.Sid).Single();
		item.Instructions = model.Instructions;
		item.Ingredients = model.Ingredients;
		item.IsEditing = false;
		IsWorking = false;
	}

	protected void HandleEditItemClicked(FormStepModel item)
	{
		item.IsEditing = true;
		IsAdding = false;
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsAdding = false;
		IsSorting = isSorting;
	}

	protected void HandleItemDropped(MudItemDropInfo<FormStepModel> droppedItem)
	{
		if (droppedItem.Item != null)
		{
			int newIdx = droppedItem.IndexInZone;
			int oldIdx = FormSteps.IndexOf(droppedItem.Item);
			FormSteps.MoveAndSort(oldIdx, newIdx);
		}
	}

	protected void HandleSortChanged(List<FormStepModel> steps)
	{
		FormSteps = steps;
	}

	protected async Task HandleUpdateClicked()
	{
		IsWorking = true;
		UpdateStepsModel model = new()
		{
			DeletedSteps = DeletedSteps,
			Steps = FormSteps
		};
		var result = await Mediator.Send(new UpdateSteps.Command(RecipeSid, model));
		ShowNotification(result);

		if (result.Success)
			await UpdateComplete.InvokeAsync();
		IsSorting = false;
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
	protected async Task LoadFormSteps()
	{
		DeletedSteps.Clear();
		FormSteps = Steps
			.Select(x => new FormStepModel
			{
				Sid = x.Sid,
				Instructions = x.Instructions,
				IsEditing = false,
				IsNew = false,
				RecipeSid = x.RecipeSid,
				SortOrder = x.SortOrder,
				Ingredients = x.Ingredients
					.Select(y => new FormStepIngredientModel
					{
						Checked = true,
						RecipeIngredientSid = y.RecipeIngredientSid,
						RecipeStepSid = y.RecipeStepSid,
						SortOrder = y.SortOrder,
						Title = y.Title
					})
					.ToList()
			})
			.ToList();

		// Get all ingredients for recipe
		var ingredients = await Mediator.Send(new ListStepIngredientsForRecipe.Query(RecipeSid));
		Ingredients = ingredients
			.Select(x => new FormStepIngredientModel()
			{
				Checked = string.IsNullOrEmpty(x.RecipeStepSid) ? false : true,
				RecipeIngredientSid = x.RecipeIngredientSid,
				RecipeStepSid = x.RecipeStepSid,
				Title = x.Title,
				SortOrder = x.SortOrder,
			})
			.ToList();

	}
}