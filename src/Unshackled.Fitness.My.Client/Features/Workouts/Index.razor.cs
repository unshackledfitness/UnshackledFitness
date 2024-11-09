using MudBlazor;
using Unshackled.Fitness.My.Client.Features.Workouts.Actions;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Workouts;

public class IndexBase : BaseSearchComponent<SearchWorkoutModel, WorkoutListModel>
{
	protected DateRange DateRangeSearch { get; set; } = new DateRange();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add New Workout";
	protected FormPropertiesModel FormModel { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		SearchKey = "SearchWorkouts";

		Breadcrumbs.Add(new BreadcrumbItem("Workouts", null, true));

		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		if (SearchModel.DateStart.HasValue && SearchModel.DateEnd.HasValue)
		{
			DateRangeSearch = new DateRange(SearchModel.DateStart.Value, SearchModel.DateEnd.Value.AddDays(-1));
		}

		await DoSearch(SearchModel.Page);
	}

	protected Color GetStatusColor(WorkoutListModel workout)
	{
		if (workout.DateStartedUtc.HasValue && workout.DateCompletedUtc.HasValue)
			return Color.Secondary;
		else
			return Color.Default;
	}

	protected string GetStatusIcon(WorkoutListModel workout)
	{
		if (workout.DateStartedUtc.HasValue && workout.DateCompletedUtc.HasValue)
			return Icons.Material.Filled.CheckCircle;
		else if (workout.DateStartedUtc.HasValue && !workout.DateCompletedUtc.HasValue)
			return Icons.Material.Filled.Contrast;
		else
			return Icons.Material.Outlined.Circle;
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchWorkouts.Query(SearchModel));
		IsLoading = false;
	}

	protected void HandleAddClicked()
	{
		FormModel = new();
		DrawerOpen = true;
	}

	protected async Task HandleAddFormSubmitted(FormPropertiesModel properties)
	{
		IsWorking = true;

		var model = new AddWorkoutModel
		{
			Title = properties.Title
		};

		var result = await Mediator.Send(new AddWorkout.Command(model));
		if (result.Success)
		{
			DrawerOpen = false;
			NavManager.NavigateTo($"/workouts/{result.Payload}");
		}
		else
		{
			ShowNotification(result);
		}

		IsWorking = false;
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected void HandleDateRangeChanged(DateRange dateRange)
	{
		DateRangeSearch = dateRange;
		SearchModel.DateStart = dateRange.Start;
		SearchModel.DateEnd = dateRange.End.HasValue ? dateRange.End.Value.AddDays(1) : dateRange.End;
	}

	protected async override Task HandleResetClicked()
	{
		DateRangeSearch = new DateRange();
		await base.HandleResetClicked();
	}
}