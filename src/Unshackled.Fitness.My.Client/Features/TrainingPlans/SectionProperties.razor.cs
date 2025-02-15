using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class SectionPropertiesBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public PlanModel Plan { get; set; } = new();
	[Parameter] public EventCallback<PlanModel> PlanChanged { get; set; }

	protected const string FormId = "formPlan";
	protected bool IsDuplicating { get; set; } = false;
	protected bool IsSaving { get; set; } = false;
	protected FormUpdatePlanModel Model { get; set; } = new();

	protected bool DisableControls => IsSaving;

	protected string GetSectionTitle()
	{
		if (!IsEditing && !IsDuplicating)
			return Plan.Title;
		else if (IsEditing)
			return "Edit Plan";
		else
			return "Duplicate Plan";
	}

	protected string GetSectionSubTitle()
	{
		if (!IsEditing && !IsDuplicating && Plan.DateStarted.HasValue)
			return $"Started: {Plan.DateStarted.Value.ToString("D")}";
		else
			return string.Empty;
	}

	protected async Task HandleCancelClicked()
	{
		IsEditing = IsDuplicating = await UpdateIsEditingSection(false);
	}

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to delete this plan? This can not be undone!",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeletePlan.Command(Plan.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/training-plans");
			}
		}
	}

	protected async Task HandleDuplicateClicked()
	{
		Model = new()
		{
			Sid = Plan.Sid,
			Title = Plan.Title,
			Description = Plan.Description
		};

		IsDuplicating = await UpdateIsEditingSection(true);
	}

	protected async Task HandleDuplicateFormSubmitted(FormUpdatePlanModel model)
	{
		var result = await Mediator.Send(new DuplicatePlan.Command(model));
		ShowNotification(result);
		IsDuplicating = await UpdateIsEditingSection(false);
		if (result.Success)
		{
			NavigateOnSuccess($"/training-plans/{result.Payload}");
		}
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = Plan.Title,
			Description = Plan.Description,
			Sid = Plan.Sid
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleEditFormSubmitted(FormUpdatePlanModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await PlanChanged.InvokeAsync(result.Payload);
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleStartPlanClicked()
	{
		var options = new DialogOptions { BackgroundClass = "bg-blur", MaxWidth = MaxWidth.Medium };

		var parameters = new DialogParameters
		{
			{ nameof(DialogAddToCalendar.ProgramType), Plan.ProgramType }
		};

		var dialog = await DialogService.ShowAsync<DialogAddToCalendar>("Add To Calendar", parameters, options);
		var adding = await dialog.Result;
		if (adding != null && !adding.Canceled && adding.Data != null)
		{
			IsSaving = true;
			FormStartPlanModel model = new()
			{
				DateStart = (DateTime)adding.Data,
				Sid = Plan.Sid,
				StartingSessionIndex = Plan.StartingTemplate()?.SortOrder ?? 0
			};
			var result = await Mediator.Send(new StartPlan.Command(model));
			if (result.Success)
			{
				Plan.DateStarted = model.DateStart;
				Plan.NextSessionIndex = model.StartingSessionIndex;
				await PlanChanged.InvokeAsync(Plan);
			}
			ShowNotification(result);
			IsSaving = false;
		}
	}

	protected async Task HandleStopPlanClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to remove this plan from your calendar?",
				yesText: "Remove", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			IsSaving = true;
			var result = await Mediator.Send(new StopPlan.Command(Plan.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				Plan.DateStarted = null;
				await PlanChanged.InvokeAsync(Plan);
			}
			IsSaving = false;
		}
	}
}