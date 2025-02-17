using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class DeleteInvite
{
	public class Command : IRequest<CommandResult>
	{
		public string Sid { get; private set; }

		public Command(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete-invite", request.Sid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
