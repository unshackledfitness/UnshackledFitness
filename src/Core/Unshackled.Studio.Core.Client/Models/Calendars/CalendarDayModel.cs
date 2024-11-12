namespace Unshackled.Studio.Core.Client.Models.Calendars;

public class CalendarDayModel
{
	public DateOnly Date { get; set; }
	public List<CalendarBlockModel> Blocks { get; set; } = new();
}
