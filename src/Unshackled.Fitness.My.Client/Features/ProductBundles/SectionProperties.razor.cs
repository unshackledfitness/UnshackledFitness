using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductBundleModel ProductBundle { get; set; } = new();
	[Parameter] public EventCallback<ProductBundleModel> ProductBundleChanged { get; set; }

	protected const string FormId = "formProductBundleProperies";
	protected bool IsWorking { get; set; }
	protected FormProductBundleModel Model { get; set; } = new();

	protected bool DisableControls => IsWorking;

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this product bundle?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteProductBundle.Command(ProductBundle.Sid));
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
			Title = ProductBundle.Title,
			Sid = ProductBundle.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormProductBundleModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateProductBundleProperties.Command(model));
		if (result.Success)
		{
			await ProductBundleChanged.InvokeAsync(result.Payload);
		}
		ShowNotification(result);
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}