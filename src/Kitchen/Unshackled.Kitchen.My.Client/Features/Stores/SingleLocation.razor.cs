using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.Stores.Actions;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Kitchen.My.Client.Features.Stores;

public class SingleLocationBase : BaseComponent<Member>, IAsyncDisposable
{
	[Inject] protected StorageSettings StorageSettings { get; set; } = default!;

	protected enum Views
	{
		None,
		ChangeLocation
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string StoreSid { get; set; } = string.Empty;
	[Parameter] public string StoreLocationSid { get; set; } = string.Empty;

	[Parameter]
	[SupplyParameterFromQuery(Name = "list")] 
	public string? List {  get; set; }

	protected StoreLocationModel StoreLocation { get; set; } = new();
	protected List<FormProductLocationModel> FormProducts { get; set; } = [];
	protected List<FormStoreLocationModel> Locations { get; set; } = [];
	protected bool IsLoading { get; set; } = true;
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsWorking || IsSorting;
	protected bool CanEdit => ActiveMember.HasHouseholdPermissionLevel(PermissionLevels.Write);
	protected FormProductLocationModel CurrentItem { get; set; } = new();
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;

	protected string DrawerTitle => DrawerView switch
	{
		Views.ChangeLocation => "Change Aisle/Department",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (string.IsNullOrEmpty(List))
		{
			Breadcrumbs.Add(new BreadcrumbItem("Stores", "/stores", false));
			Breadcrumbs.Add(new BreadcrumbItem("Store", $"/stores/{StoreSid}", false));
			Breadcrumbs.Add(new BreadcrumbItem("Aisle", null, true));
		}
		else
		{
			Breadcrumbs.Add(new BreadcrumbItem("Shopping Lists", "/shopping-lists", false));
			Breadcrumbs.Add(new BreadcrumbItem("Shopping List", $"/shopping-lists/{List}", false));
			Breadcrumbs.Add(new BreadcrumbItem("Aisle", null, true));
		}

		State.OnActiveMemberChange += StateHasChanged;

		await LoadProducts();

		Locations = await Mediator.Send(new ListStoreLocations.Query(StoreSid));

		IsLoading = false;
		StateHasChanged();
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected void HandleChangeAisleClicked(FormProductLocationModel item)
	{
		CurrentItem = item;
		DrawerView = Views.ChangeLocation;
	}

	protected async Task HandleChangeLocationSubmitted(string locationSid)
	{
		DrawerView = Views.None;

		if (CurrentItem.StoreLocationSid != locationSid)
		{
			IsWorking = true;
			ChangeLocationModel model = new()
			{
				ProductSid = CurrentItem.ProductSid,
				StoreSid = StoreSid,
				StoreLocationSid = locationSid
			};
			var result = await Mediator.Send(new ChangeProductLocation.Command(model));
			if (result.Success)
			{
				await LoadProducts();
			}
			ShowNotification(result);
			IsWorking = false;
			StateHasChanged();
		}
	}

	protected async Task HandleDeleteClicked(FormProductLocationModel item)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this item from the aisle/department?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;

			var result = await Mediator.Send(new DeleteProductLocation.Command(StoreSid, item.ProductSid));
			ShowNotification(result);
			if (result.Success)
			{
				await LoadProducts();
			}
			IsWorking = false;
			StateHasChanged();
		}
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsSorting = isSorting;
		StateHasChanged();
	}

	protected async Task HandleSortChanged(List<FormProductLocationModel> list)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateProductLocations.Command(StoreLocationSid, list));
		ShowNotification(result);
		if (result.Success)
		{
			FormProducts = list;
		}
		IsWorking = false;
		StateHasChanged();
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(StoreLocation.HouseholdSid);
	}

	protected async Task LoadProducts()
	{
		StoreLocation = await Mediator.Send(new GetStoreLocation.Query(StoreLocationSid));
		FormProducts = StoreLocation.ProductLocations
			.Select(x => (FormProductLocationModel)x.Clone())
			.ToList();
	}
}