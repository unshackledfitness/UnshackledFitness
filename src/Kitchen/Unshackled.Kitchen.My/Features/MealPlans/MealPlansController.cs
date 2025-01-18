using Microsoft.AspNetCore.Mvc;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Kitchen.My.Features.MealPlans.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Kitchen.My.Features.MealPlans;

[ApiController]
[ApiRoute("meal-plans")]
public class MealPlansController : BaseController
{
	[HttpPost("add-definition")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] MealDefinitionModel model)
	{
		return Ok(await Mediator.Send(new AddMealDefinition.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-recipe")]
	public async Task<IActionResult> AddRecipe([FromBody] AddPlanRecipeModel model)
	{
		return Ok(await Mediator.Send(new AddMealRecipe.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("delete-definition")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteMealDefinition.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("delete-recipe")]
	public async Task<IActionResult> DeleteRecipe([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteMealRecipe.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("list")]
	public async Task<IActionResult> List([FromBody] DateOnly dateStart)
	{
		return Ok(await Mediator.Send(new ListMealPlanRecipes.Query(Member.Id, Member.ActiveHouseholdId, dateStart)));
	}

	[HttpGet("list-meal-definitions")]
	public async Task<IActionResult> ListMealDefinitions()
	{
		return Ok(await Mediator.Send(new ListMealDefinitions.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("list-make-it")]
	public async Task<IActionResult> ListMakeIt([FromBody] Dictionary<string, decimal> recipesAndScales)
	{
		return Ok(await Mediator.Send(new ListMakeIt.Query(Member.Id, Member.ActiveHouseholdId, recipesAndScales)));
	}

	[HttpGet("list-shopping-lists")]
	public async Task<IActionResult> ListShoppingLists()
	{
		return Ok(await Mediator.Send(new ListShoppingLists.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("search-recipes")]
	public async Task<IActionResult> SearchRecipes([FromBody] SearchRecipeModel model)
	{
		return Ok(await Mediator.Send(new SearchRecipes.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-definition")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] MealDefinitionModel model)
	{
		return Ok(await Mediator.Send(new UpdateMealDefinition.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-definition-sort")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateSort([FromBody] List<MealDefinitionModel> definitions)
	{
		return Ok(await Mediator.Send(new UpdateMealDefinitionSort.Command(Member.Id, Member.ActiveHouseholdId, definitions)));
	}

	[HttpPost("update-sort")]
	public async Task<IActionResult> UpdateSort([FromBody] List<UpdateSortModel> updates)
	{
		return Ok(await Mediator.Send(new UpdateSort.Command(Member.Id, Member.ActiveHouseholdId, updates)));
	}
}
