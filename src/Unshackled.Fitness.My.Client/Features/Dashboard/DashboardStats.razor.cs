using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardStatsBase : BaseComponent
{
	[Parameter] public DashboardStatsModel Model { get; set; } = default!;
	protected string FirstYear { get; set; } = string.Empty;
	protected Member ActiveMember => (Member)State.ActiveMember;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		FirstYear = Model.Years.FirstOrDefault().ToString();
	}
}