using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Models;

public class FormNotesModel
{
	public string Sid { get; set; } = string.Empty;
	public string? Notes { get; set; }

	public class Validator : BaseValidator<FormNotesModel>
	{
		public Validator()
		{
		}
	}
}
