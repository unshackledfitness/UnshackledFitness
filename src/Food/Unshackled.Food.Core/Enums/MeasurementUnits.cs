namespace Unshackled.Food.Core.Enums;

public enum MeasurementUnits
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
	Tbsp = 14,
	tad = 15,
	dash = 16,
	pinch = 17,
	smidgen = 18,
	drop = 19,
	shake = 20,
	nip = 21
}

public static class MeasurementUnitsExtensions
{
	public static decimal ConversionFactor(this MeasurementUnits unit)
	{
		return unit switch
		{
			MeasurementUnits.lb => 2.2046M, // To Kg
			MeasurementUnits.kg => 1M,
			_ => 1M,
		};
	}

	public static MeasurementUnits? FindMeasurementUnit(this string value)
	{
		var unitList = new Dictionary<string, MeasurementUnits>
		{
			{ "mg", MeasurementUnits.mg },
			{ "milligram", MeasurementUnits.mg },
			{ "milligrams", MeasurementUnits.mg },
			{ "g", MeasurementUnits.g },
			{ "gram", MeasurementUnits.g },
			{ "grams", MeasurementUnits.g },
			{ "kg", MeasurementUnits.kg },
			{ "kilogram", MeasurementUnits.kg },
			{ "kilograms", MeasurementUnits.kg },
			{ "oz", MeasurementUnits.oz },
			{ "oz.", MeasurementUnits.oz },
			{ "ounce", MeasurementUnits.oz },
			{ "ounces", MeasurementUnits.oz },
			{ "lb", MeasurementUnits.lb },
			{ "pound", MeasurementUnits.lb },
			{ "pounds", MeasurementUnits.lb },
			{ "ml", MeasurementUnits.ml },
			{ "milliliter", MeasurementUnits.ml },
			{ "milliliters", MeasurementUnits.ml },
			{ "l", MeasurementUnits.L },
			{ "liter", MeasurementUnits.L },
			{ "liters", MeasurementUnits.L },
			{ "fl oz", MeasurementUnits.floz },
			{ "fl oz.", MeasurementUnits.floz },
			{ "fl.oz", MeasurementUnits.floz },
			{ "fl.oz.", MeasurementUnits.floz },
			{ "fl. oz", MeasurementUnits.floz },
			{ "fl. oz.", MeasurementUnits.floz },
			{ "fluid ounce", MeasurementUnits.floz },
			{ "fluid ounces", MeasurementUnits.floz },
			{ "c", MeasurementUnits.cup },
			{ "cup", MeasurementUnits.cup },
			{ "cups", MeasurementUnits.cup },
			{ "pt", MeasurementUnits.pint },
			{ "pint", MeasurementUnits.pint },
			{ "pints", MeasurementUnits.pint },
			{ "qt", MeasurementUnits.quart },
			{ "quart", MeasurementUnits.quart },
			{ "quarts", MeasurementUnits.quart },
			{ "gal", MeasurementUnits.gallon },
			{ "gallon", MeasurementUnits.gallon },
			{ "gallons", MeasurementUnits.gallon },
			{ "t", MeasurementUnits.tsp },
			{ "tsp", MeasurementUnits.tsp },
			{ "teaspoon", MeasurementUnits.tsp },
			{ "teaspoons", MeasurementUnits.tsp },
			{ "tbsp", MeasurementUnits.Tbsp },
			{ "tablespoon", MeasurementUnits.Tbsp },
			{ "tablespoons", MeasurementUnits.Tbsp },
			{ "tad", MeasurementUnits.tad },
			{ "dash", MeasurementUnits.dash },
			{ "pinch", MeasurementUnits.pinch },
			{ "smidgen", MeasurementUnits.smidgen },
			{ "drop", MeasurementUnits.drop },
			{ "shake", MeasurementUnits.shake },
			{ "nip", MeasurementUnits.nip }
		};

		foreach (var key in unitList.Keys)
		{
			if (key.ToLower() == value.ToLower())
			{
				return unitList[key];
			}
		}

		return null;
	}

	public static bool IsVolume(this MeasurementUnits unit)
	{
		return unit switch
		{
			MeasurementUnits.Item => false,
			MeasurementUnits.mg => false,
			MeasurementUnits.g => false,
			MeasurementUnits.kg => false,
			MeasurementUnits.oz => false,
			MeasurementUnits.lb => false,
			MeasurementUnits.ml => true,
			MeasurementUnits.L => true,
			MeasurementUnits.floz => true,
			MeasurementUnits.cup => true,
			MeasurementUnits.pint => true,
			MeasurementUnits.quart => true,
			MeasurementUnits.gallon => true,
			MeasurementUnits.tsp => true,
			MeasurementUnits.Tbsp => true,
			MeasurementUnits.tad =>  true,
			MeasurementUnits.dash =>  true,
			MeasurementUnits.pinch =>  true,
			MeasurementUnits.smidgen =>  true,
			MeasurementUnits.drop =>  true,
			MeasurementUnits.shake =>  true,
			MeasurementUnits.nip =>  true,
			_ => false,
		};
	}

