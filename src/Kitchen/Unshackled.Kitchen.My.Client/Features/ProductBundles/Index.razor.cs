using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Actions;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Kitchen.My.Client.Features.ProductBundles;

public class IndexBase : BaseSearchComponent<SearchProductBundlesModel, ProductBundleListModel, Member>
{
	[Inject] protected ClientConfiguration ClientConfig { get; set; } = default!;
	[Inject] protected IDialogService DialogService { get; set; } = default!;

	protected enum Views
	{
		None,
		Add
	}

	protected const string FormId = "formAddProductBundle";
	protected FormProductBundleModel FormModel { get; set; } = new();
	protected bool IsSaving { get; set; } = false;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Bundle",
		_ => string.Empty
	};

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
		DrawerView = Views.Add;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleFormAddSubmit(FormProductBundleModel model)
	{
		DrawerView = Views.None;
		IsSaving = true;
		var result = await Mediator.Send(new AddProductBundle.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo($"/product-bundles/{result.Payload}");
		}
		IsSaving = false;
	}
}