using MediatR;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class UpdateSetProperties
{
	public class Command : IRequest<CommandResult>
	{
		public FormSetPropertiesModel Set { get; private set; }

		public Command(FormSetPropertiesModel set)
		{
			Set = set;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-set-properties", request.Set)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
