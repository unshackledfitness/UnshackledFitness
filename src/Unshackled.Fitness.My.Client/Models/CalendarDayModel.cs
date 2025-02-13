namespace Unshackled.Fitness.My.Client.Models;

public class CalendarDayModel
{
	public DateOnly Date { get; set; }
	public List<CalendarBlockModel> Blocks { get; set; } = new();
}
