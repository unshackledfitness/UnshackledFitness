using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class PlanListModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public ProgramTypes ProgramType { get; set; }
	public int LengthWeeks { get; set; }
	public int Sessions { get; set; }
	public DateTime? DateStarted { get; set; }
}
