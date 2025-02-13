using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Households.Actions;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Households;

public class SectionMembersBase : BaseSearchComponent<MemberSearchModel, MemberListModel, Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public HouseholdModel Household { get; set; } = new();
	[Parameter] public EventCallback<HouseholdModel> HouseholdChanged { get; set; }
	[Parameter] public bool IsEditMode { get; set; }
	[Parameter] public bool DisableSectionControls { get; set; } = false;
	[Parameter] public EventCallback<bool> OnIsEditingChange { get; set; }
	protected bool IsEditing { get; set; } = false;
	protected MemberListModel EditingMember { get; set; } = new();
	protected FormMemberModel FormMember { get; set; } = new();
	protected override bool DisableControls => DisableSectionControls || IsLoading || IsWorking;
	protected string DrawerIcon => Icons.Material.Filled.Edit;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Edit Member";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		await DoSearch(1);
	}

	protected bool DisableMenu(MemberListModel model)
	{
		// DisableControls or current member or member is owner
		return DisableControls || State.ActiveMember.Sid == model.MemberSid || model.MemberSid == Household.MemberSid;
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchMembers.Query(Household.Sid, SearchModel));
		IsLoading = false;
	}

	protected async Task HandleCancelEditClicked()
	{
		EditingMember.IsEditing = false;
		FormMember = new();
		DrawerOpen = await UpdateIsEditing(false);
	}

	protected async Task HandleDeleteClicked(MemberListModel model)
	{
		await UpdateIsEditing(true);
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this member?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteMember.Command(Household.Sid, model.MemberSid));

			if (result.Success)
			{
				await DoSearch(SearchModel.Page);
			}
			ShowNotification(result);
			IsWorking = false;
		}
		await UpdateIsEditing(false);
	}

	protected async Task HandleEditClicked(MemberListModel model)
	{
		EditingMember = model;
		FormMember = new()
		{
			HouseholdSid = Household.Sid,
			MemberSid = model.MemberSid,
			PermissionLevel = model.PermissionLevel
		};
		DrawerOpen = await UpdateIsEditing(true);
	}

	protected async Task HandleFormEditSubmitted()
	{
		IsWorking = true;
		DrawerOpen = await UpdateIsEditing(false);
		var result = await Mediator.Send(new UpdateMember.Command(FormMember));
		ShowNotification(result);
		if (result.Success)
		{
			EditingMember.PermissionLevel = FormMember.PermissionLevel;
			FormMember = new();
		}
		IsWorking = false;
	}

	protected async Task HandleMakeOwnerClicked(MemberListModel model)
	{
		await UpdateIsEditing(true);
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Change",
				"Are you sure you want to change the household owner?",
				yesText: "Yes", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			MakeOwnerModel m = new()
			{
				HouseholdSid = Household.Sid,
				MemberSid = model.MemberSid,
			};
			var result = await Mediator.Send(new MakeOwner.Command(m));
			ShowNotification(result);
			if (result.Success)
			{
				Household.MemberSid = model.MemberSid;
				await HouseholdChanged.InvokeAsync(Household);

				await DoSearch(SearchModel.Page);
			}
			IsWorking = false;
		}
		await UpdateIsEditing(false);
	}

	protected async Task<bool> UpdateIsEditing(bool value)
	{
		await OnIsEditingChange.InvokeAsync(value);
		return value;
	}
}