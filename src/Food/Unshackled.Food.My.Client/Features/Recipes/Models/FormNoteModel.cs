using FluentValidation;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class FormNoteModel : ISortable, ICloneable
{
	public string Sid { get; set; } = Guid.NewGuid().ToString();
	public string RecipeSid { get; set; } = string.Empty;
	public string Note { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsNew { get; set; }

	public object Clone()
	{
		return new FormNoteModel
		{
			IsNew = IsNew,
			Note = Note,
			RecipeSid = RecipeSid,
			Sid = Sid,
			SortOrder = SortOrder
		};
	}

	public class Validator : BaseValidator<FormNoteModel>
	{
		public Validator()
		{
			RuleFor(x => x.Note)
				.NotEmpty().WithMessage("A note cannot be empty.");
		}
	}
}
