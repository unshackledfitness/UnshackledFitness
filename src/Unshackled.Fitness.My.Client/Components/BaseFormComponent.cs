using Microsoft.AspNetCore.Components;

namespace Unshackled.Fitness.My.Client.Components;

public class BaseFormComponent<T, TValidator> : ComponentBase where T : new() where TValidator : new()
{
	[Parameter] public bool DisableFormControls { get; set; } = false;
	[Parameter] public string FormId { get; set; } = string.Empty;
	[Parameter] public T Model { get; set; } = new();
	[Parameter] public EventCallback OnCanceled { get; set; }
	[Parameter] public EventCallback<T> OnFormSubmitted { get; set; }
	[Parameter] public RenderFragment? FormToolbar { get; set; }
	protected TValidator ModelValidator { get; set; } = new();
	protected bool IsSaving { get; set; } = false;
	protected bool DisableControls => IsSaving || DisableFormControls;

	protected async virtual Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}

	protected async virtual Task HandleFormSubmitted()
	{
		IsSaving = true;
		await OnFormSubmitted.InvokeAsync(Model);
		IsSaving = false;
	}
}
