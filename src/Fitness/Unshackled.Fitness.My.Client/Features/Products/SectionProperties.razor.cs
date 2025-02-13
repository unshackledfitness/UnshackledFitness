using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Products.Actions;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Products;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ProductModel Product { get; set; } = new();
	[Parameter] public EventCallback<ProductModel> ProductChanged { get; set; }

	protected const string FormId = "formProductProperties";
	protected bool IsWorking { get; set; }
	protected FormProductModel Model { get; set; } = new();
	protected List<CategoryModel> Categories { get; set; } = [];

	protected bool DisableControls => IsWorking;
	public int StatElevation => IsEditMode ? 0 : 1;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Categories = await Mediator.Send(new ListCategories.Query());
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

	protected async Task HandleFormSubmitted(FormProductModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new UpdateProduct.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await ProductChanged.InvokeAsync(result.Payload);
		}
		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleToggleArchiveClicked()
	{
		IsWorking = true;
		var result = await Mediator.Send(new ToggleIsArchived.Command(Product.Sid));
		if (result.Success)
		{
			Product.IsArchived = result.Payload;
		}
		ShowNotification(result);
		IsWorking = false;
	}
}