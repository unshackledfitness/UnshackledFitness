using MudBlazor;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.My.Client.Extensions;

public static class ColorExtensions
{
	public static string BackgroundColorClass(this WorkoutSetTypes setType)
	{
		return setType switch
		{
			WorkoutSetTypes.Standard => $"bg-palette-{Color.Secondary.ToString().ToLower()}",
			WorkoutSetTypes.Warmup => $"bg-palette-{Color.Warning.ToString().ToLower()}",
			_ => $"bg-palette-{Color.Default.ToString().ToLower()}"
		};
	}

	public static Color DisplayColor(this WorkoutSetTypes setType)
	{
		return setType switch
		{
			WorkoutSetTypes.Standard => Color.Secondary,
			WorkoutSetTypes.Warmup => Color.Warning,
			_ => Color.Default
		};
	}

	public static string DisplayColorClass(this WorkoutSetTypes setType)
	{
		return setType switch
		{
			WorkoutSetTypes.Standard => $"palette-{Color.Secondary.ToString().ToLower()}",
			WorkoutSetTypes.Warmup => $"palette-{Color.Warning.ToString().ToLower()}",
			_ => $"palette-{Color.Default.ToString().ToLower()}"
		};
	}

	public static string SwatchStyle(this string? color, string size = "1.5em")
	{
		if (!string.IsNullOrEmpty(color))
		{
			return $"min-width:{size};min-height:{size};background-color:{color};border-radius:.1em;";
		}
		return string.Empty;
	}
}
