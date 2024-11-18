using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.Products.Actions;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Products;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductModel Product { get; set; } = new();
	[Parameter] public EventCallback<ProductModel> ProductChanged { get; set; }

	protected const string FormId = "formProductProperties";
	protected bool IsEditing { get; set; } = false;
	protected bool IsSaving { get; set; }
	protected FormProductModel Model { get; set; } = new();

	protected bool DisableControls => IsSaving;
	public int StatElevation => IsEditMode ? 0 : 1;

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Brand = Product.Brand,
			Calories = Product.Calories,
			Carbohydrates = Product.Carbohydrates,
			CarbohydratesUnit = Product.CarbohydratesUnit,
			Description = Product.Description,
			Fat = Product.Fat,
			FatUnit = Product.FatUnit,
			HasNutritionInfo = Product.HasNutritionInfo,
			Protein = Product.Protein,
			ProteinUnit = Product.ProteinUnit,
			ServingSizeText = Product.ServingSize.ToString("0.###"),
			ServingSize = Product.ServingSize,
			ServingSizeMetric = Product.ServingSizeMetric,
			ServingSizeMetricUnit = Product.ServingSizeMetricUnit,
			ServingSizeUnit = Product.ServingSizeUnit,
			ServingSizeUnitLabel = Product.ServingSizeUnitLabel,
			ServingsPerContainer = Product.ServingsPerContainer,
			Sid = Product.Sid,
			Title = Product.Title,
		};

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

	protected async Task HandleToggleArchiveClicked()
	{
		IsSaving = true;
		var result = await Mediator.Send(new ToggleIsArchived.Command(Product.Sid));
		if (result.Success)
		{
			Product.IsArchived = result.Payload;
		}
		ShowNotification(result);
		IsSaving = false;
	}
}