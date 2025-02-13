namespace Unshackled.Fitness.Core.Enums;

public enum DistanceUnits
{
	Any = 0,
	Meter = 1,
	Feet = 2,
	Kilometer = 3,
	Mile = 4
}

public static class DistanceUnitsExtensions
{
	public static double ConvertFromMeters(this DistanceUnits? unit, double? meters)
	{
		if (unit == null || meters == null)
			return 0;

		return unit.Value.ConvertFromMeters(meters);
	}

	public static double ConvertFromMeters(this DistanceUnits unit, double? meters)
	{
		if (meters == null)
			return 0;

		// From meters
		return unit switch
		{
			DistanceUnits.Meter => meters.Value,
			DistanceUnits.Kilometer => 0.001 * meters.Value,
			DistanceUnits.Feet => 3.280839895 * meters.Value,
			DistanceUnits.Mile => 0.000621371 * meters.Value,
			_ => meters.Value,
		};
	}

	public static double ConvertToMeters(this DistanceUnits? unit, double? value)
	{
		if (unit == null || value == null)
			return 0;

		return unit.Value.ConvertToMeters(value);
	}

	public static double ConvertToMeters(this DistanceUnits unit, double? value)
	{
		if (value == null)
			return 0;

		// From meters
		return unit switch
		{
			DistanceUnits.Meter => value.Value,
			DistanceUnits.Kilometer => 1000 * value.Value,
			DistanceUnits.Feet => 0.3048 * value.Value,
			DistanceUnits.Mile => 1609.34 * value.Value,
			_ => value.Value,
		};
	}

	public static string Label(this DistanceUnits unit)
	{
		return unit switch
		{
			DistanceUnits.Any => "Any",
			DistanceUnits.Meter => "m",
			DistanceUnits.Feet => "ft",
			DistanceUnits.Kilometer => "km",
			DistanceUnits.Mile => "mi",
			_ => string.Empty,
		};
	}

	public static string Title(this DistanceUnits unit)
	{
		return unit switch
		{
			DistanceUnits.Any => "Any",
			DistanceUnits.Meter => "Meters (m)",
			DistanceUnits.Feet => "Feet (ft)",
			DistanceUnits.Kilometer => "Kilometers (km)",
			DistanceUnits.Mile => "Miles (mi)",
			_ => string.Empty,
		};
	}
}
