using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class SaveSessions
{
	public class Command : IRequest<CommandResult>
	{
		public FormUpdateSessionsModel Model { get; private set; }

		public Command(FormUpdateSessionsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}save-sessions", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
