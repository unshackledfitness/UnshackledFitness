namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class UpdateNotesModel
{
	public List<FormNoteModel> Notes { get; set; } = new();
	public List<FormNoteModel> DeletedNotes { get; set; } = new();
}
