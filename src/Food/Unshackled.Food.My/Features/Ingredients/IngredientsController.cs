using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Food.My.Features.Ingredients.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.Ingredients;

[ApiController]
[ApiRoute("ingredients")]
public class IngredientsController : BaseController
{
	[HttpPost("add-substitution")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddSubstitution([FromBody] FormSubstitutionModel model)
	{
		return Ok(await Mediator.Send(new AddSubstitution.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("delete-substitution")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormSubstitutionModel model)
	{
		return Ok(await Mediator.Send(new DeleteSubstitution.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpGet("get/{key}")]
	public async Task<IActionResult> Get(string key)
	{
		return Ok(await Mediator.Send(new GetIngredient.Query(Member.Id, Member.ActiveHouseholdId, key)));
	}

	[HttpPost("make-primary")]
	[ActiveMemberRequired]
	public async Task<IActionResult> MakePrimary([FromBody] FormSubstitutionModel model)
	{
		return Ok(await Mediator.Send(new MakePrimary.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchIngredientModel model)
	{
		return Ok(await Mediator.Send(new SearchIngredients.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("search-products")]
	public async Task<IActionResult> SearchProducts([FromBody] SearchProductModel model)
	{
		return Ok(await Mediator.Send(new SearchProducts.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormIngredientModel model)
	{
		return Ok(await Mediator.Send(new UpdateIngredient.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}
}
