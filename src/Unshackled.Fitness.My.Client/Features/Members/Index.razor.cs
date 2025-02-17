using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Members.Actions;
using Unshackled.Fitness.My.Client.Features.Members.Models;

namespace Unshackled.Fitness.My.Client.Features.Members;

public partial class IndexBase : BaseComponent
{
	protected CurrentAccountStatusModel AccountStatus { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", null, true));

		AccountStatus = await Mediator.Send(new GetCurrentAccountStatus.Query());
	}
}
