using Unshackled.Studio.Core.Client.Features;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Models;
public class FormBulkAddIngredientModel
{
	public string RecipeSid { get; set; } = string.Empty;
	public string BulkText {  get; set; } = string.Empty;

	public class Validator : BaseValidator<FormBulkAddIngredientModel>
	{
		public Validator()
		{
			
		}
	}
}
