using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Products.Actions;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Products;

public class SectionServingsBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductModel Product { get; set; } = new();
	[Parameter] public EventCallback<ProductModel> ProductChanged { get; set; }

	protected const string FormId = "formProductServings";
	protected bool IsSaving { get; set; }
	protected FormProductModel Model { get; set; } = new();
	protected FormProductModel.Validator ModelValidator { get; set; } = new();
	protected bool DrawerOpen { get; set; }
	protected bool DisableControls => IsSaving;
	public int StatElevation => IsEditMode ? 0 : 1;
	public decimal TotalServings => Model.ServingSize * Model.ServingsPerContainer;
	public decimal TotalServingsMetric => Model.ServingSizeMetric * Model.ServingsPerContainer;

	protected void HandleConvertToItemClicked()
	{
		if (Product.ServingSizeUnit != ServingSizeUnits.Item)
		{
			Model = new();
			Model.Fill(Product);
			Model.ConvertToSingleServing(ServingSizeUnits.Item.Label());
			DrawerOpen = true;
		}
	}

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

	protected async Task HandleFormConvertSubmitted()
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateProduct.Command(Model));
		ShowNotification(result);
		if (result.Success)
		{
			await ProductChanged.InvokeAsync(result.Payload);
		}
		IsSaving = false;
		DrawerOpen = false;
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