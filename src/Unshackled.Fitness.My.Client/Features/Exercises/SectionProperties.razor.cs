using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Components;
using Unshackled.Fitness.My.Client.Features.Exercises.Actions;
using Unshackled.Fitness.My.Client.Features.Exercises.Models;

namespace Unshackled.Fitness.My.Client.Features.Exercises;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public ExerciseModel Exercise { get; set; } = new();
	[Parameter] public EventCallback<ExerciseModel> ExerciseChanged { get; set; }

	protected const string FormId = "formExercise";
	protected bool DrawerOpen { get; set; }
	protected bool IsEditing { get; set; } = false;
	protected bool IsUpdating { get; set; } = false;
	protected FormExerciseModel Model { get; set; } = new();
	protected ExerciseNoteModel FormNoteModel { get; set; } = new();

	public bool DisableControls => IsUpdating;

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleCancelEditNoteClicked()
	{
		DrawerOpen = false;
		await UpdateIsEditingSection(false);
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			DefaultSetMetricType = Exercise.DefaultSetMetricType,
			Description = Exercise.Description,
			Equipment = Exercise.Equipment,
			IsTrackingSplit = Exercise.IsTrackingSplit,
			Muscles = Exercise.Muscles,
			Title = Exercise.Title,
			Sid = Exercise.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditNoteClicked()
	{
		FormNoteModel = new()
		{
			Sid = Exercise.Sid,
			MemberSid = Exercise.MemberSid,
			Notes = Exercise.Notes
		};
		DrawerOpen = true;
		await UpdateIsEditingSection(true);
	}

	protected async Task HandleFormSubmitted(FormExerciseModel model)
	{
		IsUpdating = true;
		var result = await Mediator.Send(new UpdateExercise.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			if (ExerciseChanged.HasDelegate)
				await ExerciseChanged.InvokeAsync(result.Payload);
		}
		IsUpdating = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleSaveNoteClicked()
	{
		IsUpdating = true;
		var result = await Mediator.Send(new SaveExerciseNote.Command(FormNoteModel));
		if (result.Success)
		{
			Exercise.Notes = FormNoteModel.Notes;
			DrawerOpen = false;
			await ExerciseChanged.InvokeAsync(Exercise);
		}
		ShowNotification(result);
		IsUpdating = false;
		await UpdateIsEditingSection(false);
	}

	protected async Task HandleToggleArchiveClicked()
	{
		IsUpdating = true;
		var result = await Mediator.Send(new ToggleIsArchived.Command(Exercise.Sid));
		if (result.Success)
		{
			Exercise.IsArchived = result.Payload;
		}
		ShowNotification(result);
		IsUpdating = false;
	}
}