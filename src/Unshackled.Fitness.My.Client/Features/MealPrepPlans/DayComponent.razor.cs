using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans;

public class DayComponentBase : BaseComponent
{
	[Parameter] public List<SlotModel> Meals { get; set; } = [];
	[Parameter] public DayModel Model { get; set; } = new();
	[Parameter] public EventCallback<DayModel> ModelChanged { get; set; }
	[Parameter] public bool IsEditMode { get; set; }
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public bool DisableControls { get; set; }
	[Parameter] public EventCallback<DateOnly> OnAddRecipeClicked { get; set; }
	[Parameter] public EventCallback<List<MealPrepPlanRecipeModel>> OnApplySortClicked { get; set; }
	[Parameter] public EventCallback<MealPrepPlanRecipeModel> OnDeleteClicked { get; set; }
	[Parameter] public EventCallback OnCheckedChanged { get; set; }
	[Parameter] public EventCallback<List<MealPrepPlanRecipeModel>> OnMakeItClicked { get; set; }
	[Parameter] public EventCallback<KeyValuePair<string, int>> OnDaySwitchClicked { get; set; }

	protected bool IsMoving { get; set; }
	protected bool IsSwitching { get; set; }
	protected bool DisableComponentControls => DisableControls || IsMoving || IsSwitching;
	protected List<MealPrepPlanRecipeModel> SortRecipes { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
	}

	protected async Task HandleAddRecipeClicked()
	{
		await OnAddRecipeClicked.InvokeAsync(Model.Date);
	}

	protected async Task HandleApplySortClicked()
	{
		await OnApplySortClicked.InvokeAsync(SortRecipes);
		IsMoving = false;
	}

	protected void HandleCancelMoveOrSwitchClicked()
	{
		IsMoving = false;
		IsSwitching = false;
	}

	protected async Task HandleDeleteClicked(MealPrepPlanRecipeModel model)
	{
		await OnDeleteClicked.InvokeAsync(model);
	}

	protected async Task HandleItemChecked(bool value)
	{
		Model.IsChecked = value;
		await ModelChanged.InvokeAsync(Model);
		await OnCheckedChanged.InvokeAsync();
	}

	protected async Task HandleMakeItClicked(List<MealPrepPlanRecipeModel> recipes)
	{
		await OnMakeItClicked.InvokeAsync(recipes);
	}

	protected void HandleMoveDown(MealPrepPlanRecipeModel recipe, SlotModel def)
	{
		if (IsDownDisabled(def))
			return;

		int idx = Meals.IndexOf(def);
		var newDef = Meals[idx + 1];
		recipe.SlotSid = newDef.Sid;
		StateHasChanged();
	}

	protected void HandleMoveUp(MealPrepPlanRecipeModel recipe, SlotModel def)
	{
		int idx = Meals.IndexOf(def);
		if (idx <= 0)
			return;

		var newDef = Meals[idx - 1];
		recipe.SlotSid = newDef.Sid;
		StateHasChanged();
	}

	protected void HandleMoveClicked()
	{
		SortRecipes = Model.Recipes
			.Select(x => (MealPrepPlanRecipeModel)x.Clone())
			.ToList();
		IsMoving = true;
		StateHasChanged();
	}

	protected void HandleSwitchClicked()
	{
		IsSwitching = true;
		StateHasChanged();
	}

	protected async Task HandleSwitchNext(MealPrepPlanRecipeModel model)
	{
		if (Model.DayOfWeek < (int)DayOfWeek.Saturday)
		{
			KeyValuePair<string, int> switchDay = new(model.Sid, 1);
			await OnDaySwitchClicked.InvokeAsync(switchDay);
			IsSwitching = false;
		}
	}

	protected async Task HandleSwitchPrev(MealPrepPlanRecipeModel model)
	{
		if (Model.DayOfWeek > (int)DayOfWeek.Sunday)
		{
			KeyValuePair<string, int> switchDay = new(model.Sid, -1);
			await OnDaySwitchClicked.InvokeAsync(switchDay);
			IsSwitching = false;
		}
	}

	protected bool IsDownDisabled(SlotModel model)
	{
		if (string.IsNullOrEmpty(model.Sid))
			return true;

		int idx = Meals.IndexOf(model);

		if (idx == Meals.Count - 2 || idx + 1 >= Meals.Count)
			return true;

		if (string.IsNullOrEmpty(Meals[idx + 1].Sid))
			return true;

		return false;
	}

	protected List<MealPrepPlanRecipeModel> ListRecipes(SlotModel def)
	{
		return Model.Recipes
			.Where(x => x.SlotSid == def.Sid)
			.ToList();
	}

	protected List<MealPrepPlanRecipeModel> ListSortRecipes(SlotModel def)
	{
		return SortRecipes
			.Where(x => x.SlotSid == def.Sid)
			.ToList();
	}
}