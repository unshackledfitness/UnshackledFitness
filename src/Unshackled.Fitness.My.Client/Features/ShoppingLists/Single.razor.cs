using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists;

public class SingleBase : BaseComponent<Member>, IAsyncDisposable
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public string ShoppingListSid { get; set; } = string.Empty; 
	protected bool IsLoading { get; set; } = true;
	protected ShoppingListModel ShoppingList { get; set; } = new();
	protected bool IsEditMode { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool DisableControls => !IsEditMode || IsEditing;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Shopping Lists", "/shopping-lists", false));
		Breadcrumbs.Add(new BreadcrumbItem("Shopping List", null, true));

		State.OnActiveMemberChange += StateHasChanged;

		ShoppingList = await Mediator.Send(new GetShoppingList.Query(ShoppingListSid));
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
		await Mediator.OpenMemberHousehold(ShoppingList.HouseholdSid);
	}
}