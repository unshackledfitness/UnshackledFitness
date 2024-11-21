using MudBlazor;
using Unshackled.Food.My.Client.Features.Stores.Actions;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Stores;

public class IndexBase : BaseSearchComponent<SearchStoreModel, StoreListModel>
{
	protected enum Views
	{
		None,
		Add
	}

	protected const string FormId = "formAddStore";
	protected FormStoreModel FormModel { get; set; } = new();
	protected bool IsSaving { get; set; } = false;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Store",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Stores", null, true));

		SearchKey = "searchStores";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(SearchModel.Page);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchStores.Query(SearchModel));
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

	protected async Task HandleFormAddSubmit(FormStoreModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new AddStore.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo($"/stores/{result.Payload}");
		}
		DrawerView = Views.None;
		IsSaving = false;
	}
}