using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.Stores.Actions;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Stores;

public class SingleLocationBase : BaseComponent, IAsyncDisposable
{
	[Parameter] public string StoreSid { get; set; } = string.Empty;
	[Parameter] public string StoreLocationSid { get; set; } = string.Empty;

	[Parameter]
	[SupplyParameterFromQuery(Name = "list")] 
	public string? List {  get; set; }

	protected StoreLocationModel StoreLocation { get; set; } = new();
	protected List<FormProductLocationModel> FormLocations { get; set; } = new();
	protected bool IsLoading { get; set; } = true;
	protected bool IsSorting { get; set; } = false;
	protected bool IsWorking { get; set; } = false;
	protected bool DisableControls => IsWorking || IsSorting;
	protected Member ActiveMember => (Member)State.ActiveMember;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (string.IsNullOrEmpty(List))
		{
			Breadcrumbs.Add(new BreadcrumbItem("Stores", "/stores", false));
			Breadcrumbs.Add(new BreadcrumbItem("Store", $"/stores/{StoreSid}", false));
			Breadcrumbs.Add(new BreadcrumbItem("Aisle", null, true));
		}
		else
		{
			Breadcrumbs.Add(new BreadcrumbItem("Shopping Lists", "/shopping-lists", false));
			Breadcrumbs.Add(new BreadcrumbItem("Shopping List", $"/shopping-lists/{List}", false));
			Breadcrumbs.Add(new BreadcrumbItem("Aisle", null, true));
		}

		State.OnActiveMemberChange += StateHasChanged;

		StoreLocation = await Mediator.Send(new GetStoreLocation.Query(StoreLocationSid));

		LoadForm();

		IsLoading = false;
		StateHasChanged();
	}

	public override ValueTask DisposeAsync()
	{
		State.OnActiveMemberChange -= StateHasChanged;
		return base.DisposeAsync();
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsSorting = isSorting;
		StateHasChanged();
	}

	protected async Task HandleSortChanged(List<FormProductLocationModel> list)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateProductLocations.Command(StoreLocationSid, list));
		ShowNotification(result);
		if (result.Success)
		{
			FormLocations = list;
		}
		IsWorking = false;
		StateHasChanged();
	}

	protected async Task HandleSwitchHousehold()
	{
		await Mediator.OpenMemberHousehold(StoreLocation.HouseholdSid);
	}

	protected void LoadForm()
	{
		FormLocations = StoreLocation.ProductLocations
			.Select(x => (FormProductLocationModel)x.Clone())
			.ToList();
	}
}