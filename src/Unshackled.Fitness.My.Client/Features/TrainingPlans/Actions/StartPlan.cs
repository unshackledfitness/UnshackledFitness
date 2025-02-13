using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class StartPlan
{
	public class Command : IRequest<CommandResult>
	{
		public FormStartPlanModel Model { get; private set; }

		public Command(FormStartPlanModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}start", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
