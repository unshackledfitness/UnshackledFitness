using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;

public class FormTargetModel
{
	public string Sid { get; set; } = string.Empty;
	public string? ActivityTypeSid { get; set; }
	public string Title { get; set; } = string.Empty;
	public int? TargetTimeSeconds { get; set; }
	public double? TargetDistance { get; set; }
	public DistanceUnits TargetDistanceUnit { get; set; } = DistanceUnits.Meter;
	public int? TargetCalories { get; set; }
	public int? TargetPace { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public double? TargetCadence { get; set; }
	public CadenceUnits TargetCadenceUnit { get; set; }
	public double? TargetPower { get; set; }
	public string? Notes { get; set; }

	public class Validator : BaseValidator<FormTargetModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
		}
	}
}
