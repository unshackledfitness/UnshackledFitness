using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Models;

public class FormStepModel : ISortable, ICloneable
{
	public string Sid { get; set; } = string.Empty;
	public string RecipeSid { get; set; } = string.Empty;
	public string Instructions { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsNew { get; set; }

	public List<FormStepIngredientModel> Ingredients { get; set; } = new();

	[JsonIgnore]
	public string IngredientList => String.Join(", ", Ingredients.Select(x => x.Title).ToArray());

	public object Clone()
	{
		return new FormStepModel
		{
			Ingredients = Ingredients,
			Instructions = Instructions,
			IsNew = IsNew,
			RecipeSid = RecipeSid,
			Sid = Sid,
			SortOrder = SortOrder
		};
	}

	public class Validator : BaseValidator<FormStepModel>
	{
		public Validator()
		{
			RuleFor(x => x.Instructions)
				.NotEmpty().WithMessage("Instructions are required.");
		}
	}
}
