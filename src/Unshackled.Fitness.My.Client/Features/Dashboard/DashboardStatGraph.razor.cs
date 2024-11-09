using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Dashboard;

public class DashboardStatGraphBase : BaseComponent
{
	[Parameter] public WorkoutStatsModel Model { get; set; } = default!;
	[Parameter] public EventCallback<DateTime> OnYearChanged { get; set; }
	protected DateTime ToDateUtc { get; set; } = DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime();
	public string LabelYear { get; set; } = "Past Year";
	public bool IsWorking { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		Model.Fill();
	}

	protected string GetMonthStyle(int start, int end)
	{
		return $"grid-column-start: {start}; grid-column-end: {end};";
	}

	protected string GetDayName(int day)
	{
		return day switch
		{
			1 => "M",
			3 => "W",
			5 => "F",
			_ => string.Empty
		};
	}

	protected async Task HandlePastYearClicked()
	{
		if (LabelYear != "Past Year")
		{
			LabelYear = "Past Year";
			ToDateUtc = DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime();
			await OnYearChanged.InvokeAsync(ToDateUtc);
		}
	}

	protected async Task HandleYearClicked(int year)
	{
		if (year != ToDateUtc.Year)
		{
			LabelYear = year.ToString();
			ToDateUtc = new DateTime(year + 1, 1, 1).ToUniversalTime();
			await OnYearChanged.InvokeAsync(ToDateUtc);
		}
	}
}