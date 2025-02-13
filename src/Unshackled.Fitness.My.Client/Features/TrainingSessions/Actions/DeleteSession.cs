using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public class DeleteSession
{
	public class Command : IRequest<CommandResult>
	{
		public string SessionSid { get; private set; }

		public Command(string sessionSid)
		{
			SessionSid = sessionSid;
		}
	}

	public class Handler : BaseTrainingSessionHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.SessionSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
