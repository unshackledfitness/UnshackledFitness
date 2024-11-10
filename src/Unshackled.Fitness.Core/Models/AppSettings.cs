using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Models;

public class AppSettings : ICloneable
{
	// General
	public UnitSystems DefaultUnits { get; set; } = UnitSystems.Metric;

	// Dashboard
	public string ActivityDisplayColor { get; set; } = "#4e85f6ff";
	public string WorkoutDisplayColor { get; set; } = "#2660f5ff";
	public string MixedDisplayColor { get; set; } = "#1841a3ff";

	// Strength
	public int DisplaySplitTracking { get; set; } = 0;
	public bool HideCompleteSets { get; set; } = false;

	// Metrics
	public MetricDisplayOptions MetricsDashboardDisplay { get; set; } = MetricDisplayOptions.Grouped;

	public object Clone()
	{
		return new AppSettings
		{
			ActivityDisplayColor = ActivityDisplayColor,
			DefaultUnits = DefaultUnits,
			DisplaySplitTracking = DisplaySplitTracking,
			HideCompleteSets = HideCompleteSets,
			MetricsDashboardDisplay = MetricsDashboardDisplay,
			MixedDisplayColor = MixedDisplayColor,
			WorkoutDisplayColor = WorkoutDisplayColor
		};
	}
}