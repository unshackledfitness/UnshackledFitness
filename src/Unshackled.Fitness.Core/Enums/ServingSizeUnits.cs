namespace Unshackled.Fitness.Core.Enums;

public enum ServingSizeUnits
{
	Item = 0,
	mg = 1,
	g = 2,
	kg = 3,
	oz = 4,
	lb = 5,
	ml = 6,
	L = 7,
	floz = 8,
	cup = 9,
	pint = 10,
	quart = 11,
	gallon = 12,
	tsp = 13,
	Tbsp = 14
}

public static class ServingSizeUnitsExtensions
{
	public static string Label(this ServingSizeUnits unit)
	{
		return unit switch
		{
			ServingSizeUnits.Item => "item",
			ServingSizeUnits.mg => "mg",
			ServingSizeUnits.g => "g",
			ServingSizeUnits.kg => "kg",
			ServingSizeUnits.oz => "oz",
			ServingSizeUnits.lb => "lb",
			ServingSizeUnits.ml => "mL",
			ServingSizeUnits.L => "L",
			ServingSizeUnits.floz => "fl oz",
			ServingSizeUnits.cup => "cup",
			ServingSizeUnits.pint => "pint",
			ServingSizeUnits.quart => "quart",
			ServingSizeUnits.gallon => "gallon",
			ServingSizeUnits.tsp => "tsp",
			ServingSizeUnits.Tbsp => "tbsp",
			_ => string.Empty,
		};
	}

	public static decimal NormalizeAmount(this ServingSizeUnits unit, decimal amount)
	{
		decimal normalized = unit switch
		{
			ServingSizeUnits.Item => amount,
			ServingSizeUnits.mg => amount,
			ServingSizeUnits.g => amount * 1000, // to mg
			ServingSizeUnits.kg => amount * 1000000, // to mg
			ServingSizeUnits.oz => amount * 28349.5M, // to mg
			ServingSizeUnits.lb => amount * 453592, // to mg
			ServingSizeUnits.ml => amount,
			ServingSizeUnits.L => amount * 1000, // to mL
			ServingSizeUnits.floz => amount * 29.5735M, // to mL
			ServingSizeUnits.cup => amount * 236.59M, // to mL
			ServingSizeUnits.pint => amount * 473.18M, // to mL
			ServingSizeUnits.quart => amount * 946.36M, // to mL
			ServingSizeUnits.gallon => amount * 3785M, // to mL
			ServingSizeUnits.tsp => amount * 4.93M, // to mL
			ServingSizeUnits.Tbsp => amount * 14.79M, // to mL
			_ => 0,
		};

		return Math.Round(normalized, 3, MidpointRounding.AwayFromZero);
	}

	public static string Title(this ServingSizeUnits unit)
	{
		return unit switch
		{
			ServingSizeUnits.Item => "Items",
			ServingSizeUnits.mg => "Milligrams",
			ServingSizeUnits.g => "Grams",
			ServingSizeUnits.kg => "Kilograms",
			ServingSizeUnits.oz => "Ounces",
			ServingSizeUnits.lb => "Pounds",
			ServingSizeUnits.ml => "Milliliters",
			ServingSizeUnits.L => "Liters",
			ServingSizeUnits.floz => "Fluid Ounces",
			ServingSizeUnits.cup => "Cups",
			ServingSizeUnits.pint => "Pints",
			ServingSizeUnits.quart => "Quarts",
			ServingSizeUnits.gallon => "Gallons",
			ServingSizeUnits.tsp => "Teaspoons",
			ServingSizeUnits.Tbsp => "Tablespoons",
			_ => string.Empty,
		};
	}

	public static UnitTypes UnitType(this ServingSizeUnits unit)
	{
		return unit switch
		{
			ServingSizeUnits.Item => UnitTypes.Item,
			ServingSizeUnits.mg => UnitTypes.Weight,
			ServingSizeUnits.g => UnitTypes.Weight,
			ServingSizeUnits.kg => UnitTypes.Weight,
			ServingSizeUnits.oz => UnitTypes.Weight,
			ServingSizeUnits.lb => UnitTypes.Weight,
			ServingSizeUnits.ml => UnitTypes.Volume,
			ServingSizeUnits.L => UnitTypes.Volume,
			ServingSizeUnits.floz => UnitTypes.Volume,
			ServingSizeUnits.cup => UnitTypes.Volume,
			ServingSizeUnits.pint => UnitTypes.Volume,
			ServingSizeUnits.quart => UnitTypes.Volume,
			ServingSizeUnits.gallon => UnitTypes.Volume,
			ServingSizeUnits.tsp => UnitTypes.Volume,
			ServingSizeUnits.Tbsp => UnitTypes.Volume,
			_ => UnitTypes.Item,
		};
	}
}
