using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ShoppingLists;

public class IndexBase : BaseSearchComponent<SearchShoppingListsModel, ShoppingListModel, Member>
{
	protected const string FormId = "formAddShoppingList";
	protected FormShoppingListModel FormModel { get; set; } = new();
	protected bool IsSaving { get; set; } = false;
	protected bool DrawerOpen { get; set; } = false;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Shopping Lists", null, true));

		SearchKey = "searchShoppingLists";
		SearchModel = await GetLocalSetting(SearchKey) ?? new();

		await DoSearch(SearchModel.Page);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchShoppingLists.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		DrawerOpen = true;
	}

	protected void HandleCancelClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleFormAddSubmit(FormShoppingListModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new AddShoppingList.Command(model));
		ShowNotification(result);
		DrawerOpen = false;
		if (result.Success)
		{
			NavManager.NavigateTo($"/shopping-lists/{result.Payload}");
		}
		IsSaving = false;
	}
}