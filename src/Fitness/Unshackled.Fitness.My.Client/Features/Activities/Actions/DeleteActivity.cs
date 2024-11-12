using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public class DeleteActivity
{
	public class Command : IRequest<CommandResult>
	{
		public string ActivitySid { get; private set; }

		public Command(string workoutSid)
		{
			ActivitySid = workoutSid;
		}
	}

	public class Handler : BaseActivityHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.ActivitySid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
