using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class IndexBase : BaseComponent
{
	public WorkoutStatsModel Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		// Get data
		var toDateUtc = DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime();
		Model.ToDateUtc = toDateUtc;
		Model = await Mediator.Send(new GetWorkoutStats.Query(toDateUtc));
	}

	protected async Task HandleYearChanged(DateTime toDateUtc)
	{
		Model = await Mediator.Send(new GetWorkoutStats.Query(toDateUtc));
	}
}
