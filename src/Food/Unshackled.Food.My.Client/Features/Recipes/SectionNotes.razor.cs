using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Extensions;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class SectionNotesBase : BaseSectionComponent
{
	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public List<RecipeNoteModel> Notes { get; set; } = new();
	[Parameter] public EventCallback UpdateComplete { get; set; }

	protected List<FormNoteModel> FormNotes { get; set; } = new();
	protected List<FormNoteModel> DeletedNotes { get; set; } = new();
	protected FormNoteModel AddFormModel { get; set; } = new();

	protected bool IsWorking { get; set; } = false;
	protected bool IsAdding { get; set; } = false;
	protected bool IsEditing { get; set; } = false;
	protected bool IsSorting { get; set; } = false; 
	protected bool DisableControls => IsWorking;
	protected bool IsEditingItem => FormNotes.Where(x => x.IsEditing == true).Any();

	protected void HandleAddClicked()
	{
		IsAdding = true;
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
		AddFormModel = new();
		IsAdding = false;
		IsWorking = false;
	}

	protected void HandleCancelAddClicked()
	{
		IsAdding = false;
	}

	protected async Task HandleCancelEditClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected void HandleCancelEditItemClicked(FormNoteModel item)
	{
		item.IsEditing = false;
	}

	protected void HandleDeleteClicked(FormNoteModel item)
	{
		FormNotes.Remove(item);
		DeletedNotes.Add(item);

		// Adjust sort order for remaining sets
		for (int i = 0; i < FormNotes.Count; i++)
		{
			FormNotes[i].SortOrder = i;
		}
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
		item.IsEditing = false;
		IsWorking = false;
	}

	protected void HandleEditItemClicked(FormNoteModel item)
	{
		item.IsEditing = true;
		IsAdding = false;
	}

	protected void HandleIsSorting(bool isSorting)
	{
		IsAdding = false;
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
				IsEditing = false,
				IsNew = false,
				Note = x.Note,
				RecipeSid = x.RecipeSid,
				SortOrder = x.SortOrder
			})
			.ToList();

		if (FormNotes.Count == 0)
		{
			IsAdding = true;
		}
	}
}