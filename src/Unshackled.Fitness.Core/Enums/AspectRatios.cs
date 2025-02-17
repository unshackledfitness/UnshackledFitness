namespace Unshackled.Fitness.Core.Enums;

public enum AspectRatios
{
	R1x1 = 0,
	R16x9 = 1,
}

public static class AspectRatiosExtenstions
{
	public static double Ratio(this AspectRatios ratio)
	{
		return ratio switch
		{
			AspectRatios.R16x9 => 1.778,
			AspectRatios.R1x1 => 1,
			_ => 1
		};
	}

	public static string Title(this AspectRatios ratio)
	{
		return ratio switch
		{
			AspectRatios.R16x9 => "16x9",
			AspectRatios.R1x1 => "1x1",
			_ => string.Empty
		};
	}
}
