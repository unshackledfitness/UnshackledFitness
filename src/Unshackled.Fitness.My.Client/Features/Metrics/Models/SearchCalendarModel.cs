namespace Unshackled.Fitness.My.Client.Features.Metrics.Models;

public class SearchCalendarModel
{
	public DateOnly ToDate { get; set; }
	public DateOnly FromDate { get; set; }
	public DateTimeOffset ToDateUtc { get; set; }
	public DateTimeOffset FromDateUtc { get; set; }
}
