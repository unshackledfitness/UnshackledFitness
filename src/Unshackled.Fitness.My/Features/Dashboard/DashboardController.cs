using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Fitness.My.Features.Dashboard.Actions;

namespace Unshackled.Fitness.My.Features.Dashboard;

[ApiController]
[ApiRoute("dashboard")]
public class DashboardController : BaseController
{
	[HttpPost("add-workout")]
	[ActiveMemberRequired]
	public async Task<IActionResult> AddWorkout([FromBody] string templateSid)
	{
		return Ok(await Mediator.Send(new AddWorkout.Command(Member.Id, templateSid)));
	}

	[HttpPost("get-scheduled-prep")]
	public async Task<IActionResult> GetScheduledPrep([FromBody] DateOnly displayDate)
	{
		return Ok(await Mediator.Send(new GetScheduledPrep.Query(Member.Id, Member.ActiveHouseholdId, displayDate)));
	}

	[HttpPost("list-make-it")]
	public async Task<IActionResult> ListMakeIt([FromBody] Dictionary<string, decimal> recipesAndScales)
	{
		return Ok(await Mediator.Send(new ListMakeIt.Query(Member.Id, Member.ActiveHouseholdId, recipesAndScales)));
	}

	[HttpPost("list-metrics")]
	public async Task<IActionResult> ListMetrics([FromBody] DateTime displayDateUtc)
	{
		return Ok(await Mediator.Send(new ListMetrics.Query(Member.Id, displayDateUtc)));
	}

	[HttpPost("list-scheduled-items")]
	public async Task<IActionResult> ListScheduledItems([FromBody] DateOnly displayDateUtc)
	{
		return Ok(await Mediator.Send(new ListScheduledItems.Query(Member.Id, displayDateUtc)));
	}

	[HttpPost("save-metric")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SaveMetric([FromBody] SaveMetricModel model)
	{
		return Ok(await Mediator.Send(new SaveMetric.Command(Member.Id, model)));
	}

	[HttpPost("skip-training-session")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SkipTrainingSession([FromBody] string planSid)
	{
		return Ok(await Mediator.Send(new SkipTrainingSession.Command(Member.Id, planSid)));
	}

	[HttpPost("skip-workout")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SkipWorkout([FromBody] string programSid)
	{
		return Ok(await Mediator.Send(new SkipWorkout.Command(Member.Id, programSid)));
	}

	[HttpPost("workout-stats")]
	public async Task<IActionResult> GetWorkoutStats([FromBody] DateTime toDateUtc)
	{
		return Ok(await Mediator.Send(new GetDashboardStats.Query(Member.Id, toDateUtc)));
	}
}
