using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Products.Actions;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

public class SectionVitaminsBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductModel Product { get; set; } = new();
	[Parameter] public EventCallback<ProductModel> ProductChanged { get; set; }

	protected const string FormId = "formProductVitamins";
	protected bool IsSaving { get; set; }
	protected FormProductModel Model { get; set; } = new();

	protected bool DisableControls => IsSaving;
	public int StatElevation => IsEditMode ? 0 : 1;

	protected async Task HandleEditClicked()
	{
		Model = new();
		Model.Fill(Product);
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormProductModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateProduct.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await ProductChanged.InvokeAsync(result.Payload);
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}