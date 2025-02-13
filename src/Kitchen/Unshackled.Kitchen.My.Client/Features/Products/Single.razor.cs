using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.Products.Actions;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	protected enum Views
	{
		None,
		AddToList
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string ProductSid { get; set; } = string.Empty;
	protected ProductModel Product { get; set; } = new();
	protected List<ShoppingListModel> ShoppingLists { get; set; } = [];
	protected List<ImageModel> Images { get; set; } = [];
	protected bool IsLoading { get; set; } = true;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing || IsWorking;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		IsLoading = true;
		Product = await Mediator.Send(new GetProduct.Query(ProductSid));
		Images = await Mediator.Send(new ListProductImages.Query(ProductSid));
		IsLoading = false;
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Products", "/products", false));
		Breadcrumbs.Add(new BreadcrumbItem("Product", null, true));

		State.OnActiveMemberChange += StateHasChanged;

		ShoppingLists = await Mediator.Send(new ListShoppingLists.Query());
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected string GetDrawerTitle()
	{
		return DrawerView switch
		{
			Views.AddToList => "Add To List",
			_ => string.Empty
		};
	}

	protected void HandleAddToListClicked()
	{
		DrawerView = Views.AddToList;
	}

	protected async void HandleAddToListSubmitted(AddToListModel model)
	{
		IsEditing = true;

		model.ProductSids = [ProductSid];
		var result = await Mediator.Send(new AddToList.Command(model));
		ShowNotification(result);
		DrawerView = Views.None;
		IsEditing = false;
		StateHasChanged();
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(Product.HouseholdSid);
	}

	protected async Task HandleTogglePinnedClicked(ProductModel item)
	{
		IsWorking = true;
		var result = await Mediator.Send(new ToggleIsPinned.Command(item.Sid));
		if (result.Success)
		{
			item.IsPinned = result.Payload;
		}
		ShowNotification(result);
		IsWorking = false;
	}
}