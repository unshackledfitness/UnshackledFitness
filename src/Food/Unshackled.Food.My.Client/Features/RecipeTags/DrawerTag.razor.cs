using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.RecipeTags.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.RecipeTags;

public class DrawerTagBase : BaseFormComponent<FormTagModel, FormTagModel.Validator>
{
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}
}