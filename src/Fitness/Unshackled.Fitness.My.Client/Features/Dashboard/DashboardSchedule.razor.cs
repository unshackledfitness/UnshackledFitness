using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardProgramBase : BaseComponent<Member>
{
	protected List<ScheduledListModel> Items { get; set; } = [];
	public bool IsTrackingSession { get; set; }
	public bool IsTrackingWorkout { get; set; }
	public bool IsSkippingSession { get; set; }
	public bool IsSkippingWorkout { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Items = await Mediator.Send(new ListScheduledItems.Query(DateTimeOffset.Now.Date));
	}

	protected async Task HandleSkipSessionClicked(ScheduledListModel model)
	{
		IsSkippingSession = true;

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

		IsSkippingSession = false;
	}

	protected async Task HandleSkipWorkoutClicked(ScheduledListModel model)
	{
		IsSkippingWorkout = true;

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

		IsSkippingWorkout = false;
	}

	protected async Task HandleTrackActivityClicked(string sid)
	{
		IsTrackingSession = true;
		await SaveLocalString(FitnessGlobals.LocalStorageKeys.TrackTrainingSessionSid, sid);
		NavManager.NavigateTo($"/activities");
		IsTrackingSession = false;
	}

	protected async Task HandleTrackWorkoutClicked(string sid)
	{
		IsTrackingWorkout = true;
		var result = await Mediator.Send(new AddWorkout.Command(sid));
		if (result.Success)
		{
			NavManager.NavigateTo($"/workouts/{result.Payload}");
		}
		else
		{
			ShowNotification(result);
		}
		IsTrackingWorkout = false;
	}
}