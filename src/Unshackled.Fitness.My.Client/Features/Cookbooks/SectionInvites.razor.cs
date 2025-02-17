using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks;

public class SectionInvitesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public CookbookModel Cookbook { get; set; } = new();

	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; } = false;
	protected List<InviteListModel> Invites { get; set; } = new();
	protected FormAddInviteModel AddModel { get; set; } = new();
	protected FormAddInviteModel.Validator AddModelValidator { get; set; } = new();

	protected bool DisableControls => DisableSectionControls || IsWorking;
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Invite";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Invites = await Mediator.Send(new ListMemberInvites.Query(Cookbook.Sid));
		IsLoading = false;
	}

	protected async Task HandleAddClicked()
	{
		AddModel = new();
		DrawerOpen = await UpdateIsEditingSection(true);
	}

	protected async Task HandleCancelClicked()
	{
		DrawerOpen = await UpdateIsEditingSection(false);
	}

	protected async Task HandleDeleteClicked(InviteListModel model)
	{
		await UpdateIsEditingSection(true);
		
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this invite?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = true;
			var result = await Mediator.Send(new DeleteInvite.Command(model.Sid));

			if (result.Success)
			{
				Invites.Remove(model);
			}
			ShowNotification(result);
			IsWorking = false;
		}

		await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted()
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddInvite.Command(Cookbook.Sid, AddModel));
		if (result.Success && result.Payload != null)
		{
			Invites.Add(result.Payload);
			Invites = Invites.OrderBy(x => x.Email).ToList();
		}
		ShowNotification(result);
		IsWorking = false;
		DrawerOpen = await UpdateIsEditingSection(false);
	}
}