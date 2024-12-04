using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Products;

public class DrawerBulkCategoryBase : BaseComponent<Member>
{
	[Parameter] public List<ProductCategoryModel> Categories { get; set; } = [];
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