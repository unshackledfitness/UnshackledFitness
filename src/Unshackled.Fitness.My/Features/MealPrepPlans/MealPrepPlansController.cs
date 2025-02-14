using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Features.MealPrepPlans.Actions;

namespace Unshackled.Fitness.My.Features.MealPrepPlans;

[ApiController]
[ApiRoute("meal-prep-plans")]
public class MealPrepPlansController : BaseController
{
	[HttpPost("add-recipe")]
	public async Task<IActionResult> AddRecipe([FromBody] AddPlanRecipeModel model)
	{
		return Ok(await Mediator.Send(new AddMealRecipe.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-slot")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] SlotModel model)
	{
		return Ok(await Mediator.Send(new AddSlot.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("add-to-shopping-list")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddToShoppingList([FromBody] AddRecipesToListModel model)
	{
		return Ok(await Mediator.Send(new AddToList.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("copy-recipes")]
	public async Task<IActionResult> CopyRecipes([FromBody] CopyRecipesModel model)
	{
		return Ok(await Mediator.Send(new CopyMealRecipes.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("delete-recipe")]
	public async Task<IActionResult> DeleteRecipe([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteMealRecipe.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("delete-slot")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteSlot.Command(Member.Id, Member.ActiveHouseholdId, sid)));
	}

	[HttpPost("get-add-to-list-items")]
	[ActiveMemberRequired]
	public async Task<IActionResult> GetAddToListItems([FromBody] List<SelectListModel> selects)
	{
		return Ok(await Mediator.Send(new GetAddToListItems.Query(Member.Id, selects)));
	}

	[HttpPost("list")]
	public async Task<IActionResult> List([FromBody] DateOnly dateStart)
	{
		return Ok(await Mediator.Send(new ListMealPlanRecipes.Query(Member.Id, Member.ActiveHouseholdId, dateStart)));
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

	[HttpGet("list-slots")]
	public async Task<IActionResult> ListSlots()
	{
		return Ok(await Mediator.Send(new ListSlots.Query(Member.Id, Member.ActiveHouseholdId)));
	}

	[HttpPost("search-recipes")]
	public async Task<IActionResult> SearchRecipes([FromBody] SearchRecipeModel model)
	{
		return Ok(await Mediator.Send(new SearchRecipes.Query(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-recipe")]
	public async Task<IActionResult> UpdateRecipe([FromBody] MealPrepPlanRecipeModel model)
	{
		return Ok(await Mediator.Send(new UpdateMealRecipe.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-slot")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateSlot([FromBody] SlotModel model)
	{
		return Ok(await Mediator.Send(new UpdateSlot.Command(Member.Id, Member.ActiveHouseholdId, model)));
	}

	[HttpPost("update-slot-sort")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateSlotSort([FromBody] List<SlotModel> slots)
	{
		return Ok(await Mediator.Send(new UpdateSlotSort.Command(Member.Id, Member.ActiveHouseholdId, slots)));
	}

	[HttpPost("update-sort")]
	public async Task<IActionResult> UpdateSort([FromBody] List<UpdateSortModel> updates)
	{
		return Ok(await Mediator.Send(new UpdateSort.Command(Member.Id, Member.ActiveHouseholdId, updates)));
	}
}
