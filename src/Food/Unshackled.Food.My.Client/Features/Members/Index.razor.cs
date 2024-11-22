using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Members;

public partial class IndexBase : BaseComponent<Member>
{
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Settings", null, true));
	}
}
