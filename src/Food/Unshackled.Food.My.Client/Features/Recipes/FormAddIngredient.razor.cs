using MediatR;
using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class FormAddIngredientBase : BaseFormComponent<FormAddIngredientModel, FormAddIngredientModel.Validator>
{
	[Inject] protected IMediator Mediator { get; set; } = default!;

	protected async Task<IEnumerable<string>> SearchIngredients(string value, CancellationToken token)
	{
		var results = await Mediator.Send(new SearchIngredients.Query(value))
			?? new List<string>();

		return results;
	}
}