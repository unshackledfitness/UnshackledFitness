namespace Unshackled.Studio.Core.Client.Extensions;

public static class ColorExtensions
{

	public static string SwatchStyle(this string? color, string size = "1.5em")
	{
		if (!string.IsNullOrEmpty(color))
		{
			return $"min-width:{size};min-height:{size};background-color:{color};border-radius:.1em;";
		}
		return string.Empty;
	}
}
