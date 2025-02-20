﻿using FluentValidation;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Interfaces;
using Unshackled.Fitness.My.Client.Utils;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;

public class FormIngredientModel : IGroupedSortable, ICloneable
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

	public object Clone()
	{
		return new FormIngredientModel()
		{
			Amount = Amount,
			AmountText = AmountText,
			AmountUnit = AmountUnit,
			AmountLabel = AmountLabel,
			ListGroupSid = ListGroupSid,
			Key = Key,
			PrepNote = PrepNote,
			RecipeSid = RecipeSid,
			Sid = Sid,
			SortOrder = SortOrder,
			Title = Title
		};
	}

	public class Validator : BaseValidator<FormIngredientModel>
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
