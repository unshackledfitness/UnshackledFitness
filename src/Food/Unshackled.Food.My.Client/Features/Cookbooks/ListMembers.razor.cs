using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.Cookbooks.Actions;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Cookbooks;

public class ListMembersBase : BaseSearchComponent<MemberSearchModel, MemberListModel>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public CookbookModel Cookbook { get; set; } = new();
	[Parameter] public bool IsEditMode { get; set; }
	[Parameter] public bool DisableSectionControls { get; set; } = false;
	[Parameter] public EventCallback<bool> OnIsEditingChange { get; set; }
	protected bool IsAdding { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected MemberListModel EditingMember { get; set; } = new();
	protected FormMemberModel FormMember { get; set; } = new();
	protected override bool DisableControls => DisableSectionControls || IsLoading || IsWorking;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		await DoSearch(1);
	}

	protected override async Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		SearchResults = await Mediator.Send(new SearchMembers.Query(Cookbook.Sid, SearchModel));
		IsLoading = false;
	}

	protected async Task HandleCancelEditClicked()
	{
		EditingMember.IsEditing = false;
		FormMember = new();
		IsEditing = await UpdateIsEditing(false);
	}

	protected async Task HandleDeleteClicked(MemberListModel model)
	{
		IsEditing = await UpdateIsEditing(true);
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this member?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteMember.Command(Cookbook.Sid, model.MemberSid));

			if (result.Success)
			{
				await DoSearch(SearchModel.Page);
			}
			ShowNotification(result);
			IsWorking = false;
		}

		IsEditing = await UpdateIsEditing(false);
	}

	protected async Task HandleEditClicked(MemberListModel model)
	{
		IsEditing = await UpdateIsEditing(true);

		EditingMember = model;
		EditingMember.IsEditing = true;
		FormMember = new()
		{
			CookbookSid = Cookbook.Sid,
			MemberSid = model.MemberSid,
			PermissionLevel = model.PermissionLevel
		};
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditing(false);
	}

	protected async Task HandleFormEditSubmitted()
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateMember.Command(FormMember));
		ShowNotification(result);
		if (result.Success)
		{
			EditingMember.PermissionLevel = FormMember.PermissionLevel;
			EditingMember.IsEditing = false;
			FormMember = new();

			IsEditing = await UpdateIsEditing(false);
		}
		IsWorking = false;
	}

	protected async Task<bool> UpdateIsEditing(bool value)
	{
		await OnIsEditingChange.InvokeAsync(value);
		return value;
	}
}