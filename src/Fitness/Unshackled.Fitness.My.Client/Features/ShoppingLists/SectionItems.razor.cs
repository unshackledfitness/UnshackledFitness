using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists;

public class SectionItemsBase : BaseSectionComponent<Member>
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;

	protected enum Views
	{
		None,
		AddBundle,
		AddProducts,
		AddRecipe,
		ManageItem,
		ChangeLocation,
		QuickProduct
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ShoppingListModel ShoppingList { get; set; } = new();

	protected List<FormListItemModel> Items { get; set; } = new();
	protected List<ListGroupModel> StoreLocations { get; set; } = new();
	protected List<ListGroupModel> ListGroups { get; set; } = new();
	protected List<FormListItemModel> GroupedItems { get; set; } = new();

	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsWorking;
	protected List<string> SelectedSids { get; set; } = new();
	protected bool MaxSelectionReached => SelectedSids.Count == FitnessGlobals.MaxSelectionLimit;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView {  get; set; } = Views.None;
	protected bool HideInCart { get; set; } = true;
	protected FormListItemModel EditingModel { get; set; } = new();
	protected bool CanEdit => ActiveMember.HasHouseholdPermissionLevel(PermissionLevels.Write);

	private string currentStoreSid = string.Empty;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		bool storeChanged = ShoppingList.StoreSid != currentStoreSid;
		if (ShoppingList.HasStore)
		{
			if (storeChanged)
			{
				StoreLocations = await Mediator.Send(new ListStoreLocations.Query(ShoppingList.StoreSid!));
				currentStoreSid = ShoppingList.StoreSid!;
			}
		}
		else
		{
			StoreLocations.Clear();
			currentStoreSid= string.Empty;
		}

		if (!IsLoading && storeChanged) // Only run after initialized (IsLoading = false) and store changes
		{
			OrganizeShoppingList();
		}
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		HideInCart = ActiveMember.Settings.HideIsInCart;

		Items = await Mediator.Send(new ListItems.Query(ShoppingList.Sid));

		OrganizeShoppingList();

		IsLoading = false;
	}

	public bool DisableCheckbox(string sid)
	{
		return DisableControls
			|| (!SelectedSids.Contains(sid) && MaxSelectionReached);
	}

	protected string GetDrawerTitle()
	{
		return DrawerView switch
		{
			Views.AddBundle => "Add Product Bundle",
			Views.AddProducts => "Add Products",
			Views.AddRecipe => "Add Recipe",
			Views.ManageItem => "Change Quantity",
			Views.ChangeLocation => "Change Location",
			_ => string.Empty
		};
	}

	protected void HandleAddBundleClicked()
	{
		DrawerView = Views.AddBundle;
	}

	protected void HandleAddProductClicked()
	{
		DrawerView = Views.AddProducts;
	}

	protected void HandleAddQuickClicked()
	{
		DrawerView = Views.QuickProduct;
	}

	protected void HandleAddRecipeClicked()
	{
		DrawerView = Views.AddRecipe;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleChangeLocationClicked()
	{
		DrawerView = Views.ChangeLocation;
	}

	protected void HandleManageItemClicked(FormListItemModel model)
	{
		EditingModel = model;
		DrawerView = Views.ManageItem;
	}

	protected void HandleCheckboxChanged(bool value, string sid)
	{
		if (value)
			SelectedSids.Add(sid);
		else
			SelectedSids.Remove(sid);
	}

	protected async Task HandleClearList()
	{
		IsWorking = await UpdateIsEditingSection(true);

		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Clear",
				"Are you sure you want to remove all items from the list?",
				yesText: "Remove", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			var result = await Mediator.Send(new ClearShoppingList.Command(ShoppingList.Sid));
			if (result.Success)
			{
				Items.Clear();
				GroupedItems.Clear();
			}
			ShowNotification(result);
		}

		IsWorking = await UpdateIsEditingSection(false);
	}

	protected async Task HandleDeleteClicked()
	{
		DrawerView = Views.None;

		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this item from the list?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = await UpdateIsEditingSection(true);

			DeleteProductModel deleteModel = new()
			{
				ProductSid = EditingModel.ProductSid,
				ShoppingListSid = ShoppingList.Sid
			};

			var result = await Mediator.Send(new DeleteProductFromList.Command(deleteModel));
			ShowNotification(result);
			if (result.Success)
			{
				// Remove from original source list
				var item = Items.Where(x => x.ProductSid == EditingModel.ProductSid).Single();
				Items.Remove(item);

				// Remove from group items list
				GroupedItems.Remove(EditingModel);
			}
			IsWorking = await UpdateIsEditingSection(false);
			StateHasChanged();
		}
	}

	protected async Task HandleProductBundleAdded(string sid)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);
		AddProductBundleModel model = new()
		{
			ProductBundleSid = sid,
			ShoppingListSid = ShoppingList.Sid
		};
		var result = await Mediator.Send(new AddBundleToList.Command(model));
		if (result.Success)
		{
			Items = await Mediator.Send(new ListItems.Query(ShoppingList.Sid));

			OrganizeShoppingList();
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleProductsAdded(Dictionary<string, int> products)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);
		AddProductsModel model = new()
		{
			Products = products,
			ShoppingListSid = ShoppingList.Sid
		};
		var result = await Mediator.Send(new AddProductsToList.Command(model));
		if (result.Success)
		{
			Items = await Mediator.Send(new ListItems.Query(ShoppingList.Sid));

			OrganizeShoppingList();
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleRecipeAdded()
	{
		DrawerView = Views.None;
		Items = await Mediator.Send(new ListItems.Query(ShoppingList.Sid));
		OrganizeShoppingList();
	}

	protected async Task HandleResetList()
	{
		IsWorking = await UpdateIsEditingSection(true);
		var result = await Mediator.Send(new ResetShoppingList.Command(ShoppingList.Sid));
		if (result.Success)
		{
			foreach (var item in Items)
			{
				item.IsInCart = false;
			}

			OrganizeShoppingList();
		}
		ShowNotification(result);
		IsWorking = await UpdateIsEditingSection(false);
	}

	protected async Task HandleSaveLocationClicked(string locationSid)
	{
		DrawerView = Views.None;
		if (ShoppingList.HasStore)
		{
			IsWorking = await UpdateIsEditingSection(true);
			UpdateLocationModel model = new()
			{
				ProductSid = EditingModel.ProductSid,
				StoreSid = ShoppingList.StoreSid!,
				StoreLocationSid = locationSid
			};
			var result = await Mediator.Send(new UpdateLocation.Command(model));
			if (result.Success)
			{
				if (result.Payload != null)
				{
					var sourceItem = Items.Where(x => x.ProductSid == model.ProductSid).Single();

					sourceItem.StoreLocationSid = result.Payload.StoreLocationSid;
					sourceItem.LocationSortOrder = result.Payload.LocationSortOrder;
					sourceItem.ListGroupSid = result.Payload.ListGroupSid;
					sourceItem.SortOrder = result.Payload.SortOrder;

					OrganizeShoppingList();
				}
			}
			ShowNotification(result);
			IsWorking = await UpdateIsEditingSection(false);
			StateHasChanged();
		}
	}

	protected async Task HandleSaveQuantityClicked(int quantity) 
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);
		UpdateQuantityModel model = new()
		{
			ProductSid = EditingModel.ProductSid,
			ShoppingListSid = ShoppingList.Sid,
			Quantity = quantity
		};
		var result = await Mediator.Send(new UpdateQuantity.Command(model));
		if (result.Success)
		{
			EditingModel.Quantity = quantity;

			var sourceItem = Items.Where(x => x.ProductSid == model.ProductSid).Single();
			sourceItem.Quantity = quantity;
		}
		ShowNotification(result);
		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleSaveQuickProductClicked(AddQuickProductModel model)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);
		
		model.ShoppingListSid = ShoppingList.Sid;
		var result = await Mediator.Send(new AddQuickProduct.Command(model));
		if (result.Success)
		{
			Items = await Mediator.Send(new ListItems.Query(ShoppingList.Sid));

			OrganizeShoppingList();
		}
		ShowNotification(result);
		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleToggleIsInCart(FormListItemModel item)
	{
		if (CanEdit)
		{
			IsWorking = await UpdateIsEditingSection(true);
			ToggleListItemModel model = new()
			{
				ProductSid = item.ProductSid,
				ShoppingListSid = ShoppingList.Sid,
				ToggleValue = !item.IsInCart
			};
			var result = await Mediator.Send(new ToggleIsInCart.Command(model));
			if (result.Success)
			{
				item.IsInCart = model.ToggleValue;

				var sourceItem = Items.Where(x => x.ProductSid == model.ProductSid).Single();
				sourceItem.IsInCart = model.ToggleValue;
			}
			else
			{
				ShowNotification(result);
			}
			IsWorking = await UpdateIsEditingSection(false);
			StateHasChanged();
		}
	}

	public bool IsEndOfList()
	{
		return Items.Any() && Items.Where(x => x.IsInCart == true).Count() == Items.Count();
	}

	protected bool IsGroupItemVisible(string sid)
	{
		return !HideInCart || GroupedItems
			.Where(x => x.ListGroupSid == sid && x.IsInCart == false)
			.Any();
	}

	protected string IsInCartCss(FormListItemModel item, string? additionalClasses = null)
	{
		string css = string.Empty;

		if (!string.IsNullOrEmpty(additionalClasses))
			css = additionalClasses;

		if (item.IsInCart)
		{
			string inCartCss = "strike-through mud-tertiary-text";
			css += string.IsNullOrEmpty(css) ? inCartCss : $" {inCartCss}";
		}

		return css;
	}

	protected bool IsLastInGroup(FormListItemModel item)
	{
		int idx = GroupedItems.IndexOf(item);

		// Last item overall
		if (idx == GroupedItems.Count - 1) return true;

		// Last item in group
		if (item.ListGroupSid != GroupedItems[idx + 1].ListGroupSid) return true;

		return false;
	}

	private void OrganizeShoppingList()
	{
		ListGroups = StoreLocations
			.OrderBy(x => x.SortOrder)
			.ToList();
		ListGroups.Insert(0, new ListGroupModel { Sid = FitnessGlobals.UncategorizedKey, Title = "Location Not Set", SortOrder = -1 });

		GroupedItems = Items
			.OrderBy(x => x.LocationSortOrder)
			.ThenBy(x => x.SortOrder)
			.ThenBy(x => x.Title)
			.Select(x => (FormListItemModel)x.Clone())
			.ToList();

		StateHasChanged();
	}
}