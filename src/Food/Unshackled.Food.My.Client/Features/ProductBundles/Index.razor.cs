using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.ProductBundles.Actions;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Food.My.Client.Features.ProductBundles;

public class IndexBase : BaseSearchComponent<SearchProductBundlesModel, ProductBundleListModel, Member>
{
	[Inject] protected ClientConfiguration ClientConfig { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	protected const string FormId = "formAddProductBundle";
	protected FormProductBundleModel FormModel { get; set; } = new();
	protected bool Adding { get; set; } = false;
	protected bool IsSaving { get; set; } = false;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Product Bundles", null, true));

		SearchKey = "searchProductBundles";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(SearchModel.Page);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchProductBundles.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		Adding = true;
	}

	protected void HandleCancelClicked()
	{
		Adding = false;
	}

	protected async Task HandleFormAddSubmit(FormProductBundleModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new AddProductBundle.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo($"/product-bundles/{result.Payload}");
		}
		Adding = false;
		IsSaving = false;
	}
}