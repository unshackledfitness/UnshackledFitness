using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Metrics.Actions;
using Unshackled.Fitness.My.Client.Features.Metrics.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Utils;

namespace Unshackled.Fitness.My.Client.Features.Metrics;

public class SingleBase : BaseComponent
{
	[Parameter] public string Sid { get; set; } = string.Empty;
	protected bool IsLoading { get; set; } = true;
	protected bool DisableControls => IsLoading;
	protected FormMetricDefinitionModel MetricDefinition { get; set; } = new();
	protected CalendarModel CalendarModel { get; set; } = new();
	protected SearchCalendarModel SearchModel { get; set; } = new();
	protected FormSearchModel FormModel { get; set; } = new();
	protected Views CurrentView { get; set; } = Views.Calendar;

	private string searchKey = "SearchMetrics";
	private DateTime defaultDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
	protected enum Views
	{
		Calendar,
		Charts
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Metrics", "/metrics"));
		Breadcrumbs.Add(new BreadcrumbItem("Metric", null, true));

		await InitializeSearchForm();

		MetricDefinition = await Mediator.Send(new GetDefinition.Query(Sid));

		await GetCalendar();
		IsLoading = false;
	}

	private async Task GetCalendar()
	{
		SearchModel.FromDateUtc = SearchModel.FromDate.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime();
		SearchModel.ToDateUtc = SearchModel.ToDate.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime();

		CalendarModel = await Mediator.Send(new GetCalendar.Query(Sid, SearchModel));
	}

	protected string GetMonthDisplay(int num)
	{
		string output = "--";
		if (FormModel.EndDate.HasValue && num > 0)
		{
			string title = num == 1 ? "month" : "months";
			output = $"{num} {title} ({FormModel.EndDate.Value.AddMonths(-num).ToString("MM/yyyy")})";
		}
		return output;
	}

	protected async Task HandleResetClicked()
	{
		await InitializeSearchForm(true);
		await localStorageService.RemoveItemAsync(searchKey);
		await GetCalendar();
	}

	protected async Task HandleSearchClicked()
	{
		var range = Calculator.DateRange(FormModel.EndDate, FormModel.NumberOfMonths, defaultDate);
		SearchModel = new()
		{
			FromDate = range.Start,
			ToDate = range.End
		};
		await localStorageService.SetItemAsync(searchKey, FormModel, CancellationToken.None);
		await GetCalendar();
	}

	private async Task InitializeSearchForm(bool reset = false)
	{
		var defaultModel = new FormSearchModel
		{
			EndDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1),
			NumberOfMonths = 0
		};

		FormModel = reset ? defaultModel : await localStorageService.GetItemAsync<FormSearchModel>(searchKey) ?? defaultModel;

		var range = Calculator.DateRange(FormModel.EndDate, FormModel.NumberOfMonths, defaultDate);
		SearchModel = new()
		{
			FromDate = range.Start,
			ToDate = range.End
		};
	}
}