using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Ingredients.Actions;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Ingredients;

public class FormAddSubstitutionBase : BaseSearchComponent<SearchProductModel, ProductListModel, Member>
{
	[Parameter] public SearchProductModel StartingSearch { get; set; } = new();
	[Parameter] public EventCallback<string> OnSubstitutionAdded { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		SearchModel.Brand = StartingSearch.Brand;
		SearchModel.Title = StartingSearch.Title;
		await DoSearch(1);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchProducts.Query(SearchModel));
		IsLoading = false;
	}

	protected async Task HandleAddClicked(ProductListModel model)
	{
		IsWorking = true;
		await OnSubstitutionAdded.InvokeAsync(model.Sid);
		IsWorking = false;
	}
}