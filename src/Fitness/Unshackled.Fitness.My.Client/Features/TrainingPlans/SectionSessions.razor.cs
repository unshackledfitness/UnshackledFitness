using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class SectionSessionsBase : BaseSectionComponent
{
	[Parameter] public PlanModel Plan { get; set; } = new();
	[Parameter] public EventCallback PlanUpdated { get; set; }

	protected bool DisableControls => IsSaving;
	protected FormUpdateSessionsModel FormModel { get; set; } = new();
	protected bool IsEditing { get; set; } = false;
	protected bool IsSaving { get; set; } = false;
	protected List<SessionListModel> Sessions { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Sessions = await Mediator.Send(new ListSessions.Query());
	}

	protected async Task HandleEditClicked()
	{
		FormModel = new()
		{
			LengthWeeks = Plan.LengthWeeks,
			TrainingPlanSid = Plan.Sid,
			Sessions = Plan.Sessions
				.Select(x => new FormSessionModel
				{
					DateCreatedUtc = x.DateCreatedUtc,
					DateLastModifiedUtc = x.DateLastModifiedUtc,
					DayNumber = x.DayNumber,
					IsNew = false,
					MemberSid = x.MemberSid,
					Sid = x.Sid,
					SortOrder = x.SortOrder,
					TrainingPlanSid = Plan.Sid,
					TrainingSessionName = x.TrainingSessionName,
					TrainingSessionSid = x.TrainingSessionSid,
					WeekNumber = x.WeekNumber
				})
				.ToList()
		};
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleSaveClicked()
	{
		IsSaving = true;
		var result = await Mediator.Send(new SaveSessions.Command(FormModel));
		if (result.Success)
		{
			await PlanUpdated.InvokeAsync();
		}
		ShowNotification(result);
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}
}