using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingPlans.Actions;

public class AddPlan
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormAddPlanModel Model { get; private set; }

		public Command(FormAddPlanModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingPlanHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormAddPlanModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
