using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists;

public class DrawerAddBundleBase : BaseComponent
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