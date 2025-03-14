using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans;

public class DrawerCopyBase : BaseComponent
{
	[Parameter] public List<MealPrepPlanRecipeModel> Recipes { get; set; } = [];
	[Parameter] public EventCallback<DateOnly> OnSubmitClicked { get; set; }
	[Parameter] public EventCallback OnCancelClicked { get; set; }

	protected bool IsWorking { get; set; } = false;
	protected bool IsSingleDate { get; set; }
	protected bool DisableControls => Recipes.Count == 0 || !SelectedDate.HasValue || IsWorking;
	protected DateTime? SelectedDate { get; set; } = DateTime.Today;
	protected string Title => IsSingleDate ? "Copy To Date" : "Copy To Week";

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (Recipes.Count > 0)
		{
			DateOnly firstDate = Recipes.First().DatePlanned;
			IsSingleDate = !Recipes.Where(x => x.DatePlanned != firstDate).Any();
			if (!IsSingleDate && SelectedDate.HasValue)
			{
				// Move to next Sunday
				SelectedDate = SelectedDate.Value.AddDays(7 - (int)SelectedDate.Value.DayOfWeek);
			}
		}
	}

	protected async Task HandleCancelClicked()
	{
		await OnCancelClicked.InvokeAsync();
	}

	protected async Task HandleSubmitClicked()
	{
		if (SelectedDate.HasValue)
		{
			IsWorking = true;
			DateOnly dateSelected = DateOnly.FromDateTime(SelectedDate.Value);
			await OnSubmitClicked.InvokeAsync(dateSelected);
			IsWorking = false;
		}
	}

	protected bool IsDateDisabled(DateTime date)
	{
		if (date < DateTime.Today)
			return true;
		else if (!IsSingleDate)
			return (int)date.DayOfWeek > 0;
		else
			return false;
	}
}