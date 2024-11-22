using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Food.My.Features.CookbookRecipes.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.CookbookRecipes;

[ApiController]
[ApiRoute("cookbook-recipes")]
public class CookbookRecipesController : BaseController
{
	[HttpPost("copy")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Copy([FromBody] FormCopyRecipeModel model)
	{
		return Ok(await Mediator.Send(new CopyRecipe.Command(Member.Id, model)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetRecipe(long id)
	{
		return Ok(await Mediator.Send(new GetRecipe.Query(Member.Id, Member.ActiveCookbookId, id)));
	}

	[HttpGet("list-member-households")]
	public async Task<IActionResult> ListMemberHouseholds()
	{
		return Ok(await Mediator.Send(new ListMemberHouseholds.Query(Member.Id)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchRecipeModel model)
	{
		return Ok(await Mediator.Send(new SearchRecipes.Query(Member.ActiveCookbookId, Member.Id, model)));
	}
}
