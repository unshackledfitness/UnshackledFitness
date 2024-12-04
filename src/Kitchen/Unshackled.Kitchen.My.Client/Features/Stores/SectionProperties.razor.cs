using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Stores.Actions;
using Unshackled.Kitchen.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Stores;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public StoreModel Store { get; set; } = new();
	[Parameter] public EventCallback<StoreModel> StoreChanged { get; set; }

	protected const string FormId = "formStoreProperties";
	protected bool IsWorking { get; set; }
	protected FormStoreModel Model { get; set; } = new();

	protected bool DisableControls => IsWorking;

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this store?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteStore.Command(Store.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/stores");
			}

			await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = Store.Title,
			Description = Store.Description,
			Sid = Store.Sid,
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormStoreModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateStoreProperties.Command(model));
		if (result.Success)
		{
			await StoreChanged.InvokeAsync(result.Payload);
		}
		ShowNotification(result);
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}