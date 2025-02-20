using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class StatBlockModel
{
	public StatBlockTypes Type { get; set; } = StatBlockTypes.None;
	public string Title { get; set; } = string.Empty;
	public DateTimeOffset DateCompleted { get; set; }
	public DateTimeOffset DateCompletedUtc { get; set; }
}
