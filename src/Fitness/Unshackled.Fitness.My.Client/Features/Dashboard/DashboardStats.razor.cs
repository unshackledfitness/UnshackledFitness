using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardStatsBase : BaseComponent
{
	[Parameter] public DashboardStatsModel Model { get; set; } = default!;
	protected string FirstYear { get; set; } = string.Empty;
	protected DistanceUnits DistanceUnit { get; set; }
	protected Member ActiveMember => (Member)State.ActiveMember;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		DistanceUnit = ActiveMember.Settings.DefaultUnits == UnitSystems.Imperial ? DistanceUnits.Mile : DistanceUnits.Kilometer;
		FirstYear = Model.Years.FirstOrDefault().ToString();
	}
}