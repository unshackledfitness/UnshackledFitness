using System.Text.Json;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Calendar.Actions;
using Unshackled.Fitness.My.Client.Features.Calendar.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Utils;

namespace Unshackled.Fitness.My.Client.Features.Calendar;

public class IndexBase : BaseComponent
{
	protected CalendarModel CalendarModel { get; set; } = new();
	protected SearchCalendarModel SearchModel { get; set; } = new();
	protected FormSearchModel FormModel { get; set; } = new();
	protected bool IsDrawerOpen { get; set; }
	protected bool IsLoading { get; set; } = true;
	protected bool IsSaving { get; set; } = false;
	protected bool DisableControls => IsSaving || IsLoading;
	protected Dictionary<string, bool> FilterVisibility { get; set; } = new();
	protected List<PresetModel> Presets { get; set; } = [];

	private string visibilityKey = "MetricVisibility";
	private string searchKey = "SearchCalendar";
	private DateTime defaultDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Breadcrumbs.Add(new BreadcrumbItem("Calendar", null, true));

		await InitializeSearchForm();
		FilterVisibility = await localStorageService.GetItemAsync<Dictionary<string, bool>>(visibilityKey) ?? new();

		await GetCalendar();
		IsLoading = false;

		Presets = await Mediator.Send(new ListPresets.Query());
	}

	private async Task GetCalendar()
	{
		SearchModel.FromDateUtc = SearchModel.FromDate.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime();
		SearchModel.ToDateUtc = SearchModel.ToDate.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Local).ToUniversalTime();
		SearchModel.ActivityColor = State.ActiveMember.Settings.ActivityDisplayColor;
		SearchModel.WorkoutColor = State.ActiveMember.Settings.WorkoutDisplayColor;

		CalendarModel = await Mediator.Send(new GetCalendar.Query(SearchModel));

		foreach (var filter in CalendarModel.BlockFilters)
		{
			if (FilterVisibility.ContainsKey(filter.FilterId))
			{
				filter.IsChecked = FilterVisibility[filter.FilterId];
			}
			if (!filter.IsChecked)
			{
				SetBlockVisibility(filter);
			}
		}
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

	protected async Task HandleFiltersChanged(List<CalendarBlockFilterModel> filters)
	{
		await SetFilterVisibility(filters);
	}

	protected async Task HandlePresetAdded(string title)
	{
		IsSaving = true;
		FormPresetModel model = new()
		{
			Settings = JsonSerializer.Serialize(FilterVisibility),
			Title = title
		};
		var result = await Mediator.Send(new AddPreset.Command(model));
		if (result.Success && result.Payload != null)
		{
			Presets.Add(result.Payload);
			Presets = Presets.OrderBy(x => x.Title).ToList();
		}
		ShowNotification(result);
		IsSaving = false;
	}

	protected async Task HandlePresetApplied(List<CalendarBlockFilterModel> filters)
	{
		await SetFilterVisibility(filters);
		IsDrawerOpen = false;
	}

	protected async Task HandlePresetDeleted(PresetModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new DeletePreset.Command(model.Sid));
		if (result.Success)
		{
			Presets.Remove(model);
		}
		ShowNotification(result);
		IsSaving = false;
	}

	protected async Task HandlePresetUpdated(PresetModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdatePreset.Command(model.Sid, JsonSerializer.Serialize(FilterVisibility)));
		if (result.Success && result.Payload != null)
		{
			model.Settings = result.Payload.Settings;
		}
		ShowNotification(result);
		IsSaving = false;
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

	protected void HandleShowVisibilityClicked()
	{
		IsDrawerOpen = true;
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

	private async Task SetFilterVisibility(List<CalendarBlockFilterModel> filters)
	{
		foreach (var filter in filters)
		{
			if (FilterVisibility.ContainsKey(filter.FilterId))
			{
				FilterVisibility[filter.FilterId] = filter.IsChecked;
			}
			else
			{
				FilterVisibility.Add(filter.FilterId, filter.IsChecked);
			}
			SetBlockVisibility(filter);
		}
		await localStorageService.SetItemAsync(visibilityKey, FilterVisibility, CancellationToken.None);
	}

	private void SetBlockVisibility(CalendarBlockFilterModel model)
	{
		foreach (var day in CalendarModel.Days)
		{
			foreach (var block in day.Blocks)
			{
				if (block.FilterId == model.FilterId)
				{
					block.IsVisible = model.IsChecked;
				}
			}
		}
	}
}