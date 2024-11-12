using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;

public class SearchTargetsModel : SearchModel
{
	public string? Title { get; set; }
	public string? ActivityTypeSid { get; set; }
}
