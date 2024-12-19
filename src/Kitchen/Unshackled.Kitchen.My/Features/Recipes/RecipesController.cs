using Microsoft.AspNetCore.Mvc;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.Core.Models.ShoppingLists;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Kitchen.My.Features.Recipes.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Kitchen.My.Features.Recipes;

[ApiController]
[ApiRoute("recipes")]
public class RecipesController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormRecipeModel model)
	{
		return Ok(await Mediator.Send(new AddRecipe.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-to-cookbook")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToCookbook([FromBody] AddToCookbookModel model)
	{
		return Ok(await Mediator.Send(new AddToCookbook.Command(Member.Id, model)));
	}

	[HttpPost("add-to-shopping-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToShoppingList([FromBody] AddRecipeToListModel model)
	{
		return Ok(await Mediator.Send(new AddToList.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("copy")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Copy([FromBody] FormCopyRecipeModel model)
	{
		return Ok(await Mediator.Send(new CopyRecipe.Command(Member.Id, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteRecipe.Command(Member.Id, sid)));
	}

	[HttpPost("delete-image")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteImage([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteImage.Command(Member.Id, sid)));
	}

	[HttpPost("get-add-to-list-items")]
	[ActiveMemberRequired]
	public async Task<IActionResult> GetAddToListItems([FromBody] SelectListModel model)
	{
		return Ok(await Mediator.Send(new GetAddToListItems.Query(Member.Id, model)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetRecipe(long id)
	{
		return Ok(await Mediator.Send(new GetRecipe.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/groups")]
	[DecodeId]
	public async Task<IActionResult> GetRecipeIngredientGroups(long id)
	{
		return Ok(await Mediator.Send(new ListRecipeIngredientGroups.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/images")]
	[DecodeId]
	public async Task<IActionResult> GetRecipeImages(long id)
	{
		return Ok(await Mediator.Send(new ListRecipeImages.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/ingredients")]
	[DecodeId]
	public async Task<IActionResult> GetRecipeIngredients(long id)
	{
		return Ok(await Mediator.Send(new ListRecipeIngredients.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/notes")]
	[DecodeId]
	public async Task<IActionResult> GetRecipeNotes(long id)
	{
		return Ok(await Mediator.Send(new ListRecipeNotes.Query(Member.Id, id)));
	}

	[HttpGet("get/{sid}/steps")]
	[DecodeId]
	public async Task<IActionResult> GetRecipeSteps(long id)
	{
		return Ok(await Mediator.Send(new ListRecipeSteps.Query(Member.Id, id)));
	}

	[HttpGet("list-ingredient-titles")]
	public async Task<IActionResult> ListIngredientTitles()
	{
		return Ok(await Mediator.Send(new ListIngredientTitles.Query(Member.ActiveHouseholdId, Member.Id)));
	}

	[HttpGet("list-member-cookbooks")]
	public async Task<IActionResult> ListMemberCookbooks()
	{
		return Ok(await Mediator.Send(new ListMemberCookbooks.Query(Member.Id)));
	}

	[HttpGet("list-member-households")]
	public async Task<IActionResult> ListMemberHouseholds()
	{
		return Ok(await Mediator.Send(new ListMemberHouseholds.Query(Member.Id)));
	}

	[HttpGet("list-recipe-tags")]
	public async Task<IActionResult> ListRecipeTags()
	{
		return Ok(await Mediator.Send(new ListRecipeTags.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpGet("list-shopping-lists")]
	public async Task<IActionResult> ListShoppingLists()
	{
		return Ok(await Mediator.Send(new ListShoppingLists.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search([FromBody] SearchRecipeModel model)
	{
		return Ok(await Mediator.Send(new SearchRecipes.Query(Member.ActiveHouseholdId, Member.Id, model)));
	}

	[HttpPost("set-featured-image")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SetFeaturedImage([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new SetFeaturedImage.Command(Member.Id, sid)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormRecipeModel model)
	{
		return Ok(await Mediator.Send(new UpdateRecipeProperties.Command(Member.Id, model)));
	}

	[HttpPost("update/{sid}/ingredients")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> UpdateIngredients(long id, [FromBody]UpdateIngredientsModel ingredients)
	{
		return Ok(await Mediator.Send(new UpdateIngredients.Command(Member.Id, id, ingredients)));
	}

	[HttpPost("update/{sid}/notes")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> UpdateNotes(long id, [FromBody] UpdateNotesModel model)
	{
		return Ok(await Mediator.Send(new UpdateNotes.Command(Member.Id, id, model)));
	}

	[HttpPost("update/{sid}/steps")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> UpdateSteps(long id, [FromBody] UpdateStepsModel model)
	{
		return Ok(await Mediator.Send(new UpdateSteps.Command(Member.Id, id, model)));
	}

	[HttpPost("upload-image/{sid}")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> UploadImage(long id, [FromForm] IFormFile file)
	{
		return Ok(await Mediator.Send(new UploadImage.Command(Member.Id, id, file)));
	}
}
