using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Members.Actions;
using Unshackled.Fitness.My.Client.Features.Members.Models;

namespace Unshackled.Fitness.My.Client.Features.Members;
public class ExternalLoginsBase : BaseComponent
{
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsLoading || IsWorking;
	protected ExternalLoginsModel Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", "/member"));
		Breadcrumbs.Add(new BreadcrumbItem("Manage External Logins", null, true));

		Model = await Mediator.Send(new GetExternalLoginsModel.Query());
		IsLoading = false;
	}

	protected async Task HandleRemoveProviderClicked(FormProviderModel provider)
	{
		IsWorking = true;
		var result = await Mediator.Send(new RemoveLoginProvider.Command(provider));
		ShowNotification(result);
		if (result.Success)
		{
			Model.CurrentLogins.Remove(provider);
			Model.OtherLogins.Add(provider);
		}
		IsWorking = false;
	}
}