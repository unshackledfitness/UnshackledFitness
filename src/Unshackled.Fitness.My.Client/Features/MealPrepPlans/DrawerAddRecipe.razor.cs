using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans;

public class DrawerAddRecipeBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel>
{
	[Parameter] public List<SlotModel> Meals { get; set; } = [];
	[Parameter] public EventCallback<AddPlanRecipeModel> OnAdded { get; set; }
	[Parameter] public EventCallback OnCancelClicked { get; set; }

	protected List<RecipeListModel> Recipes { get; set; } = new();
	protected List<AddToShoppingListModel> Items { get; set; } = new();
	protected RecipeListModel? SelectedRecipe { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		SearchKey = "SearchMealPlanRecipes";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(1);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchRecipes.Query(SearchModel));
		IsLoading = false;
	}

	protected async void HandleAddClicked(RecipeListModel recipe)
	{
		SelectedRecipe = recipe;
		if (SelectedRecipe != null && (Meals.Count == 0 || string.IsNullOrEmpty(Meals.First().Sid)))
		{
			await Submit(string.Empty);
		}
	}

	protected async Task HandleAddToMealClicked(SlotModel meal)
	{
		await Submit(meal.Sid);
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}

	private async Task Submit(string mealSid)
	{
		if (SelectedRecipe == null)
			return;

		IsWorking = true;
		AddPlanRecipeModel model = new()
		{
			MealDefinitionSid = mealSid,
			RecipeSid = SelectedRecipe.Sid,
			Scale = SelectedRecipe.Scale,
		};
		await OnAdded.InvokeAsync(model);
		IsWorking = false;
	}
}