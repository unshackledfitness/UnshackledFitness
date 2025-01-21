using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Services;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans;

public class IndexBase : BaseComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Inject] IScreenWakeLockService ScreenLockService { get; set; } = null!;

	protected enum Views
	{
		None,
		Add,
		AddToList
	}

	protected bool IsDrawerOpen { get; set; }
	protected bool IsLoading { get; set; } = true;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool? IsSelectAll { get; set; } = false;
	protected bool DisableControls => IsWorking || IsLoading;
	protected DateTime? DateStart { get; set; }
	protected DateOnly AddDate { get; set; }
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Meal",
		Views.AddToList => "Add To List",
		_ => string.Empty
	};
	protected bool HasSelected => Days.Where(x => x.IsChecked == true && x.Recipes.Count > 0).Any();

	protected List<MealDefinitionModel> Meals { get; set; } = [];
	protected List<DayModel> Days { get; set; } = [];
	protected List<MealPlanRecipeModel> PlanRecipes { get; set; } = [];
	protected List<MealPlanRecipeModel> SelectedRecipes { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Meal Plan", null, true));

		DateTime today = DateTime.Now;
		int currentDayOfWeek = (int)today.DayOfWeek;
		int dayStartOfWeek = today.AddDays(-currentDayOfWeek).Day;
		DateStart = new DateTime(today.Year, today.Month, dayStartOfWeek);

		FillDays();

		Meals = await Mediator.Send(new ListMealDefinitions.Query());
		Meals.Add(new MealDefinitionModel
		{
			SortOrder = Meals.Count,
			Title = Meals.Count > 0 ? "Other Recipes" : string.Empty
		});
		await LoadMealPlan();
	}

	protected void HandleAddRecipeClicked(DateOnly date)
	{
		AddDate = date;
		DrawerView = Views.Add;
	}

	protected void HandleAddToListClicked()
	{
		var selectedDays = Days.Where(x => x.IsChecked == true && x.Recipes.Count > 0).ToList();
		if (selectedDays.Count > 0)
		{
			SelectedRecipes.Clear();
			foreach (var day in selectedDays)
			{
				SelectedRecipes.AddRange(day.Recipes);
			}
			DrawerView = Views.AddToList;
		}
	}

	protected void HandleAddToListComplete()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleApplySortClicked(List<MealPlanRecipeModel> sortRecipes)
	{
		IsWorking = true;
		var updates = sortRecipes
			.Select(x => new UpdateSortModel
			{
				MealDefinitionSid = x.MealDefinitionSid,
				Sid = x.Sid
			})
			.ToList();

		var result = await Mediator.Send(new UpdateSort.Command(updates));
		ShowNotification(result);
		if (result.Success)
		{
			foreach (var recipe in sortRecipes)
			{
				var pr = PlanRecipes.Where(x => x.Sid == recipe.Sid).SingleOrDefault();
				if (pr != null)
				{
					pr.MealDefinitionSid = recipe.MealDefinitionSid;
				}
			}
			FillDays();
		}
		IsWorking = false;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleDayCheckChanged()
	{
		bool allSelected = !Days.Where(x => x.IsChecked == false).Any();
		bool allUnselected = !Days.Where(x => x.IsChecked == true).Any();
		if (allSelected)
			IsSelectAll = true;
		else if (allUnselected)
			IsSelectAll = false;
		else
			IsSelectAll = null;
	}

	protected async Task HandleDeleteClicked(MealPlanRecipeModel model)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this recipe?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteMealRecipe.Command(model.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				PlanRecipes.Remove(model);
				FillDays();
			}
			IsWorking = false;
		}
	}

	protected async Task HandleMakeItClicked(List<MealPlanRecipeModel> recipes)
	{
		if (recipes.Count > 0)
		{
			IsWorking = true;
			var recipesAndScales = recipes
				.Select(x => new KeyValuePair<string, decimal>(x.RecipeSid, x.Scale))
				.ToDictionary();

			var makeItRecipes = await Mediator.Send(new ListMakeIt.Query(recipesAndScales));
			if (makeItRecipes.Count > 0)
			{
				var state = (AppState)State;
				foreach (var model in makeItRecipes)
				{
					state.AddMakeItRecipe(model);
				}
				state.UpdateIndex(0);
				await DialogService.OpenMakeItClicked(ScreenLockService);
			}
			IsWorking = false;
		}
	}

	protected async Task HandleMealAdded(AddPlanRecipeModel model)
	{
		IsWorking = true;
		DrawerView = Views.None;

		model.DatePlanned = AddDate;
		var result = await Mediator.Send(new AddMealRecipe.Command(model));
		ShowNotification(result);
		if (result.Success && result.Payload != null)
		{
			PlanRecipes.Add(result.Payload);
			FillDays();
		}
		IsWorking = false;
	}

	protected async Task HandleNextWeekClicked() 
	{
		if (DateStart.HasValue)
		{
			DateStart = DateStart.Value.AddDays(7);
			await LoadMealPlan();
		}
	}

	protected async Task HandlePrevWeekClicked()
	{
		if (DateStart.HasValue)
		{
			DateStart = DateStart.Value.AddDays(-7);
			await LoadMealPlan();
		}
	}

	protected void HandleSelectAllChange(bool? isChecked)
	{
		IsSelectAll = isChecked;
		if (isChecked.HasValue)
		{
			foreach (var model in Days)
			{
				model.IsChecked = isChecked.Value;
			}
		}
	}

	protected async	Task HandleStartDateChanged(DateTime? date)
	{
		DateStart = date;
		await LoadMealPlan();
	}

	protected bool IsDateDisabled(DateTime date)
	{
		return (int)date.DayOfWeek > 0;
	}

	private void FillDays()
	{
		Days.Clear();
		for (int d = 0; d < 7; d++)
		{
			DateOnly currentDate = DateOnly.FromDateTime(DateStart!.Value.AddDays(d));
			DayModel model = new()
			{
				DayOfWeek = d,
				Date = currentDate,
				Recipes = PlanRecipes.Where(x => x.DatePlanned == currentDate).ToList()
			};
			Days.Add(model);
		}
	}

	private async Task LoadMealPlan()
	{
		if (DateStart.HasValue)
		{
			IsLoading = true;
			PlanRecipes = await Mediator.Send(new ListPlanRecipes.Query(DateOnly.FromDateTime(DateStart.Value)));
			FillDays();
			IsLoading = false;
		}
	}
}