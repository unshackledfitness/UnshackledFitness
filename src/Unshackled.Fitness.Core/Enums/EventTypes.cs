namespace Unshackled.Fitness.Core.Enums;

public enum EventTypes
{
	Any,
	Uncategorized,
	Training,
	Competition,
	Recreational
}

public static class EventTypesExtensions
{
	public static string Title(this EventTypes eventType)
	{
		return eventType switch
		{
			EventTypes.Any => "Any",
			EventTypes.Competition => "Competition",
			EventTypes.Recreational => "Recreational",
			EventTypes.Training => "Training",
			EventTypes.Uncategorized => "Uncategorized",
			_ => string.Empty,
		};
	}
}
