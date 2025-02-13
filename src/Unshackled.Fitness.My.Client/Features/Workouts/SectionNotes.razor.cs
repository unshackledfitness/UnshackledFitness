using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Workouts.Actions;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Workouts;

public class SectionNotesBase : BaseSectionComponent<Member>
{
	[Parameter] public FormWorkoutModel Workout { get; set; } = new();
	[Parameter] public EventCallback<FormWorkoutModel> WorkoutChanged { get; set; }

	protected const string FormId = "formNotes";
	protected bool DisableControls { get; set; } = false;
	protected FormNotesModel Model { get; set; } = new();

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Notes = Workout.Notes,
			Sid = Workout.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleEditFormSubmitted(FormNotesModel model)
	{
		IsEditing = false;
		DisableControls = true;
		var result = await Mediator.Send(new UpdateNotes.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Workout.Notes = model.Notes;
			if (WorkoutChanged.HasDelegate)
				await WorkoutChanged.InvokeAsync(Workout);
		}
		DisableControls = false;
		await UpdateIsEditingSection(false);
	}
}