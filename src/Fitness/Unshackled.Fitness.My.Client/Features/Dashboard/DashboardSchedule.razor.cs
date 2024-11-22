using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardProgramBase : BaseComponent<Member>
{
	protected List<ScheduledListModel> Items { get; set; } = new();
	public bool IsWorking { get; set; }
	public bool IsSkipping { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Items = await Mediator.Send(new ListScheduledItems.Query(DateTimeOffset.Now.Date));
	}

	protected async Task HandleSkipSessionClicked(ScheduledListModel model)
	{
		IsSkipping = true;

		var result = await Mediator.Send(new SkipTrainingSession.Command(model.ParentSid));
		if (result.Success)
		{
			int idx = Items.IndexOf(model);
			Items.Remove(model);
			if (result.Payload != null)
			{
				Items.Insert(idx, result.Payload);
			}
		}

		IsSkipping = false;
	}

	protected async Task HandleSkipWorkoutClicked(ScheduledListModel model)
	{
		IsSkipping = true;

		var result = await Mediator.Send(new SkipWorkout.Command(model.ParentSid));
		if (result.Success)
		{
			int idx = Items.IndexOf(model);
			Items.Remove(model);
			if (result.Payload != null)
			{
				Items.Insert(idx, result.Payload);
			}
		}

		IsSkipping = false;
	}

	protected async Task HandleTrackActivityClicked(string sid)
	{
		IsWorking = true;
		await SaveLocalString(FitnessGlobals.LocalStorageKeys.TrackTrainingSessionSid, sid);
		NavManager.NavigateTo($"/activities");
		IsWorking = false;
	}

	protected async Task HandleTrackWorkoutClicked(string sid)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddWorkout.Command(sid));
		if (result.Success)
		{
			NavManager.NavigateTo($"/workouts/{result.Payload}");
		}
		else
		{
			ShowNotification(result);
		}
		IsWorking = false;
	}
}