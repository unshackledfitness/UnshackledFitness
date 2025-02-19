using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes;

public partial class IndexBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel>
{
	protected List<RecipeTagSelectItem> RecipeTags { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Cookbook Recipes", null, true));

		RecipeTags = await Mediator.Send(new ListRecipeTags.Query());

		SearchKey = "SearchCookbookRecipes";
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
	}

}
