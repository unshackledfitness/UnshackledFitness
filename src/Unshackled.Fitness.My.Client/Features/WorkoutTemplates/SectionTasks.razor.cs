using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Actions;
using Unshackled.Fitness.My.Client.Features.WorkoutTemplates.Models;

namespace Unshackled.Fitness.My.Client.Features.WorkoutTemplates;

public class SectionTasksBase : BaseSectionComponent
{
	[Parameter] public string TemplateSid { get; set; } = string.Empty;
	[Parameter] public WorkoutTaskTypes TaskType { get; set; } = WorkoutTaskTypes.PreWorkout;
	protected bool IsLoading { get; set; } = true;
	protected bool IsWorking { get; set; }
	protected bool DisableControls => IsLoading || IsWorking;
	protected List<TemplateTaskModel> Tasks { get; set; } = new();
	protected List<FormTemplateTaskModel> EditingTasks { get; set; } = new();
	protected List<FormTemplateTaskModel> DeletedTasks { get; set; } = new();
	protected List<RecentTemplateTaskModel> RecentTasks { get; set; } = new();
	protected FormTemplateTaskModel Model { get; set; } = new();
	protected FormTemplateTaskModel.Validator ModelValidator { get; set; } = new();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Task";

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		RecentTasks = await Mediator.Send(new ListRecentTasks.Query(TaskType));
		await RefreshTasks();
	}

	protected void HandleAddTaskClicked()
	{
		Model = new();
		DrawerOpen = true;
	}

	protected void HandleAddRecentClicked(RecentTemplateTaskModel task)
	{
		var model = new FormTemplateTaskModel
		{
			SortOrder = EditingTasks.Count,
			Text = task.Text,
			Type = TaskType
		};
		EditingTasks.Add(model);
		DrawerOpen = false;
	}

	protected void HandleCancelAddClicked()
	{
		DrawerOpen = false;
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected void HandleDeleteClicked(FormTemplateTaskModel task)
	{
		EditingTasks.Remove(task);

		// Track tasks that need to be deleted from database
		if (!string.IsNullOrEmpty(task.Sid))
		{
			DeletedTasks.Add(task);
		}

		// Adjust sort order for remaining sets
		for (int i = 0; i < EditingTasks.Count; i++)
		{
			EditingTasks[i].SortOrder = i;
		}
	}

	protected async Task HandleEditClicked()
	{
		EditingTasks.Clear();
		DeletedTasks.Clear();
		EditingTasks.AddRange(Tasks
			.Select(x => new FormTemplateTaskModel
			{
				DateCreatedUtc = x.DateCreatedUtc,
				DateLastModifiedUtc = x.DateLastModifiedUtc,
				SortOrder = x.SortOrder,
				Text = x.Text,
				Type = x.Type,
				Sid = x.Sid
			})
			.ToList());
		Model = new();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleFormSubmitted()
	{
		Model.SortOrder = EditingTasks.Count;
		Model.Type = TaskType;
		EditingTasks.Add(Model);
		DrawerOpen = false;
	}

	protected void HandleSortChanged(List<FormTemplateTaskModel> list)
	{
		EditingTasks = list;
	}

	protected async Task HandleUpdateClicked()
	{
		IsWorking = true;

		if (DeletedTasks.Any() || EditingTasks.Any())
		{
			UpdateTemplateTasksModel model = new()
			{
				DeletedTasks = DeletedTasks,
				Tasks = EditingTasks
			};
			var result = await Mediator.Send(new UpdateTemplateTasks.Command(TemplateSid, model));
			ShowNotification(result);

			if (result.Success)
				await RefreshTasks();
		}

		IsWorking = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	private async Task RefreshTasks()
	{
		IsLoading = true;
		Tasks = await Mediator.Send(new ListTasks.Query(TemplateSid, TaskType));
		IsLoading = false;
	}
}