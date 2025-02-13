using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class DeletePlan
{
	public class Command : IRequest<CommandResult>
	{
		public string Sid { get; private set; }

		public Command(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.Sid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
