using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Stores.Actions;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Stores;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string StoreSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected StoreModel Store { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Stores", "/stores", false));
		Breadcrumbs.Add(new BreadcrumbItem("Store", null, true));

		State.OnActiveMemberChange += StateHasChanged;

		Store = await Mediator.Send(new GetStore.Query(StoreSid));
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
		await Mediator.OpenMemberHousehold(Store.HouseholdSid);
	}
}