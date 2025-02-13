using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerNoteBase : BaseFormComponent<FormNoteModel, FormNoteModel.Validator>
{
	[Parameter] public bool IsAdding { get; set; }
	[Parameter] public EventCallback OnDeleted { get; set; }

	protected async Task HandleDeleteClicked()
	{
		await OnDeleted.InvokeAsync();
	}
}