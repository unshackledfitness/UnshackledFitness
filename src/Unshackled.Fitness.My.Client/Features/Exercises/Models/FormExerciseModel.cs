using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Exercises.Models;

public class FormExerciseModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public IEnumerable<MuscleTypes> Muscles { get; set; } = new List<MuscleTypes>();
	public IEnumerable<EquipmentTypes> Equipment { get; set; } = new List<EquipmentTypes>();
	public WorkoutSetTypes DefaultSetType { get; set; } = WorkoutSetTypes.Standard;
	public SetMetricTypes DefaultSetMetricType { get; set; } = SetMetricTypes.WeightReps;
	public bool IsTrackingSplit { get; set; }

	// Used for validation on multiselect
	public MuscleTypes FirstMuscleSelected => Muscles.Any() ? Muscles.First() : MuscleTypes.Any;
	public EquipmentTypes FirstEquipmentSelected => Equipment.Any() ? Equipment.First() : EquipmentTypes.Any;

	public class Validator : AbstractValidator<FormExerciseModel>
	{
		public Validator()
		{
			RuleFor(p => p.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(p => p.FirstMuscleSelected)
				.Must(p => p != MuscleTypes.Any).WithMessage("At least one muscle must be selected.");

			RuleFor(p => p.FirstEquipmentSelected)
				.Must(p => p != EquipmentTypes.Any).WithMessage("At least one equipment item must be selected.");
		}
	}
}
