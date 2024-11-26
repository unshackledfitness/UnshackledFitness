using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Exercises.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Exercises;

public class FormPropertiesBase : BaseFormComponent<FormExerciseModel, FormExerciseModel.Validator>
{
	/* This form requires special handling to fix validation on multi-select MudSelects
	 * Everything below and the setup on MuscleTypeSelect and EquipmentTypeSelect is required for proper
	 * validation of the Model.
	 */

	protected bool IsWorking { get; set; }
	protected MudForm Form { get; set; } = default!;

	protected async Task HandleAddClicked()
	{
		await Form.Validate();

		if (Form.IsValid)
		{
			await OnFormSubmitted.InvokeAsync(Model);
		}
	}

	protected override async Task HandleCancelClicked()
	{
		Form.ResetValidation();
		await base.HandleCancelClicked();
	}

	protected async Task HandleEquipmentChanged(IEnumerable<EquipmentTypes> selected)
	{
		Model.Equipment = selected;
		await Form.Validate();
	}

	protected async Task HandleMusclesChanged(IEnumerable<MuscleTypes> selected)
	{
		Model.Muscles = selected;
		await Form.Validate();
	}
}