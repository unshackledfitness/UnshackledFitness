using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Extensions;
using Unshackled.Kitchen.My.Client.Features.Stores.Actions;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Stores;

public class SectionLocationsBase : BaseSectionComponent<Member>
{
	protected enum Views
	{
		None,
		Add,
		Edit,
		BulkAdd
	}
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string StoreSid { get; set; } = string.Empty;

	protected List<FormStoreLocationModel> Locations { get; set; } = [];
	protected FormStoreLocationModel AddFormModel { get; set; } = new();
	protected FormBulkAddLocationModel BulkAddFormModel { get; set; } = new();
	protected FormStoreLocationModel EditingModel { get; set; } = new();
	protected bool IsWorking { get; set; } = false;
	protected bool IsSorting { get; set; } = false;
	protected bool DisableControls => !CanEdit || IsWorking || IsSorting || DisableSectionControls;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected bool CanEdit => ActiveMember.HasHouseholdPermissionLevel(PermissionLevels.Write);

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Aisle/Department",
		Views.Edit => "Edit Aisle/Department",
		Views.BulkAdd => "Bulk Add Aisle/Department",
		_ => string.Empty
	};

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Locations = await Mediator.Send(new ListStoreLocations.Query(StoreSid));
	}

	protected async Task HandleAddClicked()
	{
		AddFormModel = new()
		{
			StoreSid = StoreSid,
			SortOrder = Locations.Count,
		};
		DrawerView = Views.Add;
		await UpdateIsEditingSection(true);
	}

	protected async Task HandleAddFormSubmitted(FormStoreLocationModel model)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);

		var result = await Mediator.Send(new AddLocation.Command(model));
		if (result.Success)
		{
			Locations = await Mediator.Send(new ListStoreLocations.Query(StoreSid));
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleBulkAddClicked()
	{
		BulkAddFormModel = new()
		{
			StoreSid = StoreSid,
			NumberToAdd = 1,
			Prefix = "Aisle ",
			SortDescending = false
		};
		DrawerView = Views.BulkAdd;
		await UpdateIsEditingSection(true);
	}

	protected async Task HandleBulkAddFormSubmitted(FormBulkAddLocationModel model)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);

		var result = await Mediator.Send(new BulkAddLocations.Command(model));
		if (result.Success)
		{
			Locations = await Mediator.Send(new ListStoreLocations.Query(StoreSid));
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleCancelClicked()
	{
		DrawerView = Views.None;
		await UpdateIsEditingSection(false);
	}

	protected async Task HandleDeleteClicked()
	{
		DrawerView = Views.None;

		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this aisle/department?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteLocation.Command(EditingModel.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				Locations = await Mediator.Send(new ListStoreLocations.Query(StoreSid));
			}
			IsWorking = await UpdateIsEditingSection(false);
			StateHasChanged();
		}
	}

	protected async Task HandleEditClicked(FormStoreLocationModel model)
	{
		await UpdateIsEditingSection(true);
		EditingModel = model;
		DrawerView = Views.Edit;
	}

	protected async Task HandleEditFormSubmitted(FormStoreLocationModel model)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);

		var result = await Mediator.Send(new UpdateLocation.Command(model));
		if (result.Success)
		{
			var loc = Locations.Where(x => x.Sid == model.Sid).Single();
			loc.Title = model.Title;
			loc.Description = model.Description;
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsSorting = isSorting;
		StateHasChanged();
	}

	protected async Task HandleSortChanged(List<FormStoreLocationModel> list)
	{
		IsWorking = true;

		UpdateSortsModel model = new()
		{
			Locations = list,
			StoreSid = StoreSid
		};
		var result = await Mediator.Send(new UpdateLocationSorts.Command(model));
		if (result.Success)
		{
			Locations = list;
		}

		IsWorking = false;
		StateHasChanged();
		ShowNotification(result);
	}
}