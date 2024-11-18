using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.Households.Actions;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Households;

public class IndexBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	protected const string FormId = "formAddHousehold";
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsLoading || IsWorking;
	protected List<HouseholdListModel> Households { get; set; } = new();
	protected FormHouseholdModel AddModel { get; set; } = new();
	protected Member ActiveMember => (Member)State.ActiveMember;

	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Household";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Households", null, true));

		Households = await Mediator.Send(new ListHouseholds.Query());
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		AddModel = new();
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted(FormHouseholdModel model)
	{
		DrawerOpen = false;
		IsWorking = true;
		var result = await Mediator.Send(new AddHousehold.Command(model));
		ShowNotification(result);
		if (result.Success && result.Payload != null)
		{
			Households.Add(result.Payload);
			Households = Households
				.OrderBy(x => x.Title)
				.ToList();

			// Is the new active household
			if(ActiveMember.ActiveHousehold == null)
			{
				await Mediator.GetActiveMember();
			}
		}
		IsWorking = false;
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleCloseClicked(HouseholdListModel household)
	{
		IsWorking = true;
		await Mediator.CloseMemberHousehold(household.Sid);
		IsWorking = false;
	}

	protected async Task HandleRejectInviteClicked(HouseholdListModel household)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Rejection",
				"Are you sure you want to reject this household invite?",
				yesText: "Yes", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new RejectInvite.Command(household.Sid));
			if (result.Success)
			{
				Households.Remove(household);
				StateHasChanged();
			}
			ShowNotification(result);
			IsWorking = false;
		}
	}

	protected async Task HandleJoinClicked(HouseholdListModel household)
	{
		IsWorking = true;
		var result = await Mediator.Send(new JoinHousehold.Command(household.Sid));
		if (result.Success && result.Payload != null)
		{
			household.IsInvite = false;
			household.MemberSid = result.Payload.MemberSid;
			household.DateCreatedUtc = result.Payload.DateCreatedUtc;
			household.DateLastModifiedUtc = result.Payload.DateLastModifiedUtc;

			if(ActiveMember.ActiveHousehold == null)
			{
				await Mediator.OpenMemberHousehold(household.Sid);
			}
			StateHasChanged();
		}
		ShowNotification(result);
		IsWorking = false;
	}

	protected async Task HandleOpenClicked(HouseholdListModel household)
	{
		IsWorking = true;
		await Mediator.OpenMemberHousehold(household.Sid);
		IsWorking = false;
	}
}
