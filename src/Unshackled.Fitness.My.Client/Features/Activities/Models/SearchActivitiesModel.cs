using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Models;

public class SearchActivitiesModel : SearchModel
{
	public string? ActivityTypeSid { get; set; }
	public EventTypes EventType { get; set; }
	public DateTime? DateStart { get; set; }
	public DateTime? DateEnd { get; set; }
	public string? Title { get; set; }
}