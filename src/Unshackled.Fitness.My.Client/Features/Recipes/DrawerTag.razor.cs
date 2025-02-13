using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerTagBase : BaseFormComponent<FormTagModel, FormTagModel.Validator>
{
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}
}