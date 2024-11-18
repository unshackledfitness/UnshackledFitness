using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Utils;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class FormEditIngredientModel : IGroupedSortable, ICloneable
{
	public string Sid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string ListGroupSid { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public decimal Amount { get; set; }
	public string AmountText { get; set; } = string.Empty;
	public MeasurementUnits AmountUnit { get; set; } = MeasurementUnits.mg;
	public string AmountLabel { get; set; } = string.Empty;
	public string? PrepNote { get; set; }
	public bool IsNew { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }

	public object Clone()
	{
		return new FormEditIngredientModel()
		{
			Amount = Amount,
			AmountText = AmountText,
			AmountUnit = AmountUnit,
			AmountLabel = AmountLabel,
			ListGroupSid = ListGroupSid,
			IsEditing = IsEditing,
			IsNew = IsNew,
			Key = Key,
			PrepNote = PrepNote,
			RecipeSid = RecipeSid,
			Sid = Sid,
			SortOrder = SortOrder,
			Title = Title
		};
	}

	public class Validator : BaseValidator<FormEditIngredientModel>
	{
		public Validator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("An ingredient name is required.");
			RuleFor(x => x.AmountText)
				.NotEmpty().WithMessage("An amount is required.");
			RuleFor(x => x.AmountText).Custom((input, context) => {
				var result = IngredientUtils.ParseNumber(input);
				if (result.Amount == 0)
				{
					context.AddFailure("Not a valid number.");
				}
			});
		}
	}
}
