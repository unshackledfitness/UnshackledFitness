using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.Core.Models.ShoppingLists;
using Unshackled.Food.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ShoppingLists;

public class DrawerAddRecipeBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel, Member>
{
	[Parameter] public ShoppingListModel ShoppingList { get; set; } = new();
	[Parameter] public EventCallback OnAddedComplete { get; set; }
	[Parameter] public EventCallback OnCancelClicked { get; set; }

	public bool IsSelecting { get; set; } = true;
	protected List<RecipeListModel> Recipes { get; set; } = new();
	protected List<AddToListModel> Items { get; set; } = new();
	protected RecipeListModel? SelectedRecipe {  get; set; }
	protected decimal Scale { get; set; } = 1M;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		await DoSearch(1);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchRecipes.Query(SearchModel));
		IsLoading = false;
	}

	protected async Task HandleAddClicked(RecipeListModel recipe)
	{
		IsWorking = true;

		SelectedRecipe = recipe;
		SelectListModel model = new()
		{
			ListSid = ShoppingList.Sid,
			RecipeSid = recipe.Sid,
			Scale = Scale
		};

		Items = await Mediator.Send(new GetAddToListItems.Query(model));
		IsSelecting = false;
		IsWorking = false;
		StateHasChanged();
	}

	protected async Task HandleAddToListClicked()
	{
		if (SelectedRecipe == null)
			return;

		IsWorking = true;
		var addedItems = Items.Where(x => x.Quantity > 0).ToList();
		AddRecipeToListModel model = new()
		{
			List = addedItems,
			RecipeSid = SelectedRecipe.Sid,
			RecipeTitle = SelectedRecipe.Title,
			ShoppingListSid = ShoppingList.Sid
		};
		var result = await Mediator.Send(new AddRecipeToList.Command(model));
		ShowNotification(result);
		await OnAddedComplete.InvokeAsync();
		IsWorking = false;
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}
}