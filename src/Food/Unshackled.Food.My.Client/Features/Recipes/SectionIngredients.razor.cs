using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.Recipes;
using Unshackled.Food.Core.Utils;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class SectionIngredientsBase : BaseSectionComponent
{
	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public List<RecipeIngredientGroupModel> Groups { get; set; } = new();
	[Parameter] public List<RecipeIngredientModel> Ingredients { get; set; } = new();
	[Parameter] public decimal Scale { get; set; }
	[Parameter] public EventCallback<decimal> ScaleChanged { get; set; }
	[Parameter] public EventCallback UpdateComplete { get; set; }
	[Parameter] public EventCallback OnNutritionClicked { get; set; }

	protected List<FormIngredientGroupModel> FormGroups { get; set; } = new();
	protected List<FormEditIngredientModel> FormIngredients { get; set; } = new(); 
	protected List<FormIngredientGroupModel> DeletedHouseholds { get; set; } = new();
	protected List<FormEditIngredientModel> DeletedIngredients { get; set; } = new();
	protected FormAddIngredientModel AddFormModel { get; set; } = new();
	protected FormBulkAddIngredientModel BulkAddFormModel { get; set; } = new();
	protected bool IsWorking { get; set; } = false;
	protected bool IsAdding { get; set; } = false;
	protected bool IsBulkAdding { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool DisableControls => IsWorking || IsSorting || IsAdding || IsBulkAdding;
	protected NutritionLabelModel LabelModel { get; set; } = new();
	protected bool HasMissingProductSubstitutions => Ingredients.Where(x => x.HasSubstitution == false).Any();
	protected bool IsEditingItem => FormIngredients.Where(x => x.IsEditing == true).Any();
	protected bool ShowProducts { get; set; } = false;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		LabelModel = new();
		LabelModel.LoadNutritionLabel(Recipe.TotalServings, Scale, Ingredients.ToList<ILabelIngredient>());
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		AddFormModel = new()
		{
			RecipeSid = RecipeSid
		};
	}

	protected void HandleAddClicked()
	{
		IsAdding = true;
	}

	protected void HandleAddFormSubmitted(FormAddIngredientModel model)
	{
		if (!string.IsNullOrEmpty(model.Amount))
		{
			IsWorking = true;
			var result = IngredientUtils.ParseAmount(model.Amount);
			string gSid = FormGroups.LastOrDefault()?.Sid ?? Guid.NewGuid().ToString();
			FormIngredients.Add(new()
			{
				Amount = result.Amount,
				AmountUnit = result.AmountUnit,
				AmountLabel = result.AmountLabel,
				AmountText = result.AmountText,
				Sid = Guid.NewGuid().ToString(),
				ListGroupSid = gSid,
				IsNew = true,
				Key = model.Title.NormalizeKey(),
				PrepNote = model.PrepNote,
				RecipeSid = model.RecipeSid,
				SortOrder = FormIngredients.Count,
				Title = model.Title
			});

			// Reset form
			AddFormModel = new()
			{
				RecipeSid = RecipeSid
			};
			IsWorking = false;
			StateHasChanged();
		}
	}

	protected void HandleBulkAddClicked()
	{
		IsBulkAdding = true;
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
						Sid = Guid.NewGuid().ToString(),
						ListGroupSid = gSid,
						IsNew = true,
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

	protected void HandleCancelEditItemClicked(FormEditIngredientModel item)
	{
		item.IsEditing = false;
	}

	protected void HandleDeleteClicked(FormEditIngredientModel item)
	{
		FormIngredients.Remove(item);

		DeletedIngredients.Add(item);

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormIngredients.Count; i++)
		{
			FormIngredients[i].SortOrder = i;
		}
	}

	protected async Task HandleEditClicked()
	{
		await LoadForm();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleEditFormSubmitted(FormEditIngredientModel model)
	{
		IsWorking = true;
		var result = IngredientUtils.ParseNumber(model.AmountText);
		var item = FormIngredients.Where(x => x.Sid == model.Sid).Single();
		item.Amount = result.Amount;
		item.AmountText = model.AmountText;
		item.AmountUnit = model.AmountUnit;
		item.AmountLabel = model.AmountLabel;
		item.Title = model.Title;
		item.Key = model.Title.NormalizeKey();
		item.PrepNote = model.PrepNote;
		item.IsEditing = false;
		IsWorking = false;
	}

	protected void HandleEditItemClicked(FormEditIngredientModel item)
	{
		item.IsEditing = true;
		IsAdding = false;
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsAdding = false;
		IsSorting = isSorting;
		StateHasChanged();
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

	protected void HandleSortChanged(SortableGroupResult<FormIngredientGroupModel, FormEditIngredientModel> result)
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
			.Select(x => new FormEditIngredientModel
			{
				Amount = x.Amount,
				AmountLabel = x.AmountLabel,
				AmountUnit = x.AmountUnit,
				AmountText = x.AmountText,
				Sid = x.Sid,
				ListGroupSid = x.ListGroupSid,
				IsEditing = false,
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

	protected string GetScaleFraction()
	{
		return Scale switch
		{
			0.25M => "1/4x",
			0.5M => "1/2x",
			0.75M => "3/4x",
			1M => "1x",
			2M => "2x",
			3M => "3x",
			4M => "4x",
			_ => Scale.ToString("0.##")
		};
	}
}