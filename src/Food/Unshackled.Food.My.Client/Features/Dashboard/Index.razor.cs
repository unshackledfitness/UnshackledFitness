using Unshackled.Food.Core;
using Unshackled.Food.My.Client.Features.Dashboard.Actions;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Dashboard;

public class IndexBase : BaseComponent
{
	protected enum Views
	{
		None,
		AddToList
	}

	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected List<ListGroupModel> PinnedCategories { get; set; } = [];
	protected List<PinnedProductModel> PinnedProducts { get; set; } = [];
	protected List<ShoppingListModel> ShoppingLists { get; set; } = [];
	protected string? SelectedCategorySid { get; set; }
	protected string SelectedProductSid { get; set; } = string.Empty;
	protected List<PinnedProductModel> FilteredProducts => PinnedProducts
		.Where(x => x.ProductCategorySid == SelectedCategorySid)
		.ToList();
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.AddToList => "Add To List",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		PinnedProducts = await Mediator.Send(new ListPinnedProducts.Query());

		PinnedCategories = PinnedProducts
			.Distinct(new DistinctProductCategoryComparer())
			.Select(x => new ListGroupModel
			{
				Sid = x.ProductCategorySid ?? FoodGlobals.UncategorizedKey,
				Title = string.IsNullOrEmpty(x.Category) ? "Uncategorized" : x.Category,
			})
			.OrderBy(x => x.Title)
			.ToList();

		ShoppingLists = await Mediator.Send(new ListShoppingLists.Query());

		IsLoading = false;
	}

	protected void HandleAddToListClicked(string productSid)
	{
		SelectedProductSid = productSid;
		DrawerView = Views.AddToList;
	}

	protected async void HandleAddToListSubmitted(AddToListModel model)
	{
		IsWorking = true;
		model.ProductSid = SelectedProductSid;
		var result = await Mediator.Send(new AddToList.Command(model));
		DrawerView = Views.None;
		ShowNotification(result);
		IsWorking = false;
		StateHasChanged();
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleCategoryClicked(string catSid)
	{
		SelectedCategorySid = catSid;
		StateHasChanged();
	}

	protected void HandleCategoryClearClicked()
	{
		SelectedCategorySid = null;
		StateHasChanged();
	}
}
