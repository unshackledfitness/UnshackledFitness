using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Services;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans;

public class IndexBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Inject] IScreenWakeLockService ScreenLockService { get; set; } = null!;

	protected enum Views
	{
		None,
		Add,
		AddToList,
		Copy
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
		Views.Add => "Add Slot",
		Views.AddToList => "Add To List",
		Views.Copy => "Copy To...",
		_ => string.Empty
	};
	protected bool HasSelected => Days.Where(x => x.IsChecked == true && x.Recipes.Count > 0).Any();

	protected List<SlotModel> Meals { get; set; } = [];
	protected List<DayModel> Days { get; set; } = [];
	protected List<MealPrepPlanRecipeModel> PlanRecipes { get; set; } = [];
	protected List<MealPrepPlanRecipeModel> SelectedRecipes { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Meal Prep Plan", null, true));

		DateTime today = DateTime.Now;
		int currentDayOfWeek = (int)today.DayOfWeek;
		DateStart = today.AddDays(-currentDayOfWeek);

		FillDays();

		Meals = await Mediator.Send(new ListSlots.Query());
		Meals.Add(new SlotModel
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

	protected async Task HandleApplySortClicked(List<MealPrepPlanRecipeModel> sortRecipes)
	{
		IsWorking = true;
		var updates = sortRecipes
			.Select(x => new UpdateSortModel
			{
				MealDefinitionSid = x.SlotSid,
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
					pr.SlotSid = recipe.SlotSid;
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

	protected void HandleCopyClicked()
	{
		var selectedDays = Days.Where(x => x.IsChecked == true && x.Recipes.Count > 0).ToList();
		if (selectedDays.Count > 0)
		{
			SelectedRecipes.Clear();
			foreach (var day in selectedDays)
			{
				SelectedRecipes.AddRange(day.Recipes);
			}
			DrawerView = Views.Copy;
		}
	}

	protected async Task HandleCopySubmitClicked(DateOnly dateSelected)
	{
		IsWorking = true;
		CopyRecipesModel model = new()
		{
			DateSelected = dateSelected,
			Recipes = SelectedRecipes
		};
		var result = await Mediator.Send(new CopyMealRecipes.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			DateOnly dateEnd = DateOnly.FromDateTime(DateStart!.Value.AddDays(7));
			if (dateSelected < dateEnd)
			{
				await LoadMealPlan();
			}
		}
		DrawerView = Views.None;
		IsWorking = false;
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

	protected async Task HandleDeleteClicked(MealPrepPlanRecipeModel model)
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

	protected async Task HandleMakeItClicked(List<MealPrepPlanRecipeModel> recipes)
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
				foreach (var model in makeItRecipes)
				{
					State.AddMakeItRecipe(model);
				}
				State.UpdateIndex(State.MakeItRecipes.Count - 1);
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

	protected async Task HandleSwitchDayClicked(KeyValuePair<string, int> switchDay)
	{
		var recipe = PlanRecipes
			.Where(x => x.Sid == switchDay.Key)
			.SingleOrDefault();

		if (recipe != null)
		{
			IsWorking = true;
			var model = (MealPrepPlanRecipeModel)recipe.Clone();
			model.DatePlanned = model.DatePlanned.AddDays(switchDay.Value);
			var result = await Mediator.Send(new UpdateMealRecipe.Command(model));
			ShowNotification(result);
			if (result.Success)
			{
				recipe.DatePlanned = model.DatePlanned;
				FillDays();
			}
			IsWorking = false;
		}
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