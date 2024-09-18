using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;

namespace Unshackled.Fitness.Core.Components;

/*
 *  Adapted from MudTimePicker in MudBlazor Component Library to add ability to pick seconds
 *  - https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Components/TimePicker/MudTimePicker.razor.cs
 */

public partial class SecondsTimePicker : MudPicker<TimeSpan?>, IAsyncDisposable
{
	private const string Format24Hours = "HH:mm:ss";

	public enum OpenToView
	{
		Hours,
		Minutes,
		Seconds
	}

	public SecondsTimePicker() : base(new DefaultConverter<TimeSpan?>())
	{
		Converter.GetFunc = OnGet;
		Converter.SetFunc = OnSet;
		((DefaultConverter<TimeSpan?>)Converter).Format = Format24Hours;
		AdornmentIcon = Icons.Material.Filled.AccessTime;
		AdornmentAriaLabel = "Open Time Picker";
	}

	private string OnSet(TimeSpan? timespan)
	{
		if (timespan == null)
		{
			return string.Empty;
		}

		var time = DateTime.Today.Add(timespan.Value);

		return time.ToString(((DefaultConverter<TimeSpan?>)Converter).Format, Culture);
	}

	private TimeSpan? OnGet(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}

		if (DateTime.TryParseExact(value, ((DefaultConverter<TimeSpan?>)Converter).Format, Culture, DateTimeStyles.None, out var time))
		{
			return time.TimeOfDay;
		}

