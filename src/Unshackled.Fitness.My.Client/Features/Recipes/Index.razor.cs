using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public partial class IndexBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel, Member>
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;

	protected enum Views
	{
		None,
		Add
	}

	protected const string FormId = "formAddRecipe";
	protected FormRecipeModel FormModel { get; set; } = new();
	protected List<RecipeTagSelectItem> RecipeTags { get; set; } = [];
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Recipe",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Recipes", null, true));

		RecipeTags = await Mediator.Send(new ListRecipeTags.Query());

		SearchKey = "SearchRecipes";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		await DoSearch(SearchModel.Page);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchRecipes.Query(SearchModel));
		IsLoading = false;
		StateHasChanged();
	}

	protected void HandleAddClicked()
	{
		DrawerView = Views.Add;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleFormAddSubmitted(FormRecipeModel model)
	{
		var result = await Mediator.Send(new AddRecipe.Command(model));
		ShowNotification(result);
		if (result.Success)
			NavManager.NavigateTo($"/recipes/{result.Payload}");
	}

}
