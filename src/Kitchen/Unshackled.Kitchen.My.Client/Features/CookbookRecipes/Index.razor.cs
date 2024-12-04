using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Actions;
using Unshackled.Kitchen.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.CookbookRecipes;

public partial class IndexBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel, Member>
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
