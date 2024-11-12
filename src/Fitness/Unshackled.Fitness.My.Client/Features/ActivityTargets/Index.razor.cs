using MudBlazor;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Actions;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets;

public partial class IndexBase : BaseSearchComponent<SearchTargetsModel, TargetListItem>
{
	protected override bool DisableControls => IsLoading || IsWorking;
	protected List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	protected FormTargetModel FormModel { get; set; } = new();
	protected string TrackNowSid { get; set; } = string.Empty;
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Target";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		SearchKey = "SearchActivityTargets";

		Breadcrumbs.Add(new BreadcrumbItem("Targets", null, true));

		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());

		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		await DoSearch(SearchModel.Page);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchTargets.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted(FormTargetModel model)
	{
		IsWorking = true;
		await Task.Delay(10);
		//var result = await Mediator.Send(new AddTarget.Command(model));
		//ShowNotification(result);
		//if (result.Success)
		//{
		//	DrawerOpen = false;
		//	NavManager.NavigateTo($"/templates/{result.Payload}");
		//}
		IsWorking = false;
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleTrackNowClicked(TargetListItem item)
	{
		IsWorking = true;
		TrackNowSid = item.Sid;
		await Task.Delay(10);
		//var result = await Mediator.Send(new AddWorkout.Command(item.Sid));
		//if (result.Success)
		//{
		//	NavManager.NavigateTo($"/workouts/{result.Payload}");
		//}
		//else
		//{
		//	ShowNotification(result);
		//}
		TrackNowSid = string.Empty;
		IsWorking = false;
	}
}
