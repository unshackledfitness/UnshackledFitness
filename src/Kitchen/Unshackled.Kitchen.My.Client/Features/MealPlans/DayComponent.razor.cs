using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans;

public class DayComponentBase : BaseComponent<Member>
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;

	[Parameter] public List<MealDefinitionModel> Meals { get; set; } = [];
	[Parameter] public DayModel Model { get; set; } = new();
	[Parameter] public EventCallback<DayModel> ModelChanged { get; set; }
	[Parameter] public bool IsEditMode { get; set; }
	[Parameter] public bool IsLoading { get; set; }
	[Parameter] public bool DisableControls { get; set; }
	[Parameter] public EventCallback<DateOnly> OnAddRecipeClicked { get; set; }
	[Parameter] public EventCallback<List<MealPlanRecipeModel>> OnApplySortClicked { get; set; }
	[Parameter] public EventCallback<MealPlanRecipeModel> OnDeleteClicked { get; set; }
	[Parameter] public EventCallback OnCheckedChanged { get; set; }
	[Parameter] public EventCallback<List<MealPlanRecipeModel>> OnMakeItClicked { get; set; }

	protected bool IsSorting { get; set; }
	protected bool DisableComponentControls => DisableControls || IsSorting;
	protected List<MealPlanRecipeModel> SortRecipes { get; set; } = [];

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
		IsSorting = false;
	}

	protected async Task HandleDeleteClicked(MealPlanRecipeModel model)
	{
		await OnDeleteClicked.InvokeAsync(model);
	}

	protected async Task HandleItemChecked(bool value)
	{
		Model.IsChecked = value;
		await ModelChanged.InvokeAsync(Model);
		await OnCheckedChanged.InvokeAsync();
	}

	protected async Task HandleMakeItClicked(List<MealPlanRecipeModel> recipes)
	{
		await OnMakeItClicked.InvokeAsync(recipes);
	}

	protected void HandleMoveDown(MealPlanRecipeModel recipe, MealDefinitionModel def)
	{
		if (IsDownDisabled(def))
			return;

		int idx = Meals.IndexOf(def);
		var newDef = Meals[idx + 1];
		recipe.MealDefinitionSid = newDef.Sid;
		StateHasChanged();
	}

	protected void HandleMoveUp(MealPlanRecipeModel recipe, MealDefinitionModel def)
	{
		int idx = Meals.IndexOf(def);
		if (idx <= 0)
			return;

		var newDef = Meals[idx - 1];
		recipe.MealDefinitionSid = newDef.Sid;
		StateHasChanged();
	}

	protected void HandleSortClicked()
	{
		SortRecipes = Model.Recipes
			.Select(x => (MealPlanRecipeModel)x.Clone())
			.ToList();
		IsSorting = true;
		StateHasChanged();
	}

	protected bool IsDownDisabled(MealDefinitionModel model)
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

	protected List<MealPlanRecipeModel> ListRecipes(MealDefinitionModel def)
	{
		return Model.Recipes
			.Where(x => x.MealDefinitionSid == def.Sid)
			.ToList();
	}

	protected List<MealPlanRecipeModel> ListSortRecipes(MealDefinitionModel def)
	{
		return SortRecipes
			.Where(x => x.MealDefinitionSid == def.Sid)
			.ToList();
	}
}