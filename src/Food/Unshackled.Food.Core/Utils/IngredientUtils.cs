using System.Text.RegularExpressions;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models;

namespace Unshackled.Food.Core.Utils;

public static class IngredientUtils
{
	public static ParsedIngredientResult Parse(string value)
	{
		value = ReplaceVulgarFractions(value);

		value = ReplaceNonAlphaNumericAtStart(value);

		var result = ParseNumber(value);
		if (result.Amount > 0 && !string.IsNullOrEmpty(result.AmountText))
		{
			// Remove from value
			value = value.Remove(0, result.AmountText.Length).Trim();
		}

		// Default to 1 if we don't have a value by now
		if (result.Amount == 0)
		{
			result.Amount = 1;
			result.AmountText = "1";
		}

		if (string.IsNullOrEmpty(value))
			return result;

		string[] words = value.Split(' ');

		// Only one word left in string
		if (words.Length == 1)
		{
			result.AmountUnit = MeasurementUnits.Item;
			result.AmountLabel = string.Empty;
			result.Title = value;
			return result;
		}
		else 
		{
			string firstWord = words[0];
			
			// look for measurement unit (ex. g, cup, tsp, fl. oz., L)
			// match with known values
			var unit = firstWord.FindMeasurementUnit();
			if (unit != null)
			{
				result.AmountUnit = unit.Value;
				result.AmountLabel = unit.Value.Label();
			}
			else
			{
				result.AmountUnit = MeasurementUnits.Item;
				result.AmountLabel = firstWord;
			}

			// Remove from value and update current word
			value = value.Remove(0, firstWord.Length).Trim();

			string[] phrases = value.Split(',', ';');
			if (phrases.Length == 1)
			{
				// Put remaining in title
				result.Title = value.Trim();
			}
			else
			{
				// Put first result in title
				result.Title = phrases[0].Trim();
				value = value.Remove(0, phrases[0].Length).Trim();

				// Put remaining in prep note, removing the starting comma or semi-colon
				result.PrepNote = value.Remove(0, 1).Trim();
			}
		}
		return result;
	}

	public static ParsedIngredientResult ParseAmount(string value)
	{
		var result = ParseNumber(value);
		if (!string.IsNullOrEmpty(result.AmountText))
		{
			// Remove from value
			value = value.Remove(0, result.AmountText.Length);
		}
		value = value.Trim();

		// look for measurement unit (ex. g, cup, tsp, fl. oz., L)
		// match with known values
		var unit = value.FindMeasurementUnit();
		if (unit != null)
		{
			result.AmountUnit = unit.Value;
			result.AmountLabel = unit.Value.Label();
		}
		else
		{
			result.AmountUnit = MeasurementUnits.Item;
			if (value.Length > 0)
			{
				result.AmountLabel = value;
			}
			else
			{
				result.AmountLabel = string.Empty;
			}
		}

		// Default to 1 if we don't have a value by now
		if (result.Amount == 0)
			result.Amount = 1;

		return result;
	}

	public static ParsedIngredientResult ParseNumber(string value)
	{
		ParsedIngredientResult result = new() { OriginalText = value };

		// look for fractions (1/2 or 1 1/2)
		Match m = Regex.Match(value, @"^\d+(?:\/\d*|\s\d+\/\d+)");
		if (m.Success && !string.IsNullOrEmpty(m.Value))
		{
			string fraction = m.Value;
			// look for whole numbers
			string[] parts = m.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length > 1)
			{
				result.Amount = decimal.Parse(parts[0]);
				fraction = parts[1];
			}
			// handle remaining fraction
			string[] fracParts = fraction.Split('/', StringSplitOptions.RemoveEmptyEntries);
			if (fracParts.Length > 1)
			{
				decimal numerator = decimal.Parse(fracParts[0]);
				decimal denominator = decimal.Parse(fracParts[1]);
				if (denominator > 0)
				{
					result.Amount += numerator / denominator;
				}
			}

			// Remove from value if we were able to process a value
			if (result.Amount > 0)
			{
				result.AmountText = m.Value;
			}
		}
		// look for integers or decimals
		else
		{
			m = Regex.Match(value, @"^\d*\.?\d*");
			if (m.Success && !string.IsNullOrEmpty(m.Value))
			{
				bool success = decimal.TryParse(m.Value, out decimal parsed);
				if (success)
				{
					result.Amount = parsed;
				}
				result.AmountText = m.Value;
			}
			else
			{
				result.Amount = 0;
				result.AmountText = value.Trim();
			}
		}

		return result;
	}

	private static string ReplaceVulgarFractions(string value)
	{
		Dictionary<string, string[]> fractions = new()
		{
			{ "1/4", new string[] { "&#188;", "&#xBC;", "&frac14;", "\u00BC" } },
			{ "1/2", new string[] { "&#189;", "&#xBD;", "&frac12;", "\u00BD" } },
			{ "1/3", new string[] { "&#8531;", "&#2153;", "&frac13;", "\u2153" } },
			{ "2/3", new string[] { "&#8532;", "&#2154;", "&frac23;", "\u2154" } },
			{ "3/4", new string[] { "&#190;", "&#xBE;", "	&frac34;", "\u00BE" } },
			{ "1/5", new string[] { "&#8533;", "&#2155;", "	&frac15;", "\u2155" } },
			{ "2/5", new string[] { "&#8534;", "&#2156;", "	&frac25;", "\u2156" } },
			{ "3/5", new string[] { "&#8535;", "&#2157;", "	&frac35", "\u2157" } },
			{ "4/5", new string[] { "&#8536;", "&#2158;", "	&frac45", "\u2158" } },
			{ "1/6", new string[] { "&#8537;", "&#2159;", "	&frac16", "\u2159" } },
			{ "5/6", new string[] { "&#8538;", "&#215A;", "	&frac56", "\u215A" } },
			{ "1/7", new string[] { "&#8528;", "&#2150;", "	&frac17", "\u2150" } },
			{ "1/8", new string[] { "&#8539;", "&#215B;", "	&frac18", "\u215B" } },
			{ "3/8", new string[] { "&#8540;", "&#215C;", "	&frac38", "\u215C" } },
			{ "5/8", new string[] { "&#8541;", "&#215D;", "	&frac58", "\u215D" } },
			{ "7/8", new string[] { "&#8542", "	&#215E;", "	&frac78", "\u215E" } },
			{ "1/9", new string[] { "&#8529;", "&#2151;", "\u2151" } },
			{ "1/10", new string[] { "&#8530;", "&#2152;", "\u2152" } },
		};

		foreach (string frac in fractions.Keys)
		{
			foreach (string code in fractions[frac])
			{
				value = value.Replace(code, frac);
			}			
		}

		return value;
	}

	private static string ReplaceNonAlphaNumericAtStart(string value)
	{
		while (!string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[^A-Za-z0-9]"))
		{
			value = value.Remove(0, 1);
		}

		return value;
	}

}
