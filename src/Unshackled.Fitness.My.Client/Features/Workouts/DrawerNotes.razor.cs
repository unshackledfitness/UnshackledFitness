using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts;

public class DrawerNotesBase : BaseComponent
{
	[Parameter] public string WorkoutSetSid { get; set; } = string.Empty;
	[Parameter] public string? SetNotes { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }
	[Parameter] public EventCallback<FormSetNoteModel> OnSaveClicked { get; set; }

	protected bool IsSaving { get; set; }

	protected FormSetNoteModel Model { get; set; } = new();

	protected override void OnInitialized()
	{
		base.OnInitialized();

		Model = new()
		{
			Sid = WorkoutSetSid,
			Notes = SetNotes,
		};
	}

	protected async Task HandleCancelClicked()
	{
		await OnCanceled.InvokeAsync();
	}

	protected async Task HandleUpdateClicked()
	{
		IsSaving = true;
		await OnSaveClicked.InvokeAsync(Model);
		IsSaving = false;
	}
}