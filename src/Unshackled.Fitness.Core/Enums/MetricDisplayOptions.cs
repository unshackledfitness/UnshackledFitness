namespace Unshackled.Fitness.Core.Enums;

public enum MetricDisplayOptions
{
	None = 0,
	Grouped = 1,
	Flat = 2
}

public static class MetricDisplayOptionsExtensions
{
	public static string Title(this MetricDisplayOptions option)
	{
		return option switch
		{
			MetricDisplayOptions.Flat => "Flat",
			MetricDisplayOptions.Grouped => "Grouped",
			MetricDisplayOptions.None => "None",
			_ => string.Empty,
		};
	}
}