using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public partial class IndexBase : BaseSearchComponent<SearchRecipeModel, RecipeListModel>
{
	protected Member ActiveMember => (Member)State.ActiveMember;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Recipes", null, true));

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
	}

	protected void HandleAddClicked()
	{
		NavManager.NavigateTo("/recipes/add");
	}

}
