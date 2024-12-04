using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class SectionStepsBase : BaseSectionComponent<Member>
{
	protected enum Views
	{
		None,
		AddStep,
		EditStep,
		BulkAdd
	}

	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public List<RecipeStepModel> Steps { get; set; } = new();
	[Parameter] public EventCallback UpdateComplete { get; set; }

	protected List<FormStepModel> FormSteps { get; set; } = new();
	protected List<FormStepModel> DeletedSteps { get; set; } = new();
	protected FormStepModel CurrentStepModel { get; set; } = new();
	protected FormStepModel FormModel { get; set; } = new();
	protected FormBulkAddStepModel BulkAddFormModel { get; set; } = new();

	protected bool IsWorking { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool DisableControls => IsWorking || IsSorting;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.AddStep => "Add Step",
		Views.BulkAdd => "Bulk Add Steps",
		Views.EditStep => "Edit Step",
		_ => string.Empty
	};

	protected void HandleAddClicked()
	{
		FormModel = new()
		{
			RecipeSid = RecipeSid
		};
		DrawerView = Views.AddStep;
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
		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleBulkAddClicked()
	{
		BulkAddFormModel = new()
		{
			RecipeSid = RecipeSid
		};
		DrawerView = Views.BulkAdd;
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
						Instructions = instructions,
						IsNew = true,
						RecipeSid = RecipeSid,
						SortOrder = FormSteps.Count()
					});
				}
			}
			IsWorking = false;
		}
		DrawerView = Views.None;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected void HandleDeleteClicked()
	{
		FormSteps.Remove(CurrentStepModel);

		if (!CurrentStepModel.IsNew)
		{
			DeletedSteps.Add(CurrentStepModel);
		}

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormSteps.Count; i++)
		{
			FormSteps[i].SortOrder = i;
		}

		DrawerView = Views.None;
	}

	protected async Task HandleEditClicked()
	{
		DeletedSteps.Clear();
		FormSteps = Steps
			.Select(x => new FormStepModel
			{
				Sid = x.Sid,
				Instructions = x.Instructions,
				IsNew = false,
				RecipeSid = x.RecipeSid,
				SortOrder = x.SortOrder,
				Ingredients = x.Ingredients
					.Select(y => new FormStepIngredientModel
					{
						RecipeIngredientSid = y.RecipeIngredientSid,
						RecipeStepSid = y.RecipeStepSid,
						Title = y.Title
					})
					.ToList()
			})
			.ToList();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleEditFormSubmitted(FormStepModel model)
	{
		IsWorking = true;
		CurrentStepModel.Instructions = model.Instructions;
		CurrentStepModel.Ingredients = model.Ingredients;
		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleEditItemClicked(FormStepModel item)
	{
		CurrentStepModel = item;
		FormModel = (FormStepModel)item.Clone();
		DrawerView = Views.EditStep;
	}

	protected void HandleIsSorting(bool isSorting)
	{
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
}