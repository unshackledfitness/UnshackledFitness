using MediatR;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class AddMealDefinition
{
	public class Command : IRequest<CommandResult<List<MealDefinitionModel>>>
	{
		public MealDefinitionModel Model { get; private set; }

		public Command(MealDefinitionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult<List<MealDefinitionModel>>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<List<MealDefinitionModel>>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<MealDefinitionModel, List<MealDefinitionModel>>($"{baseUrl}add-definition", request.Model)
				?? new CommandResult<List<MealDefinitionModel>>(false, Globals.UnexpectedError);
		}
	}
}