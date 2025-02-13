using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<PlanModel>>
	{
		public FormUpdatePlanModel Model { get; private set; }

		public Command(FormUpdatePlanModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult<PlanModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<PlanModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormUpdatePlanModel, PlanModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<PlanModel>(false, Globals.UnexpectedError);
		}
	}
}
