using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class PlanModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public ProgramTypes ProgramType { get; set; }
	public string? Description { get; set; }
	public int LengthWeeks { get; set; }
	public DateTime? DateStarted { get; set; }
	public int NextSessionIndex { get; set; }
	public string? ActiveSessionSid { get; set; }

	public List<PlanSessionModel> Sessions { get; set; } = new();

	public PlanSessionModel? StartingTemplate()
	{
		if (!DateStarted.HasValue)
			return null;

		if (!Sessions.Any())
			return null;

		if (ProgramType == ProgramTypes.FixedRepeating)
			return null;

		return Sessions.First();
	}
}
