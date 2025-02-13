using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans;

public class DrawerAddRecipeBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel, Member>
{
	[Parameter] public List<MealDefinitionModel> Meals { get; set; } = [];
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

	protected async Task HandleAddToMealClicked(MealDefinitionModel meal)
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