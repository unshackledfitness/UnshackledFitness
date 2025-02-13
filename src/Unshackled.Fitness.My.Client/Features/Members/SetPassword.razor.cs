using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Members.Actions;
using Unshackled.Fitness.My.Client.Features.Members.Models;

namespace Unshackled.Fitness.My.Client.Features.Members;
public class SetPasswordBase : BaseComponent
{
	protected FormSetPasswordModel Model { get; set; } = new();
	protected FormSetPasswordModel.Validator ModelValidator { get; set; } = new();

	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsWorking;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", "/member"));
		Breadcrumbs.Add(new BreadcrumbItem("Set Password", null, true));

		var accountStatus = await Mediator.Send(new GetCurrentAccountStatus.Query());
		if (accountStatus.HasPassword)
			NavManager.NavigateTo("/member/change-password");
	}

	protected async Task HandleFormSubmitted()
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddPassword.Command(Model));
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo("/member");
		}
		IsWorking = false;
		StateHasChanged();
	}
}