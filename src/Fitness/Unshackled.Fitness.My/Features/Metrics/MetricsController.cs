using Microsoft.AspNetCore.Mvc;
using Unshackled.Fitness.My.Client.Features.Metrics.Models;
using Unshackled.Fitness.My.Features.Metrics.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Fitness.My.Features.Metrics;

[ApiController]
[ApiRoute("metrics")]
public class MetricsController : BaseController
{
	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteDefinition.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> Get(long id)
	{
		return Ok(await Mediator.Send(new GetDefinition.Query(Member.Id, id)));
	}

	[HttpPost("get-calendar/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetCalendar(long id, [FromBody] SearchCalendarModel model)
	{
		return Ok(await Mediator.Send(new GetCalendar.Query(Member.Id, id, model)));
	}

	[HttpPost("list")]
	public async Task<IActionResult> List([FromBody] DateTime displayDateUtc)
	{
		return Ok(await Mediator.Send(new ListMetrics.Query(Member.Id, displayDateUtc)));
	}

	[HttpGet("list-definitions")]
	public async Task<IActionResult> ListDefinitions()
	{
		return Ok(await Mediator.Send(new ListDefintions.Query(Member.Id)));
	}

	[HttpPost("save")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Save([FromBody] SaveMetricModel model)
	{
		return Ok(await Mediator.Send(new SaveMetric.Command(Member.Id, model)));
	}

	[HttpPost("save-definition")]
	[ActiveMemberRequired]
	public async Task<IActionResult> SaveDefinition([FromBody] FormMetricDefinitionModel model)
	{
		return Ok(await Mediator.Send(new SaveDefinition.Command(Member.Id, model)));
	}

	[HttpPost("toggle-archived/{sid}")]
	[ActiveMemberRequired]
	[DecodeId]
	public async Task<IActionResult> ToggleArchived(long id, [FromBody] bool isArchived)
	{
		return Ok(await Mediator.Send(new ToggleArchived.Command(Member.Id, id, isArchived)));
	}

	[HttpPost("update-sort")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateSort([FromBody] UpdateSortModel model)
	{
		return Ok(await Mediator.Send(new UpdateSort.Command(Member.Id, model)));
	}
}
