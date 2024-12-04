using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks.Actions;

public class DeleteMember
{
	public class Command : IRequest<CommandResult>
	{
		public string CookbookSid { get; private set; }
		public string MemberSid { get; private set; }

		public Command(string cookbookSid, string memberSid)
		{
			CookbookSid = cookbookSid;
			MemberSid = memberSid;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete-from/{request.CookbookSid}", request.MemberSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
