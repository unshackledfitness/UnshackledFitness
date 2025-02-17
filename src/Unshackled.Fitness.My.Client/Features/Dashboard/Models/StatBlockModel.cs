using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class StatBlockModel
{
	public StatBlockTypes Type { get; set; } = StatBlockTypes.None;
	public string Title { get; set; } = string.Empty;
	public DateTime DateCompleted { get; set; }
	public DateTime DateCompletedUtc { get; set; }
}
