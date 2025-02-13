using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class UpdateMealDefinitionSort
{
	public class Command : IRequest<CommandResult>
	{
		public List<MealDefinitionModel> Definitions { get; private set; }

		public Command(List<MealDefinitionModel> definitions)
		{
			Definitions = definitions;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-definition-sort", request.Definitions)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}