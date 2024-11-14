using MudBlazor;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions;

public partial class IndexBase : BaseSearchComponent<SearchSessionsModel, SessionListItem>
{
	protected override bool DisableControls => IsLoading || IsWorking;
	protected List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	protected FormSessionModel FormModel { get; set; } = new();
	protected string TrackNowSid { get; set; } = string.Empty;
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Training Session"; 
	protected bool HasActivityTypes => ActivityTypes.Count > 0;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		SearchKey = "SearchTrainingSessions";

		Breadcrumbs.Add(new BreadcrumbItem("Training Sessions", null, true));

		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());

		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		await DoSearch(SearchModel.Page);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchSessions.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		bool isMetric = ((Member)State.ActiveMember).Settings.DefaultUnits == UnitSystems.Metric;
		FormModel = new();
		FormModel.SetUnits(isMetric);
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted(FormSessionModel model)
	{
		IsWorking = true;
		var result = await Mediator.Send(new AddSession.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			DrawerOpen = false;
			NavManager.NavigateTo($"/training-sessions/{result.Payload}");
		}
		IsWorking = false;
	}

	protected void HandleCancelClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleTrackNowClicked(SessionListItem item)
	{
		IsWorking = true;
		TrackNowSid = item.Sid;
		await SaveLocalSetting(FitnessGlobals.LocalStorageKeys.TrackTrainingSessionSid, TrackNowSid);
		NavManager.NavigateTo($"/activities");
	}
}
