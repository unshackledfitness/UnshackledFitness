using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Ingredients.Actions;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients;

public class IndexBase : BaseSearchComponent<SearchIngredientModel, IngredientListModel>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Ingredients", null, true));

		SearchKey = "searchIngredients";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(SearchModel.Page);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchIngredients.Query(SearchModel));
		IsLoading = false;
	}
}