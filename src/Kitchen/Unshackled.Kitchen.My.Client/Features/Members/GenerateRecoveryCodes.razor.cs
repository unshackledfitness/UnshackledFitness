using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Members.Actions;
using Unshackled.Kitchen.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Members;

public class GenerateRecoveryCodesBase : BaseComponent<Member>
{
	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsWorking;
	protected RecoveryCodesModel Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", "/member"));
		Breadcrumbs.Add(new BreadcrumbItem("Recovery Codes", null, true));
	}

	protected async Task HandleGenerateClicked()
	{
		IsWorking = true;
		var result = await Mediator.Send(new ResetRecoveryCodes.Command());
		ShowNotification(result);
		if (result.Success && result.Payload != null)
		{
			Model = result.Payload;
			StateHasChanged();
		}
		IsWorking = false;
	}
}