using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.Members.Actions;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Members;

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
