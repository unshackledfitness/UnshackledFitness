using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

public class DrawerCategoryBase : BaseFormComponent<FormCategoryModel, FormCategoryModel.Validator>
{
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}
}