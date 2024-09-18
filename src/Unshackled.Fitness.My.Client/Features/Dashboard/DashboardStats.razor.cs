using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.Core.Components;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardStatsBase : BaseComponent
{
	[Parameter] public WorkoutStatsModel Model { get; set; } = default!;
	public string FirstYear { get; set; } = string.Empty;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		FirstYear = Model.Years.FirstOrDefault().ToString();
	}
}