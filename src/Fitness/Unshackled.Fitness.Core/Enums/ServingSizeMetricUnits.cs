namespace Unshackled.Fitness.Core.Enums;

public enum ServingSizeMetricUnits
{
	mg = 0,
	g = 1,
	kg = 2,
	ml = 3,
	L = 4
}

public static class ServingSizeMetricUnitsExtensions
{
	public static string Label(this ServingSizeMetricUnits unit)
	{
		return unit switch
		{
			ServingSizeMetricUnits.mg => "mg",
			ServingSizeMetricUnits.g => "g",
			ServingSizeMetricUnits.kg => "kg",
			ServingSizeMetricUnits.ml => "mL",
			ServingSizeMetricUnits.L => "L",
			_ => string.Empty,
		};
	}

	public static decimal NormalizeAmount(this ServingSizeMetricUnits unit, decimal amount)
	{
		decimal normalized = unit switch
		{
			ServingSizeMetricUnits.mg => amount,
			ServingSizeMetricUnits.g => amount * 1000, // to mg
			ServingSizeMetricUnits.kg => amount * 1000000, // to mg
			ServingSizeMetricUnits.ml => amount,
			ServingSizeMetricUnits.L => amount * 1000, // to mL
			_ => 0,
		};

		return Math.Round(normalized, 3, MidpointRounding.AwayFromZero);
	}

	public static string Title(this ServingSizeMetricUnits unit)
	{
		return unit switch
		{
			ServingSizeMetricUnits.mg => "Milligrams",
			ServingSizeMetricUnits.g => "Grams",
			ServingSizeMetricUnits.kg => "Kilograms",
			ServingSizeMetricUnits.ml => "Milliliters",
			ServingSizeMetricUnits.L => "Liters",
			_ => string.Empty,
		};
	}

	public static UnitTypes UnitType(this ServingSizeMetricUnits unit)
	{
		return unit switch
		{
			ServingSizeMetricUnits.mg => UnitTypes.Weight,
			ServingSizeMetricUnits.g => UnitTypes.Weight,
			ServingSizeMetricUnits.kg => UnitTypes.Weight,
			ServingSizeMetricUnits.ml => UnitTypes.Volume,
			ServingSizeMetricUnits.L => UnitTypes.Volume,
			_ => UnitTypes.Volume,
		};
	}
}
