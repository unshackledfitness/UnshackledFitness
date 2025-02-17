using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks;

public class IndexBase : BaseComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	protected const string FormId = "formAddCookbook";
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsLoading || IsWorking;
	protected List<CookbookListModel> Cookbooks { get; set; } = new();
	protected FormCookbookModel AddModel { get; set; } = new();

	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Cookbook";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Cookbooks", null, true));

		Cookbooks = await Mediator.Send(new ListCookbooks.Query());
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		AddModel = new();
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted(FormCookbookModel model)
	{
		DrawerOpen = false;
		IsWorking = true;
		var result = await Mediator.Send(new AddCookbook.Command(model));
		ShowNotification(result);
		if (result.Success && result.Payload != null)
		{
			Cookbooks.Add(result.Payload);
			Cookbooks = Cookbooks
				.OrderBy(x => x.Title)
				.ToList();

			// Is the new active cookbook
			if(State.ActiveMember.ActiveCookbook == null)
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

	protected async Task HandleCloseClicked(CookbookListModel cookbook)
	{
		IsWorking = true;
		await Mediator.CloseMemberCookbook(cookbook.Sid);
		IsWorking = false;
	}

	protected async Task HandleRejectInviteClicked(CookbookListModel cookbook)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Rejection",
				"Are you sure you want to reject this cookbook invite?",
				yesText: "Yes", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new RejectInvite.Command(cookbook.Sid));
			if (result.Success)
			{
				Cookbooks.Remove(cookbook);
				StateHasChanged();
			}
			ShowNotification(result);
			IsWorking = false;
		}
	}

	protected async Task HandleJoinClicked(CookbookListModel cookbook)
	{
		IsWorking = true;
		var result = await Mediator.Send(new JoinCookbook.Command(cookbook.Sid));
		if (result.Success && result.Payload != null)
		{
			cookbook.IsInvite = false;
			cookbook.MemberSid = result.Payload.MemberSid;
			cookbook.DateCreatedUtc = result.Payload.DateCreatedUtc;
			cookbook.DateLastModifiedUtc = result.Payload.DateLastModifiedUtc;

			if(State.ActiveMember.ActiveCookbook == null)
			{
				await Mediator.OpenMemberCookbook(cookbook.Sid);
			}
			StateHasChanged();
		}
		ShowNotification(result);
		IsWorking = false;
	}

	protected async Task HandleOpenClicked(CookbookListModel cookbook)
	{
		IsWorking = true;
		await Mediator.OpenMemberCookbook(cookbook.Sid);
		IsWorking = false;
	}
}
