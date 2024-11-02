using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public class AppSettings : ICloneable
{
	public Themes AppTheme { get; set; } = Themes.System;

	// General
	public UnitSystems DefaultUnits { get; set; } = UnitSystems.Metric;

	// Strength
	public int DisplaySplitTracking { get; set; } = 0;
	public bool HideCompleteSets { get; set; } = false;

	// Metrics
	public MetricDisplayOptions MetricsDashboardDisplay { get; set; } = MetricDisplayOptions.Grouped;

	public object Clone()
	{
		return new AppSettings
		{
			AppTheme = AppTheme,
			DefaultUnits = DefaultUnits,
			DisplaySplitTracking = DisplaySplitTracking,
			HideCompleteSets = HideCompleteSets,
			MetricsDashboardDisplay = MetricsDashboardDisplay
		};
	}
}