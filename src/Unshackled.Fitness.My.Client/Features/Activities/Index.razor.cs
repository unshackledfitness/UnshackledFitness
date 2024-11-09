using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Activities.Actions;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Activities;

public class IndexBase : BaseSearchComponent<SearchActivitiesModel, ActivityListModel>
{
	protected enum Views
	{
		None,
		Add
	}

	protected DateRange DateRangeSearch { get; set; } = new DateRange();
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.Add => "Add Activity",
		_ => string.Empty
	};
	protected List<ActivityTypeListModel> ActivityTypes { get; set; } = [];
	protected bool HasActivityTypes => ActivityTypes.Count > 0;
	protected AppSettings AppSettings => ((Member)State.ActiveMember).Settings;	

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		SearchKey = "SearchActivities";

		Breadcrumbs.Add(new BreadcrumbItem("Activities", null, true));

		SearchModel = await GetLocalSetting(SearchKey) ?? new();
		if (SearchModel.DateStart.HasValue && SearchModel.DateEnd.HasValue)
		{
			DateRangeSearch = new DateRange(SearchModel.DateStart.Value, SearchModel.DateEnd.Value.AddDays(-1));
		}

		ActivityTypes = await Mediator.Send(new ListActivityTypes.Query());

		await DoSearch(1);
	}

	protected async override Task DoSearch(int page)
	{
		SearchModel.Page = page;

		IsLoading = true;
		await SaveLocalSetting(SearchKey, SearchModel);
		SearchResults = await Mediator.Send(new SearchActivities.Query(SearchModel));
		IsLoading = false;
	}

	protected async Task HandleActivityAdded(string sid)
	{
		DrawerView = Views.None;
		await DoSearch(SearchModel.Page);
	}

	protected void HandleAddActivityClicked()
	{

		DrawerView = Views.Add;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
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