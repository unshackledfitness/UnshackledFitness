using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Programs.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Fitness.My.Client.Features.Programs;

public class ViewSequentialBase : BaseSectionComponent<Member>
{
	[Parameter] public FormUpdateTemplatesModel FormModel { get; set; } = new();
	[Parameter] public EventCallback<FormUpdateTemplatesModel> FormModelChanged { get; set; }
	[Parameter] public bool IsEditing { get; set; } = false;
	[Parameter] public bool IsSaving { get; set; } = false;
	[Parameter] public ProgramModel Program { get; set; } = new();
	[Parameter] public List<TemplateListModel> Templates { get; set; } = new();
	protected string DrawerIcon => Icons.Material.Filled.AddCircle;
	protected bool DrawerOpen { get; set; } = false;
	protected string DrawerTitle => "Add Template";

	protected void HandleAddClicked()
	{
		DrawerOpen = true;
	}

	protected async Task HandleAddTemplateClicked(TemplateListModel model)
	{
		int sortOrder = FormModel.Templates.Any()
			? FormModel.Templates.Max(x => x.SortOrder) + 1
			: 0;

		FormModel.Templates.Add(new()
		{
			DayNumber = 0,
			IsNew = true,
			MemberSid = Program.MemberSid,
			ProgramSid = Program.Sid,
			Sid = Guid.NewGuid().ToString(),
			SortOrder = sortOrder,
			WeekNumber = 0,
			WorkoutTemplateSid = model.Sid,
			WorkoutTemplateName = model.Title
		});

		FormModel.Templates = ReorderTemplates();
		await FormModelChanged.InvokeAsync(FormModel);
		DrawerOpen = false;
	}

	protected async Task HandleDeleteTemplateClicked(string sid)
	{
		var model = FormModel.Templates
			.Where(x => x.Sid == sid)
			.SingleOrDefault();

		if (model != null)
		{
			if (!model.IsNew)
			{
				FormModel.DeletedTemplates.Add(model);
			}
			FormModel.Templates.Remove(model);
			FormModel.Templates = ReorderTemplates();
			await FormModelChanged.InvokeAsync(FormModel);
		}
	}

	protected async Task HandleSortChanged(List<FormProgramTemplateModel> list)
	{
		FormModel.Templates = list;
		await FormModelChanged.InvokeAsync(FormModel);
	}

	private List<FormProgramTemplateModel> ReorderTemplates()
	{
		var list = FormModel.Templates
			.OrderBy(x => x.SortOrder)
			.ToList();

		for (int i = 0; i < list.Count; i++)
		{
			list[i].SortOrder = i;
		}

		return list;
	}
}