		if (DateTime.TryParseExact(value, Format24Hours, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
		{
			return time.TimeOfDay;
		}

		HandleParsingError();
		return null;
	}

	private void HandleParsingError()
	{
		const string ParsingErrorMessage = "Not a valid time span";
		Converter.GetError = true;
		Converter.GetErrorMessage = ParsingErrorMessage;
		Converter.OnError?.Invoke(ParsingErrorMessage);
	}

	private OpenToView currentView;
	private string timeFormat = string.Empty;

	internal TimeSpan? TimeIntermediate { get; private set; }

	/// <summary>
	/// First view to show in the MudDatePicker.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.PickerBehavior)]
	public OpenToView OpenTo { get; set; } = OpenToView.Hours;

	/// <summary>
	/// Choose the edition mode. By default, you can edit hours and minutes.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.PickerBehavior)]
	public TimeEditMode TimeEditMode { get; set; } = TimeEditMode.Normal;

	/// <summary>
	/// Sets the amount of time in milliseconds to wait before closing the picker. This helps the user see that the time was selected before the popover disappears.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.PickerBehavior)]
	public int ClosingDelay { get; set; } = 200;

	/// <summary>
	/// If AutoClose is set to true and PickerActions are defined, the hour and the minutes can be defined without any action.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.PickerBehavior)]
	public bool AutoClose { get; set; }

	/// <summary>
	/// Sets the number interval for minutes.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.PickerBehavior)]
	public int MinuteSelectionStep { get; set; } = 1;

	/// <summary>
	/// String Format for selected time view
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.Behavior)]
	public string TimeFormat
	{
		get => timeFormat;
		set
		{
			if (timeFormat == value)
			{
				return;
			}

			timeFormat = value;
			if (Converter is DefaultConverter<TimeSpan?> defaultConverter)
			{
				defaultConverter.Format = timeFormat;
			}

			Touched = true;
			SetTextAsync(Converter.Set(_value), false).CatchAndLog();
		}
	}

	/// <summary>
	/// The currently selected time (two-way bindable). If null, then nothing was selected.
	/// </summary>
	[Parameter]
	[Category(CategoryTypes.FormComponent.Data)]
	public TimeSpan? Time
	{
		get => _value;
		set => SetTimeAsync(value, true).CatchAndLog();
	}

	protected async Task SetTimeAsync(TimeSpan? time, bool updateValue)
	{
		if (_value != time)
		{
			Touched = true;
			TimeIntermediate = time;
			_value = time;
			if (updateValue)
			{
				await SetTextAsync(Converter.Set(_value), false);
			}

			UpdateTimeSetFromTime();
			await TimeChanged.InvokeAsync(_value);
			await BeginValidateAsync();
			FieldChanged(_value);
		}
	}

	/// <summary>
	/// Fired when the date changes.
	/// </summary>
	[Parameter] public EventCallback<TimeSpan?> TimeChanged { get; set; }

	protected override Task StringValueChangedAsync(string value)
	{
		Touched = true;

		// Update the time property (without updating back the Value property)
		return SetTimeAsync(Converter.Get(value), false);
	}

	//The last line cannot be tested
	[ExcludeFromCodeCoverage]
	protected override async Task OnPickerOpenedAsync()
	{
		await base.OnPickerOpenedAsync();
		currentView = TimeEditMode switch
		{
			TimeEditMode.Normal => OpenTo,
			TimeEditMode.OnlyHours => OpenToView.Hours,
			TimeEditMode.OnlyMinutes => OpenToView.Minutes,
			_ => currentView
		};
	}

	protected override Task SubmitAsync()
	{
		if (GetReadOnlyState())
		{
			return Task.CompletedTask;
		}

		Time = TimeIntermediate;

		return Task.CompletedTask;
	}

	public override async Task ClearAsync(bool close = true)
	{
		TimeIntermediate = null;
		await SetTimeAsync(null, true);

		if (AutoClose)
		{
			await CloseAsync(false);
		}
	}

	private string GetHourString()
	{
		if (TimeIntermediate == null)
		{
			return "--";
		}

		var h = TimeIntermediate.Value.Hours;
		return $"{Math.Min(23, Math.Max(0, h)):D2}";
	}

	private string GetMinuteString()
	{
		if (TimeIntermediate == null)
		{
			return "--";
		}

		return $"{Math.Min(59, Math.Max(0, TimeIntermediate.Value.Minutes)):D2}";
	}

	private string GetSecondString()
	{
		if (TimeIntermediate == null)
		{
			return "--";
		}

		return $"{Math.Min(59, Math.Max(0, TimeIntermediate.Value.Seconds)):D2}";
	}

	private Task UpdateTimeAsync()
	{
		lastSelectedHour = timeSet.Hour;
		TimeIntermediate = new TimeSpan(timeSet.Hour, timeSet.Minute, 0);
		if ((PickerVariant == PickerVariant.Static && PickerActions == null) || (PickerActions != null && AutoClose))
		{
			return SubmitAsync();
		}

		return Task.CompletedTask;
	}

	private async Task OnHourClickAsync()
	{
		currentView = OpenToView.Hours;
		await FocusAsync();
	}

	private async Task OnMinutesClick()
	{
		currentView = OpenToView.Minutes;
		await FocusAsync();
	}

	private async Task OnSecondsClick()
	{
		currentView = OpenToView.Seconds;
		await FocusAsync();
	}

	protected string ToolbarClassname =>
	new CssBuilder("mud-picker-timepicker-toolbar")
	  .AddClass($"mud-picker-timepicker-toolbar-landscape", Orientation == Orientation.Landscape && PickerVariant == PickerVariant.Static)
	  .AddClass(Class)
	.Build();

	protected string HoursButtonClassname =>
	new CssBuilder("mud-timepicker-button")
	  .AddClass($"mud-timepicker-toolbar-text", currentView != OpenToView.Hours)
	.Build();

	protected string MinuteButtonClassname =>
	new CssBuilder("mud-timepicker-button")
	  .AddClass($"mud-timepicker-toolbar-text", currentView != OpenToView.Minutes)
	.Build();

	protected string SecondButtonClassname =>
	new CssBuilder("mud-timepicker-button")
	  .AddClass($"mud-timepicker-toolbar-text", currentView != OpenToView.Seconds)
	.Build();

	private string HourDialClassname =>
	new CssBuilder("mud-time-picker-hour")
	  .AddClass($"mud-time-picker-dial")
	  .AddClass($"mud-time-picker-dial-out", currentView != OpenToView.Hours)
	  .AddClass($"mud-time-picker-dial-hidden", currentView != OpenToView.Hours)
	.Build();

	private string MinuteDialClassname =>
	new CssBuilder("mud-time-picker-minute")
	  .AddClass($"mud-time-picker-dial")
	  .AddClass($"mud-time-picker-dial-out", currentView != OpenToView.Minutes)
	  .AddClass($"mud-time-picker-dial-hidden", currentView != OpenToView.Minutes)
	.Build();

	private string SecondDialClassname =>
	new CssBuilder("mud-time-picker-minute")
	  .AddClass($"mud-time-picker-dial")
	  .AddClass($"mud-time-picker-dial-out", currentView != OpenToView.Seconds)
	  .AddClass($"mud-time-picker-dial-hidden", currentView != OpenToView.Seconds)
	.Build();

	private string GetClockPinColor()
	{
		return $"mud-picker-time-clock-pin mud-{Color.ToDescriptionString()}";
	}

	private string GetClockPointerColor()
	{
		if (PointerMoving)
		{
			return $"mud-picker-time-clock-pointer mud-{Color.ToDescriptionString()}";
		}
		else
		{
			return $"mud-picker-time-clock-pointer mud-picker-time-clock-pointer-animation mud-{Color.ToDescriptionString()}";
		}
	}

	private string GetClockPointerThumbColor()
	{
		var deg = GetDeg();
		if (deg % 30 == 0)
			return $"mud-picker-time-clock-pointer-thumb mud-onclock-text mud-onclock-primary mud-{Color.ToDescriptionString()}";
		else
			return $"mud-picker-time-clock-pointer-thumb mud-onclock-minute mud-{Color.ToDescriptionString()}-text";
	}

	private string GetNumberColor(int value)
	{
		if (currentView == OpenToView.Hours)
		{
			var h = timeSet.Hour;
			if (h == value)
				return $"mud-clock-number mud-theme-{Color.ToDescriptionString()}";
		}
		else if (currentView == OpenToView.Minutes && timeSet.Minute == value)
		{
			return $"mud-clock-number mud-theme-{Color.ToDescriptionString()}";
		}
		return $"mud-clock-number";
	}

	private double GetDeg()
	{
		double deg = 0;
		if (currentView == OpenToView.Hours)
			deg = timeSet.Hour * 30 % 360;
		if (currentView == OpenToView.Minutes)
			deg = timeSet.Minute * 6 % 360;
		return deg;
	}

	private string GetTransform(double angle, double radius, double offsetX, double offsetY)
	{
		angle = angle / 180 * Math.PI;
		var x = (Math.Sin(angle) * radius + offsetX).ToString("F3", CultureInfo.InvariantCulture);
		var y = ((Math.Cos(angle) + 1) * radius + offsetY).ToString("F3", CultureInfo.InvariantCulture);
		return $"transform: translate({x}px, {y}px);";
	}

	private string GetPointerRotation()
	{
		double deg = 0;
		if (currentView == OpenToView.Hours)
			deg = timeSet.Hour * 30 % 360;
		if (currentView == OpenToView.Minutes)
			deg = timeSet.Minute * 6 % 360;
		if (currentView == OpenToView.Seconds)
			deg = timeSet.Second * 6 % 360;
		return $"rotateZ({deg}deg);";
	}

	private string GetPointerHeight()
	{
		var height = 40;
		if (currentView == OpenToView.Hours)
		{
			if (timeSet.Hour > 0 && timeSet.Hour < 13)
				height = 26;
			else
				height = 40;
		}
		return $"{height}%;";
	}

	private readonly SetTime timeSet = new();
	private int initialHour;
	private int lastSelectedHour;
	private int initialMinute;
	private int initialSecond;
	private DotNetObjectReference<SecondsTimePicker>? dotNetRef;
	private string? clockElementReferenceId;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		UpdateTimeSetFromTime();
		currentView = OpenTo;
		initialHour = timeSet.Hour;
		lastSelectedHour = timeSet.Hour;
		initialMinute = timeSet.Minute;
		initialSecond = timeSet.Second;
		dotNetRef = DotNetObjectReference.Create(this);
	}

	[Inject] private IJSRuntime JsRuntime { get; set; } = null!;

	protected ElementReference ClockElementReference { get; private set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		// Initialize the pointer events for the clock every time it's created (ex: popover opening and closing).
		if (ClockElementReference.Id != clockElementReferenceId)
		{
			clockElementReferenceId = ClockElementReference.Id;

			await JsRuntime.InvokeVoidAsyncWithErrorHandling("mudTimePicker.initPointerEvents", ClockElementReference, dotNetRef);
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (IsJSRuntimeAvailable)
		{
			await JsRuntime.InvokeVoidAsyncWithErrorHandling("mudTimePicker.destroyPointerEvents", ClockElementReference);
		}

		dotNetRef?.Dispose();
	}

	private void UpdateTimeSetFromTime()
	{
		if (TimeIntermediate == null)
		{
			timeSet.Hour = 0;
			timeSet.Minute = 0;
			timeSet.Second = 0;
			return;
		}
		timeSet.Hour = TimeIntermediate.Value.Hours;
		timeSet.Minute = TimeIntermediate.Value.Minutes;
		timeSet.Second = TimeIntermediate.Value.Seconds;
	}

	/// <summary>
	/// <c>true</c> while the main pointer button is held down and moving.
	/// </summary>
	/// <remarks>
	/// Disables clock animations.
	/// </remarks>
	public bool PointerMoving { get; set; }

	/// <summary>
	/// Updates the position of the hands on the clock.
	/// This method is called by the JavaScript events.
	/// </summary>
	/// <param name="value">The minute or hour.</param>
	/// <param name="pointerMoving">Is the pointer being moved?</param>
	[JSInvokable]
	public async Task SelectTimeFromStick(int value, bool pointerMoving)
	{
		if (value == -1)
		{
			// This means a stick wasn't the target (which shouldn't happen).
			return;
		}

		PointerMoving = pointerMoving;

		// Update the .NET properties from the JavaScript events.
		if (currentView == OpenToView.Seconds)
		{
			var seconds = RoundToStepInterval(value);
			timeSet.Second = seconds;
		}
		else if (currentView == OpenToView.Minutes)
		{
			var minute = RoundToStepInterval(value);
			timeSet.Minute = minute;
		}
		else if (currentView == OpenToView.Hours)
		{
			timeSet.Hour = value;
		}

		await UpdateTimeAsync();

		// Manually update because the event won't do it from JavaScript.
		StateHasChanged();
	}

	/// <summary>
	/// Performs the click action for the sticks.
	/// This method is called by the JavaScript events.
	/// </summary>
	/// <param name="value">The minute or hour.</param>
	[JSInvokable]
	public async Task OnStickClick(int value)
	{
		// The pointer is up and not moving so animations can be enabled again.
		PointerMoving = false;

		// Clicking a stick will submit the time.
		if (currentView == OpenToView.Seconds)
		{
			await SubmitAndCloseAsync();
		}
		else if (currentView == OpenToView.Minutes)
		{
			if (TimeEditMode == TimeEditMode.Normal)
			{
				currentView = OpenToView.Seconds;
			}
			else if (TimeEditMode == TimeEditMode.OnlyMinutes)
			{
				await SubmitAndCloseAsync();
			}
		}
		else if (currentView == OpenToView.Hours)
		{
			if (TimeEditMode == TimeEditMode.Normal)
			{
				currentView = OpenToView.Minutes;
			}
			else if (TimeEditMode == TimeEditMode.OnlyHours)
			{
				await SubmitAndCloseAsync();
			}
		}

		// Manually update because the event won't do it from JavaScript.
		StateHasChanged();
	}

	private int RoundToStepInterval(int value)
	{
		if (MinuteSelectionStep > 1) // Ignore if step is less than or equal to 1
		{
			var interval = MinuteSelectionStep % 60;
			value = (value + interval / 2) / interval * interval;
			if (value == 60) // For when it rounds up to 60
			{
				value = 0;
			}
		}
		return value;
	}

	protected async Task SubmitAndCloseAsync()
	{
		if (PickerActions == null || AutoClose)
		{
			await SubmitAsync();

			if (PickerVariant != PickerVariant.Static)
			{
				await Task.Delay(ClosingDelay);
				await CloseAsync(false);
			}
		}
	}

	protected override async Task OnHandleKeyDownAsync(KeyboardEventArgs obj)
	{
		if (GetDisabledState() || GetReadOnlyState())
		{
			return;
		}

		await base.OnHandleKeyDownAsync(obj);

		switch (obj.Key)
		{
			case "ArrowRight":
				if (Open)
				{
					if (obj.CtrlKey == true)
					{
						await ChangeHourAsync(1);
					}
					else if (obj.ShiftKey == true)
					{
						if (timeSet.Minute > 55)
						{
							await ChangeHourAsync(1);
						}
						await ChangeMinuteAsync(5);
					}
					else
					{
						if (timeSet.Minute == 59)
						{
							await ChangeHourAsync(1);
						}
						await ChangeMinuteAsync(1);
					}
				}
				break;
			case "ArrowLeft":
				if (Open)
				{
					if (obj.CtrlKey == true)
					{
						await ChangeHourAsync(-1);
					}
					else if (obj.ShiftKey == true)
					{
						if (timeSet.Minute < 5)
						{
							await ChangeHourAsync(-1);
						}
						await ChangeMinuteAsync(-5);
					}
					else
					{
						if (timeSet.Minute == 0)
						{
							await ChangeHourAsync(-1);
						}
						await ChangeMinuteAsync(-1);
					}
				}
				break;
			case "ArrowUp":
				if (Open == false && Editable == false)
				{
					Open = true;
				}
				else if (obj.AltKey == true)
				{
					Open = false;
				}
				else if (obj.ShiftKey == true)
				{
					await ChangeHourAsync(5);
				}
				else
				{
					await ChangeHourAsync(1);
				}
				break;
			case "ArrowDown":
				if (Open == false && Editable == false)
				{
					Open = true;
				}
				else if (obj.ShiftKey == true)
				{
					await ChangeHourAsync(-5);
				}
				else
				{
					await ChangeHourAsync(-1);
				}
				break;
			case "Escape":
				await ReturnTimeBackUpAsync();
				break;
			case "Enter":
			case "NumpadEnter":
				if (!Open)
				{
					await OpenAsync();
				}
				else
				{
					await SubmitAsync();
					await CloseAsync();
					_inputReference?.SetText(Text);
				}
				break;
			case " ":
				if (!Editable)
				{
					if (!Open)
					{
						await OpenAsync();
					}
					else
					{
						await SubmitAsync();
						await CloseAsync();
						_inputReference?.SetText(Text);
					}
				}
				break;
		}

		StateHasChanged();
	}

	protected Task ChangeMinuteAsync(int val)
	{
		currentView = OpenToView.Minutes;
		timeSet.Minute = (timeSet.Minute + val + 60) % 60;
		return UpdateTimeAsync();
	}

	protected Task ChangeHourAsync(int val)
	{
		currentView = OpenToView.Hours;
		timeSet.Hour = (timeSet.Hour + val + 24) % 24;
		return UpdateTimeAsync();
	}

	protected async Task ReturnTimeBackUpAsync()
	{
		if (Time == null)
		{
			TimeIntermediate = null;
		}
		else
		{
			timeSet.Hour = Time.Value.Hours;
			timeSet.Minute = Time.Value.Minutes;
			timeSet.Second = Time.Value.Seconds;

			await UpdateTimeAsync();
		}
	}

	private class SetTime
	{
		public int Hour { get; set; }

		public int Minute { get; set; }

		public int Second { get; set; }
	}
}