using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

public class DrawerBulkCategoryBase : BaseComponent
{
	[Parameter] public List<CategoryModel> Categories { get; set; } = [];
	[Parameter] public EventCallback<string> OnSetCategory { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }

	protected string? SelectedCategorySid { get; set; }

	protected async Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}

	protected async void HandleSetCategoryClicked()
	{
		await OnSetCategory.InvokeAsync(SelectedCategorySid);
	}
}