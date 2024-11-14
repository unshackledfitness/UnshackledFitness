using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class DuplicatePlan
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormUpdatePlanModel Model { get; private set; }

		public Command(FormUpdatePlanModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormUpdatePlanModel, string>($"{baseUrl}duplicate", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
