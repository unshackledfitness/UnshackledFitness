using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Features.ProductBundles.Actions;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.ProductBundles;

public class SectionItemsBase : BaseSectionComponent<Member>
{
	protected enum Views
	{
		None,
		Add,
		Edit
	}

	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductBundleModel ProductBundle { get; set; } = new();

	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsWorking || ProductBundle.Products.Where(x => x.IsEditing == true).Any();
	protected List<string> SelectedSids { get; set; } = new();
	protected bool MaxSelectionReached => SelectedSids.Count == FoodGlobals.MaxSelectionLimit;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView {  get; set; } = Views.None;
	protected FormProductModel EditingModel { get; set; } = new();

	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Products",
		Views.Edit => "Edit Product",
		_ => string.Empty
	};

	public bool DisableCheckbox(string sid)
	{
		return DisableControls
			|| (!SelectedSids.Contains(sid) && MaxSelectionReached);
	}

	protected async Task HandleAddClicked()
	{
		DrawerView = Views.Add;
		await UpdateIsEditingSection(true);
	}

	protected async Task HandleCancelClicked()
	{
		DrawerView = Views.None;
		await UpdateIsEditingSection(false);
	}

	protected void HandleCheckboxChanged(bool value, string sid)
	{
		if (value)
			SelectedSids.Add(sid);
		else
			SelectedSids.Remove(sid);
	}

	protected async Task HandleDeleteClicked()
	{
		DrawerView = Views.None;

		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to remove this item from the bundle?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsWorking = await UpdateIsEditingSection(true);

			DeleteProductModel deleteModel = new()
			{
				ProductSid = EditingModel.ProductSid,
				ProductBundleSid = ProductBundle.Sid
			};

			var result = await Mediator.Send(new DeleteProduct.Command(deleteModel));
			ShowNotification(result);
			if (result.Success)
			{
				var item = ProductBundle.Products.Where(x => x.ProductSid == EditingModel.ProductSid).Single();
				ProductBundle.Products.Remove(item);
			}
			IsWorking = await UpdateIsEditingSection(false);
			StateHasChanged();
		}

		await UpdateIsEditingSection(false);
	}

	protected async Task HandleEditClicked(FormProductModel model)
	{
		await UpdateIsEditingSection(true);
		EditingModel = new()
		{
			Brand = model.Brand,
			Description = model.Description,
			ProductBundleSid = model.ProductBundleSid,
			ProductSid = model.ProductSid,
			Quantity = model.Quantity,
			Title = model.Title,
			IsSaving = false
		};
		DrawerView = Views.Edit;
	}

	protected async Task HandleProductsAdded(Dictionary<string, int> products)
	{
		DrawerView = Views.None;
		IsWorking = await UpdateIsEditingSection(true);
		AddProductsModel model = new()
		{
			Products = products,
			ProductBundleSid = ProductBundle.Sid
		};
		var result = await Mediator.Send(new AddProducts.Command(model));
		if (result.Success)
		{
			ProductBundle.Products = await Mediator.Send(new ListProducts.Query(ProductBundle.Sid));
		}
		ShowNotification(result);

		IsWorking = await UpdateIsEditingSection(false);
		StateHasChanged();
	}

	protected async Task HandleSaveClicked(int quantity)
	{
		UpdateProductModel updateModel = new()
		{
			ProductBundleSid = EditingModel.ProductBundleSid,
			ProductSid = EditingModel.ProductSid,
			Quantity = quantity
		};
		var result = await Mediator.Send(new UpdateProductQuantity.Command(updateModel));
		if (result.Success)
		{
			var product = ProductBundle.Products.Where(x => x.ProductSid == EditingModel.ProductSid).SingleOrDefault();
			if (product != null)
			{
				product.Quantity = quantity;
				StateHasChanged();
			}
		}
		DrawerView = Views.None;
		ShowNotification(result);

		await UpdateIsEditingSection(false);
	}
}