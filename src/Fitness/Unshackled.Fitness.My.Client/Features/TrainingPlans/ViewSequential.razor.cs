using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class ViewSequentialBase : BaseSectionComponent<Member>
{
	[Parameter] public FormUpdateSessionsModel FormModel { get; set; } = new();
	[Parameter] public EventCallback<FormUpdateSessionsModel> FormModelChanged { get; set; }
	[Parameter] public bool IsSaving { get; set; } = false;
	[Parameter] public PlanModel Plan { get; set; } = new();
	[Parameter] public List<SessionListModel> Sessions { get; set; } = new();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Session";

	protected void HandleAddClicked()
	{
		DrawerOpen = true;
	}

	protected async Task HandleAddSessionClicked(SessionListModel model)
	{
		int sortOrder = FormModel.Sessions.Any()
			? FormModel.Sessions.Max(x => x.SortOrder) + 1
			: 0;

		FormModel.Sessions.Add(new()
		{
			DayNumber = 0,
			IsNew = true,
			MemberSid = Plan.MemberSid,
			Sid = Guid.NewGuid().ToString(),
			SortOrder = sortOrder,
			TrainingPlanSid = Plan.Sid,
			TrainingSessionSid = model.Sid,
			TrainingSessionName = model.Title,
			WeekNumber = 0
		});

		FormModel.Sessions = ReorderSessions();
		await FormModelChanged.InvokeAsync(FormModel);
		DrawerOpen = false;
	}

	protected async Task HandleDeleteSessionClicked(string sid)
	{
		var model = FormModel.Sessions
			.Where(x => x.Sid == sid)
			.SingleOrDefault();

		if (model != null)
		{
			if (!model.IsNew)
			{
				FormModel.DeletedSessions.Add(model);
			}
			FormModel.Sessions.Remove(model);
			FormModel.Sessions = ReorderSessions();
			await FormModelChanged.InvokeAsync(FormModel);
		}
	}

	protected async Task HandleSortChanged(List<FormSessionModel> list)
	{
		FormModel.Sessions = list;
		await FormModelChanged.InvokeAsync(FormModel);
	}

	private List<FormSessionModel> ReorderSessions()
	{
		var list = FormModel.Sessions
			.OrderBy(x => x.SortOrder)
			.ToList();

		for (int i = 0; i < list.Count; i++)
		{
			list[i].SortOrder = i;
		}

		return list;
	}
}