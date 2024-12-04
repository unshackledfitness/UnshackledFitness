using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Members.Actions;
using Unshackled.Kitchen.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Members;

public partial class IndexBase : BaseComponent<Member>
{
	protected CurrentAccountStatusModel AccountStatus { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", null, true));

		AccountStatus = await Mediator.Send(new GetCurrentAccountStatus.Query());
	}
}
