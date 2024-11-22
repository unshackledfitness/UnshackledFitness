using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Members;

public partial class IndexBase : BaseComponent<Member>
{
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", null, true));
	}
}
