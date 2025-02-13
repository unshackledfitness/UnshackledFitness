namespace Unshackled.Fitness.My.Client.Extensions;

public static class DoubleExtensions
{
	public static string ShortLabel(this double? value)
	{
		if (!value.HasValue)
			return "0";

		return value.Value.ShortLabel();
	}

	public static string ShortLabel(this double value)
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
}
