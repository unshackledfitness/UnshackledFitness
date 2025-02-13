using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

public class SessionListModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? ActivityTypes { get; set; }
}
