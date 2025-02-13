using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardStatsBase : BaseComponent
{
	[Parameter] public DashboardStatsModel Model { get; set; } = default!;
	protected string FirstYear { get; set; } = string.Empty;
	protected DistanceUnits DistanceUnit { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		DistanceUnit = State.ActiveMember.Settings.DefaultUnits == UnitSystems.Imperial ? DistanceUnits.Mile : DistanceUnits.Kilometer;
		FirstYear = Model.Years.FirstOrDefault().ToString();
	}
}