namespace Unshackled.Fitness.Core.Enums;

public enum SpeedUnits
{
	Any = 0,
	MetersPerSecond = 1,
	FeetPerSecond = 2,
	KilometersPerHour = 3,
	MilesPerHour = 4
}

public static class SpeedUnitsExtensions
{
	public static double ConversionFactor(this SpeedUnits unit)
	{
		// To m/s
		return unit switch
		{
			SpeedUnits.MetersPerSecond => 1, 
			SpeedUnits.KilometersPerHour => 3.6,
			SpeedUnits.FeetPerSecond => 0.3048,
			SpeedUnits.MilesPerHour => 2.237,
			_ => 1,
		};
	}

	public static string Label(this SpeedUnits unit)
	{
		return unit switch
		{
			SpeedUnits.Any => "Any",
			SpeedUnits.MetersPerSecond => "m/s",
			SpeedUnits.FeetPerSecond => "ft/s",
			SpeedUnits.KilometersPerHour => "km/h",
			SpeedUnits.MilesPerHour => "mph",
			_ => string.Empty,
		};
	}
}
