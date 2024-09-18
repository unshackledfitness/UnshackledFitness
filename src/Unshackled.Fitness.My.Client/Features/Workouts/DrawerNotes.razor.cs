using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Components;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts;

public class DrawerNotesBase : BaseComponent
{
	[Parameter] public string ExerciseSid { get; set; } = string.Empty;
	[Parameter] public string? ExerciseNotes { get; set; }
	[Parameter] public EventCallback OnCanceled { get; set; }
	[Parameter] public EventCallback<ExerciseNoteModel> OnSaveClicked { get; set; }

	protected bool IsSaving { get; set; }

	protected ExerciseNoteModel Model { get; set; } = new();

	protected override void OnInitialized()
	{
		base.OnInitialized();

		Model = new()
		{
			Sid = ExerciseSid,
			Notes = ExerciseNotes,
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