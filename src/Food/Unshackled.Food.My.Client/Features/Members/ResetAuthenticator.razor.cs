using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Members.Actions;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Members;

public class ResetAuthenticatorBase : BaseComponent<Member>
{
	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsWorking;
	protected RecoveryCodesModel Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", "/member"));
		Breadcrumbs.Add(new BreadcrumbItem("Reset Authenticator Key", null, true));
	}

	protected async Task HandleResetClicked()
	{

		IsWorking = true;
		var result = await Mediator.Send(new ResetAuthenticatorKey.Command());
		ShowNotification(result);
		if (result.Success)
		{
			NavManager.NavigateTo("/member/2fa");
		}
		IsWorking = false;
	}
}