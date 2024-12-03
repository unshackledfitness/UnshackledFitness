using Microsoft.AspNetCore.Mvc;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Food.My.Features.Cookbooks.Actions;
using Unshackled.Studio.Core.Server.Features;

namespace Unshackled.Food.My.Features.Cookbooks;

[ApiController]
[ApiRoute("cookbooks")]
public class CookbooksController : BaseController
{
	[HttpPost("add")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Add([FromBody] FormCookbookModel model)
	{
		bool hasActive = Member.ActiveCookbookId != 0;
		return Ok(await Mediator.Send(new AddCookbook.Command(Member.Id, hasActive, model)));
	}

	[HttpPost("delete")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Delete([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteCookbook.Command(Member.Id, sid)));
	}

	[HttpPost("delete-from/{sid}")]
	[DecodeId]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteFromCookbook(long id, [FromBody] string memberId)
	{
		return Ok(await Mediator.Send(new DeleteMember.Command(Member.Id, memberId, id)));
	}

	[HttpPost("delete-invite")]
	[ActiveMemberRequired]
	public async Task<IActionResult> DeleteInvite([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new DeleteInvite.Command(Member.Id, sid)));
	}

	[HttpGet("get/{sid}")]
	[DecodeId]
	public async Task<IActionResult> GetCookbook(long id)
	{
		return Ok(await Mediator.Send(new GetCookbook.Query(Member.Id, id)));
	}

	[HttpPost("invite/{sid}")]
	[DecodeId]
	[ActiveMemberRequired]
	public async Task<IActionResult> InviteMember(long id, [FromBody] FormAddInviteModel model)
	{
		return Ok(await Mediator.Send(new AddInvite.Command(Member.Id, id, model)));
	}

	[HttpPost("join")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Join([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new JoinCookbook.Command(Member, sid)));
	}

	[HttpPost("leave")]
	public async Task<IActionResult> Leave([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new LeaveCookbook.Command(Member.Id, sid)));
	}

	[HttpGet("list")]
	public async Task<IActionResult> ListCookbooks()
	{
		return Ok(await Mediator.Send(new ListCookbooks.Query(Member.Id, Member.Email)));
	}

	[HttpGet("list/{sid}/invites")]
	[DecodeId]
	public async Task<IActionResult> ListInvites(long id)
	{
		return Ok(await Mediator.Send(new ListMemberInvites.Query(Member.Id, id)));
	}

	[HttpPost("make-owner")]
	[ActiveMemberRequired]
	public async Task<IActionResult> MakeOwner([FromBody] MakeOwnerModel model)
	{
		return Ok(await Mediator.Send(new MakeOwner.Command(Member.Id, model)));
	}

	[HttpPost("reject-invite")]
	[ActiveMemberRequired]
	public async Task<IActionResult> RejectInvite([FromBody] string sid)
	{
		return Ok(await Mediator.Send(new RejectInvite.Command(Member.Email, sid)));
	}

	[HttpPost("search/{sid}/members")]
	[DecodeId]
	public async Task<IActionResult> SearchMembers(long id, [FromBody] MemberSearchModel model)
	{
		return Ok(await Mediator.Send(new SearchMembers.Query(Member.Id, id, model)));
	}

	[HttpPost("update")]
	[ActiveMemberRequired]
	public async Task<IActionResult> Update([FromBody] FormCookbookModel model)
	{
		return Ok(await Mediator.Send(new UpdateProperties.Command(Member.Id, model)));
	}

	[HttpPost("update-member")]
	[ActiveMemberRequired]
	public async Task<IActionResult> UpdateMember([FromBody] FormMemberModel model)
	{
		return Ok(await Mediator.Send(new UpdateMember.Command(Member.Id, model)));
	}
}
