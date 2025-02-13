using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

public class SessionListItem : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string ActivityTypeSid { get; set; } = string.Empty;
	public string ActivityTypeName { get; set; } = string.Empty;
	public string? ActivityTypeColor { get; set; }
}
