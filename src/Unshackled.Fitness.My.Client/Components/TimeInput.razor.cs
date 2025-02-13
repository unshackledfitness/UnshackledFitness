using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Unshackled.Fitness.My.Client.Components;

public class TimeInputBase : ComponentBase
{
	[Parameter]
	public int? Value
	{
		get { return seconds; }
		set
		{
			seconds = value ?? 0;
			SplitTimes();
		}
	}

	[Parameter]
	public EventCallback<int?> ValueChanged { get; set; }

	[Parameter] public string? Label { get; set; }
	[Parameter] public string? Placeholder { get; set; } = "--";
	[Parameter] public Typo Typo { get; set; } = Typo.body1;
	[Parameter] public bool Disabled { get; set; }
	[Parameter] public Margin Margin { get; set; } = Margin.Dense;
	[Parameter] public Variant Variant { get; set; } = Variant.Text;
	[Parameter] public string? HourLabel { get; set; } = "hour";
	[Parameter] public string? MinuteLabel { get; set; } = "min";
	[Parameter] public string? SecondLabel { get; set; } = "sec";

	private int seconds;

	protected int? Hours { get; set; }
	protected int? Minutes { get; set; }
	protected int? Seconds { get; set; }

	private void CalcSeconds()
	{
		int initSecs = seconds;

		int h = Hours.HasValue ? Hours.Value : 0;
		int m = Minutes.HasValue ? Minutes.Value : 0;
		int s = Seconds.HasValue ? Seconds.Value : 0;

		var totalTime = new TimeSpan(h, m, s);
		seconds = (int)totalTime.TotalSeconds;

		if (ValueChanged.HasDelegate && initSecs != seconds)
		{
			ValueChanged.InvokeAsync(seconds);
		}
	}

	private void SplitTimes()
	{
		var input = TimeSpan.FromSeconds(seconds);
		Hours = input.Hours > 0 ? input.Hours : null;
		Minutes = input.Minutes > 0 || input.Hours > 0 ? input.Minutes : null;
		Seconds = input.Seconds > 0 || input.Hours > 0 || input.Minutes > 0 ? input.Seconds : null;
	}

	protected void HandleHourChanged(int? value)
	{
		Hours = value;
		CalcSeconds();
	}

	protected void HandleMinChanged(int? value)
	{
		Minutes = value;
		CalcSeconds();
	}

	protected void HandleSecChanged(int? value)
	{
		Seconds = value;
		CalcSeconds();
	}
}
