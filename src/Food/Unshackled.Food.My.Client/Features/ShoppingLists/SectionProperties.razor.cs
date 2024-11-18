using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.ShoppingLists.Actions;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ShoppingLists;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ShoppingListModel ShoppingList { get; set; } = new();
	[Parameter] public EventCallback<ShoppingListModel> ShoppingListChanged { get; set; }

	protected const string FormId = "formShoppingListProperies";
	protected bool IsEditing { get; set; } = false;
	protected bool IsWorking { get; set; }
	protected FormShoppingListModel Model { get; set; } = new();

	protected bool DisableControls => IsWorking;

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this shopping list?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteShoppingList.Command(ShoppingList.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/shopping-lists");
			}

			await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = ShoppingList.Title,
			Sid = ShoppingList.Sid,
			StoreSid = ShoppingList.StoreSid,
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormShoppingListModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateShoppingListProperties.Command(model));
		if (result.Success)
		{
			await ShoppingListChanged.InvokeAsync(result.Payload);
		}
		ShowNotification(result);
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}