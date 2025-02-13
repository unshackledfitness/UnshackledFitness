using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

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