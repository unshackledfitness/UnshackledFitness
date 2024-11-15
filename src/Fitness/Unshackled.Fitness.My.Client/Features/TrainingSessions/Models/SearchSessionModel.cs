using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

public class SearchSessionsModel : SearchModel
{
	public string? Title { get; set; }
	public string? ActivityTypeSid { get; set; }
}
