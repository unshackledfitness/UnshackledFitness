using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class SectionNotesBase : BaseSectionComponent<Member>
{
	protected enum Views
	{
		None,
		AddNote,
		EditNote
	}

	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public List<RecipeNoteModel> Notes { get; set; } = new();
	[Parameter] public EventCallback UpdateComplete { get; set; }

	protected List<FormNoteModel> FormNotes { get; set; } = new();
	protected List<FormNoteModel> DeletedNotes { get; set; } = new();
	protected FormNoteModel CurrentFormModel { get; set; } = new();
	protected FormNoteModel FormModel { get; set; } = new();

	protected bool IsWorking { get; set; } = false;
	protected bool IsSorting { get; set; } = false; 
	protected bool DisableControls => IsWorking;
	protected bool DrawerOpen => DrawerView != Views.None;
	protected Views DrawerView { get; set; } = Views.None;
	protected string DrawerTitle => DrawerView switch
	{
		Views.AddNote => "Add Note",
		Views.EditNote => "Edit Note",
		_ => string.Empty
	};

	protected void HandleAddClicked()
	{
		FormModel = new()
		{
			RecipeSid = RecipeSid,
		};
		DrawerView = Views.AddNote;
	}

	protected void HandleAddFormSubmitted(FormNoteModel model)
	{
		IsWorking = true;
		FormNotes.Add(new()
		{
			Sid = model.Sid,
			IsNew = true,
			Note = model.Note,
			RecipeSid = RecipeSid,
			SortOrder = FormNotes.Count()
		});
		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleCancelClicked()
	{
		DrawerView = Views.None;
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected void HandleDeleteClicked()
	{
		FormNotes.Remove(CurrentFormModel);

		if (!CurrentFormModel.IsNew)
		{
			DeletedNotes.Add(CurrentFormModel);
		}

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormNotes.Count; i++)
		{
			FormNotes[i].SortOrder = i;
		}

		DrawerView = Views.None;
	}

	protected async Task HandleEditClicked()
	{
		LoadFormNotes();
		IsEditing = await UpdateIsEditingSection(true);
	}

	protected void HandleEditFormSubmitted(FormNoteModel model)
	{
		IsWorking = true;
		var item = FormNotes.Where(x => x.Sid == model.Sid).Single();
		item.Note = model.Note;
		IsWorking = false;
		DrawerView = Views.None;
	}

	protected void HandleEditItemClicked(FormNoteModel item)
	{
		CurrentFormModel = item;
		FormModel = (FormNoteModel)item.Clone();
		DrawerView = Views.EditNote;
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsSorting = isSorting;
	}

	protected void HandleItemDropped(MudItemDropInfo<FormNoteModel> droppedItem)
	{
		if (droppedItem.Item != null)
		{
			int newIdx = droppedItem.IndexInZone;
			int oldIdx = FormNotes.IndexOf(droppedItem.Item);
			FormNotes.MoveAndSort(oldIdx, newIdx);
		}
	}

	protected void HandleSortChanged(List<FormNoteModel> notes)
	{
		FormNotes = notes;
	}

	protected async Task HandleUpdateClicked()
	{
		IsWorking = true;
		UpdateNotesModel model = new()
		{
			DeletedNotes = DeletedNotes,
			Notes = FormNotes
		};
		var result = await Mediator.Send(new UpdateNotes.Command(RecipeSid, model));
		ShowNotification(result);

		if (result.Success)
			await UpdateComplete.InvokeAsync();
		IsSorting = false;
		IsEditing = await UpdateIsEditingSection(false);
		IsWorking = false;
	}

	protected void LoadFormNotes()
	{
		DeletedNotes.Clear();
		FormNotes = Notes
			.Select(x => new FormNoteModel
			{
				Sid = x.Sid,
				IsNew = false,
				Note = x.Note,
				RecipeSid = x.RecipeSid,
				SortOrder = x.SortOrder
			})
			.ToList();
	}
}