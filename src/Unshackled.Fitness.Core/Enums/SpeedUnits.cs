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
	public static double ConvertFromMetersPerSecond(this SpeedUnits? unit, double? mps)
	{
		if (unit == null || mps == null)
			return 0;

		return unit.Value.ConvertFromMetersPerSecond(mps);
	}

	public static double ConvertFromMetersPerSecond(this SpeedUnits unit, double? mps)
	{
		if (mps == null)
			return 0;

		// From mps
		return unit switch
		{
			SpeedUnits.MetersPerSecond => mps.Value,
			SpeedUnits.KilometersPerHour => 3.6 * mps.Value,
			SpeedUnits.FeetPerSecond => 3.28084 * mps.Value,
			SpeedUnits.MilesPerHour => 2.23694 * mps.Value,
			_ => mps.Value,
		};
	}

	public static double ConvertToMetersPerSecond(this SpeedUnits? unit, double? value)
	{
		if (unit == null || value == null)
			return 0;

		return unit.Value.ConvertToMetersPerSecond(value);
	}

	public static double ConvertToMetersPerSecond(this SpeedUnits unit, double? value)
	{
		if (value == null)
			return 0;

		// From mps
		return unit switch
		{
			SpeedUnits.MetersPerSecond => value.Value,
			SpeedUnits.KilometersPerHour => 0.277778 * value.Value,
			SpeedUnits.FeetPerSecond => 0.3048 * value.Value,
			SpeedUnits.MilesPerHour => 0.44704 * value.Value,
			_ => value.Value,
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
