using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.ProductBundles.Actions;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ProductBundles;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string ProductBundleSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected ProductBundleModel ProductBundle { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Product Bundles", "/product-bundles", false));
		Breadcrumbs.Add(new BreadcrumbItem("Product Bundle", null, true));

		State.OnActiveMemberChange += StateHasChanged;

		ProductBundle = await Mediator.Send(new GetProductBundle.Query(ProductBundleSid));
		IsLoading = false;
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected void HandleSectionEditing(bool editing)
	{
		IsEditing = editing;
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(ProductBundle.HouseholdSid);
	}
}