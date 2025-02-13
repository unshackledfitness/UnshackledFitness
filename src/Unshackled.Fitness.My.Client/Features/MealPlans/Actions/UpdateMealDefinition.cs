using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class UpdateMealDefinition
{
	public class Command : IRequest<CommandResult>
	{
		public MealDefinitionModel Model { get; private set; }

		public Command(MealDefinitionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-definition", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}