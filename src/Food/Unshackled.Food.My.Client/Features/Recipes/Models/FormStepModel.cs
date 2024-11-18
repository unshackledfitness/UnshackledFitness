using System.Text.Json.Serialization;
using FluentValidation;
using Unshackled.Studio.Core.Client.Features;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Models;

public class FormStepModel : ISortable, ICloneable
{
	public string Sid { get; set; } = Guid.NewGuid().ToString();
	public string RecipeSid { get; set; } = string.Empty;
	public string Instructions { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsNew { get; set; }
	public bool IsDeleted { get; set; }

	[JsonIgnore]
	public bool IsEditing { get; set; }

	[JsonIgnore]
	public string IngredientList => String.Join(", ", Ingredients.Select(x => x.Title).ToArray());

	public List<FormStepIngredientModel> Ingredients { get; set; } = new();

	public object Clone()
	{
		return new FormStepModel
		{
			Ingredients = Ingredients,
			Instructions = Instructions,
			IsDeleted = IsDeleted,
			IsEditing = IsEditing,
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
