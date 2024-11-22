using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.ProductBundles.Actions;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ProductBundles;

public class DrawerAddProductsBase : BaseSearchComponent<SearchProductsModel, ProductListModel, Member>
{
	[Parameter] public string ProductBundleSid { get; set; } = string.Empty;
	[Parameter] public EventCallback<Dictionary<string, int>> OnProductsAdded { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }
	[Parameter] public bool SearchOnOpen { get; set; }
	public bool IsSaving { get; set; } = false;
	protected bool MaxSelectionReached => Selected.Count == FoodGlobals.MaxSelectionLimit;
	protected List<ProductListModel> Selected { get; set; } = new();

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		if (SearchOnOpen)
			await DoSearch(1);
	}

	public bool DisableCheckbox(string sid)
	{
		return DisableControls
			|| (!IsSelected(sid) && MaxSelectionReached);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;
		SearchModel.ProductBundleSid = ProductBundleSid;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchProducts.Query(SearchModel));
		IsLoading = false;
	}

	protected async Task HandleAddSelected()
	{
		var model = Selected
			.Select(x => new { x.Sid, x.Quantity })
			.ToDictionary(x => x.Sid, x => x.Quantity);

		await OnProductsAdded.InvokeAsync(model);
		Selected.Clear();
	}

	protected async Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}

	protected void HandleCheckboxChanged(bool value, ProductListModel model)
	{
		if (value)
			Selected.Add(model);
		else
			Selected.Remove(model);
	}

	protected void HandleClearSelected()
	{
		Selected.Clear();
	}

	protected void HandleQuantityChanged(ProductListModel model, int quantity)
	{
		model.Quantity = quantity;
		if (!IsSelected(model.Sid))
			Selected.Add(model);
	}


	public bool IsSelected(string sid)
	{
		return Selected.Where(x => x.Sid == sid).Any();
	}
}