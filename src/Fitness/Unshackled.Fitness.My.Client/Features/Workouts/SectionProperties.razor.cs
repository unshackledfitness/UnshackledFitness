using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Workouts.Actions;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Workouts;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public FormWorkoutModel Workout { get; set; } = new();
	[Parameter] public EventCallback<FormWorkoutModel> WorkoutChanged { get; set; }

	protected const string FormId = "formWorkout";
	protected bool DisableControls { get; set; } = false;
	protected FormPropertiesModel Model { get; set; } = new();

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to delete this workout? This can not be undone!",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteWorkout.Command(Workout.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/workouts");
			}
		}
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			DateCompleted = Workout.DateCompleted,
			DateCompletedUtc = Workout.DateCompletedUtc,
			DateStarted = Workout.DateStarted,
			DateStartedUtc = Workout.DateStartedUtc,
			IsComplete = Workout.DateCompletedUtc.HasValue,
			IsStarted = Workout.DateStartedUtc.HasValue,
			Rating = Workout.Rating,
			Title = Workout.Title,
			Sid = Workout.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleEditFormSubmitted(FormPropertiesModel model)
	{
		IsEditing = false;
		DisableControls = true;
		var result = await Mediator.Send(new UpdateProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			Workout.Title = model.Title;
			Workout.DateStarted = model.DateStarted;
			Workout.DateStartedUtc = model.DateStartedUtc;
			Workout.DateCompletedUtc = model.DateCompleted;
			Workout.DateCompletedUtc = model.DateCompletedUtc;
			Workout.Rating = model.Rating;
			if (WorkoutChanged.HasDelegate)
				await WorkoutChanged.InvokeAsync(Workout);
		}
		DisableControls = false;
		await UpdateIsEditingSection(false);
	}
}