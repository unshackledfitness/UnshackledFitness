using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Dashboard;

public class DrawerAddToListBase : BaseComponent
{
	[Parameter] public List<ShoppingListModel> ShoppingLists { get; set; } = new();
	[Parameter] public EventCallback<AddToListModel> OnAdded { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }
	protected bool IsSaving { get; set; } = false;
	protected int Quantity { get; set; } = 1;
	protected string? SelectedListSid { get; set; }

	protected async Task HandleAddSelected()
	{
		if (!string.IsNullOrEmpty(SelectedListSid))
		{
			IsSaving = true;
			AddToListModel model = new()
			{
				ListSid = SelectedListSid,
				Quantity = Quantity
			};
			await OnAdded.InvokeAsync(model);
			IsSaving = false;
		}
	}

	protected async Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}

	protected void HandleCounterSubtractClicked()
	{
		if (Quantity > 1)
		{
			Quantity--;
		}
	}

	protected void HandleCounterAddClicked()
	{
		Quantity++;
	}
}