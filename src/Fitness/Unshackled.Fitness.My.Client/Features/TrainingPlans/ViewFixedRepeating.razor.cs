using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans;

public class ViewFixedRepeatingBase : BaseSectionComponent
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public FormUpdateSessionsModel FormModel { get; set; } = new();
	[Parameter] public EventCallback<FormUpdateSessionsModel> FormModelChanged { get; set; }
	[Parameter] public bool IsEditing { get; set; } = false;
	[Parameter] public bool IsSaving { get; set; } = false;
	[Parameter] public PlanModel Plan { get; set; } = new();
	[Parameter] public List<SessionListModel> Sessions { get; set; } = new();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Session";

	private int addToWeek = 0;
	private int addToDay = 0;

	protected List<PlanSessionModel> GetDaySessions(int week, int day)
	{
		return Plan.Sessions
			.Where(x => x.WeekNumber == week && x.DayNumber == day)
			.OrderBy(x => x.SortOrder)
			.ToList();
	}

	protected List<FormSessionModel> GetFormDaySessions(int week, int day)
	{
		return FormModel.Sessions
			.Where(x => x.WeekNumber == week && x.DayNumber == day)
			.OrderBy(x => x.SortOrder)
			.ToList();
	}

	protected void HandleAddClicked(int week, int day)
	{
		addToWeek = week;
		addToDay = day;
		DrawerOpen = true;
	}

	protected async Task HandleAddSessionClicked(SessionListModel model)
	{
		int sortOrder = FormModel.Sessions
			.Where(x => x.WeekNumber == addToWeek && x.DayNumber == addToDay)
			.OrderBy(x => x.SortOrder)
			.Select(x => x.SortOrder + 1)
			.LastOrDefault();

		FormModel.Sessions.Add(new()
		{
			DayNumber = addToDay,
			IsNew = true,
			MemberSid = Plan.MemberSid,
			Sid = Guid.NewGuid().ToString(),
			SortOrder = sortOrder,
			TrainingPlanSid = Plan.Sid,
			TrainingSessionSid = model.Sid,
			TrainingSessionName = model.Title,
			WeekNumber = addToWeek
		});

		FormModel.Sessions = ReorderSessions();
		await FormModelChanged.InvokeAsync(FormModel);
		DrawerOpen = false;
	}

	protected async Task HandleAddWeekClicked()
	{
		FormModel.LengthWeeks++;
		await FormModelChanged.InvokeAsync(FormModel);
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

	protected async Task HandleDeleteWeekClicked(int week)
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Warning",
				"Are you sure you want to delete this week?",
				yesText: "Delete", cancelText: "Cancel");

		if (confirm.HasValue && confirm.Value)
		{
			var list = FormModel.Sessions.ToArray();
			foreach (var model in list)
			{
				if (model.WeekNumber == week)
				{
					if (!model.IsNew)
					{
						FormModel.DeletedSessions.Add(model);
					}
					FormModel.Sessions.Remove(model);
				}
				else if (model.WeekNumber > week)
				{
					model.WeekNumber--;
				}
			}
			list = null;
			FormModel.LengthWeeks--;
			await FormModelChanged.InvokeAsync(FormModel);
		}
	}

	private List<FormSessionModel> ReorderSessions()
	{
		var list = FormModel.Sessions
			.OrderBy(x => x.WeekNumber)
				.ThenBy(x => x.DayNumber)
				.ThenBy(x => x.SortOrder)
			.ToList();

		for (int i = 0; i < list.Count; i++)
		{
			list[i].SortOrder = i;
		}

		return list;
	}
}