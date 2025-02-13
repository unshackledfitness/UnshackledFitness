using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class DeleteStore
{
	public class Command : IRequest<CommandResult>
	{
		public string StoreSid { get; private set; }

		public Command(string storeSid)
		{
			StoreSid = storeSid;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.StoreSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
