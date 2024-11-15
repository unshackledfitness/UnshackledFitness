using Microsoft.AspNetCore.Components;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Client.Utils;

namespace Unshackled.Studio.Core.Client.Extensions;

public static class DecimalExtensions
{
	public static string ShortLabel(this decimal? value)
	{
		if (!value.HasValue)
			return "0";

		return value.Value.ShortLabel();
	}

	public static string ShortLabel(this decimal value)
	{
		if (value > 1000000000)
		{
			return (value / 1000000000).ToString("0.#") + "B";
		}
		else if (value > 1000000)
		{
			return (value / 1000000).ToString("0.#") + "M";
		}
		else if (value > 1000)
		{
			return (value / 1000).ToString("0.#") + "K";
		}
		else
		{
			return value.ToString("0.#");
		}
	}

	public static MarkupString ToHtmlFraction(this decimal value)
	{
		string v = value.ToFractionString();
		return v.ToHtmlFraction();
	}

	public static string ToFractionString(this decimal value)
	{
		string output = string.Empty;

		// Remove whole number
		int whole = (int)Math.Floor(value);
		value -= whole;

		if (value > 0M)
		{
			decimal rounded = Math.Round(value, 3, MidpointRounding.AwayFromZero);
			Fraction frac = Calculator.ClosestFraction(rounded, 0.001M);
			output = $"{frac.Numerator}/{frac.Denominator}";
		}

		if (whole > 0 && !string.IsNullOrEmpty(output))
		{
			output = $"{whole} {output}";
			return output;
		}
		else if (whole == 0 && !string.IsNullOrEmpty(output))
		{
			return output;
		}
		else
		{
			if (value == 0M)
				return whole.ToString("N0");
			else
				return (whole + value).ToString("N2");
		}
	}
}
