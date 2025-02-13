using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists;

public class DrawerAddBundleBase : BaseComponent<Member>
{
	[Parameter] public EventCallback<string> OnProductBundleAdded { get; set; }
	protected List<ProductBundleListModel> Bundles { get; set; } = new();
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Bundles = await Mediator.Send(new ListProductBundles.Query());
		IsLoading = false;
		StateHasChanged();
	}

	protected async Task HandleAddBundleClicked(string sid)
	{
		IsWorking = true;
		await OnProductBundleAdded.InvokeAsync(sid);
		IsWorking = false;
	}
}