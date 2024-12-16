using System.Text.Json.Serialization;
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
	[JsonIgnore]
	public MuscleTypes FirstMuscleSelected { get; set; } = MuscleTypes.Any;

	// Used for validation on multiselect
	[JsonIgnore]
	public EquipmentTypes FirstEquipmentSelected { get; set; } = EquipmentTypes.Any;

	public class Validator : AbstractValidator<FormExerciseModel>
	{
		public Validator()
		{
			RuleFor(p => p.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

			RuleFor(p => p.FirstMuscleSelected)
				.Must((model, p) =>
				{
					if (model.Muscles.Count() == 0)
					{
						return false;
					}
					return true;
				}).WithMessage("At least one muscle must be selected.");

			RuleFor(p => p.FirstEquipmentSelected)
				.Must((model, p) =>
				{
					if (model.Equipment.Count() == 0)
					{
						return false;
					}
					return true;
				}).WithMessage("At least one equipment item must be selected.");
		}

		// Required function for handling validation of multiselection MudSelects
		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<FormExerciseModel>.CreateWithOptions((FormExerciseModel)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