	public static string Label(this MeasurementUnits unit)
	{
		return unit switch
		{
			MeasurementUnits.Item => "item",
			MeasurementUnits.mg => "mg",
			MeasurementUnits.g => "g",
			MeasurementUnits.kg => "kg",
			MeasurementUnits.oz => "oz",
			MeasurementUnits.lb => "lb",
			MeasurementUnits.ml => "mL",
			MeasurementUnits.L => "L",
			MeasurementUnits.floz => "fl oz",
			MeasurementUnits.cup => "cup",
			MeasurementUnits.pint => "pint",
			MeasurementUnits.quart => "quart",
			MeasurementUnits.gallon => "gallon",
			MeasurementUnits.tsp => "tsp",
			MeasurementUnits.Tbsp => "tbsp",
			MeasurementUnits.tad => "tad",
			MeasurementUnits.dash => "dash",
			MeasurementUnits.pinch => "pinch",
			MeasurementUnits.smidgen => "smidgen",
			MeasurementUnits.drop => "drop",
			MeasurementUnits.shake => "shake",
			MeasurementUnits.nip => "nip",
			_ => string.Empty,
		};
	}

	public static decimal NormalizeAmount(this MeasurementUnits unit, decimal amount)
	{
		decimal normalized = unit switch
		{
			MeasurementUnits.Item => amount,
			MeasurementUnits.mg => amount,
			MeasurementUnits.g => amount * 1000, // to mg
			MeasurementUnits.kg => amount * 1000000, // to mg
			MeasurementUnits.oz => amount * 28349.5M, // to mg
			MeasurementUnits.lb => amount * 453592, // to mg
			MeasurementUnits.ml => amount,
			MeasurementUnits.L => amount * 1000, // to mL
			MeasurementUnits.floz => amount * 29.5735M, // to mL
			MeasurementUnits.cup => amount * 236.59M, // to mL
			MeasurementUnits.pint => amount * 473.18M, // to mL
			MeasurementUnits.quart => amount * 946.36M, // to mL
			MeasurementUnits.gallon => amount * 3785M, // to mL
			MeasurementUnits.tsp => amount * 4.93M, // to mL
			MeasurementUnits.Tbsp => amount * 14.79M, // to mL
			MeasurementUnits.tad => amount * 1.2325M, // (1/4 tsp) to mL
			MeasurementUnits.dash => amount * 0.61625M, // (1/8 tsp) to mL
			MeasurementUnits.pinch => amount * 0.308125M, // (1/16 tsp) to mL
			MeasurementUnits.smidgen => amount * 0.1540625M, // (1/32 tsp) to mL
			MeasurementUnits.drop => amount * 0.07703125M, // (1/64 tsp) to mL
			MeasurementUnits.shake => amount * 0.1540625M, // (1/32 tsp) to mL
			MeasurementUnits.nip => amount * 0.07703125M, // (1/64 tsp) to mL
			_ => 0,
		};

		return Math.Round(normalized, 3, MidpointRounding.AwayFromZero);
	}

	public static string Title(this MeasurementUnits unit)
	{
		return unit switch
		{
			MeasurementUnits.Item => "Items",
			MeasurementUnits.mg => "Milligrams",
			MeasurementUnits.g => "Grams",
			MeasurementUnits.kg => "Kilograms",
			MeasurementUnits.oz => "Ounces",
			MeasurementUnits.lb => "Pounds",
			MeasurementUnits.ml => "Milliliters",
			MeasurementUnits.L => "Liters",
			MeasurementUnits.floz => "Fluid Ounces",
			MeasurementUnits.cup => "Cups",
			MeasurementUnits.pint => "Pints",
			MeasurementUnits.quart => "Quarts",
			MeasurementUnits.gallon => "Gallons",
			MeasurementUnits.tsp => "Teaspoons",
			MeasurementUnits.Tbsp => "Tablespoons",
			MeasurementUnits.tad => "Tads",
			MeasurementUnits.dash => "Dashes",
			MeasurementUnits.pinch => "Pinches",
			MeasurementUnits.smidgen => "Smidgens",
			MeasurementUnits.drop => "Drops",
			MeasurementUnits.shake => "Shakes",
			MeasurementUnits.nip => "Nips",
			_ => string.Empty,
		};
	}
}
