using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Utils;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class SectionIngredientsBase : BaseSectionComponent
{
	protected enum Views
	{
		None,
		AddIngredient,
		EditIngredient,
		BulkAdd
	}

	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public List<RecipeIngredientGroupModel> Groups { get; set; } = new();
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public decimal Scale { get; set; }
	[Parameter] public EventCallback<decimal> ScaleChanged { get; set; }
	[Parameter] public EventCallback UpdateComplete { get; set; }
	[Parameter] public EventCallback OnNutritionClicked { get; set; }
	[Parameter] public EventCallback<List<string>> OnSelectionChange { get; set; }

	protected List<FormIngredientGroupModel> FormGroups { get; set; } = new();
	protected List<FormIngredientModel> FormIngredients { get; set; } = new(); 
	protected List<FormIngredientGroupModel> DeletedHouseholds { get; set; } = new();
	protected List<FormIngredientModel> DeletedIngredients { get; set; } = new();
	protected FormIngredientModel CurrentFormModel { get; set; } = new();
	protected FormIngredientModel FormModel { get; set; } = new();
	protected FormBulkAddIngredientModel BulkAddFormModel { get; set; } = new();
	protected bool IsWorking { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool DisableControls => IsWorking || IsSorting;
	protected NutritionLabelModel LabelModel { get; set; } = new();
	protected bool ShowProducts { get; set; } = false;
	protected bool? IsSelectAll { get; set; } = false;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.AddIngredient => "Add Ingredient",
		Views.BulkAdd => "Bulk Add Ingredients",
		Views.EditIngredient => "Edit Ingredient",
		_ => string.Empty
	};

	protected void HandleAddClicked()
	{
		FormModel = new()
		{
			RecipeSid = RecipeSid,
			AmountUnit = MeasurementUnits.Item
		};

		DrawerView = Views.AddIngredient;
	}

	protected void HandleBulkAddClicked()
	{
		BulkAddFormModel = new()
		{
			RecipeSid = RecipeSid
		};
		DrawerView = Views.BulkAdd;
	}

	protected void HandleBulkAddFormSubmitted(FormBulkAddIngredientModel model) 
	{
		if(!string.IsNullOrEmpty(model.BulkText))
		{
			IsWorking = true;

			string gSid = FormGroups.LastOrDefault()?.Sid ?? Guid.NewGuid().ToString();

			var reader = new StringReader(model.BulkText);
			string? line;
			while (null != (line = reader.ReadLine()))
			{
				if (!string.IsNullOrEmpty(line.Trim()))
				{
					var result = IngredientUtils.Parse(line);
					FormIngredients.Add(new()
					{
						Amount = result.Amount,
						AmountUnit = result.AmountUnit,
						AmountLabel = result.AmountLabel,
						AmountText = result.AmountText,
						ListGroupSid = gSid,
						Key = result.Title.NormalizeKey(),
						PrepNote = result.PrepNote,
						RecipeSid = model.RecipeSid,
						SortOrder = FormIngredients.Count,
						Title = result.Title
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
		FormIngredients.Remove(CurrentFormModel);

		if (!string.IsNullOrEmpty(CurrentFormModel.Sid))
		{
			DeletedIngredients.Add(CurrentFormModel);
		}

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormIngredients.Count; i++)
		{
			FormIngredients[i].SortOrder = i;
		}

		DrawerView = Views.None;
	}

	protected async Task HandleEditClicked()
	{
		await LoadForm();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleEditItemClicked(FormIngredientModel item)
	{
		DrawerView = Views.EditIngredient;
		CurrentFormModel = item;
		FormModel = (FormIngredientModel)item.Clone();
	}

	protected async Task HandleGroupItemChecked(RecipeIngredientGroupModel model, bool? isChecked)
	{
		model.IsSelectAll = isChecked;
		if (isChecked.HasValue)
		{
			Ingredients.Where(x => x.ListGroupSid == model.Sid)
				.ToList()
				.ForEach(x => x.IsSelected = isChecked.Value);

			if (isChecked.Value)
			{
				bool allSelected = !Ingredients.Where(x => x.IsSelected == false).Any();
				if (allSelected)
					IsSelectAll = true;
				else
					IsSelectAll = null;
			}
			else
			{
				bool allUnselected = !Ingredients.Where(x => x.IsSelected == true).Any();
				if (allUnselected)
					IsSelectAll = false;
				else
					IsSelectAll = null;
			}

			await NotifySelectedChanged();
		}
	}

	protected void HandleIngredientAddFormSubmitted(FormIngredientModel model)
	{
		IsWorking = true;

		var result = IngredientUtils.ParseNumber(model.AmountText);
		string gSid = FormGroups.LastOrDefault()?.Sid ?? Guid.NewGuid().ToString();
		model.ListGroupSid = gSid;
		model.Amount = result.Amount;
		model.SortOrder = FormIngredients.Count;
		model.Key = model.Title.NormalizeKey();
		FormIngredients.Add(model);

		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleIngredientEditFormSubmitted(FormIngredientModel model)
	{
		IsWorking = true;

		var result = IngredientUtils.ParseNumber(model.AmountText);
		CurrentFormModel.Amount = result.Amount;
		CurrentFormModel.AmountText = model.AmountText;
		CurrentFormModel.AmountUnit = model.AmountUnit;
		CurrentFormModel.AmountLabel = model.AmountLabel;
		CurrentFormModel.Title = model.Title;
		CurrentFormModel.Key = model.Title.NormalizeKey();
		CurrentFormModel.PrepNote = model.PrepNote;

		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsSorting = isSorting;
		StateHasChanged();
	}

	protected async Task HandleItemChecked(RecipeIngredientModel model, bool isChecked)
	{
		model.IsSelected = isChecked;
		if (!string.IsNullOrEmpty(model.ListGroupSid))
		{
			var group = Groups.Where(x => x.Sid == model.ListGroupSid).FirstOrDefault();
			if (group != null && model.IsSelected)
			{
				bool groupSelected = !Ingredients.Where(x => x.ListGroupSid == model.ListGroupSid && x.IsSelected == false).Any();
				if (groupSelected)
					group.IsSelectAll = true;
				else
					group.IsSelectAll = null;
			}
			else if (group != null && !model.IsSelected)
			{
				bool groupUnselected = !Ingredients.Where(x => x.ListGroupSid == model.ListGroupSid && x.IsSelected == true).Any();
				if (groupUnselected)
					group.IsSelectAll = false;
				else
					group.IsSelectAll = null;
			}
		}

		if (model.IsSelected)
		{
			bool allSelected = !Ingredients.Where(x => x.IsSelected == false).Any();
			if (allSelected)
				IsSelectAll = true;
			else
				IsSelectAll = null;
		}
		else
		{
			bool allUnselected = !Ingredients.Where(x => x.IsSelected == true).Any();
			if (allUnselected)
				IsSelectAll = false;
			else
				IsSelectAll = null;
		}

		await NotifySelectedChanged();
	} 

	protected async Task HandleOpenNutritionClicked()
	{
		await OnNutritionClicked.InvokeAsync();
	}

	protected async Task HandleScaleChanged(decimal value)
	{
		if (value != Scale)
		{
			Scale = value;
			await ScaleChanged.InvokeAsync(value);
		}
	}

	protected async Task HandleSelectAllChange(bool? isChecked)
	{
		IsSelectAll = isChecked;
		if (isChecked.HasValue)
		{
			Groups.ForEach(x => x.IsSelectAll = isChecked.Value);
			Ingredients.ForEach(x => x.IsSelected = isChecked.Value);
			await NotifySelectedChanged();
		}
	}

	protected void HandleSortChanged(SortableGroupResult<FormIngredientGroupModel, FormIngredientModel> result)
	{
		FormGroups = result.Groups;
		FormIngredients = result.Items;
		DeletedHouseholds = result.DeletedGroups;
	}

	protected async Task HandleUpdateClicked()
	{
		IsWorking = true;
		UpdateIngredientsModel model = new()
		{
			DeletedListGroups = DeletedHouseholds,
			DeletedIngredients = DeletedIngredients,
			ListGroups = FormGroups,
			Ingredients = FormIngredients
		};
		var result = await Mediator.Send(new UpdateIngredients.Command(RecipeSid, model));
		ShowNotification(result);

		if (result.Success)
		{
			var titles = model.Ingredients.Select(x => x.Title).ToList();
			await Mediator.Send(new UpdateIngredientTitles.Command(titles));
			await UpdateComplete.InvokeAsync();
		}
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task LoadForm()
	{
		DeletedIngredients.Clear();
		DeletedHouseholds.Clear();
		FormIngredients = Ingredients
			.Select(x => new FormIngredientModel
			{
				Amount = x.Amount,
				AmountLabel = x.AmountLabel,
				AmountUnit = x.AmountUnit,
				AmountText = x.AmountText,
				Sid = x.Sid,
				ListGroupSid = x.ListGroupSid,
				Key = x.Key,
				PrepNote = x.PrepNote,
				RecipeSid = x.RecipeSid,
				SortOrder = x.SortOrder,
				Title = x.Title
			})
			.ToList();

		FormGroups = Groups
			.Select(x => new FormIngredientGroupModel
			{
				Sid = x.Sid,
				SortOrder = x.SortOrder,
				Title = x.Title
			})
			.ToList();

		await Mediator.Send(new UpdateIngredientTitles.Command(new()));
	}

	protected async Task NotifySelectedChanged()
	{
		await OnSelectionChange.InvokeAsync(Ingredients.Where(x => x.IsSelected).Select(x => x.Sid).ToList());
	}
}