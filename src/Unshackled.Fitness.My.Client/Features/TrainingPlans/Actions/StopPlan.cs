using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class StopPlan
{
	public class Command : IRequest<CommandResult>
	{
		public string Sid { get; set; }

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
			return await PostToCommandResultAsync($"{baseUrl}stop", request.Sid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
