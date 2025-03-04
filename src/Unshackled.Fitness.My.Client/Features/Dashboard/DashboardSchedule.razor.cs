using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Dashboard.Actions;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardScheduleBase : BaseComponent
{
	protected bool IsLoading { get; set; } = true;
	protected List<ScheduledListModel> Items { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Items = await Mediator.Send(new ListScheduledItems.Query(DateOnly.FromDateTime(DateTimeOffset.Now.Date)));
		IsLoading = false;
	}

	protected async Task HandleSkipSessionClicked(ScheduledListModel model)
	{
		model.IsSkipping = true;

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

		model.IsSkipping = false;
	}

	protected async Task HandleSkipWorkoutClicked(ScheduledListModel model)
	{
		model.IsSkipping = true;

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

		model.IsSkipping = false;
	}

	protected async Task HandleTrackActivityClicked(ScheduledListModel model)
	{
		model.IsTracking = true;
		StateHasChanged();

		await SaveLocalString(Globals.LocalStorageKeys.TrackTrainingSessionSid, model.Sid);
		NavManager.NavigateTo($"/activities");

		model.IsTracking = false;
		StateHasChanged();
	}

	protected async Task HandleTrackWorkoutClicked(ScheduledListModel model)
	{
		model.IsTracking = true;
		var result = await Mediator.Send(new AddWorkout.Command(model.Sid));
		if (result.Success)
		{
			NavManager.NavigateTo($"/workouts/{result.Payload}");
		}
		else
		{
			ShowNotification(result);
		}
		model.IsTracking = false;
	}
}